using Kajabity.Tools.Java;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

namespace InductionExercise
{
    [TestFixture]
    public class ExecuteTests
    {

        private IWebDriver _driver;
        private VehiclePage _vehiclePage;

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

        public void NavigateToVehiclePage(JavaProperties jp)
        {
            _vehiclePage = new VehiclePage(_driver, jp).Load();
            try
            {
                if (jp.GetProperty("result") == "Success")
                {
                    Assert.AreEqual(_vehiclePage.GetDriver().Title, @"Vehicles - My ASP.NET MVC Application");
                    if (EntryPoint.Log.IsInfoEnabled) EntryPoint.Log.Info(@"Actual Result:     Success");
                }
                else
                {
                    Assert.AreNotEqual(_vehiclePage.GetDriver().Title, @"Vehicles - My ASP.NET MVC Application");
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

        public void CloseBrowser(JavaProperties jp)
        {
            _driver.Quit();
            _driver = null;
            if (EntryPoint.Log.IsInfoEnabled) EntryPoint.Log.Info(@"Actual Result:     Success");
        }

    }

}
