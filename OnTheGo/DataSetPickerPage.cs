using System;
using Kajabity.Tools.Java;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;

namespace MYOB.AutoTest.OnTheGo
{
    
    public class DataSetPickerPage : LoadableComponent<DataSetPickerPage>
    {

        private readonly IWebDriver _driver;
        private readonly JavaProperties _javaProperties;

        private string URL;

        [FindsBy(How = How.Id, Using = "ddlDatabase")]
        private IWebElement ddlDatabase;

        [FindsBy(How = How.Id, Using = "btnGo")]
        private IWebElement btnGo;

        public DataSetPickerPage(IWebDriver d)
        {
            _driver = d;
            PageFactory.InitElements(_driver, this);
            URL = @"" + EntryPoint.Properties.GetProperty("baseURL") + "/Login/DataSetPicker";
        }

        public DataSetPickerPage(IWebDriver d, JavaProperties jp)
        {
            _driver = d;
            _javaProperties = jp;
            PageFactory.InitElements(_driver, this);
            URL = @"" + _javaProperties.GetProperty("baseURL") + "/Login/DataSetPicker";
        }

        public IWebDriver GetDriver()
        {
            return _driver;
        }

        public string GetURL()
        {
            return URL;
        }

        protected override void ExecuteLoad()
        {
            if (_driver.Url.Equals(URL) == false)
            {
                Console.WriteLine("Loading Data Set Picker Page");
                _driver.Navigate().GoToUrl(URL);
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
            if ((_driver.Url.Equals(URL) == false))
            {
                UnableToLoadMessage = "Not on the Login Data Set Picker page.";
                return false;
            }
            UnableToLoadMessage = "";
            return true;
        }

        public HomePage ChooseDatabase(string Database)
        {
            SelectElement selectElement = new SelectElement(ddlDatabase);
            selectElement.SelectByText(Database);
            btnGo.Click();
            return new HomePage(_driver, _javaProperties);
        }

    }

}
