using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Kajabity.Tools.Java;

using CommandLine;
using MYOB.AutoTest;
using log4net;
using log4net.Config;

[assembly: XmlConfigurator(ConfigFile = "log.config", Watch = true)]

namespace InductionExercise
{

    class EntryPoint
    {

        public static JavaProperties Properties { get; set; }

//        public static readonly log4net.ILog Log =
//            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static TestSuite _testSuite;

        private static int Main(string[] args)
        {

            //string workingDirectory = null;
            //string propertyFile = null;
            //string testFile = null;
            var test = new ExecuteTests();

            var options = new Options();
 
            var clparser = new Parser();

            Properties = new JavaProperties();

            if (clparser.ParseArguments(args, options))
            {
                if (options.Verbose)
                {
                    Console.WriteLine("Working Directory: " + options.WorkingDirectory);
                    Console.WriteLine("Log Directory:     " + options.LogDirectory);
                    Console.WriteLine("Property File:     " + options.PropertyFile);
                    Console.WriteLine("Test File:         " + options.TestFile);
                    Console.WriteLine("Verbose:           " + options.Verbose);
                }
                else
                {
                    Console.WriteLine("Options set.");
                }
                Properties.SetProperty("workingDirectory", options.WorkingDirectory);
                //Properties.SetProperty("logDirectory", options.LogDirectory ?? options.WorkingDirectory);
                Properties.SetProperty("propertyFile", options.PropertyFile);
                Properties.SetProperty("testFile", options.TestFile);
            }

            Stream stream = null;

/*
            foreach (string s in args)
            {
                stream = new MemoryStream(Encoding.UTF8.GetBytes(s));
                Properties.Load(stream);
                stream.Close();
            }

            workingDirectory = Properties.GetProperty("workingDirectory");
            Console.WriteLine(@"Working Directory: " + Properties.GetProperty("workingDirectory"));
            if (workingDirectory == null)
            {
                Properties.SetProperty("workingDirectory", ".");
                Console.WriteLine(@"Working Directory: " + Properties.GetProperty("workingDirectory"));
            }

            propertyFile = Properties.GetProperty("propertyFile");
            Console.WriteLine(@"Property File: " + Properties.GetProperty("propertyFile"));
            if (propertyFile == null)
            {
                propertyFile = "propertyFile=.\\\\default.properties";
                stream = new MemoryStream(Encoding.UTF8.GetBytes(propertyFile));
                Properties.Load(stream);
                stream.Close();
                Console.WriteLine(@"Property File: " + Properties.GetProperty("propertyFile"));
            }

            testFile = Properties.GetProperty("testFile");
            Console.WriteLine(@"Test File: " + Properties.GetProperty("testFile"));
            if (testFile == null)
            {
                testFile = "testFile=*.csv";
                stream = new MemoryStream(Encoding.UTF8.GetBytes(testFile));
                Properties.Load(stream);
                stream.Close();
                Console.WriteLine(@"Test File: " + Properties.GetProperty("testFile"));
            }
*/

            Console.WriteLine(@"Working Directory: {0}", Properties.GetProperty("workingDirectory"));
            Console.WriteLine(@"Log Config File:   {0}", Properties.GetProperty("logConfigFile"));
            Console.WriteLine(@"Property File:     {0}", Properties.GetProperty("propertyFile"));
            Console.WriteLine(@"Test File:         {0}", Properties.GetProperty("testFile"));

            try
            {
                stream = new FileStream(Properties.GetProperty("workingDirectory") + Properties.GetProperty("propertyFile"), FileMode.Open);
                Properties.Load(stream);
            }
            catch (Exception ex)
            {
                // Handle the exception.
                Console.WriteLine("Can't find the property file. Exception: {0}", ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            Console.WriteLine(@"Working Directory: {0}", Properties.GetProperty("workingDirectory"));
            Console.WriteLine(@"Log Config File:   {0}", Properties.GetProperty("logConfigFile"));
            Console.WriteLine(@"Property File:     {0}", Properties.GetProperty("propertyFile"));
            Console.WriteLine(@"Test File:         {0}", Properties.GetProperty("testFile"));

            Console.WriteLine(@"Base URL:          {0}", Properties.GetProperty("baseURL"));
            Console.WriteLine(@"Browser:           {0}", Properties.GetProperty("browser"));

            Console.WriteLine("Press <Enter> to continue...");
            Console.ReadLine();

            var fi = new FileInfo(Properties.GetProperty("workingDirectory") + Properties.GetProperty("logConfigFile"));

            XmlConfigurator.Configure(fi);
            
            _testSuite = TestSuite.Instance;
            _testSuite.Initialise(Properties);
            _testSuite.LogHeader(Log);

            var files = Directory.GetFiles(Properties.GetProperty("workingDirectory"), Properties.GetProperty("testFile"));

            foreach (var s in files)
            {
                var scanner = new RegexCSVScanner(",", File.ReadAllText(s));
                var recordHandler = new ConsoleCSVRecordHandler();
                var parser = new CSVParser(scanner, recordHandler);
                recordHandler.BeginCSV();
                scanner.Next();
                int recordNr = 0;
                List<string> columnHeader = null;
                while (scanner.HasData)
                {
                    parser.ParseRecord(recordNr++);
                    //Console.WriteLine("Done Record.");
                    if (recordNr == 1)
                    {
                        columnHeader = new List<string>(recordHandler.GetFields());
                        string last = "";
                        for (int c = 0; c < columnHeader.Count; c++)
                        {
                            if (columnHeader[c] != "")
                            {
                                last = columnHeader[c];
                            }
                            else
                            {
                                columnHeader[c] = last;
                            }
                            //Console.WriteLine("Parameter: {0}", columnHeader[c]);
                        }
                    }
                    else
                    {
                        var columnValue = new List<string>(recordHandler.GetFields());
                        var testStepPropertyStore = new JavaProperties(Properties);
                        for (int c = 0; c < columnValue.Count; c++)
                        {
                            if (columnValue[c] == "")
                            {
                                //do nothing
                            }
                            else if (columnHeader != null && columnHeader[c] == "parameter")
                            {
                                //Console.WriteLine("Property: {0}", columnValue[c]);
                                stream = new MemoryStream(Encoding.UTF8.GetBytes(columnValue[c]));
                                testStepPropertyStore.Load(stream);
                                stream.Close();
                            }
                            else
                            {
                                //Console.WriteLine("Property: {0}={1}", columnHeader[c], columnValue[c]);
                                if (columnHeader != null)
                                    stream = new MemoryStream(Encoding.UTF8.GetBytes(columnHeader[c] + "=" + columnValue[c]));
                                testStepPropertyStore.Load(stream);
                                if (stream != null) stream.Close();
                            }
                        }
                        //Console.WriteLine(@"id: {0}", testStepPropertyStore.GetProperty("id"));
                        //Console.WriteLine(@"step: {0}", testStepPropertyStore.GetProperty("step"));
                        //Console.WriteLine(@"function: {0}", testStepPropertyStore.GetProperty("function"));
                        //Console.WriteLine(@"result: {0}", testStepPropertyStore.GetProperty("result"));
                        testStepPropertyStore.Report();
                        switch (testStepPropertyStore.GetProperty("function"))
                        {
                            case null:
                                Log.Info(@"[" + testStepPropertyStore.GetProperty("id") + "] ========================================");
                                Log.Info(@"Name:              " + testStepPropertyStore.GetProperty("name"));
                                Log.Info(@"Description:       " + testStepPropertyStore.GetProperty("description"));
                                break;
                            case "":
                                break;
                            case "Close_Browser":
                                Log.Info(@"[" + testStepPropertyStore.GetProperty("id") + "-" + testStepPropertyStore.GetProperty("step") + "] ----------------------------------------");
                                Log.Debug(@"Working Directory: " + testStepPropertyStore.GetProperty("workingDirectory"));
                                Log.Debug(@"Property File:     " + testStepPropertyStore.GetProperty("propertyFile"));
                                Log.Debug(@"Test File:         " + testStepPropertyStore.GetProperty("testFile"));
                                Log.Debug(@"Browser:           " + testStepPropertyStore.GetProperty("browser"));
                                Log.Debug(@"Base URL:          " + testStepPropertyStore.GetProperty("baseURL"));
                                Log.Info(@"Function:          " + testStepPropertyStore.GetProperty("function"));
                                Log.Info(@"Expected Result:   " + testStepPropertyStore.GetProperty("result"));
                                test.CloseBrowser(testStepPropertyStore);
                                break;
                            case "Navigate_To_Login_Page":
                                Log.Info(@"[" + testStepPropertyStore.GetProperty("id") + "-" + testStepPropertyStore.GetProperty("step") + "] ----------------------------------------");
                                Log.Debug(@"Working Directory: " + testStepPropertyStore.GetProperty("workingDirectory"));
                                Log.Debug(@"Property File:     " + testStepPropertyStore.GetProperty("propertyFile"));
                                Log.Debug(@"Test File:         " + testStepPropertyStore.GetProperty("testFile"));
                                Log.Debug(@"Browser:           " + testStepPropertyStore.GetProperty("browser"));
                                Log.Debug(@"Base URL:          " + testStepPropertyStore.GetProperty("baseURL"));
                                Log.Info(@"Function:          " + testStepPropertyStore.GetProperty("function"));
                                Log.Info(@"Expected Result:   " + testStepPropertyStore.GetProperty("result"));
                                test.NavigateToVehiclePage(testStepPropertyStore);
                                break;
                            case "Open_Browser":
                                Log.Info(@"[" + testStepPropertyStore.GetProperty("id") + "-" + testStepPropertyStore.GetProperty("step") + "] ----------------------------------------");
                                Log.Debug(@"Working Directory: " + testStepPropertyStore.GetProperty("workingDirectory"));
                                Log.Debug(@"Property File:     " + testStepPropertyStore.GetProperty("propertyFile"));
                                Log.Debug(@"Test File:         " + testStepPropertyStore.GetProperty("testFile"));
                                Log.Debug(@"Browser:           " + testStepPropertyStore.GetProperty("browser"));
                                Log.Debug(@"Base URL:          " + testStepPropertyStore.GetProperty("baseURL"));
                                Log.Info(@"Function:          " + testStepPropertyStore.GetProperty("function"));
                                Log.Info(@"Expected Result:   " + testStepPropertyStore.GetProperty("result"));
                                test.OpenBrowser(testStepPropertyStore);
                                break;
                            default:
                                Console.WriteLine(@"Invalid Function - [{0}]", testStepPropertyStore.GetProperty("function"));
                                break;
                        }
                    }
                }
                recordHandler.EndCSV(recordNr);
            }
//            Test test = new Test();
//            test.ExecuteTests();

            if (Log.IsInfoEnabled) Log.Info("Application End...");

            Console.WriteLine("Press <Enter> to exit...");
            Console.ReadLine();

            return 0;
        }

    }

}