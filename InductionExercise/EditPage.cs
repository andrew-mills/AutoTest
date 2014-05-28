using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using Kajabity.Tools.Java;

namespace InductionExercise
{

    public class EditPage : LoadableComponent<EditPage>
    {

        private readonly IWebDriver _driver;
        private readonly JavaProperties _javaProperties;

        private readonly string _url;

        [FindsBy(How = How.Id, Using = "VehicleId")]
        private IWebElement inputVehicleId;

        [FindsBy(How = How.Id, Using = "RegistrationPlate")]
        private IWebElement inputRegistrationPlate;

        [FindsBy(How = How.Id, Using = "Make")]
        private IWebElement inputMake;

        [FindsBy(How = How.Id, Using = "TheModel")]
        private IWebElement inputTheModel;

        [FindsBy(How = How.Id, Using = "Year")]
        private IWebElement inputYear;

        [FindsBy(How = How.Id, Using = "Colour")]
        private IWebElement inputColour;

        [FindsBy(How = How.Id, Using = "PurchaseDate")]
        private IWebElement inputPurchaseDate;

        [FindsBy(How = How.Id, Using = "PurchasePrice")]
        private IWebElement inputPurchasePrice;

        [FindsBy(How = How.Id, Using = "btnSave")]
        private IWebElement btnSave;

        [FindsBy(How = How.CssSelector, Using = "a[href*='/'")]
        private IWebElement hrefVehicle;

        public EditPage(IWebDriver d)
        {
            _driver = d;
            PageFactory.InitElements(_driver, this);
            _url = @"" + EntryPoint.Properties.GetProperty("baseURL") + "/Vehicle/Edit/";
        }

        public EditPage(IWebDriver d, JavaProperties jp)
        {
            _driver = d;
            _javaProperties = jp;
            PageFactory.InitElements(_driver, this);
            _url = @"" + _javaProperties.GetProperty("baseURL") + "/Vehicle/Edit/";
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
                UnableToLoadMessage = "Not on the Vehicle Edit page.";
                return false;
            }

            UnableToLoadMessage = "";

            return true;
        }

        public VehiclePage BackToList()
        {
            hrefVehicle.Click();
            return new VehiclePage(_driver, _javaProperties);
        }

        public VehiclePage Save()
        {
            btnSave.Click();
            return new VehiclePage(_driver, _javaProperties);
        }

    }

}
