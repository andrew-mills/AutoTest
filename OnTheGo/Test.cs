using System;
using Kajabity.Tools.Java;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

namespace MYOB.AutoTest.OnTheGo
{
    [TestFixture]
    public class Test
    {

        private IWebDriver _driver;
        private LoginPage _loginPage;
        private DataSetPickerPage _dataSetPickerPage;
        private HomePage _homePage;

        [SetUp]
        public void Setup()
        {
            if (_driver == null)
            {
                switch (EntryPoint.Properties.GetProperty("browser", "Internet Explorer"))
                {
                    case "Chrome":
                        _driver = new ChromeDriver();
                        break;
                    case "Firefox":
                        _driver = new FirefoxDriver();
                        break;
                    case "Internet Explorer":
                        _driver = new InternetExplorerDriver();
                        break;
                    case "Safari":
                        _driver = new SafariDriver();
                        break;
                    default:
                        _driver = new InternetExplorerDriver();
                        break;
                }
            }
        }

        [TearDown]
        public void Teardown()
        {
            _driver.Quit();
            _driver = null;
        }

        public void standalone_LoginTest()
        {
            Setup();
            LoginTest();
            Teardown();
        }

        [Test]
        public void LoginTest()
        {

            LoginPage _LoginPage = new LoginPage(_driver).Load();

            //_LoginPage.Load();

            DataSetPickerPage _DataSetPickerPage = _LoginPage.ValidLogin("onthego@myob.com", "Password1").Load();

            //_DataSetPickerPage.Load();

            HomePage _HomePage = _DataSetPickerPage.ChooseDatabase("VPMSER").Load();

            //_HomePage.Load();

        }

        public void standalone_LogOutTest()
        {
            Setup();
            LogOutTest();
            Teardown();
        }

        [Test]
        public void LogOutTest()
        {

            var _LoginPage = new LoginPage(_driver).Load();

            //_LoginPage.Load();

            DataSetPickerPage _DataSetPickerPage = _LoginPage.ValidLogin("onthego@myob.com", "Password1").Load();

            //_DataSetPickerPage.Load();

            HomePage _HomePage = _DataSetPickerPage.ChooseDatabase("VPMSER").Load();

            //_HomePage.Load();

            _LoginPage = _HomePage.LogOut().Load();

            //_LoginPage.Load();

        }

        public void ExecuteTests()
        {
            standalone_LogOutTest();
            standalone_LoginTest();
        }

        public void OpenBrowser(JavaProperties jp)
        {
            if (_driver != null) return;
            switch (jp.GetProperty("browser", "Internet Explorer"))
            {
                case "Chrome":
                    _driver = new ChromeDriver();
                    break;
                case "Firefox":
                    _driver = new FirefoxDriver();
                    break;
                case "Internet Explorer":
                    _driver = new InternetExplorerDriver();
                    break;
                case "Safari":
                    _driver = new SafariDriver();
                    break;
                default:
                    _driver = new InternetExplorerDriver();
                    break;
            }
            if (EntryPoint.Log.IsInfoEnabled) EntryPoint.Log.Info(@"Actual Result:     Success");
        }

        public void NavigateToLoginPage(JavaProperties jp)
        {
            _loginPage = new LoginPage(_driver, jp).Load();
            try
            {
                if (jp.GetProperty("result") == "Success")
                {
                    Assert.AreEqual(_loginPage.GetDriver().Title, @"" + jp.GetProperty("baseURL") + "/Login");
                    if (EntryPoint.Log.IsInfoEnabled) EntryPoint.Log.Info(@"Actual Result:     Success");
                }
                else
                {
                    Assert.AreNotEqual(_loginPage.GetDriver().Title, @"" + jp.GetProperty("baseURL") + "/Login");
                    if (EntryPoint.Log.IsInfoEnabled) EntryPoint.Log.Info(@"Actual Result:     Failure");
                }
            }
            catch (AssertionException)
            {
                if (jp.GetProperty("result") == "Success")
                {
                    if (EntryPoint.Log.IsErrorEnabled) EntryPoint.Log.Error(@"Actual Result:     Failure");
                }
                else
                {
                    if (EntryPoint.Log.IsErrorEnabled) EntryPoint.Log.Error(@"Actual Result:     Success");
                }
            }
            //EntryPoint.Log.Info("Login Page Title: [" + _loginPage.Driver().Title + "]");
        }

