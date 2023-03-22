using System.IO;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Quiz
{
    internal class Program
    {
        static void Main()
        {
            StartGame();
        }
        static string[] SplitFileByString(string path)
        {
            using (var rs = new StreamReader(File.Open(path, FileMode.Open)))
            {
                string[] s = Regex.Split(rs.ReadToEnd(), @"\n").Select(x => x.ToString()).ToArray();
                return s[..(s.Length - 1)];
            }
        }
        /// <summary>
        /// Находит и возвращает вопрос
        /// </summary>
        static string GetQuestions(string s) =>
            Regex.Match(s, @".*?\?").ToString();
        /// <summary>
        /// Находит и возвращает варианты ответа
        /// </summary>
        static string[] GetAnswers(string s) =>
            Regex.Matches(s, @"(?<=\|\|).*?(?=\=)").Select(x => x.ToString()).ToArray();
        /// <summary>
        /// Возвращает номер правильного ответа
        /// </summary>
        static int RightAnswer(string s) =>
            Regex.Matches(s, @"(?<=\=\>)\w+").Select(x => x.ToString()).ToList().FindIndex(x => x == "True") + 1;
        /// <summary>
        /// Считывает ответ пользователя
        /// </summary>
        static int GetPlayerAnswer(int count)
        {
            var ans = Console.ReadLine();
            while (ans.Length > 1 || !"123345".Contains(ans) || ans == "")
            {
                Console.WriteLine("Вы ввели некорректный ответ! Пожалуйста, введи ответ правильно.");
                ans = Console.ReadLine();
            }
            while (count == 0 && int.Parse(ans) == 5)
            {
                Console.WriteLine("Вы использовали всю помощь! Введите ответ на вопрос.");
                ans = Console.ReadLine();
            }
            return int.Parse(ans);
        }
        /// <summary>
        /// Вызов помощи
        /// </summary>
        static int Dopomoga(string[] ans, int chek)
        {
            var r = new Random();
            var num = r.Next(1, 5);
            while (num == chek)
                num = r.Next(1, 5);
            if (num < chek)
                Console.WriteLine($"{num}) {ans[num - 1]}   {chek}) {ans[chek - 1]}");
            else
                Console.WriteLine($"{chek}) {ans[chek - 1]}   {num}) {ans[num - 1]}");
            var a = GetPlayerAnswer(2);
            while (a != num && a != chek || a == 5)
            {
                Console.WriteLine("Вы уже использовали помощь! Введите ответ из предложенных вариантов.");
                a = GetPlayerAnswer(2);
            }
            return a;
        }
        /// <summary>
        /// Игра
        /// </summary>
        static void StartGame()
        {
            Console.WriteLine("Добро пожаловать в викторину! Правила просты - " +
                "чем больше приавильных ответов вы дадите, тем больше баллов получите. Для того, чтобы ответить, " +
                "необходимо ввести номер варианта ответа. Также у вас есть возможность дважды за игру вызвать помощь, " +
                "которая уберёт два неправильных варианта ответа. Для её вызова, необходимо будает нажать цифру '5'. " +
                "После завершения игры, ваш рузультат будет внесён в таблицу лидеров.");

            string[] str = SplitFileByString("bank.txt");
            string[] answer = new string[4];
            int chek;
            int score = 0;
            int playerAns;
            int dopomogaCount = 2;
            for (int i = 0; i < str.Length; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"Вопрос №{i + 1}!");
                Console.WriteLine(GetQuestions(str[i]));
                answer = GetAnswers(str[i]);
                chek = RightAnswer(str[i]);
                for (int j = 0; j < answer.Length; j++)
                    Console.Write($"{j + 1}) {answer[j]}   ");
                Console.WriteLine();
                Console.WriteLine("Ваш ответ?");
                playerAns = GetPlayerAnswer(dopomogaCount);
                if (playerAns == 5 && dopomogaCount > 0)
                {
                    playerAns = Dopomoga(answer, chek);
                    if (playerAns == chek)
                    {
                        --dopomogaCount;
                        score += 5;
                        Console.WriteLine("Верный ответ дали вы");
                    }
                    else
                        Console.WriteLine("Ну ответили неправильно и ответили. Чего бубнеть то?");
                }
                else
                {
                    if (playerAns == chek)
                    {
                        score += 10;
                        Console.WriteLine("Верный ответ дали вы");
                    }
                    else
                        Console.WriteLine("Ну ответили неправильно и ответили. Чего бубнеть то?");
                }
            }
            Console.WriteLine();
            Console.WriteLine($"Поздравляем! Вы завершили викторину. Ваш результат - {score} баллов из 120. " +
                $"Для записи в таблицу лидеров введите, как вас зовут.");
            string name = Console.ReadLine();
            while (name == "")
            {
                Console.WriteLine("Пожалуйста, введите своё имя!");
                name = Console.ReadLine();
            }
            File.AppendAllLines("leaderboard.txt", new string[] { $"{name} - {score}" });
        }
    }
}