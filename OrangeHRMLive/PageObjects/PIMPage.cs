﻿using OpenQA.Selenium;
using OrangeHRMLive.Utilities;

namespace OrangeHRMLive.PageObjects
{
    public class PIMPage : BasePage
    {
        public PIMPage(IWebDriver driver): base(driver) { }

        protected IWebElement TextField(string text) => Driver.FindElement(By.XPath($"//input[normalize-space(translate(@placeholder, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))=\"{text}\"] | //div[label[translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=\"{text}\"]]/following-sibling::div//input"));

        public void RegisterNewEmployee()
        {
            Mainmenu_item("PIM").Click();
            Button_button("add").Click();
            TextField("first name").SendKeys(DataGenerator.GenerateRandomString());
            TextField("middle name").SendKeys(DataGenerator.GenerateRandomString());
            TextField("last name").SendKeys(DataGenerator.GenerateRandomString());
            Button_button("save").Click();
            TextField("driver's license number").SendKeys(DataGenerator.GenerateRandomAlphanumerics());
        }
    }
}
