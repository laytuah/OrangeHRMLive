using OpenQA.Selenium;
using OrangeHRMLive.Utilities;

namespace OrangeHRMLive.PageObjects
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver) { }

        IWebElement UsernameText => Driver.FindElement(By.XPath("//p[@class='oxd-text oxd-text--p' and contains(.,'Username')]"));
        IWebElement UsernameField => Driver.FindElement(By.XPath("//input[@name='username' and @placeholder='Username']"));
        IWebElement PasswordText => Driver.FindElement(By.XPath("//p[@class='oxd-text oxd-text--p' and contains(.,'Password')]"));
        IWebElement PasswordField => Driver.FindElement(By.XPath("//input[@name='password' and @placeholder='Password']"));
        IWebElement LoginButton => Driver.FindElement(By.XPath("//button[contains(@class,'login-button')]"));

        public void Login()
        {
            UsernameField.EnterText(ExtractText(UsernameText));
            PasswordField.EnterText(ExtractText(PasswordText));
            LoginButton.Click();
        }

        string ExtractText(IWebElement element)
        {
            return element.Text.Split(':')[1].Trim();
        }
    }
}
