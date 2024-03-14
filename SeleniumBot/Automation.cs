using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V120.Debugger;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace SeleniumBot
{
    public class Automation
    {
        public IWebDriver driver;
        
        // Initialize Chrome Web Driver in a Maximized Window
        public Automation()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }
        // Close the Browser
        public void CloseWebDrive()
        {
            driver.Close();
        }
        
        // Navigate to the website
        public void GoToWebSite(string url)
        {
            Console.WriteLine("Iniciando bot...");
            driver.Navigate().GoToUrl(url);
            // Close Ads and cookies pop-up
            driver.FindElement(By.XPath("//*[@id=\"closeIconHit\"]")).Click();
            driver.FindElement(By.XPath("//*[@id=\"CybotCookiebotDialogBodyButtonDecline\"]")).Click();

        }
        
        // Close the Alert Pop-up
        private void HandleAlert()
        {
            try
            {
                IAlert al = driver.SwitchTo().Alert();
                al.Accept();
            }
            catch (UnhandledAlertException ex)
            {
                Console.WriteLine("Aguardando Alert " + ex.Message);
                return;
            }
        }

        // Verify if the alert pop-up exists
        private static bool IsAlertPresent(IWebDriver driver)
        {
            try
            {
                Console.WriteLine("Fechando Alert Pop-up");
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        // Capture the words and handle with the inputBox
        public List<object> PopulateText()
        {
            Console.WriteLine("Início de digitação...");

            // capture input textbox element
            var textBox = driver.FindElement(By.XPath("//*[@id=\"inputfield\"]"));
                
            // capture the element with the words
            var divText = driver.FindElement(By.XPath("//*[@id=\"words\"]"));

            // capture each word in the words list
            var wordsList = divText.FindElements(By.TagName("span"));

            // populate each word
            foreach (var word in wordsList)
            {
                textBox.SendKeys(word.Text + " ");
                Console.WriteLine(word.Text);
            }
                    
            // Wait 45 seconds for the alert pop-up appears
            try
            {
                Console.WriteLine("Fim da digitação, aguardando finalização do tempo....");
                Thread.Sleep(45000);
                                
                var alertPresent = IsAlertPresent(driver);

                if (alertPresent)
                {
                    HandleAlert();
                    Console.WriteLine("fechando alert");
                }
               
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Tempo limite de espera de 45 segundos excedido.");
            }
            
            // wait the result-table appears to capture data
            var timeToWait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            Console.WriteLine("Capturando resultado...");
            var resultTable = timeToWait.Until(e => e.FindElement(By.XPath("//*[@id=\"result-table\"]")));

            var wpm = resultTable.FindElement(By.XPath("//*[@id=\"wpm\"]/strong")).Text;
            var keystrokes = resultTable.FindElement(By.XPath("//*[@id=\"keystrokes\"]/td[2]")).Text;
            var accuracy = resultTable.FindElement(By.XPath("//*[@id=\"accuracy\"]/td[2]/strong")).Text;
            var correctWords = resultTable.FindElement(By.XPath("//*[@id=\"correct\"]/td[2]/strong")).Text;
            var wrongWords = resultTable.FindElement(By.XPath("//*[@id=\"wrong\"]/td[2]/strong")).Text;

            List<object> webList = SanitizeWords(wpm, keystrokes, accuracy, correctWords, wrongWords);

            return webList;
        }

        // Cleans data to be inserted into the database
        private List<object> SanitizeWords(string wpm, string keystrokes, string accuracy, string correctWords, string wrongWords)
        {
            Console.WriteLine("Tratando resultado...");
            List<object> listWords = new List<object>();
            int cleanWPM = int.Parse(Regex.Replace(wpm, "[^0-9]", ""));
            Match cleanKeystrokes = Regex.Match(keystrokes, @"\((\d+)\s*\|\s*\d+\)\s*(\d+)");
            int intKeystroke = int.Parse(cleanKeystrokes.Groups[1].Value);
            int cleanAccuracy = int.Parse(Regex.Replace(accuracy, @"%", ""));
            int cleanCorrectWords = int.Parse(correctWords);
            int cleanWrongWords = int.Parse(wrongWords);
            
            listWords.Add(cleanWPM);
            listWords.Add(intKeystroke);
            listWords.Add(cleanAccuracy);
            listWords.Add(cleanCorrectWords);
            listWords.Add(cleanWrongWords);
            
            return listWords;
        }

    }
}
