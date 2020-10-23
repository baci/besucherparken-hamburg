using System;
using System.Configuration;
using System.Collections.Generic;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace BesucherParken.Backend
{
    public class BesucherparkenSeleniumRunner
    {
        private string geburtsdatum;
        private string strasse;
        private string hausnummer;
        private string plz;
        private string username;
        private string passwort;
        private RemoteWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;
        public BesucherparkenSeleniumRunner()
        {
            driver = new FirefoxDriver();
            js = driver;
            vars = new Dictionary<string, object>();

            FixDriverCommandExecutionDelay(driver);
            KonfigurationAuslesen();
        }

        ~BesucherparkenSeleniumRunner()
        {
            driver.Quit();
        }

        private void FixDriverCommandExecutionDelay(RemoteWebDriver driver)
        {
            PropertyInfo commandExecutorProperty = typeof(RemoteWebDriver).GetProperty("CommandExecutor", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);
            ICommandExecutor commandExecutor = (ICommandExecutor)commandExecutorProperty.GetValue(driver);

            FieldInfo remoteServerUriField = commandExecutor.GetType().GetField("remoteServerUri", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField);

            if (remoteServerUriField == null)
            {
                FieldInfo internalExecutorField = commandExecutor.GetType().GetField("internalExecutor", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
                commandExecutor = (ICommandExecutor)internalExecutorField.GetValue(commandExecutor);
                remoteServerUriField = commandExecutor.GetType().GetField("remoteServerUri", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField);
            }

            if (remoteServerUriField != null)
            {
                string remoteServerUri = remoteServerUriField.GetValue(commandExecutor).ToString();

                string localhostUriPrefix = "http://localhost";

                if (remoteServerUri.StartsWith(localhostUriPrefix))
                {
                    remoteServerUri = remoteServerUri.Replace(localhostUriPrefix, "http://127.0.0.1");

                    remoteServerUriField.SetValue(commandExecutor, new Uri(remoteServerUri));
                }
            }
        }        

        private void KonfigurationAuslesen()
        {
            geburtsdatum = ConfigurationManager.AppSettings["geburtsdatum"];
            strasse = ConfigurationManager.AppSettings["strasse"];
            hausnummer = ConfigurationManager.AppSettings["hausnr"];
            plz = ConfigurationManager.AppSettings["plz"];
            username = ConfigurationManager.AppSettings["username"];
            passwort = ConfigurationManager.AppSettings["passwort"];
        }

        // Kennzeichen: XX-XX-XXXX
        // Datum: DD.MM.YYYY
        public void ErstelleParkausweis(string kennzeichen, string parkdatum)
        {           
            driver.Navigate().GoToUrl("https://serviceportal.hamburg.de/HamburgGateway/FVS/FV/LBV/Bewohnerparken/?nep=1");
            driver.Manage().Window.Size = new System.Drawing.Size(1550, 838);

            driver.FindElement(By.Id("Username")).SendKeys(username);
            driver.FindElement(By.Id("Password")).SendKeys(passwort);
            driver.FindElement(By.Name("LoginUsingUsernamePassword")).Click();
            
            driver.FindElement(By.Id("Dsgvo_DsgvoAcceptance")).Click();
            driver.FindElement(By.CssSelector(".ml-auto")).Click();
            driver.FindElement(By.Id("BesucherStartProcessButton")).Click();
            driver.FindElement(By.CssSelector(".col-12:nth-child(2) > #radioSalutation")).Click();
            driver.FindElement(By.Id("citizenBirthDateTextbox")).Click();
            driver.FindElement(By.Id("citizenBirthDateTextbox")).SendKeys(geburtsdatum);
            driver.FindElement(By.Id("citizenStreetTextbox")).Click();
            driver.FindElement(By.Id("citizenStreetTextbox")).SendKeys(strasse);
            driver.FindElement(By.Id("citizenNumberTextbox")).Click();
            driver.FindElement(By.Id("citizenNumberTextbox")).SendKeys(hausnummer);
            driver.FindElement(By.Id("citizenZipCodeTextbox")).Click();
            driver.FindElement(By.Id("citizenZipCodeTextbox")).SendKeys(plz);
            driver.FindElement(By.Id("registrationPlateTextbox")).Click();
            driver.FindElement(By.Id("registrationPlateTextbox")).SendKeys(kennzeichen);
            driver.FindElement(By.Id("Besucherparken_VisitModel_Begin")).Click();
            driver.FindElement(By.Id("Besucherparken_VisitModel_Begin")).SendKeys(parkdatum);
            driver.FindElement(By.Id("checkboxPrintStatus")).Click();
            driver.FindElement(By.CssSelector(".ml-auto")).Click();
            driver.FindElement(By.CssSelector(".ml-auto")).Click();
            driver.FindElement(By.LinkText("Weiter")).Click();
            driver.FindElement(By.LinkText("Parkausweis drucken und parken!")).Click();
            driver.FindElement(By.LinkText("Beenden")).Click();
        }
    }

}
