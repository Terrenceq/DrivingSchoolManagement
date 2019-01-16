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
    class AddNewVehicleTest
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

        [TestCase("Audi", "A8", "19.05.2015", "50000", "Samochód osobowy", "B", "Good car!")]
        public void AddNewVehicle(string brand, string model, string dateOfProduction, string mileage, string vehicleType, 
            string category, string additionalInfo)
        {
            OpenVehiclesPage();
            FillNewVehicleData(brand, model, dateOfProduction, mileage, vehicleType, category, additionalInfo);
            CheckAddedVehicleData(model);
            DeleteAddedVehicle();
        }

        private void DeleteAddedVehicle()
        {
            var deleteButton = browser.FindElement(By.Id("VehiclesGrid_DXCBtn2"));
            deleteButton.Click();
            Thread.Sleep(1000);

            var result = browser.FindElement(By.Id("VehiclesGrid_DXEmptyRow"));
            Assert.NotNull(result);
        }
        private void CheckAddedVehicleData(string model)
        {
            var modelFilter = browser.FindElement(By.Id("VehiclesGrid_DXFREditorcol3_I"));
            modelFilter.Click();
            Thread.Sleep(2000);
            modelFilter.SendKeys(model);
            Thread.Sleep(2000);

            var result = browser.FindElement(By.Id("VehiclesGrid_DXDataRow0"));

            Assert.NotNull(result);
        }
        private void FillNewVehicleData(string brand, string model, string dateOfProduction, string mileage, string vehicleType,
            string category, string additionalInfo)
        {
            var addNewVehicleButton = browser.FindElement(By.Id("VehiclesGrid_DXCBtn0"));
            addNewVehicleButton.Click();

            var brandInput = browser.FindElement(By.Id("VehiclesGrid_DXEFL_DXEditor2_I"));
            brandInput.Click();
            brandInput.SendKeys(brand);

            var modelInput = browser.FindElement(By.Id("VehiclesGrid_DXEFL_DXEditor3_I"));
            modelInput.Click();
            modelInput.SendKeys(model);

            var dateOfProductionInput = browser.FindElement(By.Id("VehiclesGrid_DXEFL_DXEditor4_I"));
            dateOfProductionInput.Click();
            dateOfProductionInput.SendKeys(dateOfProduction);

            var mileageInput = browser.FindElement(By.Id("VehiclesGrid_DXEFL_DXEditor5_I"));
            mileageInput.Click();
            mileageInput.SendKeys(mileage);

            var vehicleTypeInput = browser.FindElement(By.Id("VehiclesGrid_DXEFL_DXEditor6_I"));
            vehicleTypeInput.Click();
            vehicleTypeInput.SendKeys(vehicleType);

            var categoryInput = browser.FindElement(By.Id("VehiclesGrid_DXEFL_DXEditor7_I"));
            categoryInput.Click();
            categoryInput.SendKeys(category);

            var additionalInfoInput = browser.FindElement(By.Id("VehiclesGrid_DXEFL_DXEditor8_I"));
            additionalInfoInput.Click();
            additionalInfoInput.SendKeys(additionalInfo);  

            var saveButton = browser.FindElement(By.Id("VehiclesGrid_DXEFL_DXCBtn9"));
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
