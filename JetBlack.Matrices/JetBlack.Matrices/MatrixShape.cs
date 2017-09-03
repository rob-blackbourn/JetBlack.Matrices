using System;
using System.Collections.Generic;
using System.Text;

namespace JetBlack.Matrices
{
    public static class MatrixShape
    {
        public static bool IsSquare(this Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));
            return matrix.Rows == matrix.Columns;
        }

        public static void CopyRow(this Matrix source, int sourceRow, int sourceOffset, int sourceCount, Vector destination, int destinationOffset)
        {
            for (var i = 0; i < sourceCount; ++i)
                destination[i + destinationOffset] = source[sourceRow, i + sourceOffset];
        }

        public static Vector SliceRow(this Matrix matrix, int row, int offset, int count)
        {
            var v = new Vector(count);
            matrix.CopyRow(row, offset, count, v, 0);
            return v;
        }

        public static Vector SliceRow(this Matrix matrix, int row)
        {
            return matrix.SliceRow(row, 0, matrix.Columns);
        }

        public static void CopyColumn(this Matrix source, int sourceColumn, int sourceOffset, int sourceCount, Vector destination, int destinationOffset)
        {
            for (var j = 0; j < sourceCount; ++j)
                destination[j + destinationOffset] = source[j + sourceOffset, sourceColumn];
        }

        public static Vector SliceColumn(this Matrix matrix, int column, int offset, int count)
        {
            var v = new Vector(count);
            matrix.CopyColumn(column, offset, count, v, 0);
            return v;
        }

        public static Vector SliceColumn(this Matrix matrix, int column)
        {
            return matrix.SliceColumn(column, 0, matrix.Rows);
        }

        public static Vector Slice(this Vector vector, int offset, int count)
        {
            var result = new Vector(count);
            for (var i = 0; i < count; ++i)
                result[i] = vector[i + offset];
            return result;
        }

        public static Matrix Slice(this Matrix matrix, int rowOffset, int rowCount, int columnOffset, int columnCount)
        {
            var result = new Matrix(rowCount, columnCount);
            for (var i = 0; i < rowCount; ++i) // copy the values
                for (var j = 0; j < columnCount; ++j)
                    result[i, j] = matrix[i + rowOffset, j + columnOffset];
            return result;
        }

        public static Matrix SliceRows(this Matrix matrix, int offset, int count)
        {
            return matrix.Slice(offset, count, 0, matrix.Columns);
        }

        public static Matrix SliceColumns(this Matrix matrix, int offset, int count)
        {
            return matrix.Slice(0, matrix.Rows, offset, count);
        }

        public static Matrix Duplicate(this Matrix matrix)
        {
            return Slice(matrix, 0, matrix.Rows, 0, matrix.Columns);
        }

        public static Vector Duplicate(this Vector vector)
        {
            return vector.Slice(0, vector.Length);
        }

        public static Matrix Transpose(this Matrix matrix)
        {
            var result = new Matrix(matrix.Columns, matrix.Rows);
            for (int i = 0; i < matrix.Rows; ++i)
                for (int j = 0; j < matrix.Columns; ++j)
                    result[j, i] = matrix[i, j];
            return result;
        }

        public static Matrix ConcatRows(this Matrix lhs, Matrix rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException(nameof(lhs));
            if (rhs == null)
                throw new ArgumentNullException(nameof(rhs));

            if (rhs.Columns != lhs.Columns)
                throw new ArgumentException("The matrices must have the same number of columns");

            var m = new Matrix(lhs.Rows + rhs.Rows, lhs.Columns);

            lhs.Copy(0, lhs.Rows, 0, lhs.Columns, m, 0, 0);
            rhs.Copy(0, rhs.Rows, 0, lhs.Columns, m, lhs.Rows, 0);

            return m;
        }

        public static Matrix ConcatRows(this Matrix lhs, Vector rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException(nameof(lhs));
            if (rhs == null)
                throw new ArgumentNullException(nameof(rhs));

            if (rhs.Length != lhs.Columns)
                throw new ArgumentException("The matrices must have the same number of columns");

            var m = new Matrix(lhs.Rows + 1, lhs.Columns);

            lhs.Copy(0, lhs.Rows, 0, lhs.Columns, m, 0, 0);
            rhs.CopyColumns(0, rhs.Length, m, lhs.Rows, 0);

            return m;
        }

        public static Matrix ConcatRows(this Vector lhs, Matrix rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException(nameof(lhs));
            if (rhs == null)
                throw new ArgumentNullException(nameof(rhs));

            if (rhs.Columns != lhs.Length)
                throw new ArgumentException("The matrices must have the same number of columns");

            var m = new Matrix(1 + rhs.Rows, rhs.Columns);

            lhs.CopyColumns(0, lhs.Length, m, 0, 0);
            rhs.Copy(0, rhs.Rows, 0, rhs.Columns, m, 1, 0);

            return m;
        }

        public static Matrix ConcatColumns(this Matrix lhs, Matrix rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException(nameof(lhs));
            if (rhs == null)
                throw new ArgumentNullException(nameof(rhs));

            if (rhs.Rows != lhs.Rows)
                throw new ArgumentException("The matrices must have the same number of rows");

            var m = new Matrix(lhs.Rows, lhs.Columns + rhs.Columns);

            lhs.Copy(0, lhs.Rows, 0, lhs.Columns, m, 0, 0);
            rhs.Copy(0, lhs.Rows, 0, rhs.Columns, m, 0, lhs.Columns);

            return m;
        }

        public static Matrix ConcatColumns(this Matrix lhs, Vector rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException(nameof(lhs));
            if (rhs == null)
                throw new ArgumentNullException(nameof(rhs));

            if (rhs.Length != lhs.Rows)
                throw new ArgumentException("The matrices must have the same number of rows");

            var m = new Matrix(lhs.Rows, lhs.Columns + 1);

            lhs.Copy(0, lhs.Rows, 0, lhs.Columns, m, 0, 0);
            rhs.CopyRows(0, rhs.Length, m, 0, lhs.Columns);

            return m;
        }

        public static Matrix ConcatColumns(this Vector lhs, Matrix rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException(nameof(lhs));
            if (rhs == null)
                throw new ArgumentNullException(nameof(rhs));

            if (rhs.Rows != lhs.Length)
                throw new ArgumentException("The matrices must have the same number of rows");

            var m = new Matrix(lhs.Length, 1 + rhs.Columns);

            lhs.CopyRows(0, lhs.Length, m, 0, 0);
            rhs.Copy(0, lhs.Length, 0, rhs.Columns, m, 0, 1);

            return m;
        }

        public static void CopyRows(this Vector source, int sourceOffset, int sourceCount, Matrix destination, int destinationRowOffset, int destinationColumnOffset)
        {
            for (var i = 0; i < sourceCount; ++i)
                destination[i + destinationRowOffset, destinationColumnOffset] = source[i + sourceOffset];
        }

        public static void CopyColumns(this Vector source, int sourceOffset, int sourceCount, Matrix destination, int destinationRowOffset, int destinationColumnOffset)
        {
            for (var j = 0; j < sourceCount; ++j)
                destination[destinationRowOffset, j + destinationColumnOffset] = source[j + sourceOffset];
        }

        public static void Copy(this Matrix source, int sourceRowOffset, int sourceRowCount, int sourceColumnOffset, int sourceColumnCount, Matrix destination, int destinationRowOffset, int destinationColumnOffset)
        {
            for (var i = 0; i < sourceRowCount; ++i)
                for (var j = 0; j < sourceColumnCount; ++j)
                    destination[i + destinationRowOffset, j + destinationColumnOffset] = source[i + sourceRowOffset, j + sourceColumnOffset];
        }

        public static IEnumerable<double> EnumerateRow(this Matrix matrix, int offset)
        {
            for (var i = 0; i < matrix.Columns; ++i)
                yield return matrix[offset, i];
        }

        public static IEnumerable<double> EnumerateColumn(this Matrix matrix, int offset)
        {
            for (var i = 0; i < matrix.Rows; ++i)
                yield return matrix[i, offset];
        }

        public static string Append(this StringBuilder sb, Matrix matrix, Func<double, string> toString)
        {
            for (var r = 0; r < matrix.Rows; ++r)
            {
                sb.Append("[");
                for (var c = 0; c < matrix.Columns; ++c)
                {
                    if (c > 0)
                        sb.Append(", ");
                    sb.Append(toString(matrix[r, c]));
                }
                sb.Append("]").AppendLine();
            }

            return sb.ToString();
        }

        public static string Append(this StringBuilder sb, Vector vector, Func<double, string> toString)
        {
            sb.Append("[");
            for (var i = 0; i < vector.Length; ++i)
            {
                if (i > 0)
                    sb.Append(", ");
                sb.Append(toString(vector[i]));
            }
            sb.Append("]").AppendLine();

            return sb.ToString();
        }

        public static Matrix Fill(this Matrix matrix, double value)
        {
            for (var i = 0; i < matrix.Rows; ++i)
                for (var j = 0; j < matrix.Columns; ++j)
                    matrix[i, j] = value;
            return matrix;
        }

        public static Vector Fill(this Vector vector, double value)
        {
            for (var i = 0; i < vector.Length; ++i)
                vector[i] = value;
            return vector;
        }

        public static Matrix Diagonal(this Matrix matrix, double value)
        {
            for (var i = 0; i < matrix.Rows; ++i)
                matrix[i, i] = value;
            return matrix;
        }

        public static Matrix Diagonal(this Matrix matrix, double value, double defaultValue)
        {
            for (var i = 0; i < matrix.Rows; ++i)
                for (var j = 0; j < matrix.Columns; ++j)
                    matrix[i, j] = i == j ? value : defaultValue;
            return matrix;
        }

        public static Matrix Identity(this Matrix matrix, bool isDirty = false)
        {
            return isDirty ? matrix.Diagonal(1, 0) : matrix.Diagonal(1);
        }

        public static Matrix Clear(this Matrix matrix)
        {
            return matrix.Fill(0);
        }
    }
}
