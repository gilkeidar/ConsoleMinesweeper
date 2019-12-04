using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
	class Game
	{
		int bombs;
		static int difficulty = 3; //easy
		static int width = difficulty * 4;
		static int height = difficulty * 3;
		static int[,] map = new int[width, height]; //1 = bomb, 0 = no bomb
		static bool[,] visibleSquares = new bool[width, height]; //false = invisible ("?"), true = visible (" ", 1-8, or * for exploded bomb)
		static char[,] printedMap = new char[width, height];
		static int[,] adjacentBombs = new int[width, height];
		static Random rand = new Random();

		void MainGameLoop()
		{
			while (true)
			{

			}
		}

		public void GameSet()
		{
			bombs = difficulty * 5;
			int bombsToPlace = bombs;
			/*for(int j = 0; j < height; j++)
			{
				for (int i = 0; i < width; i++)
				{
					double bombPlacer = rand.NextDouble();

					if (bombPlacer >= 0.5 && bombsToPlace > 0)
					{
						map[i, j] = 1;
						bombsToPlace -= 1;
					}
				}
			}*/
			for(int i = 1; i <= bombs; i++)
			{
				int x;
				int y;
				do
				{
					x = rand.Next(0, width);
					y = rand.Next(0, height);
				} while (map[x, y] == 1);
				map[x, y] = 1;

				for (int a = -1; a < 2; a++)
				{
					for (int b = -1; b < 2; b++)
					{
						if (a == 0 && b == 0)
						{
							adjacentBombs[x, y] = 0;
						}
						else if ((a + x) >= 0 && (a + x) < width && (b + y) >= 0 && (b + y) < height && map[a+x,b+y] == 0)
						{
							adjacentBombs[a+x,b+y] += 1;
						}
					}
				}
			}
			setVisibleScreen();
			//SetAdjacentBombsArray();
			PrintScreen();
			Console.WriteLine();
			PrintMapArray();
			Console.WriteLine();
			PrintAdjacentBombsArray();
			string input = Console.ReadLine();
		}

		void PrintScreen()
		{
			for(int j = 0; j < height; j++)
			{
				for(int i = 0; i < width; i++)
				{
					Console.Write(printedMap[i, j]);
				}
				Console.WriteLine();
			}

		}
		void PrintAdjacentBombsArray()
		{
			for (int j = 0; j < height; j++)
			{
				for (int i = 0; i < width; i++)
				{
					Console.Write(adjacentBombs[i, j]);
				}
				Console.WriteLine();
			}
		}
		void PrintMapArray()
		{
			for (int j = 0; j < height; j++)
			{
				for (int i = 0; i < width; i++)
				{
					Console.Write(map[i, j]);
				}
				Console.WriteLine();
			}
		}

		void setVisibleScreen()
		{
			for (int j = 0; j < height; j++)
			{
				for (int i = 0; i < width; i++)
				{
					if (visibleSquares[i, j] == true)
					{
						//if there are bombs adjacent to the current square, printedMap[i,j] = that number.
						//if there are no bombs adjacent to the current square, printedMap[i,j] = ' ';
						if (adjacentBombs[i, j] != 0)
						{
							printedMap[i, j] = Convert.ToChar(adjacentBombs[i, j]);
						}
						else
						{
							printedMap[i, j] = ' '; //or '.'
						}
					}
					else
					{
						printedMap[i, j] = '?';
					}
				}
			}
		}

		void SetAdjacentBombsArray()
		{
			int adjacentBombsSum = 0;
			for(int j = 0; j < height; j++)
			{
				for(int i = 0; i < width; i++)
				{
					if (map[i, j] == 0)
					{
						for (int a = -1; a < 2; a++)
						{
							for (int b = -1; b < 2; b++)
							{
								if (a == 0 && b == 0)
								{
									adjacentBombsSum += 0;
								}
								else if ((a + j) >= 0 && (a + j) < height && (b + i) >= 0 && (b + i) < width && map[b + i, a + j] == 1)
								{
									adjacentBombsSum += 1;
								}
							}
						}
					}
					else
					{
						adjacentBombsSum = 0;
					}
					adjacentBombs[i, j] = adjacentBombsSum;
					adjacentBombsSum = 0;
				}
			}
		}

		void GameStart()
		{
			Console.WriteLine("MineSweeper");
			Console.WriteLine("Welcome to MineSweeper!");

			Console.Write("Please select a difficulty (easy, medium, or hard): ");
			string input = Console.ReadLine();
			switch (input)
			{
				case "easy":
					difficulty = 0;
					break;
				case "medium":
					difficulty = 1;
					break;
				case "hard":
					difficulty = 2;
					break;
			}
		}
	}
	class Program
	{
		static void Main(string[] args)
		{
			Game game = new Game();
			game.GameSet();
		}
	}
}
