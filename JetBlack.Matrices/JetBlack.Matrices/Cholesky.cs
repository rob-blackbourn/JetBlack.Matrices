using System;

namespace JetBlack.Matrices
{
    public static class Cholesky
    {
        public static Vector Solve(Matrix a, Vector b)
        {
            return cholsl(choldc(a), b);
        }

        public static Matrix Inverse(Matrix a)
        {
            if (a.Rows != a.Columns)
                throw new ArgumentException("matrix must be square");

            var y = a.Duplicate();
            var col = new Vector(a.Rows);

            var dcmp = choldc(a);
            for (var j = 0; j < y.Rows; ++j)
            {
                for (var i = 0; i < y.Columns; ++i)
                    col[i] = 0;
                col[j] = 1;
                var x = cholsl(dcmp, col);
                for (var i = 0; i < col.Length; ++i)
                    y[i, j] = x[i];
            }

            return y;
        }

        public struct choldc_results
        {
            public Matrix a;
            public Vector p;
        }

        public static choldc_results choldc(Matrix a)
        {
            if (a.Rows != a.Columns)
                throw new ArgumentException("Matrix must be square");

            var u = a.Duplicate();
            var p = new Vector(a.Rows);
            Recipe.choldc(u, a.Rows, p);
            return new choldc_results { a = u, p = p };
        }

        public static Vector cholsl(choldc_results dcmp, Vector b)
        {
            if (dcmp.a.Rows != dcmp.a.Columns)
                throw new ArgumentException("Matrix must be square");

            var x = new Vector(dcmp.a.Rows);
            Recipe.cholsl(dcmp.a, dcmp.a.Rows, dcmp.p, b, x);
            return x;
        }
    }
}
