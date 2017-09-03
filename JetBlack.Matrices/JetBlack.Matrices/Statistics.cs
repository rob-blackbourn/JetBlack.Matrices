using System;
using System.Collections.Generic;

namespace JetBlack.Matrices
{
    public static class Statistics
    {
        public static double OneWayAnova(double[,] data)
        {
            var a = 1 + data.GetUpperBound(0); // number of groups
            var n = 1 + data.GetUpperBound(1); // number of observations in a group

            var sum = new double[a];
            var sumsq = new double[a];
            for (var i = 0; i < a; ++i)
            {
                sum[i] = sumsq[i] = 0;

                for (int j = 0; j < n; ++j)
                {
                    sum[i] += data[i, j];
                    sumsq[i] += data[i, j] * data[i, j];
                }
            }

            var totalSum = 0.0;
            var totalSumSq = 0.0;
            var totalSqSum = 0.0;
            double[] sqsum = new double[a];
            for (int i = 0; i < a; ++i)
            {
                sqsum[i] = sum[i] * sum[i];

                totalSum += sum[i];
                totalSumSq += sumsq[i];
                totalSqSum += sqsum[i];
            }

            var T = totalSumSq;
            var A = totalSqSum / n;
            var CF = (totalSum * totalSum) / (n * a);

            var DFa = a - 1;
            var SSa = A - CF;
            var MSa = SSa / DFa;

            int DFe = a * (n - 1);
            var SSe = T - A;
            var MSe = SSe / (a * (n - 1));

            var F = MSa / MSe;

            return F;
        }

        /// <summary>
        /// Compute the two-tailed probability of correct rejection of the null
        /// hypothesis with an F-ratio of x, for m degrees of freedom in the numerator and
        /// n degrees of freedom in the denominator.In the special case of only two
        /// populations, this is equivalent to Student's t-test with m=1 and x=t**2.
        /// Coded by Matthew Belmonte <mkb4 @Cornell.edu>, 28 September 1995.  This
        /// implementation Copyright (c) 1995 by Matthew Belmonte.Permission for use and
        /// distribution is hereby granted, subject to the restrictions that this
        /// copyright notice and reference list be included in its entirety, and that any
        /// and all changes made to the program be clearly noted in the program text.
        /// 
        /// References:
        /// 
        /// Egon Dorrer, "Algorithm 322: F-Distribution [S14]", Communications of the
        /// Association for Computing Machinery 11:2:116-117 (1968).
        /// J.B.F.Field, "Certification of Algorithm 322 [S14] F-Distribution",
        /// Communications of the Association for Computing Machinery 12:1:39 (1969).
        /// 
        /// Hubert Tolman, "Remark on Algorithm 322 [S14] F-Distribution", Communications
        /// of the Association for Computing Machinery 14:2:117 (1971).
        /// </summary>
        /// <param name="m">F score</param>
        /// <param name="n">Degress of freedom</param>
        /// <param name="x">F ratio</param>
        /// <returns></returns>
        public static double fisher(int m, int n, double x)
        {
            double d;
            double p;
            double y;
            double zk;

            int a = 2 * (m / 2) - m + 2;
            int b = 2 * (n / 2) - n + 2;
            double w = (x * m) / n;
            double z = 1.0 / (1.0 + w);

            if (a == 1)
            {
                if (b == 1)
                {
                    p = Math.Sqrt(w);
                    y = 0.3183098862;
                    d = y * z / p;
                    p = 2.0 * y * Math.Atan(p);
                }
                else
                {
                    p = Math.Sqrt(w * z);
                    d = 0.5 * p * z / w;
                }
            }
            else if (b == 1)
            {
                p = Math.Sqrt(z);
                d = 0.5 * z * p;
                p = 1.0 - p;
            }
            else
            {
                d = z * z;
                p = w * z;
            }

            y = 2.0 * w / z;

            if (a == 1)
            {
                for (int j = b + 2; j <= n; j = j + 2)
                {
                    d = d * (1.0 + 1.0 / (j - 2)) * z;
                    p = p + d * y / (j - 1);
                }
            }
            else
            {
                zk = Math.Pow(z, (double)((n - 1) / 2));
                d = d * (zk * n) / b;
                p = p * zk + w * z * (zk - 1.0) / (z - 1.0);
            }

            y = w * z;
            z = 2.0 / z;
            b = n - 2;

            for (int i = a + 2; i <= m; i = i + 2)
            {
                int j = i + b;
                d = d * (y * j) / (i - 2);
                p = p - z * d / j;
            }
            if (p < 0.0)
            {
                p = 0.0;
            }
            if (1.0 < p)
            {
                p = 1.0;
            }
            return p;
        }

        public static double student(int df, double t)
        {
            return (fisher(1, df, t * t));
        }

        public static Summary Summarise(this IEnumerable<double> source)
        {
            return new Summary(source);
        }

        public struct Summary
        {
            public Summary(IEnumerable<double> source)
            {
                Sum = 0.0;
                SumSq = 0.0;
                Count = 0;
                foreach (var value in source)
                {
                    Sum += value;
                    SumSq += value * value;
                    ++Count;
                }

            }
            public double Sum { get; }
            public double SumSq { get; }
            public int Count { get; }
            public double Mean => Sum / Count;
            public double SqSum => Sum * Sum;
            public double PopulationVariance => (SumSq - SqSum / Count) / Count;
            public double PopulationStdDev => Math.Sqrt(PopulationVariance);
            public double SampleVariance => (SumSq - SqSum / Count) / (Count - 1);
            public double SampleStdDev => Math.Sqrt(SampleVariance);

            public override string ToString()
            {
                return $"Sum={Sum}, SumSq={SumSq}, Count={Count}, Mean={Mean}, SqSum={SqSum}, PopulationVariance={PopulationVariance}, PopulationStdDev={PopulationStdDev}, SampleVariance={SampleVariance}, SampleStdDev={SampleStdDev}";
            }
        }
    }
}
