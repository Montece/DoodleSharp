using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace DoodleJump
{
    class Program
    {
        public const string Platform = "-";
        public const string Empty = " ";
        public const string Player = "M";
        public const string PlayerAndPlatform = "♦";
        static bool First = true;

        public static RenderInit render;

        static void Main(string[] args)
        {
            render = new RenderInit();
            Menu();
        }

        static void Test()
        {
            int k = 0;
            char[] letters = Enumerable.Range('a', 'z' - 'a' + 1).Select(c => (char)c).ToArray();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write(letters[k]);
                    if (k < letters.Length - 1) k++;
                }
            }
            Console.ReadLine();
        }

        public static void Menu()
        {
            Console.Clear();
            Draw("DOODLE JUMP", ConsoleColor.Green);
            Console.WriteLine("");
            Console.WriteLine("");
            Draw("(0) PLAY", ConsoleColor.Yellow);
            Console.WriteLine("");
            Draw("(1) ABOUT", ConsoleColor.Yellow);
            Console.WriteLine("");
            Draw("(2) EXIT", ConsoleColor.Yellow);
            Console.WriteLine("");

            bool b = true;

            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.NumPad0: ChooseDifficulty(); b = false; break;
                case ConsoleKey.D0: ChooseDifficulty(); b = false; break;
                case ConsoleKey.NumPad1: Process.Start("https://ru.wikipedia.org/wiki/Doodle_Jump"); break;
                case ConsoleKey.D1: Process.Start("https://ru.wikipedia.org/wiki/Doodle_Jump"); break;
                case ConsoleKey.NumPad2: b = false; break;
                case ConsoleKey.D2: b = false; break;
            }
            if (b) Menu();
        }

        public static void ChooseDifficulty()
        {
            Console.Clear();
            Draw("DOODLE JUMP", ConsoleColor.Green);
            Console.WriteLine("");
            Console.WriteLine("");
            Draw("(0) EASY", ConsoleColor.Yellow);
            Console.WriteLine("");
            Draw("(1) NORMAL", ConsoleColor.Yellow);
            Console.WriteLine("");
            Draw("(2) HARD", ConsoleColor.Yellow);
            Console.WriteLine("");
            Draw("(3) BACK", ConsoleColor.Yellow);
            Console.WriteLine("");

            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.NumPad0: Start(300); break;
                case ConsoleKey.D0: Start(300); break;
                case ConsoleKey.NumPad1: Start(200); break;
                case ConsoleKey.D1: Start(200); break;
                case ConsoleKey.NumPad2: Start(100); break;
                case ConsoleKey.D2: Start(100); break;
                case ConsoleKey.NumPad3: Menu(); break;
                case ConsoleKey.D3: Menu(); break;
            }
        }

        public static void Start(int speed)
        {
            int x = 30;
            int y = 20;
            string[,] Field = new string[x, y];

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Field[i, j] = Empty;
                }
            }

            for (int j = 0; j < x; j++)
            {
                Field[j, y - 1] = Platform;
            }
            render = new RenderInit();
            render.render = new Render(x, y, Field);
            render.render.DELAY = speed;

            if (First)
            {
                new Thread(GetInput).Start();
                First = false;
            }
        }

        public static void Draw(object text, ConsoleColor color)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = temp;
        }

        static void GetInput()
        {
            while (!render.render.IsLose)
            {
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.RightArrow: render.render.GetInput(0); break;
                    case ConsoleKey.LeftArrow: render.render.GetInput(1); break;
                    case ConsoleKey.Spacebar: render.render.GetInput(2); break;
                }
                Thread.Sleep(render.render.DELAY);
            }
        }
    }
}
