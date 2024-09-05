//using OpenQA.Selenium;
//using OpenQA.Selenium.Interactions;
//using OpenQA.Selenium.Support.UI;
//using OrangeHRMLive.Configuration;
//using SeleniumExtras.WaitHelpers;

//namespace OrangeHRMLive.Utilities
//{
//    public static class WebElementExtensions
//    {
//        static IWebDriver _driver;
//        public static void InitializeDriver(IWebDriver driver)
//        {
//            _driver = driver;
//        }

//        public static void ClickElement(this IWebElement element)
//        {
//            if (element != null && element.Displayed && element.Enabled)
//            {
//                element.Click();
//                WaitForLoadingIconToDisappear();
//            }
//            else
//                throw new ElementNotInteractableException("The element is not interactable.");
//        }

//        public static void EnterText(this IWebElement element, string text)
//        {
//            if (element != null && element.Displayed && element.Enabled)
//            {
//                element.Clear();
//                element.SendKeys(text);
//            }
//            else
//                throw new ElementNotInteractableException("The element is not interactable.");
//        }

//        public static string GetTrimmedText(this IWebElement element)
//        {
//            return element.Text.Trim();
//        }

//        public static bool ElementExits(this By locator)
//        {
//            try
//            {
//                return _driver.FindElements(locator).Count() > 0;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }

//        public static void ScrollAndClick(this IWebElement element)
//        {
//            if (element != null && element.Displayed && element.Enabled)
//            {
//                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
//                element.Click();
//            }
//            else
//                throw new ElementNotInteractableException("The element is not interactable.");
//        }

//        public static void WaitForVisibility(this IWebElement element, TimeSpan timeout)
//        {
//            var wait = new WebDriverWait(_driver, timeout);
//            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(GetXPath(element))));
//        }

//        public static void WaitForClickability(this IWebElement element, TimeSpan timeout)
//        {
//            var wait = new WebDriverWait(_driver, timeout);
//            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(GetXPath(element))));
//        }

//        public static string GetAttributeOrDefault(this IWebElement element, string attribute, string defaultValue = "")
//        {
//            return element?.GetAttribute(attribute) ?? defaultValue;
//        }

//        public static void Hover(this IWebElement element)
//        {
//            Actions actions = new Actions(_driver);
//            actions.MoveToElement(element).Perform();
//        }

//        public static bool ContainsText(this IWebElement element, string text)
//        {
//            return element?.Text.Contains(text) ?? false;
//        }

//        public static bool IsChecked(this IWebElement element)
//        {
//            return element.Selected;
//        }

//        public static void DoubleClick(this IWebElement element)
//        {
//            Actions actions = new Actions(_driver);
//            actions.DoubleClick(element).Perform();
//        }

//        public static void RightClick(this IWebElement element)
//        {
//            Actions actions = new Actions(_driver);
//            actions.ContextClick(element).Perform();
//        }

//        public static void DragAndDrop(this IWebElement source, IWebElement target)
//        {
//            Actions actions = new Actions(_driver);
//            actions.DragAndDrop(source, target).Perform();
//        }

//        public static string GetInnerHtml(this IWebElement element)
//        {
//            return element.GetAttribute("innerHTML");
//        }

//        public static string GetOuterHtml(this IWebElement element)
//        {
//            return element.GetAttribute("outerHTML");
//        }

//        public static void SetCheckbox(this IWebElement element, bool check)
//        {
//            if (element.Selected != check)
//                element.Click();
//            else
//                throw new Exception("Unable to click checkbox");
//        }

//        public static void SelectByText(this IWebElement element, string text)
//        {
//            var selectElement = new SelectElement(element);
//            selectElement.SelectByText(text);
//        }

//        public static void SelectByValue(this IWebElement element, string value)
//        {
//            var selectElement = new SelectElement(element);
//            selectElement.SelectByValue(value);
//        }

//        public static void SelectByIndex(this IWebElement element, int index)
//        {
//            var selectElement = new SelectElement(element);
//            selectElement.SelectByIndex(index);
//        }

//        public static void DeselectAll(this IWebElement element)
//        {
//            var selectElement = new SelectElement(element);
//            selectElement.DeselectAll();
//        }

//        public static void EnterTextWithDelay(this IWebElement element, string text, int delayInMilliseconds = 100)
//        {
//            element.Clear();
//            foreach (char c in text)
//            {
//                element.SendKeys(c.ToString());
//                Thread.Sleep(delayInMilliseconds);
//            }
//        }

//        public static string GetCssValue(this IWebElement element, string propertyName)
//        {
//            return element.GetCssValue(propertyName);
//        }

//        public static void SubmitForm(this IWebElement element)
//        {
//            element.Submit();
//        }

//        public static bool IsDisplayed(this IWebElement element)
//        {
//            try
//            {
//                return element != null && element.Displayed;
//            }
//            catch (NoSuchElementException)
//            {
//                return false;
//            }
//            catch (StaleElementReferenceException)
//            {
//                return false;
//            }
//        }

//        public static IWebElement? FindElementWithRetry(this IWebElement locator, int retryCount = 3, int delayInSeconds = 1)
//        {
//            IWebElement? element = null;
//            for (int i = 0; i < retryCount; i++)
//            {
//                try
//                {
//                    element = locator;
//                    break;
//                }
//                catch (NoSuchElementException)
//                {
//                    if (i == retryCount - 1)
//                        throw;
//                    Thread.Sleep(TimeSpan.FromSeconds(delayInSeconds));
//                }
//            }
//            return element;
//        }

//        static void WaitForLoadingIconToDisappear()
//        {
//            var loadingElements = _driver.FindElements(By.XPath(ConfigurationManager.LoadingIconXpath));
//            if (loadingElements.Count > 0 && loadingElements[0].Displayed)
//            {
//                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
//                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(ConfigurationManager.LoadingIconXpath)));
//            }
//        }

//        static string GetXPath(IWebElement element)
//        {
//            return (string)((IJavaScriptExecutor)_driver).ExecuteScript(
//                "function getXPath(node) {" +
//                "  if (node.id !== '') {" +
//                "    return '//' + node.tagName.toLowerCase() + '[@id=\"' + node.id + '\"]';" +
//                "  }" +
//                "  if (node === document.body) {" +
//                "    return '//html/' + node.tagName.toLowerCase();" +
//                "  }" +
//                "  var index = 0;" +
//                "  var siblings = node.parentNode.childNodes;" +
//                "  for (var i = 0; i < siblings.length; i++) {" +
//                "    var sibling = siblings[i];" +
//                "    if (sibling === node) {" +
//                "      return getXPath(node.parentNode) + '/' + node.tagName.toLowerCase() + '[' + (index + 1) + ']';" +
//                "    }" +
//                "    if (sibling.nodeType === 1 && sibling.tagName === node.tagName) {" +
//                "      index++;" +
//                "    }" +
//                "  }" +
//                "}" +
//                "return getXPath(arguments[0]);", element);
//        }
//    }
//}



