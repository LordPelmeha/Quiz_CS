using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Quiz
{
    internal class Program
    {
        static void Main()
        {
            string[] str =SplitFileByString("bank.txt");
        }
        static string[] SplitFileByString(string path)
        {
            using (var rs = new StreamReader(File.Open(path, FileMode.Open)))
            { 
                string[] s = Regex.Split(rs.ReadToEnd(), @"\n").Select(x => x.ToString()).ToArray();
                return s[..(s.Length-1)];
            }
        }
        static string GetQuestions(string s) => 
            Regex.Match(s, @".*?\?").ToString();
        static string GetAnswers(string s) => 
            Regex.Match(s, @"\|\|.*").ToString();
        static string[] ShowAnswers(string s)=>
            Regex.Matches(s, @"(?<=\|\|).*?(?=\=)").Select((x)=>x.ToString()).ToArray();
    }
} 