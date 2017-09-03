using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace JetBlack.Matrices.Test
{
    [TestClass]
    public class LUTest
    {
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

            var Xt = Xdesign.Transpose();
            var coef = (Xt * Xdesign).Inverse() * Xt * Y;

            //var coef = LU.Solve(Xdesign, Y);

            var r2 = X.ConcatColumns(Y).RSquared(coef);
            Debug.Print($"R2={r2}");

            var y = MatrixMath.Predict(new double[] { 14, 12, 0 }, coef);
            Debug.Print($"Prediction is {y}");
        }
    }
}
