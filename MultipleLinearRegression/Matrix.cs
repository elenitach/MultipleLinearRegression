using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleLinearRegression
{
    static class Matrix
    {
        public static void CopyMatrix(double[,] mFrom, double[,] mTo)
        {
            int numRows = mFrom.GetLength(0);
            int numCols = mFrom.GetLength(1);

            if (mTo.GetLength(0) != numRows || mTo.GetLength(1) != numRows)
                throw new InvalidOperationException();

            for (int i = 0; i < numRows; i++)
                for (int j = 0; j < numCols; j++)
                    mTo[i, j] = mFrom[i, j];
        }

        public static double[,] InversedMatrix(double[,] matrix)
        {
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);
            if (numRows != numCols)
                throw new InvalidOperationException();

            double[,] inversedMatrix = new double[numRows, numCols];
            CopyMatrix(matrix, inversedMatrix);

            int info;
            alglib.matinvreport rep;
            alglib.rmatrixinverse(ref inversedMatrix, out info, out rep);

            return inversedMatrix;
        }

        public static double[,] TransposedMatrix(double[,] matrix)
        {
            int numRows = matrix.GetLength(1);
            int numCols = matrix.GetLength(0);

            double[,] transposedMatrix = new double[numRows, numCols];

            for (int i = 0; i < numRows; i++)
                for (int j = 0; j < numCols; j++)
                {
                    transposedMatrix[i, j] = matrix[j, i];
                }

            return transposedMatrix;
        }

        public static double[,] MultipliedMatrices(double[,] A, double[,] B)
        {
            int numRowsA = A.GetLength(0);
            int numColsA = A.GetLength(1);
            int numRowsB = B.GetLength(0);
            int numColsB = B.GetLength(1);
            if (numColsA != numRowsB)
                throw new InvalidOperationException();

            double[,] multAB = new double[numRowsA, numColsB];

            for (int i = 0; i < numRowsA; i++)
                for (int j = 0; j < numColsB; j++)
                {
                    multAB[i, j] = 0;
                    for (int k = 0; k < numColsA; k++)
                        multAB[i, j] += A[i, k] * B[k, j];

                }

            return multAB;
        }

        public static double[] MultipliedMatrixVector(double[,] A, double[] v)
        {
            int numRowsA = A.GetLength(0);
            int numColsA = A.GetLength(1);
            int numRowsV = v.Length;
            if (numColsA != numRowsV)
                throw new InvalidOperationException();

            double[] multAV = new double[numRowsA];

            for (int i = 0; i < numRowsA; i++)
            {
                multAV[i] = 0;
                for (int k = 0; k < numColsA; k++)
                    multAV[i] += A[i, k] * v[k];
            }

            return multAV;
        }

        public static double[] SubtractedVectors(double[] minuendVector, double[] subtrahendVector)
        {
            if (minuendVector.Length != subtrahendVector.Length)
                throw new InvalidOperationException();

            var resultVector = new double[minuendVector.Length];
            for (int i = 0; i < resultVector.Length; i++)
                resultVector[i] = minuendVector[i] - subtrahendVector[i];
            return resultVector;
        }
    }
}
