using AutomationTesting.Extensions;
using AutomationTesting.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Security;

namespace AutomationTesting.StepDefinitions
{
    public class Facebook
    {
        public static void SignUp(IWebDriver driver, string email, string password)
        {
            var firstNameStr = StringGenerators.GenerateRandomName(4);
            var surnameStr = StringGenerators.GenerateRandomName(5);

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
            driver.FindElement(emailSelector).SendKeys(email);
            driver.FindElement(emailConfirmSelector).SendKeys(email);
            var selectElement = new SelectElement(driver.FindElement(birthdayYearSelector));
            selectElement.SelectByValue("1995");
            driver.FindElement(passwordSelector).SendKeys("Welcome@1");
            driver.FindElements(genderSelector).FirstOrDefault().Click();
            var signUpBtnSelector = By.XPath("//button[normalize-space() = 'Sign Up']");
            var confirmTextSelector = By.XPath("//span[normalize-space() = 'We need more information']");
            // Submit sign up account
            driver.FindElement(signUpBtnSelector).Click();

            // Wait Until account is registed sussessfully
            Common.WaitUntilElementInVisible(driver, signUpBtnSelector, 10);

            // Save account information
            if (Common.IsElementPresent(driver, confirmTextSelector))
            {
                ExcelService.SaveExcel(email, password);
            }
            else
            {
                ExcelService.SaveExcel("N/A", "N/A");
            }

            //Clear all cookies before navigating to new window
            driver.Manage().Cookies.DeleteAllCookies();

            // create another account
            driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["FacebookUrl"]);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        }

    }
}
