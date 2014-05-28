using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using Kajabity.Tools.Java;

namespace InductionExercise
{

    public class VehiclePage : LoadableComponent<VehiclePage>
    {

        private readonly IWebDriver _driver;
        private readonly JavaProperties _javaProperties;

        private readonly string _url;

        [FindsBy(How = How.CssSelector, Using = "a[href*='Create'")]
        private IWebElement hrefCreate;

        [FindsBy(How = How.CssSelector, Using = "a[href*='Edit'")]
        private IWebElement hrefEdit;

        [FindsBy(How = How.CssSelector, Using = "a[href*='Details'")]
        private IWebElement hrefDetails;

        [FindsBy(How = How.CssSelector, Using = "a[href*='Delete'")]
        private IWebElement hrefDelete;

        public VehiclePage(IWebDriver d)
        {
            _driver = d;
            PageFactory.InitElements(_driver, this);
            _url = @"" + EntryPoint.Properties.GetProperty("baseURL") + "/";
        }

        public VehiclePage(IWebDriver d, JavaProperties jp)
        {
            _driver = d;
            _javaProperties = jp;
            PageFactory.InitElements(_driver, this);
            _url = @"" + _javaProperties.GetProperty("baseURL") + "/";
        }

        public IWebDriver GetDriver()
        {
            return _driver;
        }

        public string GetURL()
        {
            return _url;
        }

        protected override void ExecuteLoad()
        {
            if (_driver.Url.Equals(_url) != false) return;
            Console.WriteLine(@"Current [{0}], Loading [{1}]", _driver.Url, _url);
            _driver.Navigate().GoToUrl(_url);
        }

        protected override bool EvaluateLoadedStatus()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            wait.Until((d) => (((IJavaScriptExecutor) d).ExecuteScript("return document.readyState")).ToString()
                                                                                                     .Equals("complete"));
            if ((_driver.Url.Equals(_url) == false))
            {
                UnableToLoadMessage = "Not on the Vehicle page.";
                return false;
            }

            UnableToLoadMessage = "";

            return true;
        }

        public VehiclePage InvalidLogin(string UserName, string Password)
        {
            hrefCreate.SendKeys(UserName);
            hrefEdit.SendKeys(Password);
            hrefDetails.Click();
            return new VehiclePage(_driver, _javaProperties);
        }

    }

}
