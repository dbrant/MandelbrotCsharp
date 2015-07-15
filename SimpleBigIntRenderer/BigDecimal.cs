using System;

namespace Mandelbrot.SimpleBigIntRenderer
{
    /// <summary>
    /// Arbitrary precision decimal.
    /// All operations are exact, except for division. Division never determines more digits than the given precision.
    /// Based on http://stackoverflow.com/a/4524254
    /// Author: Jan Christoph Bernack (contact: jc.bernack at googlemail.com)
    /// </summary>
    public struct BigDecimal : IComparable, IComparable<BigDecimal>
    {
        /// <summary>
        /// Specifies whether the significant digits should be truncated to the given precision after each operation.
        /// </summary>
        public static bool AlwaysTruncate = false;

        /// <summary>
        /// Sets the maximum precision of division operations.
        /// If AlwaysTruncate is set to true all operations are affected.
        /// </summary>
        public static int Precision = 10;

        private BigInteger mantissa;
        public BigInteger Mantissa
        {
            get { return mantissa; }
            set
            {
                mantissa = new BigInteger(value);
            }
        }
        public int Exponent;

        public BigDecimal(BigInteger mantissa, int exponent)
        {
            this.mantissa = mantissa;
            Exponent = exponent;
            
            if (AlwaysTruncate)
            {
                Truncate();
            }
        }

        public void Set(BigDecimal d)
        {
            this.mantissa = new BigInteger(d.mantissa);
            this.Exponent = d.Exponent;
        }


        /// <summary>
        /// Removes trailing zeros on the mantissa
        /// </summary>
        public void Normalize()
        {
            if (Mantissa.IsZero())
            {
                Exponent = 0;
            }
            else
            {
                while (Mantissa.Mod10() > 0)
                {
                    Mantissa.DivByPow10(1);
                    Exponent++;
                }
            }
        }

        /// <summary>
        /// Truncate the number to the given precision by removing the least significant digits.
        /// </summary>
        public void Truncate()
        {
            int numDigits = Mantissa.NumDigits;
            if (numDigits > Precision)
            {
                int powTen = numDigits - Precision;
                BigInteger b = Mantissa.DivByPow10(powTen);
                Mantissa = b;
                Exponent += powTen;
            }
        }

        #region Conversions

        public static implicit operator BigDecimal(int value)
        {
            return new BigDecimal(value, 0);
        }

        public static implicit operator BigDecimal(double value)
        {
            var mantissa = (BigInteger)value;
            var exponent = 0;
            double scaleFactor = 1;
            while (Math.Abs(value * scaleFactor - (double)mantissa) > 0)
            {
                exponent -= 1;
                scaleFactor *= 10;
                mantissa = (BigInteger)(value * scaleFactor);
            }
            return new BigDecimal(mantissa, exponent);
        }

        public static explicit operator double(BigDecimal value)
        {
            return (double)value.Mantissa * Math.Pow(10, value.Exponent);
        }

        public static explicit operator float(BigDecimal value)
        {
            return Convert.ToSingle((double)value);
        }

        public static explicit operator int(BigDecimal value)
        {
            return (int)(new BigInteger(value.Mantissa).MulByPow10(value.Exponent));
        }

        public static explicit operator uint(BigDecimal value)
        {
            return (uint)(new BigInteger(value.Mantissa).MulByPow10(value.Exponent));
        }

        #endregion

        #region Operators

        public static BigDecimal operator +(BigDecimal value)
        {
            return value;
        }

        public void Zero()
        {
            this.Mantissa = 0;
            this.Exponent = 0;
        }

        /// <summary>
        /// Returns the mantissa of value, aligned to the exponent of reference.
        /// Assumes the exponent of value is larger than of value.
        /// </summary>
        private static BigInteger AlignExponent(BigDecimal value, BigDecimal reference)
        {
            var b = new BigInteger(value.Mantissa).MulByPow10(value.Exponent - reference.Exponent);
            return b;
        }

        private static BigInteger AlignExponentInPlace(BigDecimal value, BigDecimal reference)
        {
            value.mantissa.MulByPow10(value.Exponent - reference.Exponent);
            return value.mantissa;
        }

        public void Add(BigDecimal value)
        {
            if (Exponent > value.Exponent)
            {
                mantissa.MulByPow10(Exponent - value.Exponent);
                mantissa.Add(value.mantissa);
                Exponent = value.Exponent;
            }
            else if (Exponent < value.Exponent)
            {
                //mantissa.MulByPow10(Exponent - value.Exponent);
                mantissa.Add(AlignExponent(value, this));
            }
            else
                mantissa.Add(value.Mantissa);
        }

        public void Multiply(BigDecimal value)
        {
            this.mantissa.Multiply(value.Mantissa);
            this.Exponent += value.Exponent;
        }

