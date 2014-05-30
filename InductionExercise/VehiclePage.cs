using System;
using System.Collections.Generic;
using System.Linq;
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

        public CreatePage CreateNew(JavaProperties jp)
        {
            hrefCreate.Click();
            return new CreatePage(_driver, jp);
        }

        public VehiclePage Delete(JavaProperties jp)
        {
            List<IWebElement> fields = _driver.FindElements(By.PartialLinkText("Delete")).ToList();
            var href = jp.GetProperty("baseURL") + "/Vehicle/Delete/" + jp.GetProperty("VehicleId");
            foreach (IWebElement field in fields)
            {
                if (field.GetAttribute("href").Equals(href) != true) continue;
                field.Click();
                break;
            }
            return new VehiclePage(_driver, jp);
        }

        public VehiclePage Delete_Last(JavaProperties jp, bool confirm)
        {
            var field = _driver.FindElements(By.PartialLinkText("Delete")).ToList().Last();
            field.Click();
            IAlert alert = _driver.SwitchTo().Alert();
            if (confirm)
            {
                alert.Accept();
                return new VehiclePage(_driver, jp);
            }
            else
            {
                alert.Dismiss();
                return this;
            }
        }

        public DetailsPage Details(JavaProperties jp)
        {
            List<IWebElement> fields = _driver.FindElements(By.PartialLinkText("Details")).ToList();
            var href = jp.GetProperty("baseURL") + "/Vehicle/Details/" + jp.GetProperty("VehicleId");
            foreach (IWebElement field in fields)
            {
                if (field.GetAttribute("href").Equals(href) != true) continue;
                field.Click();
                break;
            }
            return new DetailsPage(_driver, jp);
        }

        public DetailsPage Details_Last(JavaProperties jp)
        {
            var field = _driver.FindElements(By.PartialLinkText("Details")).ToList().Last();
            field.Click();
            return new DetailsPage(_driver, jp);
        }

        public EditPage Edit(JavaProperties jp)
        {
            List<IWebElement> fields = _driver.FindElements(By.LinkText("Edit")).ToList();
            var href = jp.GetProperty("baseURL") + "/Vehicle/Edit/" + jp.GetProperty("VehicleId");
            foreach (IWebElement field in fields.Where(field => field.GetAttribute("href").Equals(href) == true))
            {
                field.Click();
                break;
            }
            return new EditPage(_driver, jp);
        }

        public EditPage Edit_Last(JavaProperties jp)
        {
            var field = _driver.FindElements(By.PartialLinkText("Edit")).ToList().Last();
            field.Click();
            return new EditPage(_driver, jp);
        }

    }

}
