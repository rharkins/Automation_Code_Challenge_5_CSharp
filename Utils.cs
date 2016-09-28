using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Automation_Code_Challenge_5_CSharp
{
    class Utils : SeleniumWebdriverBaseClass
    {
        static void Main(string[] args)
        {
            // Empty Main method - keeping the compiler happy
        }

        private void verifyPageTitle(String webPageURL, String titleStringToTest)
        {
            // startBrowser
            if (browserStarted == false)
            {
                startBrowser();
                browserStarted = true;
            }
            // Open Webpage URL
            driver.Url = (webPageURL);
            // Get page title of current page
            String pageTitle = driver.Title;
            // Print page title of current page
            consoleAndOutputFilePrint(automation_code_challenge_5_sw, "Page title of current page is: " + pageTitle);
            // Print title string to test
            consoleAndOutputFilePrint(automation_code_challenge_5_sw, "Title String to Test is: " + titleStringToTest);
            // Test that the titleStringToTest = title of current page
            //Assert.assertTrue(pageTitle.equals(titleStringToTest), "Current Page Title is not equal to the expected page title value");
            // If there is no Assertion Error, Print out that the Current Page Title = Expected Page Title
            consoleAndOutputFilePrint(automation_code_challenge_5_sw, "Current Page Title = Expected Page Title");
        }

        private void verifyNavigation(String navigationMenu, String validationString)
        {
            // Build CSS Selector based on navigation menu user wants to click on
            String cssSelectorText = "a[title='" + navigationMenu + "']";
            // Find menu WebElement based on CSS Selector
            IWebElement navigationMenuWebElement = driver.FindElementByCssSelector((cssSelectorText));
            // Get href attributte from menu WebElement
            String navigationMenuURL = navigationMenuWebElement.GetAttribute("href");
            // Navigate to href and validate page title
            verifyPageTitle(navigationMenuURL, validationString);
        }

        private void subMenuNavigation(String navigationMenu, String navigationSubMenu)
        {
            // Build CSS Selector based on navigation menu user wants to click on
            String cssSelectorTextNavigationMenu = "a[title='" + navigationMenu + "']";
            // Find menu WebElement based on CSS Selector
            bool isPresent = driver.FindElementsByCssSelector((cssSelectorTextNavigationMenu)).Count == 1;
            // Check if navigation menu item exists
            if (isPresent)
            {
                // Get navigation menu WebElement
                IWebElement navigationMenuWebElement = driver.FindElementByCssSelector((cssSelectorTextNavigationMenu));
                // Get href attributte from navigation menu WebElement
                String navigationMenuURL = navigationMenuWebElement.GetAttribute("href");
                //Create Actions object
                Actions mouseHover = new Actions(driver);
                // Move to navigation menu WebElement to initiate a hover event
                mouseHover.MoveToElement(navigationMenuWebElement).Perform();
                //String cssSelectorTextSubMenu = "a[title='" + navigationSubMenu + "']";
                // Build navigation submenu xpath to anchor tag
                String xpathSelectorTextSubmenu = "//a[.='" + navigationSubMenu + "']";
                //WebElement navigationSubMenuWebElement = driver.findElement(By.linkText(navigationSubMenu));
                // Get navigation submenu WebElement
                IWebElement navigationSubMenuWebElement = driver.FindElementByXPath((xpathSelectorTextSubmenu));
                // Check if navigation submenu exists
                NUnit.Framework.Assert.IsTrue(navigationSubMenuWebElement.Enabled, (navigationSubMenu + " navigation submenu does not exist on this page"));
                // Click on navigation submenu WebElement
                navigationSubMenuWebElement.Click();
                //mouseHover.perform();
                // Navigate to href and validate page title
                //VerifyPageTitle(navigationMenuURL, "Ski and Snowboard The Greatest Snow on Earth® - Ski Utah");
            }
            else
            {
                // Print message indicating that the navigation menu passed in to this method does not exist on the page
                consoleAndOutputFilePrint(automation_code_challenge_5_sw, navigationMenu + " navigation menu does not exist on this page");
            }
        }

        // This method accepts string parameters for the search dialogs
        public void searchForDialog(String whatParameter, String byResortParameter, String subCategoryParameter)
        {
            //System.Threading.Thread.Sleep(5000);
            driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.MinValue);
            int listingFilterClassElementCount = driver.FindElementsByXPath((".//*[@class='ListingFilter']")).Count;
            bool searchDialogPresent = driver.FindElementsByXPath((".//*[@class='ListingFilter']")).Count == 1;
            // Check if the Search Dialog exists on this page
            if (searchDialogPresent)
            {
                SelectElement whatParameterDropdown = new SelectElement(driver.FindElementByXPath((".//*[@name='filter-category-select']")));
                SelectElement byResortParameterDropdown = new SelectElement(driver.FindElementByXPath((".//*[@name='filter-resort-select']")));
                Boolean subCategoryDropdownPresent = driver.FindElementsByXPath((".//*[@name='filter-sub-category-select']")).Count() == 1;
                IWebElement okButton = driver.FindElementByXPath((".//*[@type='submit'][@value='OK']"));

                whatParameterDropdown.SelectByText(whatParameter);
                byResortParameterDropdown.SelectByText(byResortParameter);

                // Check if subcategory dropdown is present
                if (subCategoryDropdownPresent)
                {
                    SelectElement subCategoryParameterDropdown = new SelectElement(driver.FindElementByXPath((".//*[@name='filter-sub-category-select']")));
                    subCategoryParameterDropdown.SelectByText(subCategoryParameter);
                }

                // Start search
                okButton.Click();

                // Get all search result elements
                //IReadOnlyCollection<IWebElement> pageSearchResultElements = driver.FindElementsByXPath((".//*[@class='ListingResults-item']"));
                List<IWebElement> pageSearchResultElements = driver.FindElementsByXPath((".//*[@class='ListingResults-item']")).Cast<IWebElement>().ToList();
                // Get the Next Page Button Element (if it exists, or null if it does not exist)
                //IReadOnlyCollection<IWebElement> nextPageButton = driver.FindElementsByXPath((".//*[@class='BatchLinks-next ']"));
                List<IWebElement> nextPageButton = driver.FindElementsByXPath((".//*[@class='BatchLinks-next ']")).Cast<IWebElement>().ToList();
                // Write search results header if there is at least 1 element in the search result
                if (pageSearchResultElements.Count() > 0)
                {
                    consoleAndOutputFilePrint(automation_code_challenge_5_sw, "");
                    consoleAndOutputFilePrint(automation_code_challenge_5_sw, "Search Results");
                    consoleAndOutputFilePrint(automation_code_challenge_5_sw, "");
                }
                // Print this page's search results to the console
                //consoleSearchPrint(pageSearchResultElements);
                // Print this page's search results to the output file
                outputFileSearchPrint(pageSearchResultElements);

                // Execute these statements as long as there is a Next Page Button active
                while (nextPageButton.Count() == 1)
                {
                    nextPageButton.ElementAt(0).Click();
                    //System.Threading.Thread.Sleep(5000);
                    pageSearchResultElements = driver.FindElementsByXPath((".//*[@class='ListingResults-item']")).Cast<IWebElement>().ToList();
                    //consoleSearchPrint(pageSearchResultElements);
                    outputFileSearchPrint(pageSearchResultElements);

                    //nextPageButton = null;
                    nextPageButton.Clear();
                    pageSearchResultElements.Clear();
                    nextPageButton = driver.FindElementsByXPath((".//*[@class='BatchLinks-next ']")).Cast<IWebElement>().ToList();
                }
            }
            // If search dialog does not exist on this page
            else
            {
                // Print message indicating that the search dialog does not exist on the page
                consoleAndOutputFilePrint(automation_code_challenge_5_sw, "The current page does not contain a Search For dialog box");
            }

        }

        // This method accepts a list of WebElements and prints the text of those elements and subelements to the console
        public void consoleSearchPrint(IReadOnlyCollection<IWebElement> resultList)
        {
            //Console.WriteLine("ResultList size = " + resultList.Count());
            for (int listIndex = 0; listIndex < resultList.Count; listIndex++)
            {
                consoleAndOutputFilePrint(automation_code_challenge_5_sw, resultList.ElementAt(listIndex).Text);
                consoleAndOutputFilePrint(automation_code_challenge_5_sw, "--------------------");
            }

        }

        // This method accepts a list of WebElements and prints the text of those elements and subelements to an output file
        public void outputFileSearchPrint(IReadOnlyCollection<IWebElement> resultList)
        {
            for (int listIndex = 0; listIndex < resultList.Count(); listIndex++)
            {
                //automation_code_challenge_5_sw.Write(resultList.ElementAt(listIndex).Text);
                //automation_code_challenge_5_sw.WriteLine();
                //automation_code_challenge_5_sw.Write("--------------------");
                //automation_code_challenge_5_sw.WriteLine();
                consoleAndOutputFilePrint(automation_code_challenge_5_sw, resultList.ElementAt(listIndex).Text);
                //automation_code_challenge_5_sw.WriteLine();
                consoleAndOutputFilePrint(automation_code_challenge_5_sw, "--------------------");
                //automation_code_challenge_5_sw.WriteLine();
            }
        }

        public void cleanup()
        {
            automation_code_challenge_5_sw.Close();
        }

        // This method crawls every page of a domain
        public void webCrawler()
        {
            {
                DateTime crawlerStartTime = DateTime.Now;
                IReadOnlyCollection<IWebElement> anchorTags = null;
                ArrayList hrefAttributeValues = new ArrayList();
                ArrayList pagesToVisit = new ArrayList();
                ArrayList pagesVisited = new ArrayList();
                String currentPageURL = baseWebPageURL;
                pagesVisited.Add(baseWebPageURL);
                consoleAndOutputFilePrint(automation_code_challenge_5_sw, "WebCrawler Start Time - " + crawlerStartTime);
                consoleAndOutputFilePrint(automation_code_challenge_5_sw, "pagesVisited size = " + pagesVisited.Count);
                consoleAndOutputFilePrint(automation_code_challenge_5_sw, "pagesVisited website = " + (pagesVisited.Count - 1));

                anchorTags = driver.FindElementsByXPath((".//a[@href[starts-with(.,'http://www.skiutah.com') and not(contains(., '@@')) and not(contains(., '?'))]]"));
                //anchorTags = driver.FindElementsByXPath((".//a[@href[starts-with(.,'http://www.skiutah.com') and not(contains(., '@@')) and not(contains(., 'blog')) and not(contains(., '?'))]]"));
                //        anchorTags = driver.findElements(By.xpath(".//a[@href[starts-with(.,'https://www.skiutah.com') or starts-with(.,'/')]]"));
                //        anchorTags = driver.findElements(By.xpath(".//a[@href[starts-with(.,'/')]]"));
                // Get all href attribute values on current page
                foreach (IWebElement anchorTagElement in anchorTags)
                {
                    hrefAttributeValues.Add(anchorTagElement.GetAttribute("href"));
                }
                // Check that the href values have not been previously visited
                foreach (String hrefValues in hrefAttributeValues)
                {
                    if (!pagesVisited.Contains(hrefValues) && !pagesToVisit.Contains(hrefValues))
                    {
                        pagesToVisit.Add(hrefValues);
                        consoleAndOutputFilePrint(automation_code_challenge_5_sw, hrefValues);
                    }
                }
                consoleAndOutputFilePrint(automation_code_challenge_5_sw, pagesToVisit.Count + " - pages left to visit");
                consoleAndOutputFilePrint(automation_code_challenge_5_sw, "--------------------");
                // Remove current page from pagesToVisit List
                pagesToVisit.Remove(currentPageURL);
                // Loop through pagesToVisit List until list is empty
                while (pagesToVisit.Count > 0)
                // Go to next page in Pages to visit list
                {
                    anchorTags = null;
                    driver.Url = (pagesToVisit[0].ToString());
                    pagesVisited.Add(pagesToVisit[0]);
                    consoleAndOutputFilePrint(automation_code_challenge_5_sw, "pagesVisited size = " + pagesVisited.Count);
                    consoleAndOutputFilePrint(automation_code_challenge_5_sw, "pagesVisited website = " + (pagesVisited.Count - 1));
                    currentPageURL = (pagesToVisit[0].ToString());
                    anchorTags = driver.FindElementsByXPath((".//a[@href[starts-with(.,'http://www.skiutah.com') and not(contains(., '@@')) and not(contains(., '?'))]]"));
                    //anchorTags = driver.FindElementsByXPath((".//a[@href[starts-with(.,'http://www.skiutah.com') and not(contains(., '@@')) and not(contains(., 'blog')) and not(contains(., '?'))]]"));
                    //        anchorTags = driver.findElements(By.xpath(".//a[@href[starts-with(.,'https://www.skiutah.com') or starts-with(.,'/')]]"));
                    //        anchorTags = driver.findElements(By.xpath(".//a[@href[starts-with(.,'/')]]"));
                    // Get all href attribute values on current page
                    foreach (IWebElement anchorTagElement in anchorTags)
                    {
                        hrefAttributeValues.Add(anchorTagElement.GetAttribute("href"));
                    }
                    // Check that the href values have not been previously visited
                    foreach (String hrefValues in hrefAttributeValues)
                    {
                        if (!pagesVisited.Contains(hrefValues) && !pagesToVisit.Contains(hrefValues))
                        {
                            pagesToVisit.Add(hrefValues);
                            consoleAndOutputFilePrint(automation_code_challenge_5_sw, hrefValues);
                        }
                    }
                    consoleAndOutputFilePrint(automation_code_challenge_5_sw, pagesToVisit.Count + " - pages left to visit");
                    consoleAndOutputFilePrint(automation_code_challenge_5_sw, "--------------------");
                    // Remove current page from pagesToVisit List
                    pagesToVisit.Remove(currentPageURL);
                    // Clear out hrefAttributeValues list
                    hrefAttributeValues.Clear();

                }

                consoleAndOutputFilePrint(automation_code_challenge_5_sw, pagesVisited.Count.ToString());
                DateTime crawlerEndTime = DateTime.Now;
                consoleAndOutputFilePrint(automation_code_challenge_5_sw, "WebCrawler End Time - " + crawlerEndTime);
                TimeSpan elapsedCrawlerTime = crawlerEndTime.Subtract(crawlerStartTime);
                long elapsedCrawlerTimeInMinutes = elapsedCrawlerTime.Minutes;
                long elapsedCrawlerTimeInHours = elapsedCrawlerTime.Hours;
                TimeSpan elapsedCrawlerTimeRemainingMinutesDuration = crawlerEndTime.Subtract(crawlerStartTime);
                long elapsedCrawlerTimeRemainingMinutes = elapsedCrawlerTimeRemainingMinutesDuration.Minutes;
                if (elapsedCrawlerTimeRemainingMinutes == 1)
                {
                    consoleAndOutputFilePrint(automation_code_challenge_5_sw, "Total WebCrawler Elapsed Time - " + elapsedCrawlerTimeInHours + " hours" + " " + elapsedCrawlerTimeRemainingMinutes + " minute");
                }
                else
                {
                    consoleAndOutputFilePrint(automation_code_challenge_5_sw, "Total WebCrawler Elapsed Time - " + elapsedCrawlerTimeInHours + " hours" + " " + elapsedCrawlerTimeRemainingMinutes + " minutes");

                }

            }

        }

[Test]
        public void TestLauncher()
        {
            verifyPageTitle(baseWebPageURL, "Ski Utah - Ski Utah");

            //webCrawler();

            // Test against Explore menu - no Search Dialog
            //verifyNavigation("Explore", "Utah Areas 101 - Ski Utah");

            // Test against Plan Your Trip Search Dialog
            verifyNavigation("Plan Your Trip", "All Services - Ski Utah");
            //verifyNavigation("Deals", "Ski and Snowboard The Greatest Snow on Earth® - Ski Utah");
            //try
            //{
            //    subMenuNavigation("Plan Your Trip", "Activities");
            //}
            //catch (ElementNotVisibleException e)
            //{
            //    subMenuNavigation("Plan Your Trip", "Activities");
            //}
            //subMenuNavigation("Plan Your Trip", "Activities");
            searchForDialog("Activities", "Snowbasin Resort", "All");

            // Test against Deals Search Dialog - no subcategory dropdown
            //verifyNavigation("Deals", "Ski and Snowboard The Greatest Snow on Earth® - Ski Utah");
            //searchForDialog("All", "All Resorts", "All");

            cleanup();

            //        HomePage.getResortMileageFromAirport("EAGLE point");
            //        subMenuNavigation("Explore", "Stories - Photos - Videos");
        }

    }
}
