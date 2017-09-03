using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetBlack.Matrices
{
    public class Matrix : IEquatable<Matrix>, IFormattable
    {
        private readonly double[,] _matrix;

        public Matrix(int n)
            : this(n, n)
        {
        }

        public Matrix(int rows, int columns)
            : this(new double[rows, columns])
        {
        }

        public Matrix(double[,] matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));
            if (!(matrix.GetLowerBound(0) == 0 & matrix.GetLowerBound(1) == 0))
                throw new ArgumentException("Bounds must start at 0");

            _matrix = matrix;
            Rows = matrix.GetUpperBound(0) + 1;
            Columns = matrix.GetUpperBound(1) + 1;
        }

        public int Rows { get; }
        public int Columns { get; }

        public double this[int row, int column]
        {
            get => _matrix[row, column];
            set => _matrix[row, column] = value;
        }

        public static implicit operator Matrix(double[,] matrix)
        {
            return new Matrix(matrix);
        }

        public static implicit operator double[,] (Matrix matrix)
        {
            return matrix._matrix;
        }

        public bool Equals(Matrix other)
        {
            if (other == null || !(Rows == other.Rows && Columns == other.Columns))
                return false;

            for (int r = 0; r < Rows; ++r)
                for (var c = 0; c < Columns; ++c)
                    if (_matrix[r, c] != other[r, c])
                        return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Matrix);
        }

        public override int GetHashCode()
        {
            return Enumerable.Range(0, Rows).Select(r => Enumerable.Range(0, Columns).Select(c => _matrix[r, c])).SelectMany(x => x).Aggregate(0, (hash, value) => hash ^= value.GetHashCode());
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            //"G" is .Net's standard for general formatting--all
            //types should support it
            if (format == null) format = "G";

            // is the user providing their own format provider?
            if (formatProvider != null)
            {
                var formatter = formatProvider.GetFormat(GetType()) as ICustomFormatter;
                if (formatter != null)
                    return formatter.Format(format, this, formatProvider);
            }

            var sb = new StringBuilder();
            sb.Append(this, x => x.ToString(format));
            return sb.ToString();
        }


        public override string ToString()
        {
            return ToString(null, null);
        }

        public static Matrix operator +(Matrix lhs, Matrix rhs)
        {
            return MatrixMath.Add(lhs, rhs);
        }

        public static Matrix operator -(Matrix lhs, Matrix rhs)
        {
            return MatrixMath.Subtract(lhs, rhs);
        }

        public static Matrix operator -(Matrix value)
        {
            return MatrixMath.Negate(value);
        }

        public static Matrix operator *(Matrix lhs, Matrix rhs)
        {
            return MatrixMath.Multiply(lhs, rhs);
        }

        public static Matrix operator *(double lhs, Matrix rhs)
        {
            return MatrixMath.Multiply(lhs, rhs);
        }

        public static Vector operator *(Matrix lhs, Vector rhs)
        {
            return MatrixMath.Multiply(lhs, rhs);
        }

        public static double Dot(double[] v, double[] w)
        {
            if (v.Length == 0 || v.Length != w.Length)
                throw new ArgumentException("Vectors must be of the same length.");

            double buf = 0;

            for (var i = 0; i < v.Length; ++i)
                buf += v[i] * w[i];

            return buf;
        }

        public double[] Row(int row)
        {
            return Row(row, 0, Columns);
        }

        public double[] Row(int row, int offset, int count)
        {
            var v = new double[count];
            for (var i = 0; i < count; ++i)
                v[i] = _matrix[row, i + offset];
            return v;
        }

        public double[] Column(int column)
        {
            return Column(column, 0, Rows);
        }

        public double[] Column(int column, int offset, int count)
        {
            var v = new double[count];
            for (var i = 0; i < count; ++i)
                v[i] = _matrix[i + offset, column];
            return v;
        }

        /// <summary>
        /// Creates n by n identity matrix.
        /// </summary>
        /// <param name="n">Number of rows and columns respectively.</param>
        /// <returns>n by n identity matrix.</returns>
        public static Matrix Identity(int n)
        {
            return Diag(Ones(n));
        }

        public static double[] Ones(int n)
        {
            var v = new double[n];

            for (int i = 0; i < n; ++i)
                v[i] = 1;

            return v;
        }

        /// <summary>
        /// Generates diagonal matrix
        /// </summary>
        /// <param name="v">column vector containing the diag elements</param>
        /// <returns></returns>
        public static Matrix Diag(double[] v)
        {
            if (v.Length == 0)
                throw new ArgumentException("vector must be 1xN or Nx1");

            var matrix = new Matrix(new double[v.Length, v.Length]);

            for (int i = 0; i < v.Length; ++i)
                matrix[i, i] = v[i];

            return matrix;
        }
    }
}
