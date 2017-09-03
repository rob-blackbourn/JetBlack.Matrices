using System;

namespace JetBlack.Matrices
{
    public static class QR
    {
        public struct Decomposition
        {
            public Decomposition(double[,] qt, double[,] r, bool isSingularity)
            {
                Qt = qt;
                R = r;
                IsSingularity = isSingularity;
            }

            public double[,] Qt { get; }
            public double[,] R { get; }
            public bool IsSingularity { get; }
        }

        public static Decomposition Decompose(Matrix matrix)
        {
            var n = matrix.Rows;
            var a = matrix.Duplicate();
            var isSingularity = false;

            var c = new double[n];
            var d = new double[n];

            Recipe.qrdcmp(a, n, c, d, out isSingularity);

            var qt = new double[n, n];

            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                    qt[i, j] = 0.0;
                qt[i, i] = 1.0;
            }

            for (var k = 0; k < n - 1; k++)
            {
                if (c[k] == 0)
                    continue;

                for (var j = 0; j < n; j++)
                {
                    var sum = 0.0;
                    for (var i = k; i < n; i++)
                        sum += a[i, k] * qt[i, j];

                    sum /= c[k];

                    for (var i = k; i < n; i++)
                        qt[i, j] -= sum * a[i, k];
                }
            }

            for (var i = 0; i < n; i++)
            {
                a[i, i] = d[i];
                for (var j = 0; j < i; j++)
                    a[i, j] = 0.0;
            }

            return new Decomposition(qt, a, isSingularity);
        }

        public struct qrdcmp_result
        {
            public Matrix a;
            public Vector c;
            public Vector d;
            public bool IsSingularity;
        }

        public static qrdcmp_result qrdcmp(Matrix matrix)
        {
            if (matrix.Rows != matrix.Columns)
                throw new ArgumentException("Matrix must be square");

            var n = matrix.Rows;
            var a = matrix.Duplicate();
            var isSingularity = false;

            var c = new double[n];
            var d = new double[n];

            Recipe.qrdcmp(a, n, c, d, out isSingularity);
            return new qrdcmp_result { a = a, c = c, d = d, IsSingularity = isSingularity };
        }
    }
}
