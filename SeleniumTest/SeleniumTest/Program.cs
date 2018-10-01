using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using System.Threading;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTest
{
    class Program
    {
        private static IWebDriver driver;

        static void Main(string[] args)
        {
			//create the web driver
            driver = new OpenQA.Selenium.Firefox.FirefoxDriver();
            //driver = new OpenQA.Selenium.Chrome.ChromeDriver();

            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));

			//open google and do a search
            driver.Navigate().GoToUrl("https://www.google.ch/");

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("lst-ib")));
            driver.FindElement(By.Id("lst-ib")).Click();
            driver.FindElement(By.Id("lst-ib")).Clear();
            driver.FindElement(By.Id("lst-ib")).SendKeys("test");
            driver.FindElement(By.Id("lst-ib")).SendKeys(Keys.Enter);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.LinkText("Vidéos")));

            driver.FindElement(By.LinkText("Vidéos")).Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.LinkText("Tous")));
            driver.FindElement(By.LinkText("Tous")).Click();

            Thread.Sleep(5000);

			//close the web driver
            driver.Close();
            driver.Dispose();

        }
    }
}
