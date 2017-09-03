using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JetBlack.Matrices.Test
{
    [TestClass]
    public class CholeskyTest
    {
        [TestMethod]
        public void ShouldSolve()
        {
            var a = new double[,]
            {
                { 6.0, 15.0, 55.0 },
                { 15.0, 55.0, 225.0 },
                { 55.0, 225.0, 979.0 }
            };

            var rhs = new double[] { 9.5, 50.0, 237.0 };

            var solution = Cholesky.Solve(a, rhs);
            var expected = new double[] { -0.5, -1.0, 0.5 };
            Assert.AreEqual(expected.Length, solution.Length);
            for (var i = 0; i < solution.Length; ++i)
                Assert.AreEqual(expected[i], solution[i], 0.000001);
        }

        [TestMethod]
        public void SmokeTest()
        {
            var aorig = new Matrix(new double[,]
            {
                { 100.0, 15.0, 0.01 },
                { 15.0, 2.3, 0.01 },
                { 0.01, 0.01, 1.0}
            });

            // Copy the matrix
            var a = new Matrix(aorig.Rows, aorig.Columns);
            for (var i = 0; i < a.Rows; ++i)
                for (var j = 0; j < a.Columns; ++j)
                    a[i, j] = aorig[i, j];

            // Decompose.
            var p = new Vector(aorig.Columns);
            Recipe.choldc(a, p.Length, p);

            // Original matrix
            var chol = new Matrix(a.Rows, a.Columns);
            for (var i = 0; i < a.Rows; ++i)
            {
                for (var j = 0; j < a.Columns; ++j)
                {
                    if (i > j)
                        chol[i, j] = a[i, j];
                    else
                        chol[i, j] = i == j ? p[i] : 0.0;
                }
            }

            // Product of Cholesky factors
            var atest = new Matrix(chol.Rows, chol.Columns);
            for (var i = 0; i < chol.Rows; ++i)
            {
                for (var j = 0; j < chol.Columns; ++j)
                {
                    var sum = 0.0;
                    for (var k = 0; k < chol.Columns; ++k)
                        sum += chol[i, k] * chol[j, k];
                    atest[i, j] = sum;
                }
            }

            // Check solution vector
            var b = new Vector(new double[] { 0.4, 0.02, 99.0 });
            var x = new Vector(b.Length);
            Recipe.cholsl(a, a.Rows, p, b, x);
            for (var i = 0; i < aorig.Rows; ++i)
            {
                var sum = 0.0;
                for (var j = 0; j < aorig.Columns; ++j)
                    sum += aorig[i, j] * x[j];
                p[i] = sum;

                Assert.AreEqual(p[i], b[i], 0.00001);
            }
        }

        [TestMethod]
        public void ShouldInverse()
        {
            var a = new Matrix(new double[,]
            {
                { 4, 12, -16 },
                { 12, 37, -43 },
                {-16, -43, 98 }
            });

            var m1 = Cholesky.Inverse(a);
        }
    }
}
