using ParallelComputing.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace ParallelComputing
{
    internal class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mas"></param>
        /// <param name="method_count">Sequential, Task or Thread</param>
        /// <param name="NumThread"></param>
        static void parallAdd(long[,]mas, string method_count= "Sequential", int NumThread = 0)
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
                Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds}\n мс кол-во потоков:{NumThread}\n резульат:{sum}\n\n");

        }
        

        static void lab2(int rows, int cols)
        {
            long[,] mas = MatrixAdd.GenerateRandomMatrix(rows, cols);
            Console.WriteLine($"Массив размером {rows * cols} [{rows}:{cols}] \n");
            parallAdd(mas);
            parallAdd(mas, "Task",2);
            parallAdd(mas, "Thread",2);
           

        }
        
        static void Main(string[] args)
        {
            lab2(10000, 10000);
            Console.ReadKey();
        }
    }
}
