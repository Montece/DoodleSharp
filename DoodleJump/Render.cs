using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DoodleJump
{
    public class Render
    {
        public string[,] Field;
        int x;
        int y;
        int PlayerX;
        int PlayerY;
        public bool IsLose = false;
        int PlayerForceY = 0;
        int PlayerForceX = 0;
        public int DELAY = 400;
        Random rand;
        string Temp = " ";
        int TempX = 0;
        int TempY = 0;
        int TempWait = -1;

        int PlatformY = 2;
        static int PlatformStepY = 2;
        int PlatformForce = PlatformStepY * 3;

        public Render(int x, int y, string[,] field)
        {
            Field = field;
            this.x = x;
            this.y = y;
            Console.CursorVisible = false;
            rand = new Random();

            SpawnPlayer(x / 2, y / 2, Program.Player);
            PlatformY = 0;
            new Thread(Rendering).Start();
            CreatePlatform(y / PlatformStepY);
        }

        public void SpawnPlayer(int x, int y, string Player)
        {
            PlayerX = x / 2;
            PlayerY = y / 2;
            Field[PlayerX, PlayerY] = Player;
        }

        public void Rendering()
        {
            while (!IsLose)
            {
                if (PlayerY >= y - 1 && PlayerY != 0) Lose();
                if (PlayerY <= 0)
                {
                    if (Field[PlayerX, y - 8] == Program.Platform)
                    {
                        Field[PlayerX, y - 8] = Program.PlayerAndPlatform;
                        Temp = Program.Platform;
                        TempX = PlayerX;
                        TempY = y - 8;
                        TempWait = 1;
                    }
                    Field[PlayerX, PlayerY] = Program.Empty;
                    PlayerY = y - 8;
                }
                else
                {
                    if (Field[PlayerX, PlayerY + 1] == Program.Platform)
                    {
                        PlayerForceY = PlatformForce;
                        CorrectPlatforms();
                    }

                    Field[PlayerX, PlayerY] = Program.Empty;

                    if (PlayerForceY > 0)
                    {
                        PlayerForceY--;
                        PlayerY--;
                    }
                    else PlayerY++;

                    if (PlayerForceX != 0)
                    {
                        if (PlayerForceX > 0)
                        {
                            PlayerX--;
                            PlayerForceX--;
                        }
                        if (PlayerForceX < 0)
                        {
                            PlayerX++;
                            PlayerForceX++;
                        }
                        if (PlayerX >= x)
                        {
                            PlayerX = 2;
                        }
                        else if (PlayerX <= 1)
                        {
                            PlayerX = x - 1;
                        }
                    }

                    if (Field[PlayerX, PlayerY] == Program.Platform)
                    {
                        Field[PlayerX, PlayerY] = Program.PlayerAndPlatform;
                        Temp = Program.Platform;
                        TempX = PlayerX;
                        TempY = PlayerY;
                        TempWait = 1;
                    }
                    else Field[PlayerX, PlayerY] = Program.Player;
                }
           
                Console.Clear();
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        if (i == 0 || i == x - 1)
                        {
                            Console.SetCursorPosition(i + 1, j + 1);
                            Program.Draw("*", ConsoleColor.Red);
                            Console.SetCursorPosition(i, j);
                        }
                        if (j == 0 || j == y - 1)
                        {
                            Console.SetCursorPosition(i + 1, j + 1);
                            Program.Draw("*", ConsoleColor.Red);
                            Console.SetCursorPosition(i, j);
                        }
                        if (Field[i, j] == Program.Platform) Program.Draw(Field[i, j], ConsoleColor.Blue);
                        if (Field[i, j] == Program.PlayerAndPlatform) Program.Draw(Field[i, j], ConsoleColor.Yellow);
                        if (Field[i, j] == Program.Player) Program.Draw(Field[i, j], ConsoleColor.Green);
                        Console.SetCursorPosition(i, j);
                    }
                }
                if (TempWait > 0) TempWait--;
                else if (TempWait == 0)
                {
                    Field[TempX, TempY] = Temp;
                    Console.SetCursorPosition(TempX, TempY);
                    Console.Write(Temp);
                    TempWait = -1;
                }
                Thread.Sleep(DELAY);
            }
        }

        public void GetInput(int k)
        {
            if (k == 0) PlayerForceX--;
            if (k == 1) PlayerForceX++;
            if (k == 2) PlayerForceX = 0;
        }

        void Lose()
        {
            if (!IsLose)
            {
                IsLose = true;
                Console.Clear();
                Console.WriteLine("You Lose!");
                Console.ReadLine();

                Program.Start(DELAY);
            }
        }

        void CreatePlatform(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                PlatformY += PlatformStepY;
                Field[rand.Next(2, x), y - PlatformY] = Program.Platform;
            }            
        }

        void CorrectPlatforms()
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (Field[i, j] == Program.Platform)
                    {
                        Field[i, j] = Program.Empty;
                        if (j + PlatformStepY < y) Field[i, j + PlatformStepY] = Program.Platform;
                        j += PlatformStepY;
                    }
                }
            }
            PlatformY = y - PlatformStepY;
            CreatePlatform(1);
        }
    }
}
