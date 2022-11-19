using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationTesting.StepDefinitions
{
    public class Gmail
    {
        public static void FillRegisterInformation(IWebDriver driver)
        {
            var lastName = By.Id("lastName");
            var firstName = By.Id("firstName");
            var username = By.Id("username");
            var pw = By.CssSelector("#passwd > div.aCsJod.oJeWuf > div > div.Xb9hP > input");
            var confirmPw = By.CssSelector("#confirm-passwd > div.aCsJod.oJeWuf > div > div.Xb9hP > input");
            var nextBtn = By.CssSelector("#accountDetailsNext > div > button");

            driver.FindElement(lastName).SendKeys("John");
            driver.FindElement(firstName).SendKeys("Cena");
            driver.FindElement(username).SendKeys("johnfbat2");
            driver.FindElement(pw).SendKeys("Welcome@1");
            driver.FindElement(confirmPw).SendKeys("Welcome@1");
            driver.FindElement(nextBtn).Click();
           
            var nextBtnPage2 = By.CssSelector("#view_container > div > div > div.pwWryf.bxPAYd > div > div.zQJV3 > div > div.qhFLie > div > div > button");
            Common.WaitUntilElementVisible(driver, nextBtnPage2, 10);
            driver.FindElement(nextBtnPage2).Click();

        }

    }
}
