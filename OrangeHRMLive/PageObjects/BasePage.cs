﻿using OpenQA.Selenium;
using OrangeHRMLive.Configuration;

namespace OrangeHRMLive.PageObjects
{
    public class BasePage
    {
        protected CustomWebDriver Driver { get; }

        public BasePage(CustomWebDriver driver)
        {
            Driver = driver;
        }

        protected IWebElement Mainmenu_item(string itemName) => Driver.FindElement(By.XPath($"//span[@class='oxd-text oxd-text--span oxd-main-menu-item--name' and normalize-space(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))=\"{itemName}\"]"));
        protected IWebElement Button_button(string buttonText) => Driver.FindElement(By.XPath($"//button[(@type='button' or @type = 'submit') and normalize-space(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))=\"{buttonText}\"]"));
        protected IWebElement Select_dropdown(string selectText) => Driver.FindElement(By.XPath($"//div[contains(@class,'oxd-select-dropdown')]//span[contains(normalize-space(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')),\"{selectText}\")]"));

        

        public void LoadAUT()
        {
            Driver.Navigate().GoToUrl(ConfigurationManager.Url);
        }

        public void ScrollToElement(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }
    }
}


