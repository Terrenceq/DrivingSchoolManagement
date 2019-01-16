using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DrivingSchoolManagement.Tests.Tests
{
    [TestFixture]
    class EditVehicleTest
    {
        private IWebDriver browser;

        [SetUp]
        public void Init()
        {
            browser = new ChromeDriver();
            OpenLoginPage();
        }

        [TearDown]
        public void Dispose()
        {
            browser.Close();
            browser.Dispose();
        }

        [TestCase("255000", "250000")]
        public void EditVehicle(string mileage, string previousMileage)
        {
            OpenVehiclesPage();
            FillEditedVehicleData(mileage);
            CheckEditedVehicleData(mileage);
            FillEditedVehicleData(previousMileage);
        }

        private void CheckEditedVehicleData(string model)
        {
            var resultRow = browser.FindElement(By.XPath("//tr[@id='VehiclesGrid_DXDataRow0']//td[5]"));
            Thread.Sleep(2000);

            Assert.AreEqual(resultRow.Text, model);
        }
        private void FillEditedVehicleData(string mileage)
        {
            var editVehicleButton = browser.FindElement(By.Id("VehiclesGrid_DXCBtn1"));
            editVehicleButton.Click();

            var mileageInput = browser.FindElement(By.Id("VehiclesGrid_DXEFL_DXEditor5_I"));
            mileageInput.Click();
            mileageInput.Clear();
            mileageInput.SendKeys(mileage);

            var saveButton = browser.FindElement(By.Id("VehiclesGrid_DXEFL_DXCBtn7"));
            saveButton.Click();
            Thread.Sleep(1000);
        }

        private void OpenVehiclesPage()
        {
            var vehiclesButton = browser.FindElement(By.Id("HeaderMenu_DXI3_T"));
            vehiclesButton.Click();
            Thread.Sleep(2000);
        }

        private void OpenLoginPage()
        {
            browser.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            browser.Manage().Window.Maximize();
            browser.Navigate().GoToUrl(@"http://localhost/DrivingSchoolManagement");

            FillFormAndClickLogin("piotr.oleskow", "zaq1@WSX");
        }

        private void FillFormAndClickLogin(string userName, string password)
        {
            var loginInput = browser.FindElement(By.Id("Login_I"));
            loginInput.Click();
            loginInput.SendKeys(userName);

            var passwordInput = browser.FindElement(By.Id("Password_I"));
            passwordInput.Click();
            passwordInput.SendKeys(password);

            var loginButton = browser.FindElement(By.Id("LoginButton"));
            loginButton.Click();

            Thread.Sleep(5000);
        }
    }
}
