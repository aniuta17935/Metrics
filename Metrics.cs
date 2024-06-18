using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace se3
{
    public class Metrics
    {
        string _text;
        int _physicalLines;
        int _blankLines;
        int _logicalLines;
        int _commentLines;
        double _commentingLevel;


        public Metrics(string text)
        {
            _text = text;
            _physicalLines = 0;
            _blankLines = 0;
            _logicalLines = 0;
            _commentLines = 0;
            _commentingLevel = 0;
        }

        public void Process()
        {
            CountPhysicalLines(_text);
            CountBlankLines(_text);
            CountLogicalLines(_text);
            CountCommentLines(_text);
            CountCommentingLevel();
        }

        public string Result()
        {
            return $"Physical lines = {_physicalLines}\n" +
                   $"Logical lines = {_logicalLines}\n" +
                   $"Blank lines = {_blankLines}\n" +
                   $"Comment lines = {_commentLines}\n" +
                   $"Commenting level = {_commentingLevel:F3}%\n";
        }


        void CountPhysicalLines(string text)
        {
            _physicalLines = text.Split('\n').Length;
        }

        void CountBlankLines(string text)
        {
            _blankLines = text.Split('\n').Count(line => string.IsNullOrWhiteSpace(line));
        }

        void CountLogicalLines(string text)
        {
            string cleanedText = DeleteComments(text);
            List<string> parts = Regex.Split(cleanedText, @"\s+").Where(part => !string.IsNullOrWhiteSpace(part)).ToList();

            List<string> selections = new() { "if", "else", "?", "try", "catch", "switch", "case" };
            List<string> iterations = new () { "for", "while", "do" };
            List<string> jumps = new () { "return", "break", "goto", "exit", "continue", "throw" };
            List<string> expressions = new () { "=", "(", "[", "{", "+=", "-=", "*=", "/=", "%=" };

            int logicalLinesNum = 0;

            foreach (string part in parts)
            {
                if (selections.Contains(part) || iterations.Contains(part) || jumps.Contains(part) || expressions.Contains(part))
                {
                    logicalLinesNum++;
                }
            }

            _logicalLines = logicalLinesNum;
        }

        static string DeleteComments(string text)
        {
            return Regex.Replace(text, @"\/\/.*$", "", RegexOptions.Multiline)
                        .Replace(@"/*[\s\S]*?\*/", "");
        }


        void CountCommentLines(string text)
        {
            string [] lines = text.Split('\n');
            int commentLinesNum = 0;
            bool comment = false;
            foreach (string line in lines)
            {
                string cleanedLine = line.Trim();

                if (cleanedLine.StartsWith("/*"))
                {
                    comment = true;
                }

                if (comment || cleanedLine.StartsWith("//") || cleanedLine.EndsWith("*/"))
                {
                    commentLinesNum++;
                }

                if (cleanedLine.EndsWith("*/"))
                {
                    comment = false;
                }
            }

            _commentLines = commentLinesNum;
        }

        void CountCommentingLevel()
        {
            _commentingLevel = _physicalLines == 0 ? 0 : (double)_commentLines / _physicalLines * 100;
        }
    }
}
