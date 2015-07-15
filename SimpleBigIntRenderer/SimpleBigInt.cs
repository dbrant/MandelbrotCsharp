using System;
using System.Text;

namespace Mandelbrot.SimpleBigIntRenderer
{

    public class BigInteger : IComparable, IComparable<BigInteger>
    {
        private const int SAMPLE_MAX_DIGITS = 128;
        private const sbyte NUM_BASE = 10;

        private sbyte[] digits;
        public sbyte[] Digits { get { return digits; } }

        private sbyte[] mulTemp1;
        private sbyte[] mulTemp2;

        private int numDigits;
        public int NumDigits { get { return numDigits; } }

        private int maxDigits;

        private int sign;
        public int Sign { get { return sign; } }

        public BigInteger(BigInteger other)
        {
            // just make a deep copy
            maxDigits = other.maxDigits;
            numDigits = other.numDigits;
            digits = new sbyte[other.digits.Length];
            mulTemp1 = new sbyte[digits.Length];
            mulTemp2 = new sbyte[digits.Length];
            Array.Copy(other.digits, digits, digits.Length);
            sign = other.sign;
        }

        public BigInteger(long init, int maxDigits)
        {
            if (init >= 0) {
                sign = 1;
            } else {
                sign = -1;
                init = -init;
            }
            this.maxDigits = maxDigits;
            digits = new sbyte[maxDigits * 4];
            mulTemp1 = new sbyte[digits.Length];
            mulTemp2 = new sbyte[digits.Length];
            if (init == 0)
            {
                numDigits = 1;
            }
            else
            {
                numDigits = 0;
                while (init > 0)
                {
                    digits[numDigits++] = (sbyte)(init % NUM_BASE);
                    init /= 10;
                }
            }
        }

        public BigInteger(int init) : this((long)init, SAMPLE_MAX_DIGITS) { }
        public BigInteger(long init) : this(init, SAMPLE_MAX_DIGITS) { }
        public BigInteger(float init) : this((long)init, SAMPLE_MAX_DIGITS) { }
        public BigInteger(double init) : this((long)init, SAMPLE_MAX_DIGITS) { }

        public static implicit operator BigInteger(int value)
        {
            return new BigInteger(value);
        }

        public static implicit operator BigInteger(double value)
        {
            return new BigInteger(value);
        }

        public static explicit operator double(BigInteger value)
        {
            double d = 0;
            for (int i = value.numDigits; i >= 0; i--)
            {
                d *= 10.0;
                d += value.digits[i];
            }
            return value.sign == -1 ? -d : d;
        }


        /*
        public static explicit operator float(BigDecimal value)
        {
            return Convert.ToSingle((double)value);
        }

        public static explicit operator decimal(BigDecimal value)
        {
            return (decimal)value.Mantissa * (decimal)Math.Pow(10, value.Exponent);
        }

        public static explicit operator int(BigDecimal value)
        {
            return (int)(value.Mantissa * BigInteger.Pow(10, value.Exponent));
        }

        public static explicit operator uint(BigDecimal value)
        {
            return (uint)(value.Mantissa * BigInteger.Pow(10, value.Exponent));
        }
        */

        public BigInteger Negate()
        {
            if (!IsZero())
            {
                sign = -sign;
            }
            return this;
        }

        public BigInteger Zero()
        {
            digits[0] = 0;
            numDigits = 1;
            sign = 1;
            return this;
        }

        public bool IsZero()
        {
            return numDigits == 1 && digits[0] == 0;
        }

        public bool IsIntEqual(int num)
        {
            return numDigits == 1 && digits[0] == num;
        }

        /*
        public int Truncate()
        {
            int i;
            for (i = 0; i < numDigits; i++)
            {
                if (digits[i] > 0) { break; }
            }
            return i;
        }
        */


        public BigInteger DivByPow10(int power)
        {
            if (IsZero())
            {
                return this;
            }
            if (power >= numDigits)
            {
                Zero();
                return this;
            }
            for (int i = power; i < numDigits; i++)
            {
                digits[i - power] = digits[i];
            }
            this.numDigits -= power;
            return this;
        }

        public BigInteger MulByPow10(int power)
        {
            if (IsZero())
            {
                return this;
            }
            numDigits += power;
            for (int i = numDigits - 1; i >= power; i--)
            {
                digits[i] = digits[i - power];
            }
            for (int i = 0; i < power; i++)
            {
                digits[i] = 0;
            }
            return this;
        }

        public int Mod10()
        {
            return digits[0];
        }

        public static BigInteger Pow10(int power)
        {
            BigInteger b = 0;
            b.digits[power] = 1;
            b.numDigits = power + 1;
            return b;
        }







        public static BigInteger operator +(BigInteger value)
        {
            return value;
        }

        public static BigInteger operator -(BigInteger value)
        {
            return new BigInteger(value).Negate();
        }

