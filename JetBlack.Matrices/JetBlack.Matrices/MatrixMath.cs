using System;
using System.Collections.Generic;
using System.Text;

namespace JetBlack.Matrices
{
    public static class MatrixMath
    {
        public static Matrix Multiply(this Matrix lhs, Matrix rhs)
        {
            if (lhs.Columns != rhs.Rows)
                throw new Exception("Non-conformable matrices in Product");

            var result = new Matrix(lhs.Rows, rhs.Columns);

            for (var i = 0; i < lhs.Rows; ++i) // each row of A
                for (var j = 0; j < rhs.Columns; ++j) // each col of B
                    for (var k = 0; k < lhs.Columns; ++k) // could use k < bRows
                        result[i, j] += lhs[i, k] * rhs[k, j];

            return result;
        }

        public static Vector Multiply(this Matrix lhs, Vector rhs)
        {
            if (lhs.Columns != rhs.Length)
                throw new Exception("The length of the vector must be the same as the number of columns in the matrix");

            var result = new Vector(lhs.Rows);
            for (var i = 0; i < lhs.Rows; ++i) // each row of A
                for (var j = 0; j < lhs.Columns; ++j) // could use k < bRows
                    result[i] += lhs[i, j] * rhs[j];

            return result;
        }

        public static Matrix Multiply(double lhs, Matrix rhs)
        {
            var matrix = new Matrix(rhs.Rows, rhs.Columns);

            for (var i = 0; i < rhs.Rows; ++i)
                for (var j = 0; j < rhs.Columns; ++j)
                    matrix[i, j] = rhs[i, j] * lhs;

            return matrix;
        }

        public static Matrix Add(Matrix lhs, Matrix rhs)
        {
            if (!(lhs.Rows == rhs.Rows && lhs.Columns == rhs.Columns))
                throw new ArgumentException("Matrices must be the same size");

            var mat = new Matrix(lhs.Rows, lhs.Columns);
            for (var i = 0; i < lhs.Rows; ++i)
                for (var j = 0; j < lhs.Columns; ++j)
                    mat[i, j] = lhs[i, j] + rhs[i, j];
            return mat;
        }

        public static Matrix Subtract(Matrix lhs, Matrix rhs)
        {
            if (!(lhs.Rows == rhs.Rows && lhs.Columns == rhs.Columns))
                throw new ArgumentException("Matrices must be the same size");

            var mat = new Matrix(lhs.Rows, lhs.Columns);
            for (var i = 0; i < lhs.Rows; ++i)
                for (var j = 0; j < lhs.Columns; ++j)
                    mat[i, j] = lhs[i, j] - rhs[i, j];
            return mat;
        }

        public static Matrix Negate(Matrix value)
        {
            var matrix = new Matrix(value.Rows, value.Columns);

            for (var i = 0; i < value.Rows; ++i)
                for (var j = 0; j < value.Columns; ++j)
                    matrix[i, j] = -value[i, j];

            return matrix;
        }

        public static double RSquared(this Matrix data, Vector coef)
        {
            // 'coefficient of determination'

            // 1. compute mean of y
            double ySum = 0.0;
            for (int i = 0; i < data.Rows; ++i)
                ySum += data[i, data.Columns - 1]; // last column
            double yMean = ySum / data.Rows;

            // 2. sum of squared residuals & tot sum squares
            double ssr = 0.0;
            double sst = 0.0;
            double y; // actual y value
            double predictedY; // using the coef[] 
            for (int i = 0; i < data.Rows; ++i)
            {
                y = data[i, data.Columns - 1]; // get actual y

                predictedY = coef[0]; // start w/ intercept constant
                for (int j = 0; j < data.Columns - 1; ++j) // j is col of data
                    predictedY += coef[j + 1] * data[i, j]; // careful

                ssr += (y - predictedY) * (y - predictedY);
                sst += (y - yMean) * (y - yMean);
            }

            if (sst == 0.0)
                throw new Exception("All y values equal");
            else
                return 1.0 - (ssr / sst);
        }

        public static double Predict(Vector independents, Vector coef)
        {
            var sum = coef[0];
            for (var i = 0; i < independents.Length; ++i)
                sum += independents[i] * coef[1 + i];
            return sum;
        }

        public static Statistics.Summary[] SummariseColumns(this Matrix matrix)
        {
            //var cols = matrix.Columns;
            var summaries = new Statistics.Summary[matrix.Columns];
            for (var i = 0; i < matrix.Columns; ++i)
                summaries[i] = matrix.EnumerateColumn(i).Summarise();
            return summaries;
        }

        public static Statistics.Summary[] SummariseRows(this Matrix matrix)
        {
            var summaries = new Statistics.Summary[matrix.Rows];
            for (var i = 0; i < matrix.Rows; ++i)
                summaries[i] = matrix.EnumerateRow(i).Summarise();
            return summaries;
        }

        public static Matrix Inverse(this Matrix matrix, MatrixMethod method = MatrixMethod.LU)
        {
            switch (method)
            {
                case MatrixMethod.LU:
                    return LU.Inverse(matrix);
                case MatrixMethod.Cholesky:
                    return Cholesky.Inverse(matrix);
                case MatrixMethod.SVD:
                    return SVD.Inverse(matrix);
                default:
                    throw new IndexOutOfRangeException(nameof(method));
            }
        }

        public static Vector Solve(this Matrix matrix, Vector vector, MatrixMethod method = MatrixMethod.LU)
        {
            switch (method)
            {
                case MatrixMethod.LU:
                    return LU.Solve(matrix, vector);
                case MatrixMethod.Cholesky:
                    return Cholesky.Solve(matrix, vector);
                case MatrixMethod.SVD:
                    return SVD.Solve(matrix, vector);
                default:
                    throw new IndexOutOfRangeException(nameof(method));
            }
        }
    }
}
