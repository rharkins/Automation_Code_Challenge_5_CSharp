using System;
using System.IO;
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
    class SeleniumWebdriverBaseClass
    {
        public ChromeDriver driver;
        static public String baseWebPageURL = "http://www.skiutah.com";
        public bool browserStarted = false;
        public Dictionary<String, String> resortList = new Dictionary<String, String>();
        public StreamWriter automation_code_challenge_5_sw = createOutputFile();


        public void startup()
        {
            initialize();
            startup();
        }

        public void initialize()
        {
            // Populate resortList HashMap
            resortList.Add("beaver mountain", "Beaver Mtn");
            resortList.Add("cherry peak", "Cherry Peak");
            resortList.Add("nordic valley", "Nordic Valley");
            resortList.Add("powder mountain", "Powder Mtn");
            resortList.Add("snowbasin", "Snowbasin");
            resortList.Add("alta", "Alta");
            resortList.Add("brighton", "Brighton");
            resortList.Add("snowbird", "Snowbird");
            resortList.Add("solitude", "Solitude");
            resortList.Add("deer valley", "Deer Valley");
            resortList.Add("park city", "Park City");
            resortList.Add("sundance", "Sundance");
            resortList.Add("brian head", "Brian Head");
            resortList.Add("eagle point", "Eagle Point");
        }

        public void startBrowser()
        {
            //File pathToBinary = new File("C:\\Program Files (x86)\\Mozilla Firefox\\firefox.exe");
            //FirefoxBinary ffBinary = new FirefoxBinary(pathToBinary);
            //FirefoxProfile firefoxProfile = new FirefoxProfile();
            //driver = new FirefoxDriver(ffBinary,firefoxProfile);
            //driver = new FirefoxDriver();
            //File file = new File("C:\\ChromeDriver\\chromedriver.exe");
            //System.setProperty("webdriver.chrome.driver", "C:\\ChromeDriver\\chromedriver.exe");
            driver = new ChromeDriver();
        }

        public static StreamWriter createOutputFile()
        {
            String outputFile = "C:/test/csharp_automation_code_challenge_5_output.txt";
            // If file doesnt exists, then create it
            StreamWriter sw = new StreamWriter(outputFile);
            return sw;
        }

        // This method accepts a string and prints that string to the console and the output file
        public void consoleAndOutputFilePrint(StreamWriter outputFileWriter, String outputString)
        {
            Console.WriteLine(outputString);
            outputFileWriter.Write(outputString);
            outputFileWriter.WriteLine();
        }

    }
}
