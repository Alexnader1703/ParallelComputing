using ParallelComputing.Libraries;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ParallelComputing
{
    internal class Program
    {
        
        static long parallAdd(long[,] mas, string method_count = "Sequential", int NumThread = 0)
        {
            Stopwatch stopwatch = new Stopwatch();
            long sum = 0;
            stopwatch.Start();
            switch (method_count)
            {
                case "Sequential":
                    sum = MatrixAdd.SequentialSum(mas);
                    break;
                case "Task":
                    sum = MatrixAdd.ParallelSumTask(mas, NumThread);
                    break;
                case "Thread":
                    sum = MatrixAdd.ParallelSumThread(mas, NumThread);
                    break;
                default:
                    Console.WriteLine("Нету такого метода");
                    break;
            }
            stopwatch.Stop();
            if (sum == -999999999)
                Console.WriteLine("Ошибочка");
            else
                Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс, кол-во потоков: {NumThread}, результат: {sum}\n");
            return stopwatch.ElapsedMilliseconds;
        }

        static void lab2(int rows, int cols)
        {
            long[,] mas = MatrixAdd.GenerateRandomMatrix(rows, cols);
            Console.WriteLine($"Массив размером {rows * cols} [{rows}:{cols}] \n");

    
            long sequentialTime = parallAdd(mas, "Sequential");
            long taskTime = parallAdd(mas, "Task", 2);
            long threadTime = parallAdd(mas, "Thread", 2);

            SaveResultsToFile(sequentialTime, taskTime, threadTime, rows * cols);
        }

        static void SaveResultsToFile(long sequentialTime, long taskTime, long threadTime, int numberOfElements)
        {
            string filePath = "results.csv";

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "SequentialTime,TaskTime,ThreadTime,NumberOfElements\n");
            }

            string line = $"{sequentialTime},{taskTime},{threadTime},{numberOfElements}\n";
            File.AppendAllText(filePath, line);
        }

        static void Main(string[] args)
        {
            if (File.Exists("results.csv"))
                File.Delete("results.csv");

            for (int i = 1000; i <= 16000; i += 1000)
            {
                lab2(i, i);
            }

            Console.WriteLine("Результаты сохранены в файл results.csv");
            Console.ReadKey();
        }
    }
}
