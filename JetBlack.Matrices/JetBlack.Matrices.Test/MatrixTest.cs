using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JetBlack.Matrices.Test
{
    [TestClass]
    public class MatrixTest
    {
        [TestMethod]
        public void ShouldCreateMatrix()
        {
            var a = new[,]
                {
                    { 1.0, 2.0, 3.0 },
                    { 4.0, 5.0, 6.0 }
                };
            var lb0 = a.GetLowerBound(0);
            var ub0 = a.GetUpperBound(0);
            var lb1 = a.GetLowerBound(1);
            var ub1 = a.GetUpperBound(1);

            var x = a[0, 0];
            Assert.AreEqual(x, 1.0);
            var y = a[1, 2];
            Assert.AreEqual(y, 6.0);

            var m = new Matrix(
                new[,]
                {
                    { 1.0, 2.0, 3.0 },
                    { 4.0, 5.0, 6.0 }
                });

            Assert.AreEqual(2, m.Rows);
            Assert.AreEqual(3, m.Columns);
        }
    }
}
