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
    class EditManagerTest
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

        [TestCase("Jakub", "Piotr")]
        public void EditManager(string firstName, string previousName)
        {
            OpenManagersPage();
            FillEditedManagerData(firstName);
            CheckEditedManagerData(firstName);
            FillEditedManagerData(previousName);
        }

        private void CheckEditedManagerData(string firstName)
        {
            var resultRow = browser.FindElement(By.XPath("//tr[@id='ManagersGrid_DXDataRow0']//td[2]"));
            Thread.Sleep(2000);

            Assert.AreEqual(resultRow.Text, firstName);
        }
        private void FillEditedManagerData(string firstName)
        {
            var editManagerButton = browser.FindElement(By.Id("ManagersGrid_DXCBtn1"));
            editManagerButton.Click();

            var firstNameInput = browser.FindElement(By.Id("ManagersGrid_DXEFL_DXEditor2_I"));
            firstNameInput.Click();
            firstNameInput.Clear();
            firstNameInput.SendKeys(firstName);

            var saveButton = browser.FindElement(By.Id("ManagersGrid_DXEFL_DXCBtn5"));
            saveButton.Click();
            Thread.Sleep(1000);
        }

        private void OpenManagersPage()
        {
            var managersButton = browser.FindElement(By.Id("HeaderMenu_DXI2_T"));
            managersButton.Click();
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
