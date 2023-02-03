using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Pac_Man
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(28, 24);
            char[,] map = ReadMap("map.txt");
            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo();
            Task.Run(() =>
            {
                while(true)
                {
                    pressedKey = Console.ReadKey();
                }
            });
            
            int playerX = 1;
            int playerY = 1;
            bool death = false;

            int cyanGhostX = 13;
            int cyanGhostY = 8;

            int score = 0;
            while (true)
            {
                DrawMap(map);
                DrawEntity(ConsoleColor.Yellow, playerX, playerY, '@');
                DrawEntity(ConsoleColor.Cyan, cyanGhostX, cyanGhostY, 'G');

                Console.SetCursorPosition(0, 23);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"score: {score}");

                Thread.Sleep(500);
                CyanGhostMove(playerX, playerY, ref cyanGhostX, ref cyanGhostY, map);
                PlayerMove(pressedKey, ref playerX, ref playerY, map, ref score);

                Console.Clear();
                death = PlayerIsDead(playerX, playerY, cyanGhostX, cyanGhostY);
                if (death == true)
                {
                    Console.SetCursorPosition(8, 11);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ВЫ ПРОИГРАЛИ");
                    Console.SetCursorPosition(10, 12);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"score: {score}");
                    Console.ReadKey();
                    break;
                }
            }
        }
        static char[,] ReadMap(string path)
        {
            string[] file = File.ReadAllLines(path);

            char[,] map = new char[file[0].Length, file.Length];

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    map[x, y] = file[y][x];
                }
            }
            return map;
        }
        
        static void DrawMap(char[,] map)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    if (map[x, y] == 'o')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.Write(map[x, y]);
                }
                Console.WriteLine();
            }
        }

        static void DrawEntity(ConsoleColor color, int X, int Y, char symbol)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(X, Y);
            Console.WriteLine(symbol);
        }

        static bool PlayerIsDead(int playerX, int playerY, int ghostX, int ghostY)
        {
            if (playerX == ghostX && playerY == ghostY)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        static void PlayerMove(ConsoleKeyInfo pressedKey, ref int playerX, ref int playerY, char[,] map, ref int score)
        {
            int[] direction = {0, 0};
            switch (pressedKey.Key)
            {
                case ConsoleKey.UpArrow: direction[1] = -1;
                    break;
                case ConsoleKey.DownArrow: direction[1] = +1;
                    break;
                case ConsoleKey.LeftArrow: direction[0] = -1;
                    break;
                case ConsoleKey.RightArrow: direction[0] = +1;
                    break;
            }

            int nextPlayerX = playerX + direction[0];
            int nextPlayerY = playerY + direction[1];
            char nextCell = map[nextPlayerX, nextPlayerY];

            if (nextCell != '#' && nextCell != '█')
            {
                playerX = nextPlayerX;
                playerY = nextPlayerY;
            }

            if (nextCell == 'o')
            {
                score++;
                map[nextPlayerX, nextPlayerY] = ' ';
            }
        }
       
        static void CyanGhostMove(int playerX, int playerY, ref int cyanGhostX, ref int cyanGhostY, char[,] map)
        {
            Random rand = new Random();
            int[] direction = {0, 0};
            int modChoice = rand.Next(1, 3);
            int directionСhoice;
            if (modChoice == 1)
            {
                directionСhoice = rand.Next(1, 5);
                switch (directionСhoice)
                {
                    case 1:
                        direction[1] = -1;
                        break;
                    case 2:
                        direction[1] = +1;
                        break;
                    case 3:
                        direction[0] = -1;
                        break;
                    case 4:
                        direction[0] = +1;
                        break;
                }
            }
            else
            {
                if (playerY < cyanGhostY)
                {
                    direction[1] = -1;
                }
                else if (playerY > cyanGhostY)
                {
                    direction[1] = +1;
                }
                if (playerX < cyanGhostX)
                {
                    direction[0] = -1;
                }
                else if (playerX > cyanGhostX)
                {
                    direction[0] = +1;
                }
            }

            int nextX = cyanGhostX + direction[0];
            int nextY = cyanGhostY + direction[1];
            char nextCell = map[nextX, nextY];

            if (nextCell != '█')
            {
                cyanGhostX = nextX;
                cyanGhostY = nextY;
            }

        }
    }
}
