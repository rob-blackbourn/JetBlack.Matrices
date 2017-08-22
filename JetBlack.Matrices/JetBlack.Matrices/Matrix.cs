using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetBlack.Matrices
{
    public class Matrix : IEquatable<Matrix>
    {
        private readonly double[,] _matrix;

        public Matrix(double[,] matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));
            if (matrix.GetLowerBound(0) != 0 || matrix.GetLowerBound(1) != 0)
                throw new ArgumentException("Lower bound must be 0", nameof(matrix));
            _matrix = matrix;
        }

        public int Rows => 1 + _matrix.GetUpperBound(0);
        public int Columns => 1 + _matrix.GetUpperBound(1);

        public double this[int r, int c]
        {
            get => _matrix[r, c];
            set => _matrix[r, c] = value;
        }

        public Matrix Add(Matrix other)
        {
            if (Rows != other.Rows || Columns != other.Columns)
                throw new ArgumentException("Rows and columns must be the sa,e size");

            var m = new Matrix(new double[Rows, Columns]);
            for (int r = 0; r < Rows; ++r)
                for (int c = 0; c < Columns; ++c)
                    m[r, c] = _matrix[r, c] + other[r, c];
            return m;
        }

        public Matrix Subract(Matrix other)
        {
            if (Rows != other.Rows || Columns != other.Columns)
                throw new ArgumentException("Rows and columns must be the sa,e size");

            var m = new Matrix(new double[Rows, Columns]);
            for (int r = 0; r < Rows; ++r)
                for (int c = 0; c < Columns; ++c)
                    m[r, c] = _matrix[r, c] - other[r, c];
            return m;
        }

        public Matrix Negate()
        {
            var m = new Matrix(new double[Rows, Columns]);
            for (int r = 0; r < Rows; ++r)
                for (int c = 0; c < Columns; ++c)
                    m[r, c] = -_matrix[r, c];
            return m;
        }

        public Matrix Multiply(double x)
        {
            var m = new Matrix(new double[Rows, Columns]);
            for (int r = 0; r < Rows; ++r)
                for (int c = 0; c < Columns; ++c)
                    m[r, c] = _matrix[r, c] * x;
            return m;
        }

        public Matrix Divide(double x)
        {
            var m = new Matrix(new double[Rows, Columns]);
            for (int r = 0; r < Rows; ++r)
                for (int c = 0; c < Columns; ++c)
                    m[r, c] = _matrix[r, c] / x;
            return m;
        }

        public Matrix Multiply(Matrix other)
        {
            if (Rows != other.Columns || Columns != other.Rows)
                throw new ArgumentException("Invalid shape");

            var m = new Matrix(new double[Rows, other.Columns]);
            for (int i = 0; i < other.Columns; ++i)
            {
                var sum = 0.0;
                for (int j = 0; j < Rows; ++j)
                {
                    for (var k =0; k < Columns; ++k)
                        m[i,j] += _matrix[i, j] * other[j, i];
                }
            }
            return m;
        }

        public static implicit operator Matrix(double[,] value)
        {
            return new Matrix(value);
        }

        public static implicit operator double[,] (Matrix m)
        {
            return m._matrix;
        }

        public static Matrix operator *(Matrix m, double x)
        {
            return m.Multiply(x);
        }

        public static Matrix operator /(Matrix m, double x)
        {
            return m.Divide(x);
        }

        public static Matrix operator -(Matrix m)
        {
            return m.Negate();
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            return a.Subract(b);
        }

        public bool Equals(Matrix other)
        {
            return other != null &&
                Rows == other.Rows &&
                Columns == other.Columns &&
                Enumerable.Range(0, Rows)
                    .Select(r => Enumerable.Range(0, Columns)
                        .Select(c => _matrix[r, c] == other[r, c]))
                    .SelectMany(x => x)
                    .All(x => x);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Matrix);
        }

        public override int GetHashCode()
        {
            return Enumerable.Range(0, Rows)
                    .Select(r => Enumerable.Range(0, Columns)
                        .Select(c => _matrix[r, c]))
                    .SelectMany(x => x)
                    .Aggregate(0, (sum, value) => sum ^ value.GetHashCode());
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int r = 0; r < Rows; ++r)
            {
                sb.Append('[');
                for (int c = 0; c < Columns; ++c)
                {
                    if (c > 0)
                        sb.Append(", ");
                    sb.Append(_matrix[r, c]);
                }
                sb.AppendLine("]");
            }
            return sb.ToString();
        }
    }
}
