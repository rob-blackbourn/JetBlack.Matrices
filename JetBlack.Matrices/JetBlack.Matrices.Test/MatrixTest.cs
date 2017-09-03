using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JetBlack.Matrices.Test
{
    [TestClass]
    public class MatrixTest
    {
        [TestMethod]
        public void ShouldConstruct()
        {
            var a = new Matrix(new[,] { { 1.0, 2.0, 3.0 }, { 4.0, 5.0, 6.0 } });
            Assert.AreEqual(2, a.Rows);
            Assert.AreEqual(3, a.Columns);
        }

        [TestMethod]
        public void ShouldConstructImplicit()
        {
            Matrix a = new[,] { { 1.0, 2.0, 3.0 }, { 4.0, 5.0, 6.0 } };
            Assert.AreEqual(2, a.Rows);
            Assert.AreEqual(3, a.Columns);
        }

        [TestMethod]
        public void ShouldCastToMatrix()
        {
            var a = (Matrix)new[,] { { 1.0, 2.0, 3.0 }, { 4.0, 5.0, 6.0 } };
            Assert.AreEqual(2, a.Rows);
            Assert.AreEqual(3, a.Columns);
        }

        [TestMethod]
        public void ShouldCastToArray()
        {
            var a = (double[,])new Matrix(new[,] { { 1.0, 2.0, 3.0 }, { 4.0, 5.0, 6.0 } });
            Assert.AreEqual(1, a.GetUpperBound(0));
            Assert.AreEqual(2, a.GetUpperBound(1));
        }

        [TestMethod]
        public void ShouldIndex()
        {
            var a = (double[,])new Matrix(new[,] { { 1.0, 2.0, 3.0 }, { 4.0, 5.0, 6.0 } });
            Assert.AreEqual(1.0, a[0, 0]);
            Assert.AreEqual(2.0, a[0, 1]);
            Assert.AreEqual(3.0, a[0, 2]);
            Assert.AreEqual(4.0, a[1, 0]);
            Assert.AreEqual(5.0, a[1, 1]);
            Assert.AreEqual(6.0, a[1, 2]);
        }
    }
}
