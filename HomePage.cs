using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Automation_Code_Challenge_5_CSharp
{
    class HomePage : SeleniumWebdriverBaseClass
    {
        private void getResortMileageFromAirport(String resortName)
        {
            // Find the Compare Resorts WebElement and click on it
            IWebElement compareResorts = driver.FindElementByXPath((".//*[@id='ski-utah-welcome-map']/div/div[2]/div[4]/label/span[1]"));
            compareResorts.Click();

            // Find the Miles from Airport menu item popout and click on it
            IWebElement milesFromAirport = driver.FindElementByXPath(("//*[@id=\"ski-utah-welcome-map\"]/div/div[2]/div[4]/div/label[1]"));
            milesFromAirport.Click();

            // Check if passed in resortName is empty or null
            if (String.IsNullOrEmpty(resortName))
            {
                // Print that no resort name was entered
                consoleAndOutputFilePrint(automation_code_challenge_5_sw, "No Resort Name entered");
                // Exit from method
                return;
            }

            // Convert passed in resortName to all lowercase
            String resortNameLowercase = resortName.ToLower();
            // Declare resortValueName for holding value from HashMap
            String resortValueName = null;
            // Get resortValueName from HashMap
            resortValueName = resortList[resortNameLowercase];

            // Check if value returned from HashMap does not exist - returns null from HashMap
            if (String.IsNullOrEmpty(resortValueName))
            {
                consoleAndOutputFilePrint(automation_code_challenge_5_sw, "Resort Name not found");
            }

            // Resort Name found in HashMap
            else
            {
                // Print passed in resortName
                // consoleAndOutputFilePrint(automation_code_challenge_5_sw, "Entered Resort Name = " + resortName);
                // Print resortValueName from HashMap
                // consoleAndOutputFilePrint(automation_code_challenge_5_sw, "Resort Value = " + resortValueName);
                // Get WebElement (first span tag) using resortValueName
                IWebElement resortMileageFromAirport = driver.FindElementByXPath(("//span[@class='map-Area-shortName'][.='" + resortValueName + "']"));
                // Print class value of WebElement
                // consoleAndOutputFilePrint(automation_code_challenge_5_sw, resortMileageFromAirport.getAttribute("class"));
                // Print text value of WebElement
                // consoleAndOutputFilePrint(automation_code_challenge_5_sw, resortMileageFromAirport.getText());
                // Get WebElement (span tag with distance value) using resortValueName
                IWebElement resortMileageFromAirportElement = resortMileageFromAirport.FindElement(By.XPath("//span[@class='map-Area-shortName'][.='" + resortValueName + "']" + "/following-sibling::span[contains(@class,'distance')]"));
                // Print class value of WebElement
                // consoleAndOutputFilePrint(automation_code_challenge_5_sw, resortMileageFromAirportElement.getAttribute("class"));
                // Print innerHtml value of WebElement
                // consoleAndOutputFilePrint(automation_code_challenge_5_sw, resortMileageFromAirportElement.getAttribute("innerHTML"));
                // Create a variable to hold the resortMileage value
                String resortMileage = resortMileageFromAirportElement.GetAttribute("innerHTML");
                // Print the mileage from the airport for the passed in resort name
                consoleAndOutputFilePrint(automation_code_challenge_5_sw, resortValueName + " is " + resortMileage + " miles from the airport");

            }
        }

    }
}