        public void Divide(BigDecimal divisor)
        {
            int exponentChange = Precision - (Mantissa.NumDigits - divisor.Mantissa.NumDigits);
            if (exponentChange < 0)
            {
                exponentChange = 0;
            }
            this.Mantissa.MulByPow10(exponentChange);
            this.Mantissa.Divide(divisor.Mantissa);
            this.Exponent -= divisor.Exponent + exponentChange;
        }

        public static BigDecimal operator -(BigDecimal value)
        {
            BigDecimal b = 0;
            b.Set(value);
            b.mantissa.Negate();
            return b;
        }

        public static BigDecimal operator ++(BigDecimal value)
        {
            return value + 1;
        }

        public static BigDecimal operator --(BigDecimal value)
        {
            return value - 1;
        }

        public static BigDecimal operator +(BigDecimal left, BigDecimal right)
        {
            return Add(left, right);
        }

        public static BigDecimal operator -(BigDecimal left, BigDecimal right)
        {
            return Add(left, -right);
        }

        private static BigDecimal Add(BigDecimal left, BigDecimal right)
        {
            BigDecimal b = 0;
            b.Set(left);

            return left.Exponent > right.Exponent
                ? new BigDecimal(AlignExponentInPlace(b, right).Add(right.Mantissa), right.Exponent)
                : new BigDecimal(AlignExponent(right, left).Add(left.Mantissa), left.Exponent);
        }

        public static BigDecimal operator *(BigDecimal left, BigDecimal right)
        {
            return new BigDecimal(left.Mantissa * right.Mantissa, left.Exponent + right.Exponent);
        }

        public static BigDecimal operator /(BigDecimal dividend, BigDecimal divisor)
        {
            var exponentChange = Precision - (dividend.Mantissa.NumDigits - divisor.Mantissa.NumDigits);
            if (exponentChange < 0)
            {
                exponentChange = 0;
            }
            BigInteger b = new BigInteger(dividend.Mantissa);
            b.MulByPow10(exponentChange);
            return new BigDecimal(b.Divide(divisor.Mantissa), dividend.Exponent - divisor.Exponent - exponentChange);
        }

        public static bool operator ==(BigDecimal left, BigDecimal right)
        {
            return left.Exponent == right.Exponent && left.Mantissa == right.Mantissa;
        }

        public static bool operator !=(BigDecimal left, BigDecimal right)
        {
            return left.Exponent != right.Exponent || left.Mantissa != right.Mantissa;
        }

        public static bool operator <(BigDecimal left, BigDecimal right)
        {
            return left.Exponent > right.Exponent ? AlignExponent(left, right) < right.Mantissa : left.Mantissa < AlignExponent(right, left);
        }

        public static bool operator >(BigDecimal left, BigDecimal right)
        {
            return left.Exponent > right.Exponent ? AlignExponent(left, right) > right.Mantissa : left.Mantissa > AlignExponent(right, left);
        }

        public static bool operator <=(BigDecimal left, BigDecimal right)
        {
            return left.Exponent > right.Exponent ? AlignExponent(left, right) <= right.Mantissa : left.Mantissa <= AlignExponent(right, left);
        }

        public static bool operator >=(BigDecimal left, BigDecimal right)
        {
            return left.Exponent > right.Exponent ? AlignExponent(left, right) >= right.Mantissa : left.Mantissa >= AlignExponent(right, left);
        }

        #endregion

        #region Additional mathematical functions

        public static BigDecimal Exp(double exponent)
        {
            var tmp = (BigDecimal)1;
            while (Math.Abs(exponent) > 100)
            {
                var diff = exponent > 0 ? 100 : -100;
                tmp *= Math.Exp(diff);
                exponent -= diff;
            }
            return tmp * Math.Exp(exponent);
        }

        public static BigDecimal Pow(double basis, double exponent)
        {
            var tmp = (BigDecimal)1;
            while (Math.Abs(exponent) > 100)
            {
                var diff = exponent > 0 ? 100 : -100;
                tmp *= Math.Pow(basis, diff);
                exponent -= diff;
            }
            return tmp * Math.Pow(basis, exponent);
        }

        #endregion

        public override string ToString()
        {
            return string.Concat(Mantissa.ToString(), "E", Exponent);
        }

        public bool Equals(BigDecimal other)
        {
            return other.Mantissa.Equals(Mantissa) && other.Exponent == Exponent;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is BigDecimal && Equals((BigDecimal)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Mantissa.GetHashCode() * 397) ^ Exponent;
            }
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(obj, null) || !(obj is BigDecimal))
            {
                throw new ArgumentException();
            }
            return CompareTo((BigDecimal)obj);
        }

        public int CompareTo(BigDecimal other)
        {
            return this < other ? -1 : (this > other ? 1 : 0);
        }
    }

}
