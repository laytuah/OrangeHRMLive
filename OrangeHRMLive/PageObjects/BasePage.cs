﻿using OpenQA.Selenium;
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

        protected IWebElement Mainmenu_item(string itemName) => Driver.FindElement(By.XPath($"//span[@class='oxd-text oxd-text--span oxd-main-menu-item--name' and normalize-space(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))=\"{itemName}\"]"));
        protected IWebElement Button_button(string buttonText, int index = 1) => Driver.FindElement(By.XPath($"(//button[(@type='button' or @type = 'submit') and normalize-space(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))=\"{buttonText}\"])[{index}]"));
        protected IWebElement Select_dropdown(string selectText) => Driver.FindElement(By.XPath($"//div[contains(@class,'oxd-select-dropdown')]//span[contains(normalize-space(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')),\"{selectText}\")]"));
        protected IWebElement Link_anchor(string linkText) => Driver.FindElement(By.XPath($"//div[contains(@class,'orangehrm-tabs-wrapper')]//a[contains(normalize-space(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')),\"{linkText}\")]"));

        public void LoadAUT()
        {
            Driver.Navigate().GoToUrl(ConfigurationManager.Url);
        }
    }
}


