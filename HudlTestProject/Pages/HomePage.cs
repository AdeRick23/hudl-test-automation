using HudlProject.Utils;
using OpenQA.Selenium;

namespace HudlProject.Pages
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver) {}

        // Locators
        private By LoginSelectButton => By.CssSelector("[data-qa-id='login-select']");
        private By LoginHudlIcon => By.CssSelector("[data-qa-id='login-hudl']");

        // Methods
        public bool CheckNotLoggedIn()
        {
            return Driver.FindElement(LoginSelectButton).Displayed;
        }
        public void ClickLoginSelectButton()
        {
            Driver.FindElement(LoginSelectButton).Click();
            Driver.FindElement(LoginHudlIcon).Click();
        }
    }
}