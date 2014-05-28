using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Kajabity.Tools.Java;

using log4net;

namespace MYOB.AutoTest
{

    public class TestSuite
    {

        public string WorkingDirectory { get; set; }
        public string LogConfigFile { get; set; }
        public string PropertyFile { get; set; }
        public string TestFile { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime StartTime;
        public DateTime FinishTime;

        private static readonly object Lock = new object();
        private static TestSuite _testSuite = null;

        private List<TestCase> _testCases;

        public static TestSuite Instance
        {
            get
            {
                if (_testSuite == null)
                {
                    Thread.MemoryBarrier();
                    lock (Lock)
                    {
                        if (_testSuite == null)
                        {
                            _testSuite= new TestSuite();
                        }
                    }
                }
                return _testSuite;
            }
        }

        private TestSuite()
        {
//            _lock = new object();
        }

        public void Initialise(JavaProperties jp)
        {
            WorkingDirectory = jp.GetProperty("workingDirectory");
            LogConfigFile = jp.GetProperty("logConfigFile");
            PropertyFile = jp.GetProperty("propertyFile");
            TestFile = jp.GetProperty("testFile");
            Id = jp.GetProperty("testSuiteId");
            Name = jp.GetProperty("testSuiteName");
            Description = jp.GetProperty("testSuiteDescription");
        }

        public void LogHeader(ILog log)
        {
            if (!log.IsInfoEnabled)
                return;
            log.Info(@"================================================================================");
            log.Info(@"Id:           " + Id);
            log.Info(@"Name:         " + Name);
            log.Info(@"Description:  " + Description);
            log.Info(@"================================================================================");
            log.Info(@"");
            log.Info(@"Working Directory: " + WorkingDirectory);
            log.Info(@"Log Config File:   " + LogConfigFile);
            log.Info(@"Property File:     " + PropertyFile);
            log.Info(@"Test File:         " + TestFile);
            log.Info(@"");
        }

        public void AddTestCase(TestCase tc)
        {
            if (!_testCases.Contains(tc))
            {
                _testCases.Add(tc);
            }
        }
        // test summary
        // configuration
        // goals

    }

}
