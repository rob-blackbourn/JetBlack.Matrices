using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace JetBlack.Matrices.Test
{
    [TestClass]
    public class MatrixMathTest
    {
        [TestMethod]
        public void ShouldTranspose()
        {
            var m = new Matrix(new double[,]
            {
                { 1.0, 2.0, 3.0 },
                { 4.0, 5.0, 6.0 }
            });

            var m1 = m.Transpose();

            var expected = new Matrix(new double[,]
            {
                { 1.0, 4.0 },
                { 2.0, 5.0 },
                { 3.0, 6.0 }
            });

            Assert.AreEqual(expected, m1);
        }

        [TestMethod]
        public void ShouldInert()
        {
            var m = new Matrix(new double[,]
            {
                { 4, 7 },
                { 2, 6 }
            });

            var m1 = LU.Inverse(m);

            var expected = new double[,]
            {
                { 0.6, -0.7 },
                { -0.2, 0.4 }
            };

            for (var i = 0; i < m.Rows; ++i)
                for (var j = 0; j < m.Columns; ++j)
                    Assert.AreEqual(expected[i, j], m1[i, j], 0.0001);
        }

        [TestMethod]
        public void ShouldMultiply()
        {
            var m1 = new Matrix(new double[,]
            {
                { 1.0, 2.0, 3.0 },
                { 4.0, 5.0, 6.0 }
            });

            var m2 = new Matrix(new double[,]
            {
                { 1.0, 4.0 },
                { 2.0, 5.0 },
                { 3.0, 6.0 }
            });

            var m3 = m1.Multiply(m2);

            var expected = new Matrix(new double[,]
            {
                { 14, 32 },
                { 32, 77 }
            });

            Assert.AreEqual(expected, m3);
        }

        [TestMethod]
        public void ShouldDiagonal()
        {
            var m = new Matrix(3, 4).Diagonal(1.0);
            var expected = new Matrix(new double[,]
            {
                { 1, 0, 0, 0},
                { 0, 1, 0, 0},
                { 0, 0, 1, 0}
            });
            Assert.AreEqual(expected, m);
        }

        [TestMethod]
        public void ShouldRegress()
        {
            var X = new Matrix(new double[,]
            {
                { 13.0, 12.0, 0.0 },
                { 15.0, 19.0, 0.0 },
                { 12.0, 23.0, 0.0 },
                { 13.0, 30.0, 1.0 },
                { 13.0, 22.0, 1.0 },
                { 16.0, 11.0, 0.0 },
                { 15.0, 13.0, 1.0 },
                { 16.0, 28.0, 1.0 },
                { 15.0, 10.0, 1.0 },
                { 16.0, 11.0, 1.0 }
            });

            var Y = new Vector(new double[]
            {
                34.12,
                40.94,
                33.58,
                38.95,
                35.42,
                32.12,
                28.57,
                40.47,
                32.06,
                30.55
            });

            Debug.WriteLine($"Data:\n{X.ConcatColumns(Y):F2}");

            var Xdesign = new Vector(X.Rows).Fill(1).ConcatColumns(X);
            Debug.WriteLine($"Design:\n{Xdesign:F2}");

            var coef = LU.Solve(Xdesign, Y);

            var r2 = X.ConcatColumns(Y).RSquared(coef);
            Debug.Print($"R2={r2}");

            var y = MatrixMath.Predict(new double[] { 14, 12, 0 }, coef);
            Debug.Print($"Prediction is {y}");
        }
    }
}
