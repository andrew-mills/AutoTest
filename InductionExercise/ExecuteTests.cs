using System;
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
        private CreatePage _createPage;
        private DetailsPage _detailsPage;
        private EditPage _editPage;
        private VehiclePage _vehiclePage;

        private void AssertAreEqual(JavaProperties jp, string title)
        {
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
        }

        //
        // Browser
        //

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

        public void CloseBrowser(JavaProperties jp)
        {
            _driver.Quit();
            _driver = null;
            if (EntryPoint.Log.IsInfoEnabled) EntryPoint.Log.Info(@"Actual Result:     Success");
        }

        //
        // Vehicle
        //

        public void GetVehicleList(JavaProperties jp)
        {
            _vehiclePage = new VehiclePage(_driver, jp).Load();
            AssertAreEqual(jp, @"Vehicles - My ASP.NET MVC Application");
        }

        public void CreateNewVehicle(JavaProperties jp)
        {
            _createPage = _vehiclePage.CreateNew(jp).Load();
            AssertAreEqual(jp, @"Add Vehicle - My ASP.NET MVC Application");
        }

        public void GetDetailsForLastVehicle(JavaProperties jp)
        {
            _detailsPage = _vehiclePage.Details_Last(jp).Load();
            AssertAreEqual(jp, @"Vehicle Details - My ASP.NET MVC Application");
        }

        public void GetDetailsForSpecificVehicle(JavaProperties jp)
        {
            // TODO
        }

        public void EditLastVehicle(JavaProperties jp)
        {
            _editPage = _vehiclePage.Edit_Last(jp).Load();
            AssertAreEqual(jp, @"Edit Vehicle - My ASP.NET MVC Application");
        }

        public void EditSpecificVehicle(JavaProperties jp)
        {
            _editPage = _vehiclePage.Edit(jp).Load();
            AssertAreEqual(jp, @"Edit Vehicle - My ASP.NET MVC Application");
        }

        //
        // Create
        //

        public void SaveNewVehicle(JavaProperties jp)
        {
            _vehiclePage = _createPage.Create(jp).Load();
            AssertAreEqual(jp, @"Vehicles - My ASP.NET MVC Application");
        }

        public void CancelNewVehicle(JavaProperties jp)
        {
            _vehiclePage = _createPage.BackToList().Load();
            AssertAreEqual(jp, @"Vehicles - My ASP.NET MVC Application");
        }

        //
        // Edit
        //

        public void EditVehicle(JavaProperties jp)
        {
            _vehiclePage = _editPage.Save(jp).Load();
            AssertAreEqual(jp, @"Vehicles - My ASP.NET MVC Application");
        }

        public void CancelEditVehicle(JavaProperties jp)
        {
            _vehiclePage = _editPage.BackToList(jp).Load();
            AssertAreEqual(jp, @"Vehicles - My ASP.NET MVC Application");
        }

        //
        // Details
        //

        public void ReturnFromDetailsPage(JavaProperties jp)
        {
            _vehiclePage = _detailsPage.BackToList(jp).Load();
            AssertAreEqual(jp, @"Vehicles - My ASP.NET MVC Application");
        }

        public void EditVehicleFromDetailsPage(JavaProperties jp)
        {
            _editPage = _detailsPage.Edit(jp).Load();
            AssertAreEqual(jp, @"Edit Vehicle - My ASP.NET MVC Application");
        }

        //
        // Delete
        //

        public void DeleteLastVehicle(JavaProperties jp, bool confirm)
        {
            _vehiclePage = _vehiclePage.Delete_Last(jp, confirm).Load();
            AssertAreEqual(jp, @"Vehicles - My ASP.NET MVC Application");
        }

    }

}
