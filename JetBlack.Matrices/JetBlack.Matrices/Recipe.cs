using System;

namespace JetBlack.Matrices
{
    public static class Recipe
    {
        public static void choldc(double[,] a, int n, double[] p)
        {
            for (var i = 0; i < n; ++i)
            {
                for (var j = i; j < n; j++)
                {
                    var sum = a[i, j];
                    for (var k = i - 1; k >= 0; --k)
                        sum -= a[i, k] * a[j, k];

                    if (i == j)
                    {
                        if (sum <= 0.0)
                            throw new ArithmeticException("choldc failed");
                        p[i] = Math.Sqrt(sum);
                    }
                    else
                        a[j, i] = sum / p[i];
                }
            }
        }

        public static void cholsl(double[,] a, int n, double[] p, double[] b, double[] x)
        {
            for (var i = 0; i < n; ++i)
            {
                var sum = b[i];
                for (var k = i - 1; k >= 0; --k)
                    sum -= a[i, k] * x[k];
                x[i] = sum / p[i];
            }
            for (var i = n - 1; i >= 0; --i)
            {
                var sum = x[i];
                for (var k = i + 1; k < n; ++k)
                    sum -= a[k, i] * x[k];
                x[i] = sum / p[i];
            }
        }
        public static void lubksb(double[,] a, int n, int[] indx, double[] b)
        {
            int ii = -1;

            for (var i = 0; i < n; ++i)
            {
                var ip = indx[i];
                var sum = b[ip];
                b[ip] = b[i];
                if (ii != -1)
                    for (var j = ii; j <= i; ++j)
                        sum -= a[i, j] * b[j];
                else if (sum != 0)
                    ii = i;
                b[i] = sum;
            }

            for (var i = n - 1; i >= 0; --i)
            {
                var sum = b[i];
                for (var j = i + 1; j < n; ++j)
                    sum -= a[i, j] * b[j];
                b[i] = sum / a[i, i];
            }
        }

        public static void ludcmp(double[,] a, int n, int[] indx, out double d)
        {
            const double TINY = 1.0e-20;

            int imax = 0;
            //double dum, temp;

            var vv = new double[n];
            d = 1.0;
            for (var i = 0; i < n; ++i)
            {
                var big = 0.0;
                for (var j = 0; j < n; ++j)
                {
                    var temp = Math.Abs(a[i, j]);
                    if (temp > big)
                        big = temp;
                }

                if (big == 0.0)
                    throw new ArithmeticException("Singular matrix in routine ludcmp");

                vv[i] = 1.0 / big;
            }

            for (var j = 0; j < n; ++j)
            {
                for (var i = 0; i < j; ++i)
                {
                    var sum = a[i, j];
                    for (var k = 0; k < i; ++k)
                        sum -= a[i, k] * a[k, j];
                    a[i, j] = sum;
                }

                var big = 0.0;
                for (var i = j; i < n; ++i)
                {
                    var sum = a[i, j];
                    for (var k = 0; k < j; ++k)
                        sum -= a[i, k] * a[k, j];
                    a[i, j] = sum;
                    var dum = vv[i] * Math.Abs(sum);
                    if (dum >= big)
                    {
                        big = dum;
                        imax = i;
                    }
                }

                if (j != imax)
                {
                    for (var k = 0; k < n; ++k)
                    {
                        var dum = a[imax, k];
                        a[imax, k] = a[j, k];
                        a[j, k] = dum;
                    }
                    d = -d;
                    vv[imax] = vv[j];
                }
                indx[j] = imax;
                if (a[j, j] == 0.0) a[j, j] = TINY;
                if (j != n)
                {
                    var dum = 1.0 / (a[j, j]);
                    for (var i = j + 1; i < n; ++i)
                        a[i, j] *= dum;
                }
            }
        }

