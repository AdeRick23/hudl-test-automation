using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Text.Json;
using NUnit.Framework;

namespace HudlProject.Utils
{
    public class BasePage
    {
        private const int DefaultWaitTimeSeconds = 5;
        
        /// <summary>
        /// Initializes a new instance of the BasePage class
        /// </summary>
        /// <param name="driver">WebDriver instance</param>
        public BasePage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }
        
        /// <summary>
        /// Gets the loaded credentials configuration
        /// </summary>
        public CredentialConfig? Credentials { get; private set; }
        
        /// <summary>
        /// Gets or sets the WebDriver instance
        /// </summary>
        public IWebDriver Driver { get; set; }
        
        /// <summary>
        /// Gets a WebDriverWait instance with default timeout
        /// </summary>
        protected WebDriverWait ElementWait => Driver != null 
            ? new WebDriverWait(Driver, TimeSpan.FromSeconds(DefaultWaitTimeSeconds)) 
            : throw new InvalidOperationException("Driver is not initialized.");
        
        public static string BaseAddress => "https://www.hudl.com/en_gb/";
        
        /// <summary>
        /// Initializes a new ChromeDriver instance and navigates to the base URL
        /// </summary>
        /// <returns>Configured WebDriver instance</returns>
        public static IWebDriver InitialiseDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalOption("useAutomationExtension", false);
            
            var driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(BaseAddress);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            
            return driver;
        }

        public void CaptureScreenshotOnFailure(TestContext context, string screenshotsDir)
        {
            if (context.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed
                && Driver is ITakesScreenshot takesScreenshot)
            {
                try
                {
                    Directory.CreateDirectory(screenshotsDir);
                    var fileName = $"{context.Test.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    var filePath = Path.Combine(screenshotsDir, fileName);
                    takesScreenshot.GetScreenshot().SaveAsFile(filePath);
                    TestContext.AddTestAttachment(filePath, "Screenshot on failure");
                }
                catch (Exception ex)
                {
                    TestContext.WriteLine($"Failed to capture screenshot: {ex.Message}");
                }
            }
        }
        public void LoadCredentials(string configFile = "testcredentials.json")
        {
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Utils", configFile);
            
            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException($"Configuration file not found: {configPath}");
            }
            
            var configJson = File.ReadAllText(configPath);
            Credentials = JsonSerializer.Deserialize<CredentialConfig>(configJson);
        }
        public void QuitDriver()
        {
            Driver?.Quit();
        }
    }
}
