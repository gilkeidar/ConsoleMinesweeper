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
		bool playing = true;
		bool won = false;
		static int difficulty; //easy
		static int width;// = difficulty * 4;
		static int height;// = difficulty * 3;
		static int[,] map;// = new int[width, height]; //1 = bomb, 0 = no bomb
		static bool[,] visibleSquares;// = new bool[width, height]; //false = invisible ("?"), true = visible (" ", 1-8, or * for exploded bomb)
		static char[,] printedMap;// = new char[width, height];
		static int[,] adjacentBombs;// = new int[width, height];
		static bool[,] markedSquares;
		static bool[,] checkedBoxes;
		static Random rand = new Random();

		public void MainGameLoop()
		{
			PrintScreen();
			Console.WriteLine();
			Console.Write("Enter a location to start the game (letternumber) (e.g. b3) ");
			string input;
			bool firstMove = true;
			while (playing)
			{
				bool inputSuccess = false;
				bool findBomb = false;
				int xPosition = 0;
				int yPosition = 0;
				while (inputSuccess == false)
				{
					if(firstMove == false)
					{ 
						Console.Write("What square do you wish to show? ");
					}
					input = Console.ReadLine();
					Console.WriteLine(input);
					if (input.Length >= 2)
					{
						int i = 0;
						if(input[0] == '=')
						{
							findBomb = true;
							i = 1;
						}
						xPosition = (int)input[0 + i] - 97;
						if (difficulty == 5 && ((i == 0 && input.Length == 3) || (i == 1 && input.Length == 4)))
						{
							int firstDigit = (int)input[1 + i] - 48;
							int secondDigit = (int)input[2 + i] - 49;
							yPosition = (firstDigit * 10) + secondDigit;
						}
						else
						{
							yPosition = (int)input[1 + i] - 49;
						}
						if (xPosition >= 0 && xPosition < width && yPosition >= 0 && yPosition < height)
						{
							if (i == 1)
							{
								if (markedSquares[xPosition, yPosition] == true)
								{
									markedSquares[xPosition, yPosition] = false;
								}
								else
								{
									markedSquares[xPosition, yPosition] = true;
								}
							}
							inputSuccess = true;
							firstMove = false;

						}
						/*char initialInput = input[0];
						char xInput = input[0];
						int yInput = Convert.ToInt32(input[1]);
						bool findBomb = false;
						int xPosition = 0;
						int yPosition = 0;
						if (initialInput == 's')
						{
							findBomb = true;
							xInput = input[1];
							yInput = Convert.ToInt32(input[2]);
							int xPositionBomb = (int)xInput - 97;
							int yPositionBomb = yInput - 49;
							if (xPositionBomb >= 0 && xPositionBomb < width && yPositionBomb >= 0 && yPositionBomb < height)
							{
								if (markedSquares[xPositionBomb, yPositionBomb] == true) //if already marked
								{
									markedSquares[xPositionBomb, yPositionBomb] = false;
								}
								else
								{
									markedSquares[xPositionBomb, yPositionBomb] = true;
								}
							}
						}
						else
						{
							xPosition = (int)xInput - 97;
							yPosition = yInput - 49;
						}
						Console.WriteLine(xPosition + "," + yPosition);*/
					}
					if (firstMove == true)
					{
						Console.Write("Enter a location to start the game (letternumber) (e.g. b3) ");
					}
					//Console.Read();
				}

				if (map[xPosition, yPosition] == 1 && findBomb == false && markedSquares[xPosition, yPosition] == false)
				{
					for (int i = 0; i < width; i++)
					{
						for (int j = 0; j < height; j++)
						{
							if (map[i, j] == 1)
							{
								visibleSquares[i, j] = true;
							}
						}
					}
					Console.Clear();
					setVisibleScreen();
					PrintScreen();
					Console.WriteLine("Game Over!");
					Console.Read();
					System.Environment.Exit(1);
					
				}
				else if (findBomb == true) {
					
					setVisibleScreen();
					
				}
				else if(findBomb == false && markedSquares[xPosition,yPosition] == true)
				{
				}
				else if (visibleSquares[xPosition, yPosition] == false)
				{
					//begin search starting from point to all points that have no bombs next to them and end at points that do
					//have bombs adjacent to them; essentially make a shape that's filled with empty space and is bordered by squares
					//that have bombs adjacent to them.
					showSquares(xPosition, yPosition);
					//visibleSquares[xPosition, yPosition] = true;
					setVisibleScreen();
				}
				Console.Clear();
				PrintScreen();

				//Console.WriteLine("FinishedLoop!");
				IfWon();
				//Console.Write("What square do you wish to show? ");
				//Console.Read();
			}
		}
		public void IfWon()
		{
			if (won)
			{
				Console.WriteLine("You Won!");
				Console.Read();
				System.Environment.Exit(1);
			}
		}
		public void showSquares(int x, int y)
		{
			if (adjacentBombs[x, y] == 0)
			{
				//checkedBoxes array
				for (int i = -1; i <= 1; i++)
				{
					for (int j = -1; j <= 1; j++)
					{
						if ((x + i) >= 0 && (x + i) < width && (y + j) >= 0 && (y + j) < height)
						{
							if (checkedBoxes[x + i, y + j] == false)
							{
								checkedBoxes[x + i, y + j] = true;
								if (adjacentBombs[x + i, y + j] == 0 && map[x + i, y + j] == 0)
								{
									visibleSquares[x + i, y + j] = true;
									showSquares(x + i, y + j);
								}
								else if (adjacentBombs[x + i, y + j] != 0 && map[x + i, y + j] == 0)
								{
									visibleSquares[x + i, y + j] = true;
								}
							}
						}
					}
				}
			}
			else
			{
				visibleSquares[x, y] = true;
			}
			//loop on eight boxes around the user's chosen box
			//check that each box is within the screen, hasn't been checked already, and whether it's equal to 0 or not
			//if equal to 0, call the function for that box
			//if not equal to 0, skip to the next box
		}
		public void GameSet()
		{
			//bombs = difficulty * 5;
			int bombsToPlace = bombs;
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
			/*PrintMapArray();
			Console.WriteLine();
			PrintAdjacentBombsArray();
			Console.WriteLine();
			PrintVisibleSquaresArray();
			string input = Console.ReadLine();*/
		}

		void PrintScreen()
		{
			Console.Write("  ");
			if(height >= 10)
			{
				Console.Write(" ");
			}
			for(int i = 97; i < width + 97; i++)
			{
				string letter = Char.ConvertFromUtf32(i);
				Console.Write(letter + " ");
			}
			Console.WriteLine();
			for(int j = 0; j < height; j++)
			{
				Console.Write((j + 1) + " ");
				if (height >= 10 && j < 9)
				{
					Console.Write(" ");
				}
				for(int i = 0; i < width; i++)
				{
					char currentSquare = printedMap[i, j];
					switch (currentSquare)
					{
						case ' ':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
							Console.ForegroundColor = ConsoleColor.Black;
							Console.BackgroundColor = ConsoleColor.Green;
							break;
						case '*':
							Console.ForegroundColor = ConsoleColor.Black;
							Console.BackgroundColor = ConsoleColor.Red;
							break;
						case '!':
							Console.ForegroundColor = ConsoleColor.Black;
							Console.BackgroundColor = ConsoleColor.Blue;
							break;
						default:
							Console.ResetColor();
							break;
					}
					
					Console.Write(printedMap[i, j] + " ");
					Console.ResetColor();
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
		void PrintVisibleSquaresArray()
		{

			for (int j = 0; j < height; j++)
			{
				for (int i = 0; i < width; i++)
				{
					Console.Write(visibleSquares[i, j]);
				}
				Console.WriteLine();
			}
		}

		void setVisibleScreen()
		{
			won = true;
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
							printedMap[i, j] = Convert.ToChar(adjacentBombs[i, j]+48);
						}
						else if (map[i,j] == 1)
						{
							printedMap[i, j] = '*';
						}
						else
						{
							printedMap[i, j] = ' '; //or '.'
						}
					}
					else if (markedSquares[i,j] == true)
					{
						printedMap[i, j] = '!';
					}
					else
					{
						won = false;
						printedMap[i, j] = '?';
					}
				}
			}
		}

		public void GameStart()
		{
			Console.WriteLine("MineSweeper");
			Console.WriteLine("Welcome to MineSweeper!");

			Console.Write("Please select a difficulty (easy, medium, or hard): ");
			bool inputAnswered = false;
			while (!inputAnswered)
			{
				string input = Console.ReadLine();
				switch (input)
				{
					case "easy":
						difficulty = 1;
						bombs = 2;
						inputAnswered = true;
						break;
					case "medium":
						difficulty = 3;
						bombs = 18;
						inputAnswered = true;
						break;
					case "hard":
						difficulty = 5;
						bombs = 50;
						inputAnswered = true;
						break;
					default:
						Console.WriteLine("Please choose between easy, medium, and hard.");
						break;
				}
			}
			//bombs = difficulty * 2;
			width = difficulty * 4;
			height = difficulty * 3;
			map = new int[width, height]; //1 = bomb, 0 = no bomb
			visibleSquares = new bool[width, height]; //false = invisible ("?"), true = visible (" ", 1-8, or * for exploded bomb)
			printedMap = new char[width, height];
			adjacentBombs = new int[width, height];
			markedSquares = new bool[width, height];
			checkedBoxes = new bool[width, height];

		}
	}
	class Program
	{
		static void Main(string[] args)
		{
			Game game = new Game();
			game.GameStart();
			game.GameSet();
			game.MainGameLoop();
			
		}
	}
}
