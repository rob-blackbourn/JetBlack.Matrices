using System;
using System.Collections.Generic;
using System.Text;

namespace JetBlack.Matrices
{
    public class MatrixComparer : IEqualityComparer<Matrix>
    {
        public static MatrixComparer Default { get; } = new MatrixComparer();

        public bool Equals(Matrix x, Matrix y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;

            if (!(x.Rows == y.Rows && x.Columns == y.Columns))
                return false;

            for (var i = 0; i < x.Rows; ++i)
                for (var j = 0; j < x.Columns; ++j)
                    if (!x[i, j].Equals(y[i, j]))
                        return false;

            return true;
        }

        public int GetHashCode(Matrix obj)
        {
            if (obj == null)
                return 0;

            var hash = obj.Rows.GetHashCode() ^ obj.Columns.GetHashCode();
            for (var i = 0; i < obj.Rows; ++i)
                for (var j = 0; j < obj.Columns; j++)
                    hash ^= obj[i, j].GetHashCode();

            return hash;
        }
    }
}
