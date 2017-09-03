using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JetBlack.Matrices.Test
{
    [TestClass]
    public class VectorComparerTest
    {
        [TestMethod]
        public void SHouldCompareVectors()
        {
            var v1 = new double[] { 0.0, 1.0, 2.0, 3.0 };
            var v2 = new double[] { 0.0, 1.0, 2.0, 3.0 };
            var v3 = new double[] { 1.0, 2.0, 3.0, 4.0 };
            var v4 = new double[] { 0.0, 1.0, 2.0 };

            Assert.IsTrue(VectorComparer<double>.Default.Equals(v1, v2));
            Assert.IsFalse(VectorComparer<double>.Default.Equals(v1, v3));
            Assert.IsFalse(VectorComparer<double>.Default.Equals(v1, v4));
        }
    }
}
