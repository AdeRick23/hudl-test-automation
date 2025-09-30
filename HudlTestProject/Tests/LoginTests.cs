using NUnit.Framework;
using OpenQA.Selenium;
using HudlProject.Pages;
using HudlProject.Utils;

namespace HudlProject.Tests
{
    [TestFixture]
    public class LoginTests
    {
        private IWebDriver driver = null!;
        private LoginPage loginPage = null!;
        private HomePage homePage = null!;
        private LandingPage landingPage = null!;
        private BasePage basePage = null!;
        private CredentialConfig credentials = null!;
        private const string ScreenshotDirectory = @"C:\Pics\Hudl\Screenshots";

        [SetUp]
        public void TestSetup()
        {
            driver = BasePage.InitialiseDriver();
            basePage = new BasePage(driver);
            basePage.LoadCredentials();
            credentials = basePage.Credentials!;
            
            loginPage = new LoginPage(driver);
            homePage = new HomePage(driver);
            landingPage = new LandingPage(driver);
        }

        [TearDown]
        public void Cleanup()
        {
             try
            {
                var currentPageState = TestContext.CurrentContext;
                if (basePage != null)
                {
                    basePage.CaptureScreenshotOnFailure(currentPageState, ScreenshotDirectory);
                }
            }
            finally
            {
                if (basePage != null)
                {
                    basePage.QuitDriver();
                }
            }
        }

        #region Helper Methods

        private void NavigateToLoginAndEnterCredentials(string username, string password)
        {
           // loginPage.AcceptCookiesIfPresent();
            homePage.ClickLoginSelectButton();
            loginPage.EnterUsername(username);
            loginPage.ClickLogin();
            loginPage.EnterPassword(password);
            loginPage.ClickLogin();
        }

        private void NavigateToLoginPageOnly()
        {
            homePage.ClickLoginSelectButton();
        }

        private void NavigateToPasswordScreen(string username)
        {
            NavigateToLoginPageOnly();
            loginPage.EnterUsername(username);
            loginPage.ClickLogin();
        }

        #endregion

        #region Valid Login Tests

        [Test]
        [Category("Smoke")]
        [Description("Verifies that a user can login with valid credentials")]
        public void ValidLogin_ShouldRedirectToHome()
        {
            // Arrange & Act
            NavigateToLoginAndEnterCredentials(credentials.ValidUsername, credentials.ValidPassword);
            
            // Assert
            Assert.That(landingPage.IsUserLoggedIn(), Is.True,
                "User should be logged in with valid credentials.");
        }

        [Test]
        [Category("Smoke")]
        [Description("Verifies that username is ignores casing during login")]
        public void ValidLoginUsernameInCaps_ShouldRedirectToHome()
        {
            // Arrange & Act
            NavigateToLoginAndEnterCredentials(credentials.ValidUsername.ToUpper(), credentials.ValidPassword);
            
            // Assert  
            Assert.That(landingPage.IsUserLoggedIn(), Is.True,
                "User should be logged in with valid credentials even with uppercase username.");
        }

        [Test]
        [Category("Smoke")]
        [Description("Verifies that a logged-in user can successfully logout")]
        public void ValidLogin_UserCanLogout()
        {
            // Arrange
            NavigateToLoginAndEnterCredentials(credentials.ValidUsername, credentials.ValidPassword);
            Assert.That(landingPage.IsUserLoggedIn(), Is.True, "User should be logged in.");
            
            // Act
            landingPage.Logout();
            
            // Assert
            Assert.That(homePage.CheckNotLoggedIn(), Is.True, "User should be logged out.");
        }

        #endregion

        #region Password Security Tests

        [Test]
        [Category("Security")]
        [Description("Verifies that password field is masked by default")]
        public void ValidLogin_PasswordShouldBeMasked()
        {
            // Arrange
            NavigateToPasswordScreen(credentials.ValidUsername);
            
            // Act
            loginPage.EnterPassword(credentials.ValidPassword);
            
            // Assert
            Assert.That(loginPage.IsPasswordMasked(), Is.True, "Password field should be masked.");
        }

        [Test]
        [Category("Security")]
        [Description("Verifies that toggle show password functionality works")]
        public void ToggleShowPassword_ShouldUnmaskPassword()
        {
            // Arrange
            NavigateToPasswordScreen(credentials.ValidUsername);
            loginPage.EnterPassword(credentials.ValidPassword);
            Assert.That(loginPage.IsPasswordMasked(), Is.True, "Password field should be masked initially.");
            
            // Act
            loginPage.ToggleShowPassword();
            
            // Assert
            Assert.That(loginPage.IsPasswordMasked(), Is.False, "Password field should be unmasked after toggle.");
        }

