using System;
using System.Collections.Generic;

namespace JetBlack.Matrices
{
    public class VectorComparer<T> : IEqualityComparer<T[]> where T : IEquatable<T>
    {
        public static VectorComparer<T> Default { get; } = new VectorComparer<T>();

        public bool Equals(T[] x, T[] y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;

            if (x.Length != y.Length)
                return false;

            for (var i = 0; i < x.Length; ++i)
                if (!x[i].Equals(y[i]))
                    return false;

            return true;
        }

        public int GetHashCode(T[] obj)
        {
            if (obj == null)
                return 0;

            var hash = obj.Length.GetHashCode();
            for (var i = 0; i < obj.Length; ++i)
                hash ^= obj[i].GetHashCode();

            return hash;
        }
    }
}
