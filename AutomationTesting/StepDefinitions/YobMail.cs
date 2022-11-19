using OpenQA.Selenium;

namespace AutomationTesting.StepDefinitions
{
    public class YobMail
    {
        public static string GetVerifyCode(IWebDriver driver, string email)
        {
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://www.yopmail.com");
            driver.FindElement(By.XPath("//input[@class='ycptinput']")).SendKeys(email);
            driver.FindElement(By.XPath("//input[@type='submit']/..//i")).Click();
            driver.SwitchTo().Frame("ifmail");
            return driver.FindElement(By.XPath("xpath of the verification code element")).Text;
        }

    }
}
