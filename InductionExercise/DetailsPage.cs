using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using Kajabity.Tools.Java;

namespace InductionExercise
{

    public class DetailsPage : LoadableComponent<DetailsPage>
    {

        private readonly IWebDriver _driver;
        private readonly JavaProperties _javaProperties;

        private readonly string _url;

        [FindsBy(How = How.Id, Using = "VehicleId")]
        private IWebElement inputVehicleId;

        [FindsBy(How = How.CssSelector, Using = "a[href*='/Vehicle/Edit/'")]
        private IWebElement hrefEdit;

        [FindsBy(How = How.CssSelector, Using = "a[href*='/'")]
        private IWebElement hrefVehicle;

        public DetailsPage(IWebDriver d)
        {
            _driver = d;
            PageFactory.InitElements(_driver, this);
            _url = @"" + EntryPoint.Properties.GetProperty("baseURL") + "/Vehicle/Details/";
        }

        public DetailsPage(IWebDriver d, JavaProperties jp)
        {
            _driver = d;
            _javaProperties = jp;
            PageFactory.InitElements(_driver, this);
            _url = @"" + _javaProperties.GetProperty("baseURL") + "/Vehicle/Details/";
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
            if ((_driver.Url.StartsWith(_url) == false))
            {
                UnableToLoadMessage = "Not on the Vehicle Details page.";
                return false;
            }

            UnableToLoadMessage = "";

            return true;
        }

        public VehiclePage BackToList(JavaProperties jp)
        {
            IWebElement field = _driver.FindElement(By.LinkText("Back to List"));
            field.Click();
            return new VehiclePage(_driver, jp);
        }

        public EditPage Edit(JavaProperties jp)
        {
            IWebElement field = _driver.FindElement(By.LinkText("Edit"));
            field.Click();
            return new EditPage(_driver, _javaProperties);
        }

    }

}
