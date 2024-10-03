using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelComputing.Libraries
{
    static class MatrixAdd
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
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            long sum = 0;

            Parallel.For(0, rows, new ParallelOptions { MaxDegreeOfParallelism = numThreads },
                () => 0L,
                (i, loop, localSum) =>
                {
                    for (int j = 0; j < cols; j++)
                    {
                        localSum += matrix[i, j];
                    }
                    return localSum;
                },
                localSum => Interlocked.Add(ref sum, localSum)
            );

            return sum;
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
