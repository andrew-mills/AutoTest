using System;
using System.Globalization;
using Kajabity.Tools.Java;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;

namespace MYOB.AutoTest.OnTheGo
{

    public class HomePage : LoadableComponent<HomePage>
    {

        private readonly IWebDriver _driver;
        private readonly JavaProperties _javaProperties;

        private string URL1;
        private string URL2;

        [FindsBy(How = How.Id, Using = "logoutForm")]
        private IWebElement logoutForm;

        public HomePage(IWebDriver d)
        {
            _driver = d;
            PageFactory.InitElements(_driver, this);
            URL1 = @"" + EntryPoint.Properties.GetProperty("baseURL") + "/";
            URL2 = @"" + EntryPoint.Properties.GetProperty("baseURL") + "/Home";
        }

        public HomePage(IWebDriver d, JavaProperties jp)
        {
            _driver = d;
            _javaProperties = jp;
            PageFactory.InitElements(_driver, this);
            URL1 = @"" + _javaProperties.GetProperty("baseURL") + "/";
            URL2 = @"" + _javaProperties.GetProperty("baseURL") + "/Home";
        }

        public IWebDriver GetDriver()
        {
            return _driver;
        }

        public string GetURL1()
        {
            return URL1;
        }

        public string GetURL2()
        {
            return URL2;
        }

        protected override void ExecuteLoad()
        {
            if ((_driver.Url.Equals(URL1) == false) && (_driver.Url.Equals(URL2) == false))
            {
                Console.WriteLine(@"Loading Home Page");
                _driver.Navigate().GoToUrl(URL1);
            }
        }

        protected override bool EvaluateLoadedStatus()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            wait.Until((d) =>
            {
                return
                    (((IJavaScriptExecutor)d).ExecuteScript("return document.readyState")).ToString()
                                                                                           .Equals("complete");
            });
            if ((_driver.Url.Equals(URL1) == false) && (_driver.Url.Equals(URL2) == false))
            {
                UnableToLoadMessage = "Not on the Home page (" + _driver.Url.ToString(CultureInfo.InvariantCulture) + ").";
                return false;
            }
            UnableToLoadMessage = "";
            return true;
        }

        public LoginPage LogOut()
        {
            logoutForm.Submit();
            return new LoginPage(_driver, _javaProperties);
        }

    }

}
