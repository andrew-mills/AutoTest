using CommandLine;
using CommandLine.Text;

namespace MYOB.AutoTest
{

    public class Options
    {

        [Option('l', "log", DefaultValue = null, HelpText = "The directory where the log files are to be output. Include the trailing slash.")]
        public string LogDirectory { get; set; }

        [Option('p', "property", DefaultValue = "default.properties", HelpText = "The name of the file containing the default properties.")]
        public string PropertyFile { get; set; }

        [Option('t', "test", DefaultValue = "*.csv", HelpText = "The name of the test file(s), which may include the * and ? wild cards.")]
        public string TestFile { get; set; }

        [Option('v', "verbose", DefaultValue = false, HelpText = "Output extra detail during execution.")]
        public bool Verbose { get; set; }

        [Option('w', "working", DefaultValue = ".\\", HelpText = "The directory where the test CSV files are located. Include the trailing slash.")]
        public string WorkingDirectory { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

    }

}
