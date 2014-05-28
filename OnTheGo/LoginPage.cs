using System;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using Kajabity.Tools.Java;

namespace MYOB.AutoTest.OnTheGo
{

    public class LoginPage : LoadableComponent<LoginPage>
    {

        private readonly IWebDriver _driver;
        private readonly JavaProperties _javaProperties;

        private readonly string URL;

        [FindsBy(How = How.Id, Using = "UserName")]
        private IWebElement txtUserName;

        [FindsBy(How = How.Id, Using = "Password")]
        private IWebElement txtPassword;

        [FindsBy(How = How.Id, Using = "btnLogin")]
        private IWebElement btnLogin;

        public LoginPage(IWebDriver d)
        {
            _driver = d;
            PageFactory.InitElements(_driver, this);
            URL = @"" + EntryPoint.Properties.GetProperty("baseURL") + "/Login";
        }

        public LoginPage(IWebDriver d, JavaProperties jp)
        {
            _driver = d;
            _javaProperties = jp;
            PageFactory.InitElements(_driver, this);
            URL = @"" + _javaProperties.GetProperty("baseURL") + "/Login";
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
                Console.WriteLine(@"Current [{0}], Loading [{1}]", _driver.Url, URL);
                _driver.Navigate().GoToUrl(URL);
            }
        }

        protected override bool EvaluateLoadedStatus()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            wait.Until((d) =>
                {
                    return
                        (((IJavaScriptExecutor) d).ExecuteScript("return document.readyState")).ToString()
                                                                                               .Equals("complete");
                });
            if ((_driver.Url.Equals(URL) == false))
            {
                UnableToLoadMessage = "Not on the Login page.";
                return false;
            }

            UnableToLoadMessage = "";

            Console.WriteLine("Evaluate Loaded Status!");

            return true;
        }

        public DataSetPickerPage ValidLogin(string UserName, string Password)
        {
            txtUserName.SendKeys(UserName);
            txtPassword.SendKeys(Password);
            btnLogin.Click();
            return new DataSetPickerPage(_driver, _javaProperties);
        }

        public LoginPage InvalidLogin(string UserName, string Password)
        {
            txtUserName.SendKeys(UserName);
            txtPassword.SendKeys(Password);
            btnLogin.Click();
            return new LoginPage(_driver, _javaProperties);
        }

    }

}
