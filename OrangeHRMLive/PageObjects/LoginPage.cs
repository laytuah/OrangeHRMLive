using OpenQA.Selenium;
using OrangeHRMLive.Utilities;

namespace OrangeHRMLive.PageObjects
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver) { }

        UIElement UsernameText => new UIElement(Driver, By.XPath("//p[@class='oxd-text oxd-text--p' and contains(.,'Username')]"));
        UIElement UsernameField => new UIElement(Driver, By.XPath("//input[@name='username' and @placeholder='Username']"));
        UIElement PasswordText => new UIElement(Driver, By.XPath("//p[@class='oxd-text oxd-text--p' and contains(.,'Password')]"));
        UIElement PasswordField => new UIElement(Driver, By.XPath("//input[@name='password' and @placeholder='Password']"));
        UIElement LoginButton => new UIElement(Driver, By.XPath("//button[contains(@class,'login-button')]"));

        public void Login()
        {
            UsernameField.EnterText(ExtractText(UsernameText));
            PasswordField.EnterText(ExtractText(PasswordText));
            LoginButton.Click();
        }

        string ExtractText(UIElement element)
        {
            return element.GetTrimmedText().Split(':')[1].Trim();
        }
    }
}