        public static void qrdcmp(double[,] a, int n, double[] c, double[] d, out bool sing)
        {
            sing = false;
            for (var k = 0; k < n - 1; ++k)
            {
                var scale = 0.0;
                for (var i = k; i < n; ++i)
                    scale = Math.Max(scale, Math.Abs(a[i, k]));

                if (scale == 0)
                {
                    sing = true;
                    c[k] = d[k] = 0.0;
                }
                else
                {
                    for (var i = k; i < n; ++i)
                        a[i, k] /= scale;

                    var sum = 0.0;
                    for (var i = k; i < n; ++i)
                        sum += SQR(a[i, k]);

                    var sigma = SIGN(Math.Sqrt(sum), a[k, k]);
                    a[k, k] += sigma;
                    c[k] = sigma * a[k, k];
                    d[k] = -scale * sigma;
                    for (var j = k + 1; j < n; ++j)
                    {
                        sum = 0.0;
                        for (var i = k; i < n; ++i)
                            sum += a[i, k] * a[i, j];

                        var tau = sum / c[k];
                        for (var i = k; i < n; ++i)
                            a[i, j] -= tau * a[i, k];
                    }
                }
            }

            d[n - 1] = a[n - 1, n - 1];

            if (d[n - 1] == 0)
                sing = true;
        }

        public static void qrsolv(double[,] a, int n, double[] c, double[] d, double[] b)
        {
            for (var j = 0; j < n - 1; ++j)
            {
                var sum = 0.0;
                for (var i = j; i < n; i++)
                    sum += a[i, j] * b[i];

                var tau = sum / c[j];

                for (var i = j; i < n; ++i)
                    b[i] -= tau * a[i, j];
            }

            rsolv(a, n, d, b);
        }

        public static void rsolv(double[,] a, int n, double[] d, double[] b)
        {
            b[n] /= d[n];

            for (var i = n - 2; i >= 0; --i)
            {
                var sum = 0.0;
                for (var j = i + 1; j < n; ++j)
                    sum += a[i, j] * b[j];

                b[i] = (b[i] - sum) / d[i];
            }
        }

        public static void qrupdt(double[,] r, double[,] qt, int n, double[] u, double[] v)
        {
            int k;

            for (k = n - 1; k >= 0; --k)
                if (u[k] == 0)
                    break;

            if (k < 0)
                k = 0;

            for (var i = k - 1; i >= 0; --i)
            {
                rotate(r, qt, n, i, u[i], -u[i + 1]);

                if (u[i] == 0.0)
                    u[i] = Math.Abs(u[i + 1]);
                else if (Math.Abs(u[i]) > Math.Abs(u[i + 1]))
                    u[i] = Math.Abs(u[i]) * Math.Sqrt(1.0 + SQR(u[i + 1] / u[i]));
                else u[i] = Math.Abs(u[i + 1]) * Math.Sqrt(1.0 + SQR(u[i] / u[i + 1]));
            }

            for (var j = 0; j < n; j++)
                r[1, j] += u[1] * v[j];

            for (var i = 0; i < k - 1; i++)
                rotate(r, qt, n, i, r[i, i], -r[i + 1, i]);
        }

        public static void rotate(double[,] r, double[,] qt, int n, int i, double a, double b)
        {
            double c, s;

            if (a == 0.0)
            {
                c = 0.0;
                s = (b >= 0.0 ? 1.0 : -1.0);
            }
            else if (Math.Abs(a) > Math.Abs(b))
            {
                var fact = b / a;
                c = SIGN(1.0 / Math.Sqrt(1.0 + (fact * fact)), a);
                s = fact * c;
            }
            else
            {
                var fact = a / b;
                s = SIGN(1.0 / Math.Sqrt(1.0 + (fact * fact)), b);
                c = fact * s;
            }

            for (var j = 0; j < n; ++j)
            {
                var y = r[i, j];
                var w = r[i + 1, j];
                r[i, j] = c * y - s * w;
                r[i + 1, j] = s * y + c * w;
            }

            for (var j = 0; j < n; ++j)
            {
                var y = qt[i, j];
                var w = qt[i + 1, j];
                qt[i, j] = c * y - s * w;
                qt[i + 1, j] = s * y + c * w;
            }
        }