        [Test]
        [Category("Security")]
        [Description("Verifies that toggling show password twice masks the password again")]
        public void ToggleShowHidePassword_ShouldMaskPasswordAgain()
        {
            // Arrange
            NavigateToPasswordScreen(credentials.ValidUsername);
            loginPage.EnterPassword(credentials.ValidPassword);
            
            // Act
            loginPage.ToggleShowPassword(); // Show
            Assert.That(loginPage.IsPasswordMasked(), Is.False, "Password field should be unmasked after first toggle.");
            
            loginPage.ToggleShowPassword(); // Hide again

            // Assert
            Assert.That(loginPage.IsPasswordMasked(), Is.True, 
                "Password field should be masked again after toggling twice.");
        }

        #endregion

        #region Username Edit Tests

        [Test]
        [Category("Functional")]
        [Description("Verifies that a user can edit their username and login with correct credentials")]
        public void EditUsername_ShouldAllowChangingUsername()
        {
            // Arrange
            NavigateToLoginPageOnly();
            loginPage.EnterUsername(credentials.TestUsername);
            loginPage.ClickLogin();
            
            // Act
            loginPage.ClickEditUsername();
            loginPage.EnterUsername(credentials.ValidUsername);
            loginPage.ClickLogin();
            loginPage.EnterPassword(credentials.ValidPassword);
            loginPage.ClickLogin();

            // Assert
            Assert.That(landingPage.IsUserLoggedIn(), Is.True, 
                "Valid user can login after username edit.");
        }

        #endregion

        #region Session Management Tests

        [Test]
        [Category("Security")]
        [Description("Verifies that a user cannot access protected content after logout using browser back")]
        public void SessionExpiry_ShouldNotAllowBackNavigationAfterLogout()
        {
            // Arrange
            NavigateToLoginAndEnterCredentials(credentials.ValidUsername, credentials.ValidPassword);
            
            // Act
            landingPage.Logout();
            Assert.That(homePage.CheckNotLoggedIn(), Is.True, "User should be logged out.");
            
            driver.Navigate().Back();
            
            // Assert - This fails due improper session handling on the site
            Assert.That(landingPage.IsUserLoggedIn(), Is.False, 
                "User should not see member content after navigating back after logout.");
        }

        #endregion

        #region Invalid Credentials Tests

        [Test]
        [Category("Negative")]
        [Description("Verifies error message appears for invalid password")]
        public void InvalidPassword_ShouldShowErrorMessage()
        {
            // Arrange & Act
            NavigateToPasswordScreen(credentials.ValidUsername);
            loginPage.EnterPassword(credentials.InvalidPassword);
            loginPage.ClickLogin();
            
            // Assert
            Assert.That(loginPage.GetErrorMessage(), Does.Contain("password is incorrect"),
                "Error message should appear for invalid password.");
        }

        [Test]
        [Category("Negative")]
        [Description("Verifies that password validation is case-sensitive")]
        public void InvalidPasswordInCaps_ShouldShowErrorMessage()
        {
            // Arrange & Act
            NavigateToPasswordScreen(credentials.ValidUsername);
            loginPage.EnterPassword(credentials.InvalidPassword.ToUpper());
            loginPage.ClickLogin();
            
            // Assert
            Assert.That(loginPage.GetErrorMessage(), Does.Contain("password is incorrect"),
                "Error message should appear for invalid password regardless of case.");
        }

        [Test]
        [Category("Validation")]
        [Description("Verifies validation error for empty password field")]
        public void EmptyPassword_ShouldShowValidationError()
        {
            // Arrange & Act
            NavigateToPasswordScreen(credentials.ValidUsername);
            loginPage.EnterPassword("");
            loginPage.ClickLogin();
            
            // Assert
            Assert.That(loginPage.GetErrorMessage(), Does.Contain("Enter your password"),
                "Validation error message should appear when password field is empty.");
        }

        [Test]
        [Category("Negative")]
        [Description("Verifies error message for non-existent user")]
        public void InvalidUsername_ShouldShowError()
        {
            // Arrange & Act
            NavigateToPasswordScreen(credentials.TestUsername);
            loginPage.EnterPassword(credentials.ValidPassword);
            loginPage.ClickLogin();
            
            // Assert
            Assert.That(loginPage.GetErrorMessage(), Does.Contain("password is incorrect"),
                "Validation error should appear for non-existent user.");
        }

        [Test]
        [Category("Validation")]
        [Description("Verifies validation error for invalid email format")]
        public void InvalidEmailFormat_ShouldShowValidationError()
        {
            // Arrange & Act
            NavigateToLoginPageOnly();
            loginPage.EnterUsername(credentials.InvalidEmail);
            loginPage.ClickLogin();
            
            // Assert
            Assert.That(loginPage.GetErrorMessage(), Does.Contain("Enter a valid email."),
                "Validation error message should appear for invalid email format.");
        }

        #endregion
    }
}
