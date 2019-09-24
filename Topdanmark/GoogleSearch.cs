using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System.Threading;

namespace Tests
{
    public class Tests
    {
        public IWebDriver driver;

        public void BrowserOption(string browserName)
        {
            if(browserName.ToLower().Equals("chrome"))
            {
                driver = new ChromeDriver("C:\\Topdanmark");
            }

            if(browserName.ToLower().Equals("firefox"))
            {
                FirefoxDriverService service = FirefoxDriverService.CreateDefaultService("C:\\Topdanmark");
                service.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                driver = new FirefoxDriver(service);
            }

            if (browserName.ToLower().Equals("ie"))
            {
                driver = new InternetExplorerDriver(@"C:\\Topdanmark");
            }
        }

        public void NavigateTo(string strUrl, string strBrowserName)
        {
            BrowserOption(strBrowserName);
            driver.Navigate().GoToUrl(strUrl);
            driver.Manage().Window.Maximize();
            Thread.Sleep(3000);
        }

        [Test]
        public void CheckDefaultSuggestedPhrase()
        {
            NavigateTo("https://google.com", "chrome");
            Thread.Sleep(2000);
            IWebElement searchField = driver.FindElement(By.XPath("//*[@id='tsf']/div[2]/div[1]/div[1]/div/div[2]/input"));
            searchField.SendKeys("topdanmark");
            Thread.Sleep(2000);
            IWebElement firstResult = driver.FindElement(By.CssSelector("#tsf > div:nth-child(2) > div.A8SBwf.emcav > div.UUbT9 > div.aajZCb > ul > li:nth-child(1)"));
            Assert.AreEqual("topdanmark", firstResult.Text.ToLower(), "Suggested result not matched");
            driver.Close();
            driver.Quit();
        }

        [TestCase("chrome")]
        [TestCase("ie")]
        [TestCase("firefox")]
        public void CrossBrowserTest(string strBrowser)
        {
            NavigateTo("https://www.topdanmark.dk/", strBrowser);
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.ClassName("navbar-brand__logo")).Displayed, "Page might not be loaded correctly");
            driver.Close();
            driver.Quit();
        }
    }
}