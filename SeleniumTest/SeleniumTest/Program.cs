using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using System.Threading;

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

			//open google and do a search
            driver.Navigate().GoToUrl("https://www.google.ch/");
            driver.FindElement(By.Id("lst-ib")).Click();
            driver.FindElement(By.Id("lst-ib")).Clear();
            driver.FindElement(By.Id("lst-ib")).SendKeys("test");
            driver.FindElement(By.Id("lst-ib")).SendKeys(Keys.Enter);
			//a little wait to be sure the web page is loaded
            Thread.Sleep(500);
            driver.FindElement(By.LinkText("Images")).Click();
            driver.FindElement(By.LinkText("Tous")).Click();

			//close the web driver
            driver.Close();
            driver.Dispose();

        }
    }
}