        public void EnterValidCredentials(JavaProperties jp)
        {
            _dataSetPickerPage = _loginPage.ValidLogin(jp.GetProperty("userName"), jp.GetProperty("password")).Load();
            try
            {
                if (jp.GetProperty("result") == "Success")
                {
                    Assert.AreEqual(_dataSetPickerPage.GetDriver().Title, @"" + jp.GetProperty("baseURL") + "/Login/DataSetPicker");
                    if (EntryPoint.Log.IsInfoEnabled) EntryPoint.Log.Info(@"Actual Result:     Success");
                }
                else
                {
                    Assert.AreNotEqual(_dataSetPickerPage.GetDriver().Title, @"" + jp.GetProperty("baseURL") + "/Login/DataSetPicker");
                    if (EntryPoint.Log.IsInfoEnabled) EntryPoint.Log.Info(@"Actual Result:     Failure");
                }
            }
            catch (AssertionException)
            {
                if (jp.GetProperty("result") == "Success")
                {
                    if (EntryPoint.Log.IsErrorEnabled) EntryPoint.Log.Error(@"Actual Result:     Failure");
                }
                else
                {
                    if (EntryPoint.Log.IsErrorEnabled) EntryPoint.Log.Error(@"Actual Result:     Success");
                }
            }
        }

        public void EnterInvalidCredentials(JavaProperties jp)
        {
            _loginPage = _loginPage.InvalidLogin(jp.GetProperty("userName"), jp.GetProperty("password")).Load();
            try
            {
                if (jp.GetProperty("result") == "Success")
                {
                    Assert.AreNotEqual(_loginPage.GetDriver().Title, @"" + jp.GetProperty("baseURL") + "/Login");
                    if (EntryPoint.Log.IsInfoEnabled) EntryPoint.Log.Info(@"Actual Result:     Success");
                }
                else
                {
                    Assert.AreEqual(_loginPage.GetDriver().Title, @"" + jp.GetProperty("baseURL") + "/Login");
                    if (EntryPoint.Log.IsInfoEnabled) EntryPoint.Log.Info(@"Actual Result:     Failure");
                }
            }
            catch (AssertionException)
            {
                if (jp.GetProperty("result") == "Success")
                {
                    if (EntryPoint.Log.IsErrorEnabled) EntryPoint.Log.Error(@"Actual Result:     Failure");
                }
                else
                {
                    if (EntryPoint.Log.IsErrorEnabled) EntryPoint.Log.Error(@"Actual Result:     Success");
                }
            }
        }

        public void ChooseDatabase(JavaProperties jp)
        {
            _homePage = _dataSetPickerPage.ChooseDatabase(jp.GetProperty("database")).Load();
            try
            {
                if (jp.GetProperty("result") == "Success")
                {
                    Assert.AreEqual(_homePage.GetDriver().Title, @"Home");
                    if (EntryPoint.Log.IsInfoEnabled) EntryPoint.Log.Info(@"Actual Result:     Success");
                }
                else
                {
                    Assert.AreNotEqual(_homePage.GetDriver().Title, @"Home");
                    if (EntryPoint.Log.IsInfoEnabled) EntryPoint.Log.Info(@"Actual Result:     Failure");
                }
            }
            catch (AssertionException)
            {
                if (jp.GetProperty("result") == "Success")
                {
                    if (EntryPoint.Log.IsErrorEnabled) EntryPoint.Log.Error(@"Actual Result:     Failure");
                }
                else
                {
                    if (EntryPoint.Log.IsErrorEnabled) EntryPoint.Log.Error(@"Actual Result:     Success");
                }
            }
        }

        public void CloseBrowser(JavaProperties jp)
        {
            _driver.Quit();
            _driver = null;
            if (EntryPoint.Log.IsInfoEnabled) EntryPoint.Log.Info(@"Actual Result:     Success");
        }

    }

}
