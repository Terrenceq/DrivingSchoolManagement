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
    class EditDriverTest
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

        [TestCase("Krystian", "Jarosław")]
        public void EditDriver(string firstName, string previousName)
        {
            OpenDriversPage();
            FillEditedDriverData(firstName);
            CheckEditedDriverData(firstName);
            FillEditedDriverData(previousName);
        }

        private void CheckEditedDriverData(string firstName)
        {
            var resultRow = browser.FindElement(By.XPath("//tr[@id='DriversGrid_DXDataRow0']//td[2]"));
            Thread.Sleep(2000);

            Assert.AreEqual(resultRow.Text, firstName);
        }
        private void FillEditedDriverData(string firstName)
        {
            var editDriverButton = browser.FindElement(By.Id("DriversGrid_DXCBtn1"));
            editDriverButton.Click();

            var firstNameInput = browser.FindElement(By.Id("DriversGrid_DXEFL_DXEditor2_I"));
            firstNameInput.Click();
            firstNameInput.Clear();
            firstNameInput.SendKeys(firstName);

            var saveButton = browser.FindElement(By.Id("DriversGrid_DXEFL_DXCBtn9"));
            saveButton.Click();
            Thread.Sleep(1000);
        }

        private void OpenDriversPage()
        {
            var driversButton = browser.FindElement(By.Id("HeaderMenu_DXI1_T"));
            driversButton.Click();
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
