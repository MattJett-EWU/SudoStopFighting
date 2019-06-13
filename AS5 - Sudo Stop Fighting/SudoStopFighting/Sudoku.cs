using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Radix;

namespace SudoStopFighting
{
	#region SUDOKU CLASS
	class Sudoku
	{
		#region CLASS UTILITIES
		// PROPERTIES
		bool _solved { get; set; }
		internal int _boardWidth { get; set; }
		int _subsectionWidth { get; set; }
		int _subsectionHeight { get; set; }
		string[,] _board { get; set; }

		// INDEXER
		public string this[int r, int c]
		{
			get => _board[r, c];
			set => _board[r, c] = value;
		}

		// CONSTRUCTOR
		public Sudoku(int n, int w)
		{
			_solved = true;
			_boardWidth = n;
			_subsectionWidth = w;
			_subsectionHeight = n / w;
			_board = new string[n, n];
		}
		#endregion


		#region PUBLIC METHODS
		/// <summary>
		///		This method starts the process to solve the puzzle.
		/// </summary>
		/// <returns>Return true or false depending on if puzzle was solvable or not.</returns>
		internal void SolvePuzzle()
		{
			if (_board != null)
			{
				/*DELETE ME*/
				if (_boardWidth == 12)
					Console.WriteLine("");
				if (Assign(0, 0))
					Console.WriteLine("Solved:\n\n" + Print());
				else
					Console.WriteLine("CAN'T, WON'T\n");
			}
			// TODO: throw error here?
		}
		#endregion

		#region PRIVATE METHODS
		/// <summary>
		///		This is the recursively called method that places values on the board or increments the cell to the next one.
		/// </summary>
		/// <param name="cellValue">Value in cell</param>
		/// <param name="r">Row</param>
		/// <param name="c">Column</param>
		/// <returns>Returns true if a successful placement of a valid value in a cell. False if called recursively to increment or backtrack.</returns>
		protected bool Assign(int r, int c)
		{
			// Find next zero : switches _solved = false if "0" detected. Otherwise true...
			var o = ZeroFinder(r, c).Select((int[] arr) => new { r = arr[0], c = arr[1] }); r = o.r; c = o.c;

			/*DELETE ME*/if (r == 12 && c == 0)
				Console.Write("");

			// Assign or increment cell on board
			for (int v = 1; !_solved && v <= _boardWidth; v++)
			{
				// Duplicate value found in row|column|subsection, not safe to place, try another value.
				if (Array<string>.GetRow(_board, r).Contains(v.ToBase(_boardWidth + 1)) ||
					Array<string>.GetColumn(_board, c).Contains(v.ToBase(_boardWidth + 1)) ||
					Array<string>.GetSubBoard(_board, r, _subsectionWidth, c, _subsectionHeight).Contains(v.ToBase(_boardWidth + 1)))
				{
				}
				// Unique value found, safe to place here.
				else
				{
					_board[r, c] = v.ToBase(_boardWidth + 1);
					_solved = Assign(r, c);
				}
			}

			// IF: exceeded all possible values allowed in cell, puzzle still not solved, then reset cell to 0 and backtrack.
			if (!_solved)
				_board[r, c] = "0";

			return _solved;
		}

		[DebuggerStepThrough]
		int[] ZeroFinder(int r, int c)
		{
			_solved = true;
			while (r < _boardWidth)
			{
				while (c < _boardWidth)
				{
					if (_board[r, c] == "0")
					{
						_solved = false;
						break;
					}
					c++;
				}
				if (!_solved)
					break;
				c = 0;
				r++;
			}
			return new int[] {r,c};
		}
		
		internal string Print()
		{
			string s = "";
			for (int r = 0; r < _boardWidth; r++)
			{
				if (r % _subsectionHeight == 0 && r != 0)
					s += "\n";
				for (int c = 0; c < _boardWidth; c++)
				{
					if (c % _subsectionWidth == 0 && c != 0)
						s += " ";
					s += _board[r, c] + " ";
				}
				s += "\n";
			}
			return s;
		}
	}
	#endregion 
	#endregion


	#region EXTENSION CLASSES
	/// <summary>
	///		Helper static utility class to pull entire row or column of data out of the array by any type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <see cref="https://stackoverflow.com/questions/27427527/how-to-get-a-complete-row-or-column-from-2d-array-in-c-sharp"/>
	internal static class Array<T>
	{
		public static T[] GetRow(T[,] matrix, int lockRow)
		{
			return Enumerable.Range(0, matrix.GetLength(1))
				.Select(c => matrix[lockRow, c])
				.ToArray();
		}

		public static T[] GetColumn(T[,] matrix, int lockColumn)
		{
			return Enumerable.Range(0, matrix.GetLength(0))
					.Select(r => matrix[r, lockColumn])
					.ToArray();
		}

		public static T[] GetSubBoard(T[,] matrix, int absoluteRow, int subsectionWidth, int absoluteColumn, int subsectionHeight)
		{
			var temp = new List<T>();
			int relativeRow = absoluteRow - (absoluteRow % subsectionHeight);
			int relativeColumn = absoluteColumn - (absoluteColumn % subsectionWidth);
			for (int r = relativeRow; r < subsectionHeight + relativeRow; r++)
				for (int c = relativeColumn; c < subsectionWidth + relativeColumn; c++)
					temp.Add(matrix[r, c]); // need actual absolute board position
			return temp.ToArray();
		}
	}

	/// <summary>
	///		Helper static utility class written for me by user "IT WeiHan" on stackoverflow.com.
	///		I ask for help on how to assign 2 variables within a Linq query. And the user gave me this verbatim.
	/// </summary>
	/// <see cref="https://stackoverflow.com/questions/56301662/linq-how-to-assign-local-variables-within-linq-from-array"/>
	internal static class LinqExtension
	{
		public static T Select<T>(this int[] ints, Func<int[], T> func)
		{
			return func(ints);
		}
	}
	#endregion
}