        public static void svbksb(double[,] u, double[] w, double[,] v, int m, int n, double[] b, double[] x)
        {
            var tmp = new double[n];
            for (var j = 0; j < n; ++j)
            {
                var s = 0.0;
                if (w[j] != 0.0)
                {
                    for (var i = 0; i < m; ++i)
                        s += u[i, j] * b[i];
                    s /= w[j];
                }
                tmp[j] = s;
            }

            for (var j = 0; j < n; ++j)
            {
                var s = 0.0;
                for (var jj = 0; jj < n; ++jj)
                    s += v[j, jj] * tmp[jj];
                x[j] = s;
            }
        }

        public static double SQR(double x)
        {
            return x * x;
        }

        public static double pythag(double a, double b)
        {
            var absa = Math.Abs(a);
            var absb = Math.Abs(b);
            if (absa > absb)
                return absa * Math.Sqrt(1.0 + SQR(absb / absa));
            else
                return (absb == 0.0 ? 0.0 : absb * Math.Sqrt(1.0 + SQR(absa / absb)));
        }

        public static void svdcmp(double[,] a, int m, int n, double[] w, double[,] v)
        {
            bool flag;
            int jj, nm;
            double anorm, c, f, h, s, scale, x, y, z;

            var l = 0;
            var rv1 = new double[n];
            var g = scale = anorm = 0.0;
            for (var i = 0; i < n; ++i)
            {
                l = i + 1;
                rv1[i] = scale * g;
                g = s = scale = 0.0;
                if (i <= m)
                {
                    for (var k = i; k < m; ++k)
                        scale += Math.Abs(a[k, i]);

                    if (scale != 0)
                    {
                        for (var k = i; k < m; ++k)
                        {
                            a[k, i] /= scale;
                            s += a[k, i] * a[k, i];
                        }

                        f = a[i, i];
                        g = -SIGN(Math.Sqrt(s), f);
                        h = f * g - s;
                        a[i, i] = f - g;

                        for (var j = 0; j < n; ++j)
                        {
                            s = 0.0;
                            for (var k = i; k < m; ++k)
                                s += a[k, i] * a[k, j];

                            f = s / h;

                            for (var k = i; k < m; ++k)
                                a[k, j] += f * a[k, i];
                        }

                        for (var k = i; k < m; ++k)
                            a[k, i] *= scale;
                    }
                }

                w[i] = scale * g;
                g = s = scale = 0.0;

                if (i < m && i != n - 1)
                {
                    for (var k = l; k < n; ++k)
                        scale += Math.Abs(a[i, k]);

                    if (scale != 0)
                    {
                        for (var k = l; k < n; ++k)
                        {
                            a[i, k] /= scale;
                            s += a[i, k] * a[i, k];
                        }

                        f = a[i, l];
                        g = -SIGN(Math.Sqrt(s), f);
                        h = f * g - s;
                        a[i, l] = f - g;

                        for (var k = l; k < n; ++k)
                            rv1[k] = a[i, k] / h;

                        for (var j = l; j < m; ++j)
                        {
                            s = 0.0;
                            for (var k = l; k < n; ++k)
                                s += a[j, k] * a[i, k];

                            for (var k = l; k < n; ++k)
                                a[j, k] += s * rv1[k];
                        }

                        for (var k = l; k < n; ++k)
                            a[i, k] *= scale;
                    }
                }

                anorm = Math.Max(anorm, (Math.Abs(w[i]) + Math.Abs(rv1[i])));
            }

            for (var i = n - 1; i >= 0; --i)
            {
                if (i < n - 1)
                {
                    if (g != 0)
                    {
                        for (var j = l; j < n; ++j)
                            v[j, i] = (a[i, j] / a[i, l]) / g;

                        for (var j = l; j < n; ++j)
                        {
                            s = 0.0;
                            for (var k = l; k < n; ++k)
                                s += a[i, k] * v[k, j];
                            for (var k = l; k < n; ++k)
                                v[k, j] += s * v[k, i];
                        }
                    }

                    for (var j = l; j < n; ++j)
                        v[i, j] = v[j, i] = 0.0;
                }

                v[i, i] = 1.0;
                g = rv1[i];
                l = i;
            }

            for (var i = Math.Min(m, n) - 1; i >= 0; --i)
            {
                l = i + 1;
                g = w[i];
                for (var j = l; j < n; ++j)
                    a[i, j] = 0.0;

                if (g != 0)
                {
                    g = 1.0 / g;
                    for (var j = l; j < n; ++j)
                    {
                        s = 0.0;
                        for (var k = l; k < m; ++k)
                            s += a[k, i] * a[k, j];

                        f = (s / a[i, i]) * g;

                        for (var k = i; k < m; ++k)
                            a[k, j] += f * a[k, i];
                    }

                    for (var j = i; j < m; ++j)
                        a[j, i] *= g;
                }
                else
                    for (var j = i; j < m; ++j)
                        a[j, i] = 0.0;
                ++a[i, i];
            }

            for (var k = n - 1; k >= 0; --k)
            {
                for (var its = 1; its <= 30; ++its)
                {
                    flag = true;
                    nm = 0;
                    for (l = k; l >= 0; --l)
                    {
                        nm = l - 1;
                        if ((Math.Abs(rv1[l]) + anorm) == anorm)
                        {
                            flag = false;
                            break;
                        }

                        if ((Math.Abs(w[nm]) + anorm) == anorm)
                            break;
                    }

                    if (flag)
                    {
                        c = 0.0;
                        s = 1.0;
                        for (var i = l; i <= k; i++)
                        {
                            f = s * rv1[i];
                            rv1[i] = c * rv1[i];

                            if ((Math.Abs(f) + anorm) == anorm)
                                break;

                            g = w[i];
                            h = pythag(f, g);
                            w[i] = h;
                            h = 1.0 / h;
                            c = g * h;
                            s = -f * h;

                            for (var j = 0; j < m; ++j)
                            {
                                y = a[j, nm];
                                z = a[j, i];
                                a[j, nm] = y * c + z * s;
                                a[j, i] = z * c - y * s;
                            }
                        }
                    }

                    z = w[k];
                    if (l == k)
                    {
                        if (z < 0.0)
                        {
                            w[k] = -z;
                            for (var j = 0; j < n; ++j)
                                v[j, k] = -v[j, k];
                        }
                        break;
                    }

                    if (its == 30)
                        throw new ArithmeticException("no convergence in 30 svdcmp iterations");

                    x = w[l];
                    nm = k - 1;
                    y = w[nm];
                    g = rv1[nm];
                    h = rv1[k];
                    f = ((y - z) * (y + z) + (g - h) * (g + h)) / (2.0 * h * y);
                    g = pythag(f, 1.0);
                    f = ((x - z) * (x + z) + h * ((y / (f + SIGN(g, f))) - h)) / x;
                    c = s = 1.0;
                    for (var j = l; j < nm; j++)
                    {
                        var i = j + 1;
                        g = rv1[i];
                        y = w[i];
                        h = s * g;
                        g = c * g;
                        z = pythag(f, h);
                        rv1[j] = z;
                        c = f / z;
                        s = h / z;
                        f = x * c + g * s;
                        g = g * c - x * s;
                        h = y * s;
                        y *= c;
                        for (jj = 1; jj <= n; jj++)
                        {
                            x = v[jj, j];
                            z = v[jj, i];
                            v[jj, j] = x * c + z * s;
                            v[jj, i] = z * c - x * s;
                        }
                        z = pythag(f, h);
                        w[j] = z;
                        if (z != 0)
                        {
                            z = 1.0 / z;
                            c = f * z;
                            s = h * z;
                        }
                        f = c * g + s * y;
                        x = c * y - s * g;
                        for (jj = 1; jj <= m; jj++)
                        {
                            y = a[jj, j];
                            z = a[jj, i];
                            a[jj, j] = y * c + z * s;
                            a[jj, i] = z * c - y * s;
                        }
                    }
                    rv1[l] = 0.0;
                    rv1[k] = f;
                    w[k] = x;
                }
            }
        }

