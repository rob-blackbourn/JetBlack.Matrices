using System;

namespace JetBlack.Matrices
{
    public static class LU
    {
        public static Vector Solve(Matrix a, Vector b)
        {
            return lubksb(ludcmp(a), b);
        }

        public static Matrix Inverse(Matrix a)
        {
            if (a.Rows != a.Columns)
                throw new ArgumentException("Matrix must be square");

            var y = a.Duplicate();
            var col = new Vector(a.Rows);

            var dcmp = ludcmp(a);
            for (var j = 0; j < y.Rows; ++j)
            {
                for (var i = 0; i < y.Columns; ++i)
                    col[i] = 0;
                col[j] = 1;
                var x = lubksb(dcmp, col);
                for (var i = 0; i < col.Length; ++i)
                    y[i, j] = x[i];
            }

            return y;
        }

        public struct ludcmp_result
        {
            public Matrix u;
            public int[] indx;
            public double d;
        }

        public static ludcmp_result ludcmp(Matrix a)
        {
            if (a.Rows != a.Columns)
                throw new ArgumentException("Matrix must be square");

            var u = a.Duplicate();
            var indx = new int[a.Rows];
            double d;

            Recipe.ludcmp(u, a.Rows, indx, out d);
            return new ludcmp_result { u = u, indx = indx, d = d };
        }

        public static Vector lubksb(ludcmp_result dcmp, Vector b)
        {
            if (dcmp.u.Rows != dcmp.u.Columns)
                throw new ArgumentException("Matrix must be square");

            var x = b.Duplicate();
            Recipe.lubksb(dcmp.u, dcmp.u.Rows, dcmp.indx, x);
            return x;
        }
    }
}
