using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelComputing.Libraries
{
    public static class MatrixAdd
    {
        public static long SequentialSum(int[,] matrix)
        {
            long sum = 0;
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sum += matrix[i, j];
                }
            }

            return sum;
        }
        public static long ParallelSum(int[,] matrix, int numThreads)
        {
            try
            {
                int rows = matrix.GetLength(0);
                int cols = matrix.GetLength(1);
                long sum = 0;

                Thread[] threads = new Thread[numThreads];
                object lockObject = new object();

                for (int t = 0; t < numThreads; t++)
                {
                    int threadIndex = t;
                    threads[t] = new Thread(() =>
                    {
                        long localSum = 0;
                        for (int i = threadIndex; i < rows; i += numThreads)
                        {
                            for (int j = 0; j < cols; j++)
                            {
                                localSum += matrix[i, j];
                            }
                        }
                        lock (lockObject)
                        {
                            sum += localSum;
                        }
                    });
                    threads[t].Start();
                }

                foreach (Thread thread in threads)
                {
                    thread.Join();
                }

                return sum;
            }
            catch (Exception)
            {
                return -999999999;
            }
        }


        public static long ParallelSum2(int[,] matrix, int numThreads)
        {
            try
            {
                int rows = matrix.GetLength(0);
                int cols = matrix.GetLength(1);
                long sum = 0;

                Task[] tasks = new Task[numThreads];

                for (int t = 0; t < numThreads; t++)
                {
                    int threadIndex = t;
                    tasks[t] = Task.Run(() =>
                    {
                        long localSum = 0;
                        for (int i = threadIndex; i < rows; i += numThreads)
                        {
                            for (int j = 0; j < cols; j++)
                            {
                                localSum += matrix[i, j];
                            }
                        }
                        Interlocked.Add(ref sum, localSum);
                    });
                }

                Task.WaitAll(tasks);

                return sum;

            }
            catch (Exception)
            {
                
                return -999999999;
            }
            
        }
        

        public static int[,] GenerateRandomMatrix(int rows, int cols)
        {
            Random rand = new Random();
            int[,] matrix = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = rand.Next(1, 100);
                }
            }

            return matrix;
        }

        public static void PrintMatrix(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write($"{matrix[i, j],4}");
                }
                Console.WriteLine();
            }
        }

    }
}