        public static void svdvar(double[,] v, int ma, double[] w, double[,] cvm)
        {
            var wti = new double[ma];
            for (var i = 0; i < ma; ++i)
            {
                wti[i] = 0.0;
                if (w[i] != 0)
                    wti[i] = 1.0 / (w[i] * w[i]);
            }

            for (var i = 0; i < ma; ++i)
            {
                for (var j = 0; j < i; ++j)
                {
                    var sum = 0.0;
                    for (var k = 0; k < ma; ++k)
                        sum += v[i, k] * v[j, k] * wti[k];

                    cvm[j, i] = cvm[i, j] = sum;
                }
            }
        }

        public static void svdfit(double[] x, double[] y, double[] sig, int ndata, double[] a, int ma, double[,] u, double[,] v, double[] w, out double chisq, Action<double, double[], int> funcs)
        {
            const double TOL = 1.0e-5;

            double wmax, tmp, thresh, sum;

            var b = new double[ndata];
            var afunc = new double[ma];
            for (var i = 0; i < ndata; ++i)
            {
                funcs(x[i], afunc, ma);
                tmp = 1.0 / sig[i];
                for (var j = 0; j < ma; ++j)
                    u[i, j] = afunc[j] * tmp;
                b[i] = y[i] * tmp;
            }

            svdcmp(u, ndata, ma, w, v);
            wmax = 0.0;

            for (var j = 0; j < ma; ++j)
                if (w[j] > wmax) wmax = w[j];

            thresh = TOL * wmax;

            for (var j = 0; j < ma; ++j)
                if (w[j] < thresh) w[j] = 0.0;

            svbksb(u, w, v, ndata, ma, b, a);

            chisq = 0.0;

            for (var i = 0; i < ndata; ++i)
            {
                funcs(x[i], afunc, ma);
                sum = 0.0;
                for (var j = 0; j < ma; ++j)
                    sum += a[j] * afunc[j];
                tmp = (y[i] - sum) / sig[i];
                chisq += tmp * tmp;
            }
        }

