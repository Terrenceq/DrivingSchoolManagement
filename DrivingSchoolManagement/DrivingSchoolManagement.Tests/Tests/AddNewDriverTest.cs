﻿using NUnit.Framework;
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
    class AddNewDriverTest
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

        [TestCase("Piotr", "TestAutomatyczny", "19.05.1990", "95020110687", "pioteeertesteeer@gmail.com", "To jest tylko test!", "B", "licenceTest", "Testowa", "3", 
            "Pomlewo", "83047", "Polska")]
        public void AddNewDriver(string firstName, string lastName, string dateOfBirth, string PESEL, string eMail, 
            string additionalInfo, string category, string licenceNumber, string streetName, string houseNumber, string city,
            string postalCode, string country)
        {
            OpenDriversPage();
            FillNewDriverData(firstName, lastName, dateOfBirth, PESEL, eMail, additionalInfo, category, licenceNumber, streetName, houseNumber, city, postalCode, country);
            CheckAddedDriverData(eMail);
            DeleteAddedUser();
        }

        private void DeleteAddedUser()
        {
            var deleteButon = browser.FindElement(By.Id("DriversGrid_DXCBtn2"));
            deleteButon.Click();
            Thread.Sleep(1000);

            var result = browser.FindElement(By.Id("DriversGrid_DXEmptyRow"));
            Assert.NotNull(result);
        }
        private void CheckAddedDriverData(string email)
        {
            var emailFilter = browser.FindElement(By.Id("DriversGrid_DXFREditorcol5_I"));
            emailFilter.Click();
            Thread.Sleep(2000);
            emailFilter.SendKeys(email);
            Thread.Sleep(2000);

            var result = browser.FindElement(By.Id("DriversGrid_DXDataRow0"));

            Assert.NotNull(result);
        }
        private void FillNewDriverData(string firstName, string lastName, string dateOfBirth, string PESEL, string eMail,
            string additionalInfo, string category, string licenceNumber, string streetName, string houseNumber, string city,
            string postalCode, string country)
        {
            var addNewDriverButton = browser.FindElement(By.Id("DriversGrid_DXCBtn0"));
            addNewDriverButton.Click();

            var firstNameInput = browser.FindElement(By.Id("DriversGrid_DXEFL_DXEditor2_I"));
            firstNameInput.Click();
            firstNameInput.SendKeys(firstName);

            var lastNameInput = browser.FindElement(By.Id("DriversGrid_DXEFL_DXEditor3_I"));
            lastNameInput.Click();
            lastNameInput.SendKeys(lastName);

            var dateOfBirthInput = browser.FindElement(By.Id("DriversGrid_DXEFL_DXEditor4_I"));
            dateOfBirthInput.Click();
            dateOfBirthInput.SendKeys(dateOfBirth);

            var peselInput = browser.FindElement(By.Id("DriversGrid_DXEFL_DXEditor9_I"));
            peselInput.Click();
            Thread.Sleep(2000);
            peselInput.SendKeys(PESEL);

            var emailInput = browser.FindElement(By.Id("DriversGrid_DXEFL_DXEditor5_I"));
            emailInput.Click();
            emailInput.SendKeys(eMail);

            var additionalInfoInput = browser.FindElement(By.Id("DriversGrid_DXEFL_DXEditor8_I"));
            additionalInfoInput.Click();
            additionalInfoInput.SendKeys(additionalInfo);

            var categoryInput = browser.FindElement(By.Id("DriversGrid_DXEFL_DXEditor7_I"));
            categoryInput.Click();
            categoryInput.SendKeys(category);

            var licenceNumberInput = browser.FindElement(By.Id("DriversGrid_DXEFL_DXEditor10_I"));
            licenceNumberInput.Click();
            licenceNumberInput.SendKeys(licenceNumber);

            var streetNameInput = browser.FindElement(By.Id("DriversGrid_DXEFL_DXEditor11_I"));
            streetNameInput.Click();
            streetNameInput.SendKeys(streetName);

            var houseNumberInput = browser.FindElement(By.Id("DriversGrid_DXEFL_DXEditor12_I"));
            houseNumberInput.Click();
            houseNumberInput.SendKeys(houseNumber);

            var cityInput = browser.FindElement(By.Id("DriversGrid_DXEFL_DXEditor13_I"));
            cityInput.Click();
            cityInput.SendKeys(city);

            var postalCodeInput = browser.FindElement(By.Id("DriversGrid_DXEFL_DXEditor14_I"));
            postalCodeInput.Click();
            Thread.Sleep(2000);
            postalCodeInput.SendKeys(postalCode);

            var countryInput = browser.FindElement(By.Id("DriversGrid_DXEFL_DXEditor15_I"));
            countryInput.Click();
            countryInput.SendKeys(country);

            var saveButton = browser.FindElement(By.Id("DriversGrid_DXEFL_DXCBtn11"));
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
