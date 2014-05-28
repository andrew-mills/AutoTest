using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MYOB.AutoTest
{

    public interface ICSVRecordHandler
    {
        void BeginCSV();
        void BeginRecord(int recordNr);
        void AddField(int fieldNr, string field);
        void EndRecord(int recordNr);
        void EndCSV(int recordsTotal);
    }

    public interface ICSVParser
    {
        void ParseRecords();
        void ParseRecord(int recordNr);
    }

    public interface ICSVScanner
    {
        string Curr { get; }
        void Next();
        bool HasData { get; }
        bool IsSep { get; }
        bool IsEol { get; }
    }

    // --- CSV Grammar ---
    // csv     : { record } .
    // record  : field { sep field } EOL .
    // field   : [ qstring | raw ] .
    // qstring : '"' { '""' | NOT_QUOTE } '"' .
    // raw     : NOT_SEP_QUOTE_EOL { NOT_SEP_QUOTE_EOL } .
    // sep     : SEP .

    public class CSVParser : ICSVParser
    {
        private ICSVScanner Scanner { get; set; }
        private ICSVRecordHandler RecordHandler { get; set; }

        public CSVParser(ICSVScanner scanner, ICSVRecordHandler recordHandler)
        {
            Scanner = scanner;
            RecordHandler = recordHandler;
        }

        public void ParseRecords()
        {
            RecordHandler.BeginCSV();
            Scanner.Next();
            int recordNr = 0;
            while (Scanner.HasData) ParseRecord(recordNr++);
            RecordHandler.EndCSV(recordNr);
        }

        public void ParseRecord(int recordNr)
        {
            RecordHandler.BeginRecord(recordNr);
            int fieldNr = 0;
            ParseField(fieldNr++);
            while (Scanner.IsSep)
            {
                Scanner.Next();
                ParseField(fieldNr++);
            }
            if (Scanner.IsEol) Scanner.Next();
            RecordHandler.EndRecord(recordNr);
        }

        private void ParseField(int fieldNr)
        {
            string field = Scanner.Curr;
            if (Scanner.IsSep || Scanner.IsEol) field = string.Empty;
            else Scanner.Next();
            RecordHandler.AddField(fieldNr, field);
        }

    }

    public class RegexCSVScanner : ICSVScanner
    {
        private IEnumerator<Match> Tokenizer { get; set; }

        public string Curr
        {
            get
            {
                return HasData
                    ? Tokenizer.Current.Groups.Cast<Group>().Reverse().First(g => g.Success)
                    .Value.Replace(@"""""", @"""")
                    : string.Empty;
            }
        }

        public void Next() { HasData = HasData && Tokenizer.MoveNext(); }

        public bool HasData { get; private set; }

        public bool IsSep { get { return HasData && Tokenizer.Current.Groups[2].Success; } }

        public bool IsEol { get { return HasData && Tokenizer.Current.Groups[3].Success; } }

        public RegexCSVScanner(string sep, string data)
        {
            string tokens = string.Join("|", new[] {
                    @"""((?:""""|[^""])*)""",   // group 1: content of qstring
                                                //          (not unescaped)
                    @"(" + sep + @")",          // group 2: SEP
                    @"(\n\r?|\r\n?)",           // group 3: EOL = any variant of \n and \r
                    @"([^""\n\r" + sep + @"]+)",// group 4: word of at least one
                                                //          character not one of
                                                //          quote, eol, sep
                });
            Tokenizer = Regex.Matches(data, tokens, RegexOptions.Compiled | RegexOptions.Singleline)
                .Cast<Match>().GetEnumerator();
            HasData = true;
        }

    }

    public class ConsoleCSVRecordHandler : ICSVRecordHandler
    {

        private List<string> Fields { get; set; }

        public ConsoleCSVRecordHandler()
        { Fields = new List<string>(); }

        public void BeginCSV()
        { Console.WriteLine("------ CSV Parsing ---------"); }

        public void BeginRecord(int recordNr)
        { Fields.Clear(); }

        public void AddField(int fieldNr, string field)
        { System.Diagnostics.Debug.Assert(fieldNr == Fields.Count); Fields.Add(field); }

        public void EndRecord(int recordNr)
        { Console.WriteLine("Record {0}: '{1}'", recordNr, string.Join("', '", Fields)); }

        public void EndCSV(int recordsTotal)
        { Console.WriteLine("------ Records: {0} ---------", recordsTotal); }
    
        public List<string> GetFields()
        {
            return Fields;
        }
    }

}
