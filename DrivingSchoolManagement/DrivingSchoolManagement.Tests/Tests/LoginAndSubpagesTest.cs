using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace DrivingSchoolManagement.Tests.Tests.Basic
{
    [TestFixture]
    class LoginAndSubpagesTest
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

        [TestCase("jaroslaw.brzeczyszczykiwiecz", "11111111111")]
        public void LoginAndCheckSubpages(string userName, string password)
        {
            FillFormAndClickLogin(userName, password);
            CheckHomePageLayout();
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
        }

        private void CheckHomePageLayout()
        {
            Thread.Sleep(10000);

            var title = browser.Title;
            Assert.AreEqual(title, "Witamy w naszym OSK! - Ośrodek Szkolenia Kierowców");

            var driversButton = browser.FindElement(By.Id("HeaderMenu_DXI1_T"));
            Assert.NotNull(driversButton);
            Assert.True(driversButton.Displayed);
            driversButton.Click();
            Thread.Sleep(2000);
            Assert.AreEqual(browser.Title, "Instruktorzy - Ośrodek Szkolenia Kierowców");

            var managersButton = browser.FindElement(By.Id("HeaderMenu_DXI2_T"));
            Assert.NotNull(managersButton);
            Assert.True(managersButton.Displayed);
            managersButton.Click();
            Thread.Sleep(2000);
            Assert.AreEqual(browser.Title, "Menadżerowie - Ośrodek Szkolenia Kierowców");

            var vehiclesButton = browser.FindElement(By.Id("HeaderMenu_DXI3_T"));
            Assert.NotNull(vehiclesButton);
            Assert.True(vehiclesButton.Displayed);
            vehiclesButton.Click();
            Thread.Sleep(2000);
            Assert.AreEqual(browser.Title, "Pojazdy - Ośrodek Szkolenia Kierowców");

            var accountButton = browser.FindElement(By.Id("HeaderMenu_DXI4_T"));
            Assert.NotNull(accountButton);
            Assert.True(accountButton.Displayed);
            accountButton.Click();
            Thread.Sleep(2000);
            Assert.AreEqual(browser.Title, "Moje dane - Ośrodek Szkolenia Kierowców");
        }

        private void OpenLoginPage()
        {
            browser.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            browser.Manage().Window.Maximize();
            browser.Navigate().GoToUrl(@"http://localhost/DrivingSchoolManagement");

            Assert.AreEqual(browser.Title, "Strefa kursanta - OSK - Ośrodek Szkolenia Kierowców");
        }
    }
}
