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
        static void parallAdd(int[,]mas, int NumThread = 0)
        {
            Stopwatch stopwatch = new Stopwatch();
            long sum = 0;
            stopwatch.Start();
            if (NumThread == 0)
            {
                sum = MatrixAdd.SequentialSum(mas);
            }
            else
            {
                sum = MatrixAdd.ParallelSum(mas,NumThread);

            }
            stopwatch.Stop();
            if (sum == -999999999)
                Console.WriteLine("Ошибочка");
            else
                Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds}\n мс кол-во потоков:{NumThread}\n резульат:{sum}\n\n");

        }
        

        static void lab2(int rows, int cols)
        {
            int[,] mas = MatrixAdd.GenerateRandomMatrix(rows, cols);
            Console.WriteLine($"Массив размером {rows * cols} [{rows}:{cols}] \n");
            parallAdd(mas);
            parallAdd(mas,3);
            parallAdd(mas,10);

        }
        static void Main(string[] args)
        {
            lab2(10000, 10000);
            Console.ReadKey();
        }
    }
}