        public BigInteger UncheckedAdd(BigInteger value)
        {
            int i = 0;
            sbyte carry = 0;
            while (true)
            {
                digits[i] += value.digits[i];
                digits[i] += carry;
                if (digits[i] >= NUM_BASE)
                {
                    carry = 1;
                    digits[i] -= NUM_BASE;
                }
                else
                {
                    carry = 0;
                }
                i++;
                if (i >= numDigits && i >= value.numDigits)
                {
                    if (carry > 0)
                    {
                        digits[i++] = carry;
                    }
                    break;
                }
            }
            numDigits = i;
            return this;
        }

        public BigInteger UncheckedSubtract(BigInteger value)
        {
            int i = 0;
            sbyte borrow = 0;
            while (true)
            {
                if (digits[i] < value.digits[i] + borrow)
                {
                    digits[i] += NUM_BASE;
                    digits[i] -= value.digits[i];
                    digits[i] -= borrow;
                    borrow = 1;
                }
                else
                {
                    digits[i] -= value.digits[i];
                    digits[i] -= borrow;
                    borrow = 0;
                }
                i++;
                if (i >= numDigits && i >= value.numDigits)
                {
                    break;
                }
            }
            // trim any leading zeros
            for (i = numDigits - 1; i >= 0; i--)
            {
                if (digits[i] != 0) { break; }
            }
            numDigits = i + 1;
            return this;
        }

        public BigInteger UncheckedAddNeg(BigInteger value)
        {
            int i = 0;
            sbyte borrow = 0;
            while (true)
            {
                digits[i] -= value.digits[i];
                digits[i] += borrow;
                if (digits[i] > 0)
                {
                    digits[i] = (sbyte)-(digits[i] - NUM_BASE);
                    borrow = 1;
                }
                else
                {
                    digits[i] = (sbyte)-(digits[i]);
                    borrow = 0;
                }
                i++;
                if (i >= value.numDigits)
                {
                    break;
                }
            }
            // trim any leading zeros
            for (i = value.numDigits - 1; i >= 0; i--)
            {
                if (digits[i] != 0) { break; }
            }
            numDigits = i + 1;
            return this;
        }

        public BigInteger Add(BigInteger value) {
            if (numDigits > value.numDigits)
            {
                for (int i = numDigits - 1; i >= value.numDigits; i--)
                {
                    value.digits[i] = 0;
                }
            }
            else if (value.numDigits > numDigits)
            {
                for (int i = value.numDigits - 1; i >= numDigits; i--)
                {
                    digits[i] = 0;
                }
            }

            if ((sign > 0 && value.sign > 0) || (sign < 0 && value.sign < 0))
            {
                // plain add of two positive ints, or two negative ints, keeping the sign.
                return UncheckedAdd(value);
            }
            int m = CompareMagnitude(this, value);
            if (m == 0)
            {
                Zero();
            }
            if (sign > 0 && value.sign < 0)
            {
                if (m == -1)
                {
                    UncheckedSubtract(value);
                }
                else if (m == 1)
                {
                    UncheckedAddNeg(value);
                    sign = -1;
                }
            }
            else if (sign < 0 && value.sign > 0)
            {
                if (m == -1)
                {
                    UncheckedSubtract(value);
                }
                else if (m == 1)
                {
                    UncheckedAddNeg(value);
                    sign = 1;
                }
            }
            return this;
        }


        private static int CompareMagnitude(BigInteger left, BigInteger right)
        {
            if (left.numDigits < right.numDigits) { return 1; }
            else if (left.numDigits > right.numDigits) { return -1; }
            for (int i = left.numDigits - 1; i >= 0; i--)
            {
                if (left.digits[i] < right.digits[i]) { return 1; }
                else if (left.digits[i] > right.digits[i]) { return -1; }
            }
            // equality
            return 0;
        }

        public BigInteger UncheckedMultiply(BigInteger value)
        {
            int numTempDigits, i, newNumDigits = 0;
            sbyte carry = 0;
            Array.Clear(mulTemp2, 0, value.numDigits + numDigits + 2);

            for (int d = 0; d < value.numDigits; d++)
            {
                numTempDigits = 0;
                for (i = 0; i < numDigits; i++)
                {
                    mulTemp1[numTempDigits] = (sbyte)(value.digits[d] * digits[i] + carry);
                    if (mulTemp1[numTempDigits] >= NUM_BASE)
                    {
                        carry = (sbyte)(mulTemp1[numTempDigits] / NUM_BASE);
                        mulTemp1[numTempDigits] %= NUM_BASE;
                    }
                    else
                    {
                        carry = 0;
                    }
                    numTempDigits++;
                }
                if (carry > 0)
                {
                    mulTemp1[numTempDigits++] = carry;
                }

                i = 0;
                carry = 0;
                while (true)
                {
                    mulTemp2[i + d] += mulTemp1[i];
                    mulTemp2[i + d] += carry;
                    if (mulTemp2[i + d] >= NUM_BASE)
                    {
                        carry = 1;
                        mulTemp2[i + d] -= NUM_BASE;
                    }
                    else
                    {
                        carry = 0;
                    }
                    i++;
                    if (i >= numTempDigits)
                    {
                        if (carry > 0)
                        {
                            mulTemp2[i++ + d] = carry;
                        }
                        break;
                    }
                }
                newNumDigits = i + d;
            }
            numDigits = newNumDigits;
            // trim any leading zeros
            for (i = numDigits - 1; i >= 0; i--)
            {
                if (mulTemp2[i] != 0) { break; }
            }
            numDigits = i + 1;
            Array.Copy(mulTemp2, digits, numDigits);
            return this;
        }

