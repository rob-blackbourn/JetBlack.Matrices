using System;
using System.Linq;

namespace JetBlack.Matrices
{
    public class Vector : IEquatable<Vector>, IFormattable
    {
        private readonly double[] _vector;

        public Vector(int length)
            : this(new double[length])
        {
        }

        public Vector(double[] values)
        {
            _vector = values;
        }

        public double this[int i]
        {
            get => _vector[i];
            set => _vector[i] = value;
        }

        public int Length => _vector.Length;

        public bool Equals(Vector other)
        {
            return other != null &&
                _vector.SequenceEqual(_vector);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Vector);
        }

        public override int GetHashCode()
        {
            return _vector.Aggregate(_vector.Length.GetHashCode(), (hash, value) => hash ^ value.GetHashCode());
        }

        public override string ToString()
        {
            return ToString(null, null);
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            //"G" is .Net's standard for general formatting--all
            //types should support it
            if (format == null) format = "G";

            // is the user providing their own format provider?
            if (formatProvider != null)
            {
                var formatter = formatProvider.GetFormat(GetType()) as ICustomFormatter;
                if (formatter != null)
                    return formatter.Format(format, this, formatProvider);
            }

            return string.Concat('[', string.Join(", ", _vector.Select(x => x.ToString(format))), ']');
        }

        public static implicit operator Vector(double[] values)
        {
            return new Vector(values);
        }

        public static implicit operator double[] (Vector value)
        {
            return value._vector;
        }
    }
}
