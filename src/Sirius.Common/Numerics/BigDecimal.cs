using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Sirius.Numerics {
	/// <summary>A big decimal can hold any number with a finite amount of digits.</summary>
	/// <remarks>The backing data type is a <see cref="BigInteger"/> with additional log10 (<see cref="int"/>) exponent information.</remarks>
	public struct BigDecimal: IConvertible, IComparable, IEquatable<BigDecimal>, IComparable<BigDecimal>, IFormattable, IIncrementable<BigDecimal> {
		private const int PositiveInfinityExponent = int.MaxValue;
		private const int NegativeInfinityExponent = -int.MaxValue;
		private const int NaNExponent = int.MinValue;

		/// <summary>The positive infinity value.</summary>
		public static BigDecimal PositiveInfinity = new BigDecimal(0, PositiveInfinityExponent, false);

		/// <summary>The negative infinity value.</summary>
		public static BigDecimal NegativeInfinity = new BigDecimal(0, NegativeInfinityExponent, false);

		/// <summary>Not a Number value.</summary>
		public static BigDecimal NaN = new BigDecimal(0, NaNExponent, false);

		/// <summary>The zero.</summary>
		public static BigDecimal Zero = default;

		/// <summary>The one.</summary>
		public static BigDecimal One = new BigDecimal(BigInteger.One, 0, false);

		/// <summary>The minus one.</summary>
		public static BigDecimal MinusOne = new BigDecimal(BigInteger.MinusOne, 0, false);

		/// <summary>Implicit cast that converts the given byte to a BigDecimal.</summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operation.</returns>
		public static implicit operator BigDecimal(byte value) {
			return new BigDecimal(value);
		}

		/// <summary>Implicit cast that converts the given sbyte to a BigDecimal.</summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operation.</returns>
		[CLSCompliant(false)]
		public static implicit operator BigDecimal(sbyte value) {
			return new BigDecimal(value);
		}

		/// <summary>Implicit cast that converts the given short to a BigDecimal.</summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operation.</returns>
		public static implicit operator BigDecimal(short value) {
			return new BigDecimal(value);
		}

		/// <summary>Implicit cast that converts the given ushort to a BigDecimal.</summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operation.</returns>
		[CLSCompliant(false)]
		public static implicit operator BigDecimal(ushort value) {
			return new BigDecimal(value);
		}

		/// <summary>Implicit cast that converts the given int to a BigDecimal.</summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operation.</returns>
		public static implicit operator BigDecimal(int value) {
			return new BigDecimal(value);
		}

		/// <summary>Implicit cast that converts the given uint to a BigDecimal.</summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operation.</returns>
		[CLSCompliant(false)]
		public static implicit operator BigDecimal(uint value) {
			return new BigDecimal(value);
		}

		/// <summary>Implicit cast that converts the given long to a BigDecimal.</summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operation.</returns>
		public static implicit operator BigDecimal(long value) {
			return new BigDecimal(value);
		}

		/// <summary>Implicit cast that converts the given ulong to a BigDecimal.</summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operation.</returns>
		[CLSCompliant(false)]
		public static implicit operator BigDecimal(ulong value) {
			return new BigDecimal(value);
		}

		/// <summary>Implicit cast that converts the given float to a BigDecimal.</summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operation.</returns>
		public static implicit operator BigDecimal(float value) {
			return new BigDecimal(value);
		}

		/// <summary>Implicit cast that converts the given double to a BigDecimal.</summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operation.</returns>
		public static implicit operator BigDecimal(double value) {
			return new BigDecimal(value);
		}

		/// <summary>Implicit cast that converts the given decimal to a BigDecimal.</summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operation.</returns>
		public static implicit operator BigDecimal(decimal value) {
			return new BigDecimal(value);
		}

		/// <summary>Implicit cast that converts the given BigInteger to a BigDecimal.</summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operation.</returns>
		public static implicit operator BigDecimal(BigInteger value) {
			return new BigDecimal(value, 0);
		}

		/// <summary>Addition operator.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operation.</returns>
		public static BigDecimal operator +(BigDecimal left, BigDecimal right) {
			return Add(left, right);
		}

		/// <summary>Increment operator.</summary>
		/// <param name="left">The left.</param>
		/// <returns>The result of the operation.</returns>
		public static BigDecimal operator ++(BigDecimal left) {
			return Add(left, One);
		}

		/// <summary>Subtraction operator.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operation.</returns>
		public static BigDecimal operator -(BigDecimal left, BigDecimal right) {
			return Subtract(left, right);
		}

		/// <summary>Decrement operator.</summary>
		/// <param name="left">The left.</param>
		/// <returns>The result of the operation.</returns>
		public static BigDecimal operator --(BigDecimal left) {
			return Subtract(left, One);
		}

		/// <summary>Multiplication operator.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operation.</returns>
		public static BigDecimal operator *(BigDecimal left, BigDecimal right) {
			return Multiply(left, right);
		}

		/// <summary>Division operator.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operation.</returns>
		public static BigDecimal operator /(BigDecimal left, BigDecimal right) {
			return Divide(left, right);
		}

		/// <summary>Modulus operator.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operation.</returns>
		public static BigDecimal operator %(BigDecimal left, BigDecimal right) {
			return Remainder(left, right);
		}

		/// <summary>Equality operator.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operation.</returns>
		public static bool operator ==(BigDecimal left, BigDecimal right) {
			return left.Equals(right);
		}

		/// <summary>Inequality operator.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operation.</returns>
		public static bool operator !=(BigDecimal left, BigDecimal right) {
			return !left.Equals(right);
		}

		/// <summary>Greater-than comparison operator.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operation.</returns>
		public static bool operator >(BigDecimal left, BigDecimal right) {
			return left.CompareTo(right) > 0;
		}

		/// <summary>Greater-than-or-equal comparison operator.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operation.</returns>
		public static bool operator >=(BigDecimal left, BigDecimal right) {
			return left.CompareTo(right) >= 0;
		}

		/// <summary>Gets the compare to(right)</summary>
		/// <value>The compare to(right) &lt; 0.</value>
		public static bool operator <(BigDecimal left, BigDecimal right) {
			return left.CompareTo(right) < 0;
		}

		/// <summary>Gets the compare to(right)</summary>
		/// <value>The compare to(right) &lt;= 0.</value>
		public static bool operator <=(BigDecimal left, BigDecimal right) {
			return left.CompareTo(right) <= 0;
		}

		/// <summary>Compares two BigDecimal objects to determine their relative ordering.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>Negative if 'left' is less than 'right', 0 if they are equal, or positive if it is greater.</returns>
		public static int Compare(BigDecimal left, BigDecimal right) {
			return left.CompareTo(right);
		}

		/// <summary>Gets the absolute value of the given value.</summary>
		/// <param name="value">The value.</param>
		/// <returns>A BigDecimal.</returns>
		public static BigDecimal Abs(BigDecimal value) {
			return value.digits.Sign >= 0 ? value : Negate(value);
		}

		/// <summary>Adds the left and right values.</summary>
		/// <param name="left">The left value.</param>
		/// <param name="right">The right value.</param>
		/// <returns>A BigDecimal.</returns>
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
			return new BigDecimal(leftDigits + rightDigits, exponent);
		}

		/// <summary>Multiplies the left and right values.</summary>
		/// <param name="left">The left value.</param>
		/// <param name="right">The right value.</param>
		/// <returns>A BigDecimal.</returns>
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
			return new BigDecimal(left.digits * right.digits, left.exponent + right.exponent);
		}

		/// <summary>Divides the left value by the right value.</summary>
		/// <param name="left">The left value.</param>
		/// <param name="right">The right value.</param>
		/// <returns>A BigDecimal.</returns>
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
			return new BigDecimal(left.digits / right.digits, left.exponent - right.exponent);
		}

		/// <summary>Gets the remainder of the left value divided by the right values.</summary>
		/// <param name="left">The left value.</param>
		/// <param name="right">The right value.</param>
		/// <returns>A BigDecimal.</returns>
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

		/// <summary>Rounds the given value.</summary>
		/// <param name="value">The value.</param>
		/// <returns>A BigDecimal.</returns>
		public static BigDecimal Round(BigDecimal value) {
			return Round(value, 0, MidpointRounding.ToEven);
		}

		/// <summary>Rounds the given value.</summary>
		/// <param name="value">The value.</param>
		/// <param name="decimals">The decimals.</param>
		/// <returns>A BigDecimal.</returns>
		public static BigDecimal Round(BigDecimal value, int decimals) {
			return Round(value, decimals, MidpointRounding.ToEven);
		}

		/// <summary>Rounds the given value.</summary>
		/// <param name="value">The value.</param>
		/// <param name="mode">The mode.</param>
		/// <returns>A BigDecimal.</returns>
		public static BigDecimal Round(BigDecimal value, MidpointRounding mode) {
			return Round(value, 0, mode);
		}

		/// <summary>Rounds the given value.</summary>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
		/// <param name="value">The value.</param>
		/// <param name="decimals">The decimals.</param>
		/// <param name="mode">The mode.</param>
		/// <returns>A BigDecimal.</returns>
		public static BigDecimal Round(BigDecimal value, int decimals, MidpointRounding mode) {
			if (decimals < 0) {
				throw new ArgumentOutOfRangeException(nameof(decimals));
			}
			if (IsSpecialExponent(value.exponent) || (value.exponent >= -decimals) || IsZero(value)) {
				return value;
			}
			var divider = BigInteger.Pow(10, -value.exponent - decimals);
			var digits = BigInteger.DivRem(value.digits, divider, out var remainder);
			if (!remainder.IsZero) {
				switch ((BigInteger.Abs(remainder * 2) - divider).Sign) {
				case 1:
					return new BigDecimal(digits + digits.Sign, -decimals);
				case 0:
					switch (mode) {
					case MidpointRounding.AwayFromZero:
						return new BigDecimal(digits + digits.Sign, -decimals);
					case MidpointRounding.ToEven:
						return new BigDecimal(digits.IsEven ? digits : digits + digits.Sign, -decimals);
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
			var exponentDiff = left.exponent - right.exponent;
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

		/// <summary>Subtracts the right value from the left value.</summary>
		/// <param name="left">The left value.</param>
		/// <param name="right">The right value.</param>
		/// <returns>A BigDecimal.</returns>
		public static BigDecimal Subtract(BigDecimal left, BigDecimal right) {
			return Add(left, Negate(right));
		}

		private static BigDecimal Aggregate(IEnumerable<BigDecimal> values, Func<BigDecimal, BigDecimal, BigDecimal> fn) {
			if (values == null) {
				throw new ArgumentNullException(nameof(values));
			}
			using (var e = values.GetEnumerator()) {
				if (!e.MoveNext()) {
					throw new ArgumentException("Expected at least one value", nameof(values));
				}
				var result = e.Current;
				while (e.MoveNext() && !IsNaN(result)) {
					result = fn(result, e.Current);
				}
				return result;
			}
		}

		/// <summary>Determines the minimum of the given parameters.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The minimum value.</returns>
		public static BigDecimal Min(BigDecimal left, BigDecimal right) {
			if (IsNaN(left) || IsNaN(right)) {
				return NaN;
			}
			return Compare(left, right) <= 0 ? left : right;
		}

		/// <summary>Determines the minimum of the given parameters.</summary>
		/// <param name="values">The values.</param>
		/// <returns>The minimum value.</returns>
		public static BigDecimal Min(IEnumerable<BigDecimal> values) {
			return Aggregate(values, Min);
		}

		/// <summary>Determines the minimum of the given parameters.</summary>
		/// <param name="values">The values.</param>
		/// <returns>The minimum value.</returns>
		public static BigDecimal Min(params BigDecimal[] values) {
			return Aggregate(values, Min);
		}

		/// <summary>Determines the maximum of the given parameters.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The maximum value.</returns>
		public static BigDecimal Max(BigDecimal left, BigDecimal right) {
			if (IsNaN(left) || IsNaN(right)) {
				return NaN;
			}
			return Compare(left, right) >= 0 ? left : right;
		}

		/// <summary>Determines the maximum of the given parameters.</summary>
		/// <param name="values">The values.</param>
		/// <returns>The maximum value.</returns>
		public static BigDecimal Max(IEnumerable<BigDecimal> values) {
			return Aggregate(values, Max);
		}

		/// <summary>Determines the maximum of the given parameters.</summary>
		/// <param name="values">The values.</param>
		/// <returns>The maximum value.</returns>
		public static BigDecimal Max(params BigDecimal[] values) {
			return Aggregate(values, Max);
		}

		/// <summary>Negates the given value.</summary>
		/// <param name="value">The value.</param>
		/// <returns>A BigDecimal.</returns>
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

		/// <summary>Truncates the given value.</summary>
		/// <param name="value">The value.</param>
		/// <returns>A BigDecimal.</returns>
		public static BigDecimal Truncate(BigDecimal value) {
			if (IsSpecialExponent(value.exponent) || (value.exponent >= 0) || IsZero(value)) {
				return value;
			}
			return new BigDecimal(value.digits / BigInteger.Pow(10, -value.exponent), 0);
		}

		/// <summary>Gets the floor of the given value.</summary>
		/// <param name="value">The value.</param>
		/// <returns>A BigDecimal.</returns>
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

		/// <summary>Gets the ceiling of the given value.</summary>
		/// <param name="value">The value.</param>
		/// <returns>A BigDecimal.</returns>
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

		/// <summary>Query if 'value' is positive infinity.</summary>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if positive infinity, <c>false</c> if not.</returns>
		public static bool IsPositiveInfinity(BigDecimal value) {
			return value.exponent == PositiveInfinityExponent;
		}

		/// <summary>Query if 'value' is negative infinity.</summary>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if negative infinity, <c>false</c> if not.</returns>
		public static bool IsNegativeInfinity(BigDecimal value) {
			return value.exponent == NegativeInfinityExponent;
		}

		/// <summary>Query if 'value' is one of the infinities.</summary>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if infinity, <c>false</c> if not.</returns>
		public static bool IsInfinity(BigDecimal value) {
			return IsPositiveInfinity(value) || IsNegativeInfinity(value);
		}

		/// <summary>Query if 'value' is NaN.</summary>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if nan, <c>false</c> if not.</returns>
		public static bool IsNaN(BigDecimal value) {
			return value.exponent == NaNExponent;
		}

		/// <summary>Query if 'value' is zero.</summary>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if zero, <c>false</c> if not.</returns>
		public static bool IsZero(BigDecimal value) {
			return (value.exponent == 0) && value.digits.IsZero;
		}

		/// <summary>Parses the value.</summary>
		/// <param name="value">The value.</param>
		/// <returns>A BigDecimal.</returns>
		public static BigDecimal Parse(string value) {
			return Parse(value, NumberStyles.Any, NumberFormatInfo.CurrentInfo);
		}

		/// <summary>Parses the value.</summary>
		/// <param name="value">The value.</param>
		/// <param name="styles">The styles.</param>
		/// <returns>A BigDecimal.</returns>
		public static BigDecimal Parse(string value, NumberStyles styles) {
			return Parse(value, styles, NumberFormatInfo.CurrentInfo);
		}

		/// <summary>Parses the value.</summary>
		/// <param name="value">The value.</param>
		/// <param name="provider">The format provider.</param>
		/// <returns>A BigDecimal.</returns>
		public static BigDecimal Parse(string value, IFormatProvider provider) {
			return Parse(value, NumberStyles.Any, provider);
		}

		/// <summary>Parses the value.</summary>
		/// <exception cref="FormatException">Thrown when the format of the value is incorrect.</exception>
		/// <param name="value">The value.</param>
		/// <param name="styles">The styles.</param>
		/// <param name="provider">The format provider.</param>
		/// <returns>A BigDecimal.</returns>
		public static BigDecimal Parse(string value, NumberStyles styles, IFormatProvider provider) {
			if (!TryParse(value, styles, provider, out var result)) {
				throw new FormatException();
			}
			return result;
		}

		/// <summary>Attempts to parse a BigDecimal from the given string.</summary>
		/// <param name="value">The value.</param>
		/// <param name="result">[out] The result.</param>
		/// <returns><c>true</c> if it succeeds, <c>false</c> if it fails.</returns>
		public static bool TryParse(string value, out BigDecimal result) {
			return TryParse(value, NumberStyles.Any, NumberFormatInfo.CurrentInfo, out result);
		}

		/// <summary>Attempts to parse a BigDecimal from the given string.</summary>
		/// <param name="value">The value.</param>
		/// <param name="styles">The styles.</param>
		/// <param name="result">[out] The result.</param>
		/// <returns><c>true</c> if it succeeds, <c>false</c> if it fails.</returns>
		public static bool TryParse(string value, NumberStyles styles, out BigDecimal result) {
			return TryParse(value, styles, NumberFormatInfo.CurrentInfo, out result);
		}

		/// <summary>Attempts to parse a BigDecimal from the given string.</summary>
		/// <param name="value">The value.</param>
		/// <param name="provider">The format provider.</param>
		/// <param name="result">[out] The result.</param>
		/// <returns><c>true</c> if it succeeds, <c>false</c> if it fails.</returns>
		public static bool TryParse(string value, IFormatProvider provider, out BigDecimal result) {
			return TryParse(value, NumberStyles.Any, provider, out result);
		}

		/// <summary>Attempts to parse a BigDecimal from the given string.</summary>
		/// <param name="value">The value.</param>
		/// <param name="styles">The styles.</param>
		/// <param name="provider">The format provider.</param>
		/// <param name="result">[out] The result.</param>
		/// <returns><c>true</c> if it succeeds, <c>false</c> if it fails.</returns>
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
				value = value.Substring(0, value.Length - info.PositiveSign.Length);
			} else if (styles.HasFlag(NumberStyles.AllowLeadingSign) && !string.IsNullOrEmpty(info.NegativeSign) && value.StartsWith(info.NegativeSign, StringComparison.Ordinal)) {
				value = value.Substring(info.PositiveSign.Length);
				negative = true;
			} else if (styles.HasFlag(NumberStyles.AllowTrailingSign) && !string.IsNullOrEmpty(info.NegativeSign) && value.EndsWith(info.NegativeSign, StringComparison.Ordinal)) {
				value = value.Substring(0, value.Length - info.PositiveSign.Length);
				negative = true;
			}
			var exponent = 0;
			if (styles.HasFlag(NumberStyles.AllowDecimalPoint)) {
				var decimalPointIndex = value.IndexOf(info.NumberDecimalSeparator, StringComparison.Ordinal);
				if (decimalPointIndex >= 0) {
					value = value.Remove(decimalPointIndex, info.NumberDecimalSeparator.Length);
					exponent = decimalPointIndex - value.Length;
				}
			}
			if (styles.HasFlag(NumberStyles.AllowExponent)) {
				var exponentIndex = value.IndexOf("e", StringComparison.OrdinalIgnoreCase);
				if (exponentIndex == 0) {
					return false; // only exponent is not a valid number
				}
				if (exponentIndex > 0) {
					if (!int.TryParse(value.Substring(exponentIndex + 1), NumberStyles.AllowLeadingSign, info, out var explicitExponent)) {
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

		private static KeyValuePair<BigInteger, int> NormalizeDouble(double value) {
			if (double.IsNaN(value)) {
				return new KeyValuePair<BigInteger, int>(BigInteger.Zero, NaNExponent);
			}
			if (double.IsPositiveInfinity(value)) {
				return new KeyValuePair<BigInteger, int>(BigInteger.Zero, PositiveInfinityExponent);
			}
			if (double.IsNegativeInfinity(value)) {
				return new KeyValuePair<BigInteger, int>(BigInteger.Zero, NegativeInfinityExponent);
			}
			// ReSharper disable CompareOfFloatsByEqualityOperator
			if (Math.Abs(value) == 0) {
				return new KeyValuePair<BigInteger, int>(BigInteger.Zero, 0);
			}
			var exp = 0;
			if (value == Math.Truncate(value)) {
				// No fractional digits, normalize
				while ((value % 10.0) == 0.0) {
					value /= 10.0;
					exp++;
				}
			} else {
				// We have fractional digits, shift exponent
				do {
					value *= 10.0;
					exp--;
				} while (value != Math.Truncate(value));
			}
			// ReSharper restore CompareOfFloatsByEqualityOperator
			return new KeyValuePair<BigInteger, int>(new BigInteger(value), exp);
		}

		private static KeyValuePair<BigInteger, int> NormalizeDecimal(decimal value) {
			// ReSharper disable CompareOfFloatsByEqualityOperator
			if (value == 0) {
				return new KeyValuePair<BigInteger, int>(BigInteger.Zero, 0);
			}
			var exp = 0;
			if (value == Math.Truncate(value)) {
				// No fractional digits, normalize
				while ((value % 10m) == 0m) {
					value /= 10m;
					exp++;
				}
			} else {
				// We have fractional digits, shift exponent
				do {
					value *= 10m;
					exp--;
				} while (value != Math.Truncate(value));
			}
			// ReSharper restore CompareOfFloatsByEqualityOperator
			return new KeyValuePair<BigInteger, int>(new BigInteger(value), exp);
		}

		private readonly BigInteger digits;
		private readonly int exponent;

		/// <summary>Create a BigDecimal from an <see cref="int"/>.</summary>
		/// <param name="value">The value.</param>
		public BigDecimal(int value): this(new BigInteger(value), 0, true) { }

		/// <summary>Create a BigDecimal from a <see cref="uint"/>.</summary>
		/// <param name="value">The value.</param>
		[CLSCompliant(false)]
		public BigDecimal(uint value): this(new BigInteger(value), 0, true) { }

		/// <summary>Create a BigDecimal from a <see cref="long"/>.</summary>
		/// <param name="value">The value.</param>
		public BigDecimal(long value): this(new BigInteger(value), 0, true) { }

		/// <summary>Create a BigDecimal from a <see cref="ulong"/>.</summary>
		/// <param name="value">The value.</param>
		[CLSCompliant(false)]
		public BigDecimal(ulong value): this(new BigInteger(value), 0, true) { }

		/// <summary>Create a BigDecimal from a <see cref="double"/>.</summary>
		/// <param name="value">The value.</param>
		public BigDecimal(double value): this(NormalizeDouble(value)) { }

		/// <summary>Create a BigDecimal from a <see cref="decimal"/>.</summary>
		/// <param name="value">The value.</param>
		public BigDecimal(decimal value): this(NormalizeDecimal(value)) { }

		/// <summary>Create a BigDecimal from an <see cref="BigInteger"/>.</summary>
		/// <param name="digits">The digits.</param>
		/// <param name="exponent">The exponent.</param>
		public BigDecimal(BigInteger digits, int exponent): this(digits, exponent, true) { }

		private BigDecimal(KeyValuePair<BigInteger, int> normalizedValue): this(normalizedValue.Key, normalizedValue.Value, true) { }

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
			return !this.digits.IsZero;
		}

		char IConvertible.ToChar(IFormatProvider provider) {
			throw new InvalidCastException();
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider) {
			return checked((sbyte)this.ToInt32());
		}

		byte IConvertible.ToByte(IFormatProvider provider) {
			return checked((byte)this.ToInt32());
		}

		short IConvertible.ToInt16(IFormatProvider provider) {
			return checked((short)this.ToInt32());
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider) {
			return checked((ushort)this.ToInt32());
		}

		internal BigInteger ToBigInteger() {
			if (IsSpecialExponent(this.exponent)) {
				throw new OverflowException();
			}
			if (this.exponent <= 0) {
				return this.digits;
			}
			return this.digits * BigInteger.Pow(10, this.exponent);
		}

		internal decimal ToDecimal() {
			if (IsSpecialExponent(this.exponent)) {
				throw new OverflowException();
			}
			var result = (decimal)this.ToBigInteger();
			return this.exponent >= 0 ? result : result / (decimal)BigInteger.Pow(10, -this.exponent);
		}

		internal double ToDouble() {
			switch (this.exponent) {
			case NaNExponent:
				return double.NaN;
			case NegativeInfinityExponent:
				return double.NegativeInfinity;
			case PositiveInfinityExponent:
				return double.PositiveInfinity;
			case 0:
				return (double)this.digits;
			}
			return (this.exponent < 0)
					? (double)this.digits / (double)BigInteger.Pow(10, -this.exponent)
					: (double)(this.digits * BigInteger.Pow(10, this.exponent));
		}

		int IConvertible.ToInt32(IFormatProvider provider) {
			return this.ToInt32();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int ToInt32() {
			return (int)this.ToBigInteger();
		}

		uint IConvertible.ToUInt32(IFormatProvider provider) {
			return (uint)this.ToBigInteger();
		}

		long IConvertible.ToInt64(IFormatProvider provider) {
			return (long)this.ToBigInteger();
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider) {
			return (ulong)this.ToBigInteger();
		}

		float IConvertible.ToSingle(IFormatProvider provider) {
			return (float)this.ToDouble();
		}

		double IConvertible.ToDouble(IFormatProvider provider) {
			return this.ToDouble();
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider) {
			return this.ToDecimal();
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider) {
			throw new InvalidCastException();
		}

		/// <summary>Convert this BigDecimal into a string representation.</summary>
		/// <param name="format">Describes the format to use.</param>
		/// <returns>A string that represents this BigDecimal.</returns>
		public string ToString(string format) {
			return this.ToString(format, null);
		}

		/// <summary>Converts the value of this instance to an equivalent <see cref="T:System.String" /> using the specified culture-specific formatting information.</summary>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.</param>
		/// <returns>A <see cref="T:System.String" /> instance equivalent to the value of this instance.</returns>
		public string ToString(IFormatProvider provider) {
			return this.ToString(null, provider);
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
				return this.ToBigInteger();
			}
			if (effectiveType == typeof(string)) {
				return this.ToString(provider);
			}
			throw new InvalidCastException();
		}

		/// <summary>Formats the value of the current instance using the specified format.</summary>
		/// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable" /> implementation.</param>
		/// <param name="provider">The provider.</param>
		/// <returns>The value of the current instance in the specified format.</returns>
		public string ToString(string format, IFormatProvider provider) {
			var info = NumberFormatInfo.GetInstance(provider);
			switch (this.exponent) {
			case NaNExponent:
				return info.NaNSymbol;
			case NegativeInfinityExponent:
				return info.PositiveInfinitySymbol;
			case PositiveInfinityExponent:
				return info.NegativeInfinitySymbol;
			}
			var result = BigInteger.Abs(this.digits).ToString(info);
			if (this.exponent < 0) {
				var position = result.Length + this.exponent;
				result = result.Insert(position, info.NumberDecimalSeparator);
				if (position == 0) {
					result = result.Insert(0, info.NativeDigits[0]);
				}
			}
			if (this.digits.Sign < 0) {
				result = result.Insert(0, info.NegativeSign);
			}
			return result;
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">The big decimal to compare to this BigDecimal.</param>
		/// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
		public bool Equals(BigDecimal other) {
			return (this.exponent == other.exponent) && this.digits.Equals(other.digits);
		}

		/// <summary>
		/// 	Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in
		/// 	the sort order as the other object.
		/// </summary>
		/// <param name="other">Big decimal to compare to this.</param>
		/// <returns>
		/// 	A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" />
		/// 	in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the
		/// 	sort order.
		/// </returns>
		public int CompareTo(BigDecimal other) {
			var signDiff = this.digits.Sign - other.digits.Sign;
			if (signDiff != 0) {
				return signDiff;
			}
			if (IsNaN(this)) {
				return IsNaN(other) ? 0 : -1;
			}
			if (IsPositiveInfinity(this)) {
				return IsPositiveInfinity(other) ? 0 : 1;
			}
			if (IsNegativeInfinity(this)) {
				return IsNegativeInfinity(other) ? 0 : -1;
			}
			if (IsNaN(other)) {
				return 1;
			}
			if (IsPositiveInfinity(other)) {
				return -1;
			}
			if (IsNegativeInfinity(other)) {
				return 1;
			}
			NormalizeDigits(this, other, out var thisDigits, out var valueDigits);
			return thisDigits.CompareTo(valueDigits);
		}

		/// <summary>Indicates whether this instance and a specified object are equal.</summary>
		/// <param name="obj">The object to compare with the current instance.</param>
		/// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
		public override bool Equals(object obj) {
			return base.Equals(obj);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		public override int GetHashCode() {
			return unchecked(this.digits.GetHashCode() ^ (this.exponent * 397));
		}

		/// <summary>Gets the sign of the value.</summary>
		/// <value>The sign.</value>
		public int Sign {
			get {
				if (IsPositiveInfinity(this)) {
					return 1;
				}
				if (IsNegativeInfinity(this)) {
					return -1;
				}
				return this.digits.Sign;
			}
		}

		int IComparable.CompareTo(object obj) {
			return this.CompareTo((BigDecimal)obj);
		}

		/// <summary>Convert this BigDecimal into a string representation.</summary>
		/// <returns>A string that represents this BigDecimal.</returns>
		public override string ToString() {
			return this.ToString(null, null);
		}

		BigDecimal IIncrementable<BigDecimal>.Decrement() {
			return Subtract(this, One);
		}

		BigDecimal IIncrementable<BigDecimal>.Increment() {
			return Add(this, One);
		}
	}
}