        public static double SIGN(double a, double b)
        {
            return b >= 0
                ? (a >= 0 ? a : -a)
                : (a >= 0 ? -a : a);
        }
        public static void SWAP(ref double a, ref double b)
        {
            double temp = a;
            a = b;
            b = temp;
        }

        public static void gaussj(double[,] a, int n, double[,] b, int m)
        {
            int icol = 0, irow = 0;

            var indxc = new int[n];
            var indxr = new int[n];
            var ipiv = new int[n];
            for (var j = 0; j < n; ++j)
                ipiv[j] = 0;

            for (var i = 0; i < n; ++i)
            {
                var big = 0.0;
                for (var j = 0; j < n; ++j)
                {
                    if (ipiv[j] != 1)
                    {
                        for (var k = 0; k < n; ++k)
                        {
                            if (ipiv[k] == 0)
                            {
                                if (Math.Abs(a[j, k]) >= big)
                                {
                                    big = Math.Abs(a[j, k]);
                                    irow = j;
                                    icol = k;
                                }
                            }
                            else if (ipiv[k] > 1)
                                throw new ArithmeticException("Singular Matrix-1");
                        }
                    }
                }

                ++(ipiv[icol]);

                if (irow != icol)
                {
                    for (var l = 0; l < n; ++l)
                        SWAP(ref a[irow, l], ref a[icol, l]);
                    for (var l = 0; l < m; ++l)
                        SWAP(ref b[irow, l], ref b[icol, l]);
                }

                indxr[i] = irow;
                indxc[i] = icol;
                if (a[icol, icol] == 0)
                    throw new ArithmeticException("Singular Matrix-2");

                var pivinv = 1.0 / a[icol, icol];
                a[icol, icol] = 1.0;
                for (var l = 0; l < n; ++l)
                    a[icol, l] *= pivinv;
                for (var l = 0; l < m; ++l)
                    b[icol, l] *= pivinv;

                for (var ll = 0; ll < n; ++ll)
                {
                    if (ll != icol)
                    {
                        var dum = a[ll, icol];
                        a[ll, icol] = 0.0;
                        for (var l = 0; l < n; ++l)
                            a[ll, l] -= a[icol, l] * dum;
                        for (var l = 0; l < m; ++l)
                            b[ll, l] -= b[icol, l] * dum;
                    }
                }
            }

            for (var l = n - 1; l >= 0; --l)
            {
                if (indxr[l] != indxc[l])
                    for (var k = 0; k < n; ++k)
                        SWAP(ref a[k, indxr[l]], ref a[k, indxc[l]]);
            }
        }
    }
}
