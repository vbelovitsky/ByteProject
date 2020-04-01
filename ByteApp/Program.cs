﻿using System;
using System.IO;
using System.Text;
using ByteLibrary;

namespace ByteApp
{
	class Program
	{
		private static string FILE_PATH = "../../../t.dat";
		private static string FILE_PATH_2 = "../../../Numbers.dat";

		private static Random rnd = new Random();


		private static int MENU_ITEMS = 5; // +1

		static void RunMenu(int position)
		{
			ConsoleKey consoleKey;
			do
			{
				Console.Clear();

				DrawMenu(position);
				
				consoleKey = Console.ReadKey().Key;
				switch (consoleKey)
				{
					case ConsoleKey.UpArrow:
						position = position - 1 < 0 ? 0 : position - 1;
						RunMenu(position);
						break;
					case ConsoleKey.DownArrow:
						position = position + 1 > MENU_ITEMS ? MENU_ITEMS : position + 1;
						RunMenu(position);
						break;

					case ConsoleKey.Enter:
						switch (position)
						{
							case 0:
								WriteToFile();
								break;
							case 1:
								ReadFromFile();
								break;
							case 2:
								IncreaseNumbersInFile();
								break;
							case 3:
								ReadReversedNumbers();
								break;
							case 4:
								FillRandomNumbers();
								break;
							case 5:
								ChangeRandomNumbers();
								break;
						}
						break;
				}

			}
			while (consoleKey != ConsoleKey.Escape);
		}

		static void DrawMenu(int position)
		{
			StringBuilder menu = new StringBuilder();
			menu.Append("Choose action:").Append(Environment.NewLine);
			menu.Append(position == 0 ? "[x]" : "[ ]").Append(" Write numbers to file.").Append(Environment.NewLine);
			menu.Append(position == 1 ? "[x]" : "[ ]").Append(" Read numbers from file.").Append(Environment.NewLine);
			menu.Append(position == 2 ? "[x]" : "[ ]").Append(" Increase numbers in file.").Append(Environment.NewLine);
			menu.Append(position == 3 ? "[x]" : "[ ]").Append(" Read reversed numbers.").Append(Environment.NewLine);
			menu.Append(position == 4 ? "[x]" : "[ ]").Append(" Fill random numbers.").Append(Environment.NewLine);
			menu.Append(position == 5 ? "[x]" : "[ ]").Append(" Change random numbers.").Append(Environment.NewLine);

			menu.Append(Environment.NewLine).Append("Press Esc button to exit from menu: ");

			Console.WriteLine(menu.ToString());
		}

		static void WriteToFile()
		{
			Console.Clear();
			FileStream fileStream = new FileStream(FILE_PATH, FileMode.Create);
			CustomByteWriter writer = new CustomByteWriter(fileStream);

			int[] arr = new int[5];
			for(int i = 0; i < arr.Length; i++)
			{
				arr[i] = i + 1;
			}

			writer.WriteArray(arr);
			writer.Close();
			Console.WriteLine($"Numbers are written to a file: {string.Join(" ", arr)}");
			Console.Write("Press any button to return to menu: ");
			Console.ReadKey();
		}

		static void ReadFromFile()
		{
			Console.Clear();
			FileStream fileStream = new FileStream(FILE_PATH, FileMode.Open);
			CustomByteReader reader = new CustomByteReader(fileStream);

			int[] arr = reader.ReadArray<int>();

			reader.Close();
			Console.WriteLine(string.Join(" ", arr));
			Console.Write("Press any button to return to menu: ");
			Console.ReadKey();
		}

		static void IncreaseNumbersInFile()
		{
			Console.Clear();
			FileStream fileStream = new FileStream(FILE_PATH, FileMode.Open);
			CustomByteReader reader = new CustomByteReader(fileStream);
			int[] arr = reader.ReadArray<int>();
			reader.Close();
			Console.WriteLine($"Initial numbers: {string.Join(" ", arr)}");

			
			fileStream = new FileStream(FILE_PATH, FileMode.Create);
			CustomByteWriter writer = new CustomByteWriter(fileStream);

			int multiplier = ReadInt("multiplier", -10, 10);
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] *= multiplier;
			}

			Console.WriteLine($"Increased numbers: {string.Join(" ", arr)}");
			writer.WriteArray(arr);
			writer.Close();

			Console.Write("Press any button to return to menu: ");
			Console.ReadKey();
		}

		static void ReadReversedNumbers()
		{
			Console.Clear();
			FileStream fileStream = new FileStream(FILE_PATH, FileMode.Open);
			CustomByteReader reader = new CustomByteReader(fileStream);

			int[] arr = reader.ReadReversedArray<int>();

			reader.Close();
			Console.WriteLine(string.Join(" ", arr));
			Console.Write("Press any button to return to menu: ");
			Console.ReadKey();
		}

		static void FillRandomNumbers()
		{
			Console.Clear();
			FileStream fileStream = new FileStream(FILE_PATH_2, FileMode.Create);
			CustomByteWriter writer = new CustomByteWriter(fileStream);

			int[] arr = new int[10];
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = rnd.Next(1, 101);
			}

			writer.WriteArray(arr);
			writer.Close();
			Console.WriteLine($"Random numbers are written to a file: {string.Join(" ", arr)}");
			Console.Write("Press any button to return to menu: ");
			Console.ReadKey();
		}

		static void ChangeRandomNumbers()
		{

			do
			{
				Console.Clear();

				FileStream fileStream = new FileStream(FILE_PATH_2, FileMode.Open);
				CustomByteReader reader = new CustomByteReader(fileStream);
				int[] arr = reader.ReadArray<int>();
				Console.WriteLine(string.Join(" ", arr));
				reader.Close();

				int userNum = ReadInt("number", 1, 100);
				int[] minDist = new int[arr.Length];
				for(int i = 0; i < arr.Length; i++)
				{
					minDist[i] = Math.Abs(arr[i] - userNum);
				}
				int valueMinDist = int.MaxValue;
				int valueMinDistIndex = -1;
				for (int i = 0; i < minDist.Length; i++)
				{
					if (minDist[i] < valueMinDist)
					{
						valueMinDistIndex = i;
						valueMinDist = minDist[i];
					}
				}

				arr[valueMinDistIndex] = userNum;

				// Write changes
				fileStream = new FileStream(FILE_PATH_2, FileMode.Create);
				CustomByteWriter writer = new CustomByteWriter(fileStream);
				writer.WriteArray(arr);
				writer.Close();


				// Read again
				fileStream = new FileStream(FILE_PATH_2, FileMode.Open);
				reader = new CustomByteReader(fileStream);
				arr = reader.ReadArray<int>();
				Console.WriteLine($"Modifyed nums: {string.Join(" ", arr)}");
				reader.Close();


				Console.WriteLine("Press Esc button to exit modifying: ");
			}
			while (Console.ReadKey().Key != ConsoleKey.Escape);


			Console.Write("Press any button to return to menu: ");
			Console.ReadKey();
		}

		static int ReadInt(string name, int min, int max)
		{
			Console.WriteLine($"Input {name} in interval [{min}, {max}]:");
			int num;
			while (!(int.TryParse(Console.ReadLine(), out num) && num >= min && num <= max))
			{
				Console.WriteLine("Try again:");
			}
			return num;
		}

		static void Main(string[] args)
		{

			do
			{
				Console.Clear();

				RunMenu(0);

				Console.Clear();

				Console.Write("Press Esc button to exit: ");
			}
			while (Console.ReadKey().Key != ConsoleKey.Escape);

		}
	}
}