using System;

namespace JetBlack.Matrices
{
    public static class SVD
    {
        public static Vector Solve(Matrix a, Vector b)
        {
            return svbksb(svdcmp(a), b);
        }

        public static Matrix Inverse(Matrix a)
        {
            if (a.Rows != a.Columns)
                throw new ArgumentException("Matrix must be square");

            var y = a.Duplicate();
            var col = new Vector(a.Rows);

            var dcmp = svdcmp(a);
            for (var j = 0; j < y.Rows; ++j)
            {
                for (var i = 0; i < y.Columns; ++i)
                    col[i] = 0;
                col[j] = 1;
                var x = svbksb(dcmp, col);
                for (var i = 0; i < col.Length; ++i)
                    y[i, j] = x[i];
            }

            return y;
        }

        public struct svdcmp_result
        {
            public Matrix u;
            public Matrix v;
            public Vector w;
        }

        public static svdcmp_result svdcmp(Matrix a)
        {
            var u = a.Duplicate();
            var v = new Matrix(u.Rows);
            var w = new Vector(u.Rows);

            Recipe.svdcmp(u, a.Rows, a.Columns, w, v);

            return new svdcmp_result { u = u, v = v, w = w };
        }

        public static Vector svbksb(svdcmp_result adcmp, Vector b)
        {
            var x = new Vector(adcmp.u.Rows);
            Recipe.svbksb(adcmp.u, adcmp.w, adcmp.v, adcmp.u.Rows, adcmp.u.Columns, b, x);
            return x;
        }
    }
}
