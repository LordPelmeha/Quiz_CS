using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Quiz
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine();
            var s = "bank.txt";
        }
        static string[] SplitFileByString(string path)
        {
            using (var rs = new StreamReader(File.Open(path, FileMode.Open)))
            { 
                var s = Regex.Split(rs.ReadToEnd(), @"\n").Select(x => x.ToString()).ToArray();
                return s[..(s.Length-1)];
            }
        }
    }
} 