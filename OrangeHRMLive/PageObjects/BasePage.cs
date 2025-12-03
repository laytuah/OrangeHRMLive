using OpenQA.Selenium;
using OrangeHRMLive.Configuration;

namespace OrangeHRMLive.PageObjects
{
    public class BasePage
    {
        protected IWebDriver Driver { get; }

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
        }

        protected PageElement Mainmenu_item(string itemName) => new PageElement(Driver, By.XPath($"//span[@class='oxd-text oxd-text--span oxd-main-menu-item--name' and normalize-space(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))=\"{itemName}\"]"));
        protected PageElement Button_button(string buttonText, int index = 1) => new PageElement(Driver, By.XPath($"(//button[(@type='button' or @type = 'submit') and normalize-space(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))=\"{buttonText}\"])[{index}]"));
        protected PageElement Select_Dropdown(string dropdownFieldLabel) => new PageElement(Driver, By.XPath($"//div[contains(@class,'oxd-input-group') and contains(normalize-space(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')),\"{dropdownFieldLabel}\")]//div[@class='oxd-select-text--after']"));
        protected PageElement SelectFromDropdownList(string selectText) => new PageElement(Driver, By.XPath($"//div[contains(@class,'oxd-select-dropdown')]//span[contains(normalize-space(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')),\"{selectText}\")]"));
        protected PageElement Link_anchor(string linkText) => new PageElement(Driver, By.XPath($"//div[contains(@class,'orangehrm-tabs-wrapper')]//a[contains(normalize-space(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')),\"{linkText}\")]"));

        public void LoadAUT()
        {
            Driver.Navigate().GoToUrl(ConfigurationManager.Url);
        }
    }
}


