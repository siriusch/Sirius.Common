using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Sirius.Numerics {
	public struct BigDecimal: IConvertible, IComparable, IEquatable<BigDecimal>, IComparable<BigDecimal>, IFormattable, IIncrementable<BigDecimal> {
		private const int PositiveInfinityExponent = int.MaxValue;
		private const int NegativeInfinityExponent = -int.MaxValue;
		private const int NaNExponent = int.MinValue;

		public static BigDecimal PositiveInfinity = new BigDecimal(0, PositiveInfinityExponent, false);

		public static BigDecimal NegativeInfinity = new BigDecimal(0, NegativeInfinityExponent, false);

		public static BigDecimal NaN = new BigDecimal(0, NaNExponent, false);

		public static BigDecimal Zero = default(BigDecimal);

		public static BigDecimal One = new BigDecimal(BigInteger.One, 0, false);

		public static BigDecimal MinusOne = new BigDecimal(BigInteger.MinusOne, 0, false);

		public static BigDecimal operator +(BigDecimal left, BigDecimal right) {
			return Add(left, right);
		}

		public static BigDecimal operator ++(BigDecimal left) {
			return Add(left, One);
		}

		public static BigDecimal operator -(BigDecimal left, BigDecimal right) {
			return Subtract(left, right);
		}

		public static BigDecimal operator --(BigDecimal left) {
			return Subtract(left, One);
		}

		public static BigDecimal operator *(BigDecimal left, BigDecimal right) {
			return Multiply(left, right);
		}

		public static BigDecimal operator /(BigDecimal left, BigDecimal right) {
			return Divide(left, right);
		}

		public static BigDecimal operator %(BigDecimal left, BigDecimal right) {
			return Remainder(left, right);
		}

		public static bool operator ==(BigDecimal left, BigDecimal right) {
			return left.Equals(right);
		}

		public static bool operator !=(BigDecimal left, BigDecimal right) {
			return !left.Equals(right);
		}

		public static bool operator >(BigDecimal left, BigDecimal right) {
			return left.CompareTo(right) > 0;
		}

		public static bool operator >=(BigDecimal left, BigDecimal right) {
			return left.CompareTo(right) >= 0;
		}

		public static bool operator <(BigDecimal left, BigDecimal right) {
			return left.CompareTo(right) < 0;
		}

		public static bool operator <=(BigDecimal left, BigDecimal right) {
			return left.CompareTo(right) <= 0;
		}

		public static int Compare(BigDecimal left, BigDecimal right) {
			return left.CompareTo(right);
		}

		public static BigDecimal Abs(BigDecimal value) {
			return value.digits.Sign >= 0 ? value : Negate(value);
		}

		public static BigDecimal Add(BigDecimal left, BigDecimal right) {
			if (IsNaN(left) || IsNaN(right) || (IsNegativeInfinity(left) && IsPositiveInfinity(right)) || (IsPositiveInfinity(left) && IsNegativeInfinity(right))) {
				return NaN;
			}
			if (IsZero(left) || IsInfinity(right)) {
				return right;
			}
			if (IsZero(right) || IsInfinity(left)) {
				return left;
			}
			var exponent = NormalizeDigits(left, right, out var leftDigits, out var rightDigits);
			return new BigDecimal(leftDigits+rightDigits, exponent);
		}

		public static BigDecimal Multiply(BigDecimal left, BigDecimal right) {
			if (IsNaN(left) || IsNaN(right) || (IsZero(left) && IsInfinity(right)) || (IsInfinity(left) && IsZero(right))) {
				return NaN;
			}
			if (IsZero(left) || IsZero(right)) {
				return Zero;
			}
			if (IsInfinity(left) || IsInfinity(right)) {
				return (left.Sign * right.Sign) < 0 ? NegativeInfinity : PositiveInfinity;
			}
			return new BigDecimal(left.digits * right.digits, left.exponent+right.exponent);
		}

		public static BigDecimal Divide(BigDecimal left, BigDecimal right) {
			if (IsNaN(left) || IsNaN(right) || IsZero(right) || (IsInfinity(left) && IsInfinity(right))) {
				return NaN;
			}
			if (IsZero(left) || IsInfinity(right)) {
				return Zero;
			}
			if (IsInfinity(left)) {
				return (left.Sign * right.Sign) < 0 ? NegativeInfinity : PositiveInfinity;
			}
			return new BigDecimal(left.digits / right.digits, left.exponent-right.exponent);
		}

		public static BigDecimal Remainder(BigDecimal left, BigDecimal right) {
			if (IsNaN(left) || IsNaN(right) || IsZero(right) || (IsInfinity(left) && IsInfinity(right))) {
				return NaN;
			}
			if (IsZero(left) || IsInfinity(left) || IsInfinity(right)) {
				return Zero;
			}
			var exponent = NormalizeDigits(left, right, out var leftDigits, out var rightDigits);
			return new BigDecimal(leftDigits % rightDigits, exponent);
		}

		public static BigDecimal Round(BigDecimal value) {
			return Round(value, 0, MidpointRounding.ToEven);
		}

		public static BigDecimal Round(BigDecimal value, int decimals) {
			return Round(value, decimals, MidpointRounding.ToEven);
		}

		public static BigDecimal Round(BigDecimal value, MidpointRounding mode) {
			return Round(value, 0, mode);
		}

		public static BigDecimal Round(BigDecimal value, int decimals, MidpointRounding mode) {
			if (decimals < 0) {
				throw new ArgumentOutOfRangeException(nameof(decimals));
			}
			if (IsSpecialExponent(value.exponent) || (value.exponent >= -decimals) || IsZero(value)) {
				return value;
			}
			var divider = BigInteger.Pow(10, -value.exponent-decimals);
			var digits = BigInteger.DivRem(value.digits, divider, out var remainder);
			if (!remainder.IsZero) {
				switch ((BigInteger.Abs(remainder * 2)-divider).Sign) {
				case 1:
					return new BigDecimal(digits+digits.Sign, -decimals);
				case 0:
					switch (mode) {
					case MidpointRounding.AwayFromZero:
						return new BigDecimal(digits+digits.Sign, -decimals);
					case MidpointRounding.ToEven:
						return new BigDecimal(digits.IsEven ? digits : digits+digits.Sign, -decimals);
					}
					break;
				}
			}
			return new BigDecimal(digits, -decimals);
		}

		private static int NormalizeDigits(BigDecimal left, BigDecimal right, out BigInteger leftDigits, out BigInteger rightDigits) {
			if (IsSpecialExponent(left.exponent) || IsSpecialExponent(right.exponent)) {
				throw new InvalidOperationException();
			}
			var exponentDiff = left.exponent-right.exponent;
			if (exponentDiff > 0) {
				leftDigits = left.digits * BigInteger.Pow(10, exponentDiff);
				rightDigits = right.digits;
				return right.exponent;
			}
			if (exponentDiff < 0) {
				leftDigits = left.digits;
				rightDigits = right.digits * BigInteger.Pow(10, -exponentDiff);
				return left.exponent;
			}
			leftDigits = left.digits;
			rightDigits = right.digits;
			return left.exponent;
		}

		public static BigDecimal Subtract(BigDecimal left, BigDecimal right) {
			return Add(left, Negate(right));
		}

		public static BigDecimal Min(BigDecimal left, BigDecimal right) {
			if (IsNaN(left) || IsNaN(right)) {
				return NaN;
			}
			return Compare(left, right) <= 0 ? left : right;
		}

		public static BigDecimal Max(BigDecimal left, BigDecimal right) {
			if (IsNaN(left) || IsNaN(right)) {
				return NaN;
			}
			return Compare(left, right) >= 0 ? left : right;
		}

		public static BigDecimal Negate(BigDecimal value) {
			if (IsNaN(value) || IsZero(value)) {
				return value;
			}
			if (IsPositiveInfinity(value)) {
				return NegativeInfinity;
			}
			if (IsNegativeInfinity(value)) {
				return PositiveInfinity;
			}
			return new BigDecimal(-value.digits, value.exponent, false);
		}

		public static BigDecimal Truncate(BigDecimal value) {
			if (IsSpecialExponent(value.exponent) || (value.exponent >= 0) || IsZero(value)) {
				return value;
			}
			return new BigDecimal(value.digits / BigInteger.Pow(10, -value.exponent), 0);
		}

		public static BigDecimal Floor(BigDecimal value) {
			if (IsSpecialExponent(value.exponent) || (value.exponent >= 0) || IsZero(value)) {
				return value;
			}
			var digits = BigInteger.DivRem(value.digits, BigInteger.Pow(10, -value.exponent), out var remainder);
			if ((value.Sign < 0) && !remainder.IsZero) {
				digits--;
			}
			return new BigDecimal(digits, 0);
		}

		public static BigDecimal Ceiling(BigDecimal value) {
			if (IsSpecialExponent(value.exponent) || (value.exponent >= 0) || IsZero(value)) {
				return value;
			}
			var digits = BigInteger.DivRem(value.digits, BigInteger.Pow(10, -value.exponent), out var remainder);
			if ((value.Sign > 0) && !remainder.IsZero) {
				digits++;
			}
			return new BigDecimal(digits, 0);
		}

		public static bool IsPositiveInfinity(BigDecimal value) {
			return value.exponent == PositiveInfinityExponent;
		}

		public static bool IsNegativeInfinity(BigDecimal value) {
			return value.exponent == NegativeInfinityExponent;
		}

		public static bool IsInfinity(BigDecimal value) {
			return IsPositiveInfinity(value) || IsNegativeInfinity(value);
		}

		public static bool IsNaN(BigDecimal value) {
			return value.exponent == NaNExponent;
		}

		public static bool IsZero(BigDecimal value) {
			return (value.exponent == 0) && value.digits.IsZero;
		}

		public static BigDecimal Parse(string value) {
			return Parse(value, NumberStyles.Any, NumberFormatInfo.CurrentInfo);
		}

		public static BigDecimal Parse(string value, NumberStyles styles) {
			return Parse(value, styles, NumberFormatInfo.CurrentInfo);
		}

		public static BigDecimal Parse(string value, IFormatProvider provider) {
			return Parse(value, NumberStyles.Any, provider);
		}

		public static BigDecimal Parse(string value, NumberStyles styles, IFormatProvider provider) {
			if (!TryParse(value, styles, provider, out var result)) {
				throw new FormatException();
			}
			return result;
		}

		public static bool TryParse(string value, out BigDecimal result) {
			return TryParse(value, NumberStyles.Any, NumberFormatInfo.CurrentInfo, out result);
		}

		public static bool TryParse(string value, NumberStyles styles, out BigDecimal result) {
			return TryParse(value, styles, NumberFormatInfo.CurrentInfo, out result);
		}

		public static bool TryParse(string value, IFormatProvider provider, out BigDecimal result) {
			return TryParse(value, NumberStyles.Any, provider, out result);
		}

		public static bool TryParse(string value, NumberStyles styles, IFormatProvider provider, out BigDecimal result) {
			result = default(BigDecimal);
			if (string.IsNullOrWhiteSpace(value)) {
				return false;
			}
			if ((!styles.HasFlag(NumberStyles.AllowExponent) && !styles.HasFlag(NumberStyles.AllowDecimalPoint)) || styles.HasFlag(NumberStyles.AllowHexSpecifier)) {
				if (!BigInteger.TryParse(value, styles, provider, out var integer)) {
					return false;
				}
				result = new BigDecimal(integer, 0);
				return true;
			}
			if (styles.HasFlag(NumberStyles.AllowTrailingWhite) && styles.HasFlag(NumberStyles.AllowLeadingWhite)) {
				value = value.Trim();
			} else if (styles.HasFlag(NumberStyles.AllowLeadingWhite)) {
				value = value.TrimStart();
			} else if (styles.HasFlag(NumberStyles.AllowTrailingWhite)) {
				value = value.TrimEnd();
			}
			if (string.IsNullOrEmpty(value)) {
				return false;
			}
			var info = NumberFormatInfo.GetInstance(provider);
			if (info.NaNSymbol.Equals(value, StringComparison.OrdinalIgnoreCase)) {
				result = NaN;
				return true;
			}
			if (info.PositiveInfinitySymbol.Equals(value, StringComparison.OrdinalIgnoreCase)) {
				result = PositiveInfinity;
				return true;
			}
			if (info.NegativeInfinitySymbol.Equals(value, StringComparison.OrdinalIgnoreCase)) {
				result = NegativeInfinity;
				return true;
			}
			var negative = false;
			if (styles.HasFlag(NumberStyles.AllowLeadingSign) && !string.IsNullOrEmpty(info.PositiveSign) && value.StartsWith(info.PositiveSign, StringComparison.Ordinal)) {
				value = value.Substring(info.PositiveSign.Length);
			} else if (styles.HasFlag(NumberStyles.AllowTrailingSign) && !string.IsNullOrEmpty(info.PositiveSign) && value.EndsWith(info.PositiveSign, StringComparison.Ordinal)) {
				value = value.Substring(0, value.Length-info.PositiveSign.Length);
			} else if (styles.HasFlag(NumberStyles.AllowLeadingSign) && !string.IsNullOrEmpty(info.NegativeSign) && value.StartsWith(info.NegativeSign, StringComparison.Ordinal)) {
				value = value.Substring(info.PositiveSign.Length);
				negative = true;
			} else if (styles.HasFlag(NumberStyles.AllowTrailingSign) && !string.IsNullOrEmpty(info.NegativeSign) && value.EndsWith(info.NegativeSign, StringComparison.Ordinal)) {
				value = value.Substring(0, value.Length-info.PositiveSign.Length);
				negative = true;
			}
			var exponent = 0;
			if (styles.HasFlag(NumberStyles.AllowDecimalPoint)) {
				var decimalPointIndex = value.IndexOf(info.NumberDecimalSeparator, StringComparison.Ordinal);
				if (decimalPointIndex >= 0) {
					value = value.Remove(decimalPointIndex, info.NumberDecimalSeparator.Length);
					exponent = decimalPointIndex-value.Length;
				}
			}
			if (styles.HasFlag(NumberStyles.AllowExponent)) {
				var exponentIndex = value.IndexOf("e", StringComparison.OrdinalIgnoreCase);
				if (exponentIndex == 0) {
					return false; // only exponent is not a valid number
				}
				if (exponentIndex > 0) {
					if (!int.TryParse(value.Substring(exponentIndex+1), NumberStyles.AllowLeadingSign, info, out var explicitExponent)) {
						return false;
					}
					exponent += explicitExponent;
					value = value.Substring(0, exponentIndex);
				}
			}
			if (!BigInteger.TryParse(value, NumberStyles.None, info, out var digits)) {
				return false;
			}
			result = new BigDecimal(negative ? -digits : digits, exponent);
			return true;
		}

		private readonly BigInteger digits;
		private readonly int exponent;

		public BigDecimal(BigInteger digits, int exponent): this(digits, exponent, true) { }

		private BigDecimal(BigInteger digits, int exponent, bool normalize) {
			if (normalize) {
				if (IsSpecialExponent(exponent)) {
					if (!digits.IsZero) {
						throw new ArgumentOutOfRangeException(nameof(exponent));
					}
				} else {
					if (digits.IsZero) {
						exponent = 0;
					} else {
						while ((digits % 10).IsZero) {
							digits = digits / 10;
							exponent++;
						}
					}
				}
			}
			this.digits = digits;
			this.exponent = exponent;
		}

		private static bool IsSpecialExponent(int exponent) {
			return (exponent == NaNExponent) || (exponent == NegativeInfinityExponent) || (exponent == PositiveInfinityExponent);
		}

		TypeCode IConvertible.GetTypeCode() {
			return TypeCode.Object;
		}

		bool IConvertible.ToBoolean(IFormatProvider provider) {
			return !digits.IsZero;
		}

		char IConvertible.ToChar(IFormatProvider provider) {
			throw new InvalidCastException();
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider) {
			return checked((sbyte)ToInt32());
		}

		byte IConvertible.ToByte(IFormatProvider provider) {
			return checked((byte)ToInt32());
		}

		short IConvertible.ToInt16(IFormatProvider provider) {
			return checked((short)ToInt32());
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider) {
			return checked((ushort)ToInt32());
		}

		internal BigInteger ToBigInteger() {
			if (IsSpecialExponent(exponent)) {
				throw new OverflowException();
			}
			if (exponent <= 0) {
				return digits;
			}
			return digits * BigInteger.Pow(10, exponent);
		}

		internal decimal ToDecimal() {
			if (IsSpecialExponent(exponent)) {
				throw new OverflowException();
			}
			var result = (decimal)ToBigInteger();
			return exponent >= 0 ? result : result / (decimal)BigInteger.Pow(10, -exponent);
		}

		internal double ToDouble() {
			switch (exponent) {
			case NaNExponent:
				return double.NaN;
			case NegativeInfinityExponent:
				return double.NegativeInfinity;
			case PositiveInfinityExponent:
				return double.PositiveInfinity;
			case 0:
				return (double)digits;
			}
			return (exponent < 0)
					? (double)digits / (double)BigInteger.Pow(10, -exponent)
					: (double)(digits * BigInteger.Pow(10, exponent));
		}

		int IConvertible.ToInt32(IFormatProvider provider) {
			return ToInt32();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int ToInt32() {
			return (int)ToBigInteger();
		}

		uint IConvertible.ToUInt32(IFormatProvider provider) {
			return (uint)ToBigInteger();
		}

		long IConvertible.ToInt64(IFormatProvider provider) {
			return (long)ToBigInteger();
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider) {
			return (ulong)ToBigInteger();
		}

		float IConvertible.ToSingle(IFormatProvider provider) {
			return (float)ToDouble();
		}

		double IConvertible.ToDouble(IFormatProvider provider) {
			return ToDouble();
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider) {
			return ToDecimal();
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider) {
			throw new InvalidCastException();
		}

		public string ToString(string format) {
			return ToString(format, null);
		}

		public string ToString(IFormatProvider provider) {
			return ToString(null, provider);
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider) {
			var effectiveType = Nullable.GetUnderlyingType(conversionType) ?? conversionType;
			if (effectiveType == typeof(BigDecimal)) {
				return this;
			}
			if (effectiveType == typeof(bool)) {
				return ((IConvertible)this).ToBoolean(provider);
			}
			if (effectiveType == typeof(byte)) {
				return ((IConvertible)this).ToByte(provider);
			}
			if (effectiveType == typeof(sbyte)) {
				return ((IConvertible)this).ToSByte(provider);
			}
			if (effectiveType == typeof(short)) {
				return ((IConvertible)this).ToInt16(provider);
			}
			if (effectiveType == typeof(ushort)) {
				return ((IConvertible)this).ToUInt16(provider);
			}
			if (effectiveType == typeof(int)) {
				return ((IConvertible)this).ToInt32(provider);
			}
			if (effectiveType == typeof(uint)) {
				return ((IConvertible)this).ToUInt32(provider);
			}
			if (effectiveType == typeof(long)) {
				return ((IConvertible)this).ToInt64(provider);
			}
			if (effectiveType == typeof(ulong)) {
				return ((IConvertible)this).ToUInt64(provider);
			}
			if (effectiveType == typeof(float)) {
				return ((IConvertible)this).ToSingle(provider);
			}
			if (effectiveType == typeof(double)) {
				return ((IConvertible)this).ToDouble(provider);
			}
			if (effectiveType == typeof(decimal)) {
				return ((IConvertible)this).ToDecimal(provider);
			}
			if (effectiveType == typeof(BigInteger)) {
				return ToBigInteger();
			}
			if (effectiveType == typeof(string)) {
				return ToString(provider);
			}
			throw new InvalidCastException();
		}

		public string ToString(string format, IFormatProvider provider) {
			var info = NumberFormatInfo.GetInstance(provider);
			switch (exponent) {
			case NaNExponent:
				return info.NaNSymbol;
			case NegativeInfinityExponent:
				return info.PositiveInfinitySymbol;
			case PositiveInfinityExponent:
				return info.NegativeInfinitySymbol;
			}
			var result = BigInteger.Abs(digits).ToString(info);
			if (exponent < 0) {
				var position = result.Length+exponent;
				result = result.Insert(position, info.NumberDecimalSeparator);
				if (position == 0) {
					result = result.Insert(0, info.NativeDigits[0]);
				}
			}
			if (digits.Sign < 0) {
				result = result.Insert(0, info.NegativeSign);
			}
			return result;
		}

		public bool Equals(BigDecimal value) {
			return (exponent == value.exponent) && digits.Equals(value.digits);
		}

		public int CompareTo(BigDecimal value) {
			var signDiff = digits.Sign-value.digits.Sign;
			if (signDiff != 0) {
				return signDiff;
			}
			if (IsNaN(this)) {
				return IsNaN(value) ? 0 : -1;
			}
			if (IsPositiveInfinity(this)) {
				return IsPositiveInfinity(value) ? 0 : 1;
			}
			if (IsNegativeInfinity(this)) {
				return IsNegativeInfinity(value) ? 0 : -1;
			}
			if (IsNaN(value)) {
				return 1;
			}
			if (IsPositiveInfinity(value)) {
				return -1;
			}
			if (IsNegativeInfinity(value)) {
				return 1;
			}
			NormalizeDigits(this, value, out var thisDigits, out var valueDigits);
			return thisDigits.CompareTo(valueDigits);
		}

		public override bool Equals(object obj) {
			return base.Equals(obj);
		}

		public override int GetHashCode() {
			return unchecked(digits.GetHashCode()^(exponent * 397));
		}

		public int Sign {
			get {
				if (IsPositiveInfinity(this)) {
					return 1;
				}
				if (IsNegativeInfinity(this)) {
					return -1;
				}
				return digits.Sign;
			}
		}

		int IComparable.CompareTo(object obj) {
			return CompareTo((BigDecimal)obj);
		}

		public override string ToString() {
			return ToString(null, null);
		}

		BigDecimal IIncrementable<BigDecimal>.Decrement() {
			return Subtract(this, One);
		}

		BigDecimal IIncrementable<BigDecimal>.Increment() {
			return Add(this, One);
		}
	}
}
