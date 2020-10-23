using BesucherParken.Backend;
using System;
using System.Windows.Input;

namespace Besucherparken.GUI
{
    public class CommandHandler : ICommand
    {
        private Action _action;
        private Func<bool> _canExecute;

        /// <summary>
        /// Creates instance of the command handler
        /// </summary>
        /// <param name="action">Action to be executed by the command</param>
        /// <param name="canExecute">A bolean property to containing current permissions to execute the command</param>
        public CommandHandler(Action action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Wires CanExecuteChanged event 
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Forcess checking if execute is allowed
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }

    public class MainWindowViewModel
    {
        public string KennzeichenValue { get; set; }
        public string ParkDatumValue { get; set; }

        private ICommand _starteBesucherparkenCommand;
        public ICommand StarteBesucherparkenCommand
        {
            get
            {
                return _starteBesucherparkenCommand ?? (_starteBesucherparkenCommand = new CommandHandler(() => StarteBesucherparken(), () => CanExecute));
            }
        }
        public bool CanExecute
        {
            get
            {
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                return true;
            }
        }

        public MainWindowViewModel()
        {
            KennzeichenValue = "XX-XX-XXXX";
            ParkDatumValue = DateTime.Today.ToString("dd.MM.yyyy");
        }

        public void StarteBesucherparken()
        {
            BesucherparkenSeleniumRunner runner = new BesucherparkenSeleniumRunner();
            runner.ErstelleParkausweis(KennzeichenValue, ParkDatumValue);
        }
    }
}
