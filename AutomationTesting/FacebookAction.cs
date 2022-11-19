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
using System.Reflection;
using System.Web.Security;

namespace AutomationTesting
{
    class FacebookAction
    {
        IWebDriver driver;
        private int BatchSize { get; set; }

        [SetUp]
        public void StartBrowser()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--incognito");
            BatchSize = ConfigurationManager.AppSettings["BatchSize"] != null ? int.Parse(ConfigurationManager.AppSettings["BatchSize"]) : 5;
            string driverUrl = Directory.GetParent(Assembly.GetExecutingAssembly().Location).Parent.Parent.FullName + "\\Driver";
            driver = new ChromeDriver(driverUrl);
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
                for (int i = 0; i < BatchSize; i++)
                {
                    var emailStr = StringGenerators.GenerateRandomEmail(6);
                    var passwordStr = Membership.GeneratePassword(8, 2);
                    Facebook.SignUp(driver, emailStr, passwordStr);
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
                    // TODO: take account using batch size
                    foreach (var account in accounts)
                    {
                        Facebook.SignUp(driver, account.Email, account.Password);
                    }
                }
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
        public void CloseBrowser()
        {
            driver.Close();
            driver.Quit();
        }




    }
}
