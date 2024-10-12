using OpenQA.Selenium;

namespace OrangeHRMLive.PageObjects
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver) { }

        PageElement UsernameText => new PageElement(Driver, By.XPath("//p[@class='oxd-text oxd-text--p' and contains(.,'Username')]"));
        PageElement UsernameField => new PageElement(Driver, By.XPath("//input[@name='username' and @placeholder='Username']"));
        PageElement PasswordText => new PageElement(Driver, By.XPath("//p[@class='oxd-text oxd-text--p' and contains(.,'Password')]"));
        PageElement PasswordField => new PageElement(Driver, By.XPath("//input[@name='password' and @placeholder='Password']"));
        PageElement LoginButton => new PageElement(Driver, By.XPath("//button[contains(@class,'login-button')]"));

        public void Login()
        {
            UsernameField.ClearAndSendKeys(ExtractText(UsernameText));
            PasswordField.ClearAndSendKeys(ExtractText(PasswordText));
            LoginButton.Click();
        }

        string ExtractText(PageElement element)
        {
            return element.Text.Split(':')[1].Trim();
        }
    }
}
