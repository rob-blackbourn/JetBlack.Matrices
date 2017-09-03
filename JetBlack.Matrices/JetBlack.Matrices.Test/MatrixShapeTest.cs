using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JetBlack.Matrices.Test
{
    [TestClass]
    public class MatrixShapeTest
    {
        [TestMethod]
        public void ShouldKnowRowsAndColumns()
        {
            var m = new Matrix(3, 4);
            Assert.AreEqual(3, m.Rows);
            Assert.AreEqual(4, m.Columns);
        }

        [TestMethod]
        public void ShouldKnowIfSquare()
        {
            var m1 = new Matrix(3, 3);
            Assert.IsTrue(m1.IsSquare());
            var m2 = new Matrix(3, 4);
            Assert.IsFalse(m2.IsSquare());
        }

        [TestMethod]
        public void ShouldCopyRow()
        {
            var m = new Matrix(new double[,]
            {
                { 0.0, 0.1, 0.2, 0.3 },
                { 1.0, 1.1, 1.2, 1.3 },
                { 2.0, 2.1, 2.2, 2.3 },
            });

            var v1 = new Vector(4);
            m.CopyRow(1, 0, 4, v1, 0);
            Assert.AreEqual(new Vector(new double[] { 1.0, 1.1, 1.2, 1.3 }), v1);

            var v2 = new Vector(3);
            m.CopyRow(1, 0, 3, v2, 0);
            Assert.AreEqual(new Vector(new double[] { 1.0, 1.1, 1.2 }), v2);

            var v3 = new Vector(3);
            m.CopyRow(1, 1, 3, v3, 0);
            Assert.AreEqual(new Vector(new double[] { 1.1, 1.2, 1.3 }), v3);

            var v4 = new Vector(2);
            m.CopyRow(1, 1, 2, v4, 0);
            Assert.AreEqual(new Vector(new double[] { 1.1, 1.2 }), v4);

            var v5 = new Vector(6);
            m.CopyRow(1, 1, 2, v5, 2);
            Assert.AreEqual(new Vector(new double[] { 0.0, 0.0, 1.1, 1.2, 0.0, 0.0 }), v5);
        }

        [TestMethod]
        public void ShouldCopyColumn()
        {
            var m = new Matrix(new double[,]
            {
                { 0.0, 0.1, 0.2 },
                { 1.0, 1.1, 1.2 },
                { 2.0, 2.1, 2.2 },
                { 3.0, 3.1, 3.2 },
            });

            var v1 = new Vector(4);
            m.CopyColumn(1, 0, 4, v1, 0);
            Assert.AreEqual(new Vector(new double[] { 0.1, 1.1, 2.1, 3.1 }), v1);

            var v2 = new Vector(3);
            m.CopyColumn(1, 0, 3, v2, 0);
            Assert.AreEqual(new Vector(new double[] { 0.1, 1.1, 2.1 }), v2);

            var v3 = new Vector(3);
            m.CopyColumn(1, 1, 3, v3, 0);
            Assert.AreEqual(new Vector(new double[] { 1.1, 2.1, 3.1 }), v3);

            var v4 = new Vector(2);
            m.CopyColumn(1, 1, 2, v4, 0);
            Assert.AreEqual(new Vector(new double[] { 1.1, 2.1 }), v4);

            var v5 = new Vector(6);
            m.CopyColumn(1, 1, 2, v5, 2);
            Assert.AreEqual(new Vector(new double[] { 0.0, 0.0, 1.1, 2.1, 0.0, 0.0 }), v5);
        }

        [TestMethod]
        public void ShouldSliceColumn()
        {
            var m = new Matrix(new double[,]
            {
                { 0.0, 0.1, 0.2, 0.3 },
                { 1.0, 1.1, 1.2, 1.3 },
                { 2.0, 2.1, 2.2, 2.3 },
                { 3.0, 3.1, 3.2, 3.3 },
                { 4.0, 4.1, 4.2, 4.3 },
            });

            var v1 = m.SliceColumn(1, 0, 5);
            Assert.AreEqual(new Vector(new double[] { 0.1, 1.1, 2.1, 3.1, 4.1 }), v1);

            var v2 = m.SliceColumn(1, 0, 4);
            Assert.AreEqual(new Vector(new double[] { 0.1, 1.1, 2.1, 3.1 }), v2);

            var v3 = m.SliceColumn(1, 1, 4);
            Assert.AreEqual(new Vector(new double[] { 1.1, 2.1, 3.1, 4.1 }), v3);

            var v4 = m.SliceColumn(1, 1, 3);
            Assert.AreEqual(new Vector(new double[] { 1.1, 2.1, 3.1 }), v4);

            var v5 = m.SliceColumn(1);
            Assert.AreEqual(new Vector(new double[] { 0.1, 1.1, 2.1, 3.1, 4.1 }), v5);
        }

        [TestMethod]
        public void ShouldSliceRow()
        {
            var m = new Matrix(new double[,]
            {
                { 0.0, 0.1, 0.2, 0.3 },
                { 1.0, 1.1, 1.2, 1.3 },
                { 2.0, 2.1, 2.2, 2.3 },
                { 3.0, 3.1, 3.2, 3.3 },
                { 4.0, 4.1, 4.2, 4.3 },
            });

            var v1 = m.SliceRow(1, 0, 4);
            Assert.AreEqual(new Vector(new double[] { 1.0, 1.1, 1.2, 1.3 }), v1);

            var v2 = m.SliceRow(1, 0, 3);
            Assert.AreEqual(new Vector(new double[] { 1.0, 1.1, 1.2 }), v2);

            var v3 = m.SliceRow(1, 1, 3);
            Assert.AreEqual(new Vector(new double[] { 1.1, 1.2, 1.3 }), v3);

            var v4 = m.SliceRow(1, 1, 2);
            Assert.AreEqual(new Vector(new double[] { 1.1, 1.2 }), v4);

            var v5 = m.SliceRow(1);
            Assert.AreEqual(new Vector(new double[] { 1.0, 1.1, 1.2, 1.3 }), v5);
        }

        [TestMethod]
        public void ShouldCopy()
        {
            var m = new Matrix(new double[,]
            {
                { 0.0, 0.1, 0.2, 0.3 },
                { 1.0, 1.1, 1.2, 1.3 },
                { 2.0, 2.1, 2.2, 2.3 },
                { 3.0, 3.1, 3.2, 3.3 },
                { 4.0, 4.1, 4.2, 4.3 },
            });

            var m1 = new double[5, 4];
            m.Copy(0, 5, 0, 4, m1, 0, 0);
            Assert.AreEqual(m, m1);

            var m2 = new double[4, 3];
            m.Copy(0, 4, 0, 3, m2, 0, 0);
            Assert.AreEqual(
                new Matrix(
                    new double[,]
                    {
                        { 0.0, 0.1, 0.2 },
                        { 1.0, 1.1, 1.2 },
                        { 2.0, 2.1, 2.2 },
                        { 3.0, 3.1, 3.2 },
                    }),
                    m2);

            var m3 = new double[4, 3];
            m.Copy(1, 4, 1, 3, m3, 0, 0);
            Assert.AreEqual(
                new Matrix(
                    new double[,]
                    {
                        { 1.1, 1.2, 1.3 },
                        { 2.1, 2.2, 2.3 },
                        { 3.1, 3.2, 3.3 },
                        { 4.1, 4.2, 4.3 },
                    }),
                    m3);

            var m4 = new double[3, 2];
            m.Copy(1, 3, 1, 2, m4, 0, 0);
            Assert.AreEqual(
                new Matrix(
                    new double[,]
                    {
                        { 1.1, 1.2, },
                        { 2.1, 2.2, },
                        { 3.1, 3.2, },
                    }),
                    m4);

            var m5 = new double[7, 6];
            m.Copy(0, 5, 0, 4, m5, 1, 1);
            Assert.AreEqual(
                new Matrix(
                    new double[,]
                    {
                        { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                        { 0.0, 0.0, 0.1, 0.2, 0.3, 0.0 },
                        { 0.0, 1.0, 1.1, 1.2, 1.3, 0.0 },
                        { 0.0, 2.0, 2.1, 2.2, 2.3, 0.0 },
                        { 0.0, 3.0, 3.1, 3.2, 3.3, 0.0 },
                        { 0.0, 4.0, 4.1, 4.2, 4.3, 0.0 },
                        { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                    }),
                    m5);
        }

        [TestMethod]
        public void ShouldSlice()
        {
            var m = new Matrix(new double[,]
            {
                { 0.0, 0.1, 0.2, 0.3 },
                { 1.0, 1.1, 1.2, 1.3 },
                { 2.0, 2.1, 2.2, 2.3 },
                { 3.0, 3.1, 3.2, 3.3 },
                { 4.0, 4.1, 4.2, 4.3 },
            });

            var m1 = m.Slice(0, 5, 0, 4);
            Assert.AreEqual(m, m1);

            var m2 = m.Slice(0, 4, 0, 3);
            Assert.AreEqual(
                new Matrix(
                    new double[,]
                    {
                        { 0.0, 0.1, 0.2 },
                        { 1.0, 1.1, 1.2 },
                        { 2.0, 2.1, 2.2 },
                        { 3.0, 3.1, 3.2 },
                    }),
                    m2);

            var m3 = m.Slice(1, 4, 1, 3);
            Assert.AreEqual(
                new Matrix(
                    new double[,]
                    {
                        { 1.1, 1.2, 1.3 },
                        { 2.1, 2.2, 2.3 },
                        { 3.1, 3.2, 3.3 },
                        { 4.1, 4.2, 4.3 },
                    }),
                    m3);

            var m4 = m.Slice(1, 3, 1, 2);
            Assert.AreEqual(
                new Matrix(
                    new double[,]
                    {
                        { 1.1, 1.2, },
                        { 2.1, 2.2, },
                        { 3.1, 3.2, },
                    }),
                    m4);

            var m5 = m.Duplicate();
            Assert.AreEqual(m, m5);
        }

        [TestMethod]
        public void ShouldTranspose()
        {
            var square = new Matrix(new double[,]
            {
                { 0.0, 0.1, 0.2 },
                { 1.0, 1.1, 1.2 },
                { 2.0, 2.1, 2.2 }
            });
            var m1 = square.Transpose();
            Assert.AreEqual(
                new Matrix(
                    new double[,]
                    {
                        { 0.0, 1.0, 2.0 },
                        { 0.1, 1.1, 2.1 },
                        { 0.2, 1.2, 2.2 },
                    }),
                    m1);

            var rect1 = new Matrix(new double[,]
            {
                { 0.0, 0.1, 0.2 },
                { 1.0, 1.1, 1.2 },
            });
            var m2 = rect1.Transpose();
            Assert.AreEqual(
                new Matrix(
                    new double[,]
                    {
                        { 0.0, 1.0 },
                        { 0.1, 1.1 },
                        { 0.2, 1.2 },
                    }),
                    m2);

            var rect2 = new Matrix(new double[,]
            {
                { 0.0, 0.1 },
                { 1.0, 1.1 },
                { 2.0, 2.1 },
            });
            var m3 = rect2.Transpose();
            Assert.AreEqual(
                new Matrix(
                    new double[,]
                    {
                        { 0.0, 1.0, 2.0 },
                        { 0.1, 1.1, 2.1 },
                    }),
                    m3);
        }

        [TestMethod]
        public void ShouldConcatRows()
        {
            var m1 = new Matrix(new double[,]
            {
                { 0.0, 0.1, 0.2, 0.3 },
                { 1.0, 1.1, 1.2, 1.3 },
                { 2.0, 2.1, 2.2, 2.3 },
            });

            var m2 = new Matrix(new double[,]
            {
                { 3.0, 3.1, 3.2, 3.3 },
                { 4.0, 4.1, 4.2, 4.3 },
            });

            var m3 = m1.ConcatRows(m2);
            Assert.AreEqual(
                new Matrix(
                    new double[,]
                    {
                        { 0.0, 0.1, 0.2, 0.3 },
                        { 1.0, 1.1, 1.2, 1.3 },
                        { 2.0, 2.1, 2.2, 2.3 },
                        { 3.0, 3.1, 3.2, 3.3 },
                        { 4.0, 4.1, 4.2, 4.3 },
                    }),
                    m3);

            var v1 = new Vector(new double[] { 3.0, 3.1, 3.2, 3.3 });
            var m4 = m1.ConcatRows(v1);
            Assert.AreEqual(
                new Matrix(
                    new double[,]
                    {
                        { 0.0, 0.1, 0.2, 0.3 },
                        { 1.0, 1.1, 1.2, 1.3 },
                        { 2.0, 2.1, 2.2, 2.3 },
                        { 3.0, 3.1, 3.2, 3.3 },
                    }),
                    m4);

            var v2 = new Vector(new double[] { 3.0, 3.1, 3.2, 3.3 });
            var m5 = v2.ConcatRows(m1);
            Assert.AreEqual(
                new Matrix(
                    new double[,]
                    {
                        { 3.0, 3.1, 3.2, 3.3 },
                        { 0.0, 0.1, 0.2, 0.3 },
                        { 1.0, 1.1, 1.2, 1.3 },
                        { 2.0, 2.1, 2.2, 2.3 },
                    }),
                    m5);
        }

        [TestMethod]
        public void ShouldConcatColumns()
        {
            var m1 = new Matrix(new double[,]
            {
                { 0.0, 0.1 },
                { 1.0, 1.1 },
                { 2.0, 2.1 },
                { 3.0, 3.1 },
                { 4.0, 4.1 },
            });

            var m2 = new Matrix(new double[,]
            {
                { 0.2, 0.3 },
                { 1.2, 1.3 },
                { 2.2, 2.3 },
                { 3.2, 3.3 },
                { 4.2, 4.3 },
            });

            var m3 = m1.ConcatColumns(m2);
            Assert.AreEqual(
                new Matrix(
                    new double[,]
                    {
                        { 0.0, 0.1, 0.2, 0.3 },
                        { 1.0, 1.1, 1.2, 1.3 },
                        { 2.0, 2.1, 2.2, 2.3 },
                        { 3.0, 3.1, 3.2, 3.3 },
                        { 4.0, 4.1, 4.2, 4.3 },
                    }),
                    m3);

            var v1 = new Vector(new double[]
           {
                0.2,
                1.2,
                2.2,
                3.2,
                4.2
           });
            var m4 = m1.ConcatColumns(v1);
            Assert.AreEqual(
                new Matrix(
                    new double[,]
                    {
                        { 0.0, 0.1, 0.2 },
                        { 1.0, 1.1, 1.2 },
                        { 2.0, 2.1, 2.2 },
                        { 3.0, 3.1, 3.2 },
                        { 4.0, 4.1, 4.2 },
                    }),
                    m4);

            var m5 = v1.ConcatColumns(m1);
            Assert.AreEqual(
                new Matrix(
                    new double[,]
                    {
                        { 0.2, 0.0, 0.1 },
                        { 1.2, 1.0, 1.1 },
                        { 2.2, 2.0, 2.1 },
                        { 3.2, 3.0, 3.1 },
                        { 4.2, 4.0, 4.1 },
                    }),
                    m5);
        }
    }
}
