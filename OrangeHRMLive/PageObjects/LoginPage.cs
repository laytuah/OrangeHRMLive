using OpenQA.Selenium;

namespace OrangeHRMLive.PageObjects
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver _driver) : base(_driver)
        {
            driver = _driver;
        }
        protected IWebElement UsernameText => driver.FindElement(By.XPath("//p[@class='oxd-text oxd-text--p' and contains(.,'Username')]"));
        protected IWebElement UsernameField => driver.FindElement(By.XPath("//input[@name='username' and @placeholder='Username']"));
        protected IWebElement PasswordText => driver.FindElement(By.XPath("//p[@class='oxd-text oxd-text--p' and contains(.,'Password')]"));
        protected IWebElement PasswordField => driver.FindElement(By.XPath("//input[@name='password' and @placeholder='Password']"));
        protected IWebElement LoginButton => driver.FindElement(By.XPath("//button[contains(@class,'login-button')]"));

        public void Login()
        {
            UsernameField.Clear();
            UsernameField.SendKeys((UsernameText.Text.Split(':'))[1].Trim());
            PasswordField.Clear();
            PasswordField.SendKeys((PasswordText.Text.Split(':'))[1].Trim());
            LoginButton.Click();
        }
    }
}
