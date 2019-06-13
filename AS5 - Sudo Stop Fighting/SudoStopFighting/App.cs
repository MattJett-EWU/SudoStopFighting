using System;
using System.Text.RegularExpressions;

namespace SudoStopFighting
{
	class App
	{
		public static void Main(string[] args)
		{
			Input();
		}

		static void Input()
		{
			int r = 0;
			string lineRead;
			Sudoku SUDOKU = null;
			bool processing = false;

			while ((lineRead = Console.ReadLine()).Trim() != null && lineRead != "0")
			{
				if (Regex.IsMatch(lineRead.Trim(), @"^[0-9]+ [0-9]+$"))
				{
					if (processing)
					{
						r = 0;
						processing = false;
						SUDOKU.SolvePuzzle();
					}
					processing = true;
					string[] firstLine = lineRead.Split(" ", StringSplitOptions.RemoveEmptyEntries);
					if (firstLine[0] == "9")
						Console.Write("");
					SUDOKU = new Sudoku(int.Parse(firstLine[0]), int.Parse(firstLine[1]));
				}
				else
				{
					string[] content = lineRead.Split(" ", StringSplitOptions.RemoveEmptyEntries);
					for (int c = 0; c < content.Length; c++)
						SUDOKU[r, c] = content[c];
					r++;
				}
			}
			if (SUDOKU._boardWidth == 12)
				Console.Write("");
			SUDOKU.SolvePuzzle();

			if (Console.ReadLine() != null)
				Input();
			else
				Environment.Exit(1);
		}
	}
}
