using BesucherParken.Backend;
using System;

namespace Besucherparken
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 2)
            {
                Console.WriteLine("Falsche Parameter angegeben.");
                Console.WriteLine("Kennzeichen: XX-XX-XXXX");
                Console.WriteLine("Parkdatum: DD.MM.YYYY");
                return;
            }

            string kennzeichen = args[0];
            string parkdatum = args[1];

            BesucherparkenSeleniumRunner runner = new BesucherparkenSeleniumRunner();
            runner.ErstelleParkausweis(kennzeichen, parkdatum);
        }
    }
}
