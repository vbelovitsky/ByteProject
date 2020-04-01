using System;
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

        /// <summary>
        /// Запускает меню
        /// </summary>
        /// <param name="position"></param>
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
            } while (consoleKey != ConsoleKey.Escape);
        }

        private static string[] menuItems =
        {
            "Write numbers to file.",
            "Read numbers from file.",
            "Increase numbers in file.",
            "Read reversed numbers.",
            "Fill random numbers.",
            "Change random numbers."
        };

        /// <summary>
        /// Рисует меню
        /// </summary>
        /// <param name="position">Позиция курсора</param>
        static void DrawMenu(int position)
        {
            StringBuilder menu = new StringBuilder();
            menu.Append("Choose action:").Append(Environment.NewLine);
            for (int i = 0; i < MENU_ITEMS + 1; i++)
                menu.Append(position == i ? "[x]" : "[ ]").Append(" ").Append(menuItems[i]).Append(Environment.NewLine);

            menu.Append(Environment.NewLine).Append("Press Esc button to exit from menu: ");

            Console.WriteLine(menu.ToString());
        }

        /// <summary>
        /// Записывает в файл последовательность из пяти чисел
        /// </summary>
        static void WriteToFile()
        {
            Console.Clear();
            FileStream fileStream = new FileStream(FILE_PATH, FileMode.Create);
            CustomByteWriter writer = new CustomByteWriter(fileStream);

            int[] arr = new int[5];
            for (int i = 0; i < arr.Length; i++)
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

        /// <summary>
        /// Умножает числа на введенный множитель
        /// </summary>
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

        /// <summary>
        /// Считывает числа из файла в обратном порядке
        /// </summary>
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

        /// <summary>
        /// Создает 10 рандомных чисел и записывает в файл
        /// </summary>
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

        /// <summary>
        /// Позволяет пользователю менять числа
        /// </summary>
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
                for (int i = 0; i < arr.Length; i++)
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
            } while (Console.ReadKey().Key != ConsoleKey.Escape);


            Console.Write("Press any button to return to menu: ");
            Console.ReadKey();
        }

        /// <summary>
        /// Метод для корректного ввода целого числа
        /// </summary>
        /// <param name="name">Имя числа</param>
        /// <param name="min">минимальное значение</param>
        /// <param name="max">максимальное значение</param>
        /// <returns></returns>
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

        /// <summary>
        /// Точка входа программы
        /// </summary>
        static void Main()
        {
            do
            {
                Console.Clear();

                RunMenu(0);

                Console.Clear();

                Console.Write("Press Esc button to exit: ");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
    }
}