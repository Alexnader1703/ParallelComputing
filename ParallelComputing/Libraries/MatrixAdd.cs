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
        public static long SequentialSum(long[,] matrix)
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
        public static long PyramidalSumThread(long[,] matrix, int numThreads)
        {
            try
            {
                int rows = matrix.GetLength(0);
                int cols = matrix.GetLength(1);
                int totalElements = rows * cols;
                int elementsPerThread = (totalElements + numThreads - 1) / numThreads;

                long[] partialSums = new long[numThreads];
                Thread[] threads = new Thread[numThreads];

                for (int t = 0; t < numThreads; t++)
                {
                    int threadIndex = t;
                    threads[t] = new Thread(() =>
                    {
                        int start = threadIndex * elementsPerThread;
                        int end = Math.Min(start + elementsPerThread, totalElements);
                        long localSum = 0;

                        for (int k = start; k < end; k++)
                        {
                            int i = k / cols;
                            int j = k % cols;
                            localSum += matrix[i, j];
                        }

                        partialSums[threadIndex] = localSum;
                    });
                    threads[t].Start();
                }

                foreach (Thread thread in threads)
                {
                    thread.Join();
                }

                // Пирамидальное суммирование
                while (numThreads > 1)
                {
                    int halfThreads = numThreads / 2;
                    for (int i = 0; i < halfThreads; i++)
                    {
                        int j = i + halfThreads;
                        if (j < numThreads)
                        {
                            partialSums[i] += partialSums[j];
                        }
                    }
                    numThreads = halfThreads;
                }

                return partialSums[0];
            }
            catch (Exception)
            {
                return -999999999;
            }
        }

        public static long PyramidalSumTask(long[,] matrix, int numThreads)
        {
            try
            {
                int rows = matrix.GetLength(0);
                int cols = matrix.GetLength(1);
                int totalElements = rows * cols;
                int elementsPerTask = (totalElements + numThreads - 1) / numThreads;

                long[] partialSums = new long[numThreads];
                Task[] tasks = new Task[numThreads];

                for (int t = 0; t < numThreads; t++)
                {
                    int taskIndex = t;
                    tasks[t] = Task.Run(() =>
                    {
                        int start = taskIndex * elementsPerTask;
                        int end = Math.Min(start + elementsPerTask, totalElements);
                        long localSum = 0;

                        for (int k = start; k < end; k++)
                        {
                            int i = k / cols;
                            int j = k % cols;
                            localSum += matrix[i, j];
                        }

                        partialSums[taskIndex] = localSum;
                    });
                }

                Task.WaitAll(tasks);

                // Пирамидальное суммирование
                return Task.Run(() =>
                {
                    int activeThreads = numThreads;
                    while (activeThreads > 1)
                    {
                        int halfThreads = activeThreads / 2;
                        Task[] sumTasks = new Task[halfThreads];
                        for (int i = 0; i < halfThreads; i++)
                        {
                            int index = i;
                            sumTasks[i] = Task.Run(() =>
                            {
                                int j = index + halfThreads;
                                if (j < activeThreads)
                                {
                                    partialSums[index] += partialSums[j];
                                }
                            });
                        }
                        Task.WaitAll(sumTasks);
                        activeThreads = halfThreads;
                    }
                    return partialSums[0];
                }).Result;
            }
            catch (Exception)
            {
                return -999999999;
            }
        }

        public static long[,] GenerateRandomMatrix(int rows, int cols)
        {
            Random rand = new Random();
            long[,] matrix = new long[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = rand.Next(1, 100);
                }
            }

            return matrix;
        }

        public static void PrintMatrix(long[,] matrix)
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
