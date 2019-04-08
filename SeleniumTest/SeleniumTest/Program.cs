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
            using (driver = new OpenQA.Selenium.Firefox.FirefoxDriver())
            //using (driver = new OpenQA.Selenium.Chrome.ChromeDriver())
            {
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));

                //open google and do a search
                driver.Navigate().GoToUrl("https://www.google.ch/");

                By locator = By.XPath("//input[@type='text']");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
                driver.FindElement(locator).Click();
                driver.FindElement(locator).Clear();
                driver.FindElement(locator).SendKeys("test");
                driver.FindElement(locator).SendKeys(Keys.Enter);

                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.LinkText("Vidéos")));

                driver.FindElement(By.LinkText("Vidéos")).Click();

                //wait and click can be done in one line
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.LinkText("Tous"))).Click();                

                /*
                driver.Navigate().GoToUrl("https://demos.telerik.com/aspnet-mvc/tabstrip");

                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Moscow'])[1]")));
                //driver.FindElement(By.XPath("//div[@id='tabstrip']/ul/li[3]/span[2]")).Click();
                driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Moscow'])")).Click();

                Thread.Sleep(1000);

                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Paris'])[1]"))).Click();

                Thread.Sleep(5000);
                */

                //close the web driver
                //driver.Close();
                //driver.Dispose();
            }
        }
    }
}
