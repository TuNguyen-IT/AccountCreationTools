using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using AutomationTesting.Services;
using AutomationTesting.Extensions;
using System.Linq;
using OpenQA.Selenium.Support.UI;
using AutomationTesting.StepDefinitions;
using System.Configuration;
using System.IO;

namespace AutomationTesting
{
    class FacebookAction
    {
        IWebDriver driver;

        [SetUp]
        public void startBrowser()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--incognito");
            var driverUrl = new Uri(ConfigurationManager.AppSettings["DriverUrl"]);
            driver = new ChromeDriver(driverUrl.LocalPath);
        }

        [Test]
        public void ExecuteLogin()
        {
            //var accounts = ExcelService.ReadDataFromExcel();
            //if (accounts.Any())
            //{
            //    driver.Url = "https://facebook.com";

            //    foreach (var account in accounts)
            //    {
            //        var email = By.Id("email");
            //        var pass = By.Id("pass");
            //        var btn = By.Name("login");
            //        driver.FindElement(email).SendKeys(account.Email);
            //        driver.FindElement(pass).SendKeys(account.Password);
            //        driver.FindElement(btn).Click();
            //        Common.WaitUntilElementVisible(driver, By.Id(""), 10);
            //    }
            //}
        }

        [Test]
        public void RegisterFacebook()
        {
            try
            {
                driver.Url = ConfigurationManager.AppSettings["FacebookUrl"];
                for (int i = 0; i < 5; i++)
                {
                    Random random = new Random();
                    const string chars = "abcdefghijklmnopqrstuvwxyz";
                    var randomString = new string(Enumerable.Repeat(chars, 6)
                        .Select(s => s[random.Next(s.Length)]).ToArray());
                    var firstNameStr = StringGenerators.GenerateRandomName(4);
                    var surnameStr = StringGenerators.GenerateRandomName(5);
                    var emailStr = randomString + "@gmail.com";

                    var firstNameSelector = By.XPath("//input[@name='firstname']");
                    var surnameSelector = By.XPath("//input[@name='lastname']");
                    var emailSelector = By.XPath("//input[@name='reg_email__']");
                    var emailConfirmSelector = By.XPath("//input[@name='reg_email_confirmation__']");
                    var passwordSelector = By.XPath("//input[@name='reg_passwd__']");
                    var genderSelector = By.XPath("//input[@name='sex']");
                    var birthdayYearSelector = By.Name("birthday_year");
                    var registerBtnSelector = By.XPath("//a[@data-testid='open-registration-form-button']");
                    //Open up the registration pop up
                    driver.FindElement(registerBtnSelector).Click();

                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                    driver.FindElement(firstNameSelector).SendKeys(firstNameStr);
                    driver.FindElement(surnameSelector).SendKeys(surnameStr);
                    driver.FindElement(emailSelector).SendKeys(emailStr);
                    driver.FindElement(emailConfirmSelector).SendKeys(emailStr);
                    var selectElement = new SelectElement(driver.FindElement(birthdayYearSelector));
                    selectElement.SelectByValue("1995");
                    driver.FindElement(passwordSelector).SendKeys("Welcome@1");
                    driver.FindElements(genderSelector).FirstOrDefault().Click();
                    var signUpBtnSelector = By.XPath("//button[normalize-space() = 'Sign Up']");
                    // Submit sign up account
                    driver.FindElement(signUpBtnSelector).Click();

                    // Wait Until account is registed sussessfully
                    Common.WaitUntilElementInVisible(driver, signUpBtnSelector, 10);

                    // Save account information
                    ExcelService.SaveExcel(randomString + "@gmail.com", "Welcome@1");
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
                    
                    // create another account
                    driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["FacebookUrl"]);
                }

                driver.Close();
            }
            catch (Exception ex)
            {
                driver.Close();
                throw ex;
            }
        }

        [Test]
        public void RegisterFacebookFromFile()
        {
            try
            {
                driver.Url = ConfigurationManager.AppSettings["FacebookUrl"];
                var accounts = ExcelService.ReadDataFromExcel();
                if (accounts != null && accounts.Any())
                {
                    foreach (var account in accounts)
                    {
                        Random random = new Random();
                        const string chars = "abcdefghijklmnopqrstuvwxyz";
                        var randomString = new string(Enumerable.Repeat(chars, 6)
                            .Select(s => s[random.Next(s.Length)]).ToArray());
                        var firstNameStr = StringGenerators.GenerateRandomName(4);
                        var surnameStr = StringGenerators.GenerateRandomName(5);
                        var emailStr = randomString + "@gmail.com";

                        var firstNameSelector = By.XPath("//input[@name='firstname']");
                        var surnameSelector = By.XPath("//input[@name='lastname']");
                        var emailSelector = By.XPath("//input[@name='reg_email__']");
                        var emailConfirmSelector = By.XPath("//input[@name='reg_email_confirmation__']");
                        var passwordSelector = By.XPath("//input[@name='reg_passwd__']");
                        var genderSelector = By.XPath("//input[@name='sex']");
                        var birthdayYearSelector = By.Name("birthday_year");
                        var registerBtnSelector = By.XPath("//a[@data-testid='open-registration-form-button']");
                        //Open up the registration pop up
                        driver.FindElement(registerBtnSelector).Click();

                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
                        driver.FindElement(firstNameSelector).SendKeys(firstNameStr);
                        driver.FindElement(surnameSelector).SendKeys(surnameStr);
                        driver.FindElement(emailSelector).SendKeys(account.Email);
                        driver.FindElement(emailConfirmSelector).SendKeys(account.Email);
                        var selectElement = new SelectElement(driver.FindElement(birthdayYearSelector));
                        selectElement.SelectByValue("1995");
                        driver.FindElement(passwordSelector).SendKeys(account.Password);
                        driver.FindElements(genderSelector).FirstOrDefault().Click();
                        var signUpBtnSelector = By.XPath("//button[normalize-space() = 'Sign Up']");
                        // Submit sign up account
                        //driver.FindElement(signUpBtnSelector).Click();

                        ExcelService.SaveExcel(account.Email, account.Password);
                    }
                }

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            }
            catch (Exception ex)
            {
                driver.Close();
                driver.Quit();
                throw ex;
            }
        }

        [Test]
        public void RegisterGmail()
        {
            // Has not worked yet
            driver.Url = "https://accounts.google.com/signup/v2/webcreateaccount?continue=https%3A%2F%2Faccounts.google.com%2F&dsh=S672011053%3A1648223008861086&biz=false&flowName=GlifWebSignIn&flowEntry=SignUp";
            Gmail.FillRegisterInformation(driver);
        }

        [TearDown]
        public void closeBrowser()
        {
            driver.Close();
            driver.Quit();
        }




    }
}