        public BigInteger Multiply(BigInteger value)
        {
            if (IsZero() || value.IsZero())
            {
                Zero();
                return this;
            }
            UncheckedMultiply(value);
            if (sign == -1 && value.sign == -1)
            {
                sign = 1;
            }
            else if (sign == 1 && value.sign == -1)
            {
                sign = -1;
            }
            return this;
        }



        public BigInteger Divide(BigInteger value)
        {
            if (value > this) { Zero(); return this; }
            if (value.IsIntEqual(1)) { return this; }
            BigInteger tempVal = new BigInteger(this);
            int maxMagnitude = numDigits - value.numDigits;
            int i;
            this.Zero();
            for (int m = maxMagnitude; m >= 0; m--)
            {
                BigInteger mtest = new BigInteger(value).MulByPow10(m);

                for (i = 1; i <= NUM_BASE; i++)
                {
                    BigInteger b = new BigInteger(mtest).UncheckedMultiply(i);
                    if (b > tempVal)
                    {
                        break;
                    }
                }
                i--;
                tempVal.Add(new BigInteger(mtest).Multiply(i).Negate());

                MulByPow10(1);
                digits[0] = (sbyte)i;
            }
            return this;
        }






        public static BigInteger operator ++(BigInteger value)
        {
            return value.Add(1);
        }

        public static BigInteger operator --(BigInteger value)
        {
            return value.Add(-1);
        }

        public static BigInteger operator +(BigInteger left, BigInteger right)
        {
            return new BigInteger(left).Add(right);
        }

        public static BigInteger operator -(BigInteger left, BigInteger right)
        {
            return new BigInteger(right).Negate().Add(left);
        }

        public static BigInteger operator *(BigInteger left, BigInteger right)
        {
            return new BigInteger(left).Multiply(right);
        }


        public static bool operator ==(BigInteger left, BigInteger right)
        {
            if (left.sign != right.sign) { return false; }
            return CompareMagnitude(left, right) == 0;
        }

        public static bool operator !=(BigInteger left, BigInteger right)
        {
            return !(left == right);
        }

        public static bool operator <(BigInteger left, BigInteger right)
        {
            if (left.sign < 0 && right.sign > 0) { return true; }
            else if (left.sign > 0 && right.sign < 0) { return false; }
            bool bothNegative = left.sign < 0 && right.sign < 0;
            int m = CompareMagnitude(left, right);
            if (m == -1)
            {
                return bothNegative ? true : false;
            }
            else if (m == 1)
            {
                return bothNegative ? false : true;
            }
            // equality
            return false;
        }

        public static bool operator <=(BigInteger left, BigInteger right)
        {
            if (left.sign < 0 && right.sign > 0) { return true; }
            else if (left.sign > 0 && right.sign < 0) { return false; }
            bool bothNegative = left.sign < 0 && right.sign < 0;
            int m = CompareMagnitude(left, right);
            if (m == -1)
            {
                return bothNegative ? true : false;
            }
            else if (m == 1)
            {
                return bothNegative ? false : true;
            }
            // equality
            return true;
        }

        public static bool operator >(BigInteger left, BigInteger right)
        {
            return !(left <= right);
        }

        public static bool operator >=(BigInteger left, BigInteger right)
        {
            return !(left < right);
        }

        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(numDigits + 2);
            if (sign < 0)
            {
                sb.Append("-");
            }
            for (int i = numDigits - 1; i >= 0; i--)
            {
                sb.Append(digits[i]);
            }
            return sb.ToString();
        }

        public bool Equals(BigInteger other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is BigInteger && Equals((BigInteger)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = sign;
                for (int i = 0; i < numDigits; i++)
                {
                    hashCode += digits[i];
                }
                return hashCode;
            }
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(obj, null) || !(obj is BigInteger))
            {
                throw new ArgumentException();
            }
            return CompareTo((BigInteger)obj);
        }

        public int CompareTo(BigInteger other)
        {
            return this < other ? -1 : (this > other ? 1 : 0);
        }
    }

}
