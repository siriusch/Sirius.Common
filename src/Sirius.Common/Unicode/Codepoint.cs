using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

using Sirius.Collections;

namespace Sirius.Unicode {
	public struct Codepoint: IComparable<Codepoint>, IEquatable<Codepoint>, IFormattable, IIncrementable<Codepoint>, IConvertible {
		public static readonly IRangeSet<Codepoint> Bmp = new RangeSet<Codepoint>(new[] {
			new Range<Codepoint>(new Codepoint(0x0000, false), new Codepoint(0xD7FF, false)),
			new Range<Codepoint>(new Codepoint(0xE000, false), new Codepoint(0xFFFD, false))
		});

		public static readonly IRangeSet<Codepoint> Surrogates = new RangeSet<Codepoint>(new[] {
			new Range<Codepoint>(new Codepoint(0xD800, false), new Codepoint(0xDFFF, false))
		});

		public static readonly Codepoint MinValue = new Codepoint(0x000000, false);

		public static readonly Codepoint MaxValue = new Codepoint(0x10FFFF, false);

		public static bool operator ==(Codepoint x, Codepoint y) {
			return x.value == y.value;
		}

		public static bool operator !=(Codepoint x, Codepoint y) {
			return x.value != y.value;
		}

		public static bool operator <(Codepoint x, Codepoint y) {
			return x.value < y.value;
		}

		public static bool operator >(Codepoint x, Codepoint y) {
			return x.value > y.value;
		}

		public static bool operator >=(Codepoint x, Codepoint y) {
			return x.value >= y.value;
		}

		public static bool operator <=(Codepoint x, Codepoint y) {
			return x.value <= y.value;
		}

		public static Codepoint operator +(Codepoint c, int offset) {
			return new Codepoint(c.value+offset);
		}

		public static Codepoint operator -(Codepoint c, int offset) {
			return new Codepoint(c.value-offset);
		}

		public static explicit operator char(Codepoint codepoint) {
			return (char)codepoint.value;
		}

		public static implicit operator Codepoint(char value) {
			return new Codepoint(value, false);
		}

		public static explicit operator int(Codepoint codepoint) {
			return codepoint.value;
		}

		public static explicit operator Codepoint(int value) {
			return new Codepoint(value, false);
		}

		public static bool IsValid(Codepoint codepoint) {
			return IsValidRange(codepoint.value);
		}

		private static bool IsValidRange(int value) => ((value >= 0) && (value < 0xD800)) || ((value >= 0xE000) && (value <= 0xFFFD)) || ((value >= 0x010000) && (value <= 0x10FFFF));

		public static bool IsUpper(Codepoint codepoint) {
			return codepoint.FitsIntoChar && char.IsUpper((char)codepoint);
		}

		public static bool IsLower(Codepoint codepoint) {
			return codepoint.FitsIntoChar && char.IsLower((char)codepoint);
		}

		public static Codepoint ToUpperInvariant(Codepoint codepoint) {
			return codepoint.FitsIntoChar ? char.ToUpperInvariant((char)codepoint) : codepoint;
		}

		public static Codepoint ToLowerInvariant(Codepoint codepoint) {
			return codepoint.FitsIntoChar ? char.ToLowerInvariant((char)codepoint) : codepoint;
		}

		public static bool IsCombiningMark(Codepoint c) {
			return IsCombiningMark(c.value);
		}

		internal static bool IsCombiningMark(int c) {
			return ((c >= 0x0300) && (c <= 0x036F))
					|| ((c >= 0x1AB0) && (c <= 0x1AFF))
					|| ((c >= 0x1DC0) && (c <= 0x1DFF))
					|| ((c >= 0x20D0) && (c <= 0x20FF))
					|| ((c >= 0xFE20) && (c <= 0xFE2F));
		}

		public static Codepoint Parse(string hexValue) {
			Codepoint result;
			if (!TryParse(hexValue, out result)) {
				throw new FormatException();
			}
			return result;
		}

		public static bool TryParse(string hexValue, out Codepoint result) {
			if ((hexValue.StartsWith("0x") || hexValue.StartsWith("U+") || hexValue.StartsWith("u+"))) {
				hexValue = hexValue.Substring(2);
			}
			int value;
			if (!int.TryParse(hexValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value)) {
				result = default(Codepoint);
				return false;
			}
			result = new Codepoint(value);
			return true;
		}

		public static IEnumerable<Codepoint> FromCharsMany(IEnumerable<char> chars) {
			using (var enumerator = chars.GetEnumerator()) {
				while (enumerator.MoveNext()) {
					yield return FromCharEnumerator(enumerator);
				}
			}
		}

		public static Codepoint FromChars(IEnumerable<char> chars) {
			if (chars == null) {
				throw new ArgumentNullException(nameof(chars));
			}
			Codepoint value;
			using (var enumerator = chars.GetEnumerator()) {
				if (!enumerator.MoveNext()) {
					throw new ArgumentException("Empty enumeration of characters is not a valid codepoint", nameof(chars));
				}
				value = FromCharEnumerator(enumerator);
				if (enumerator.MoveNext()) {
					throw new ArgumentException("Excess characters in enumeration", nameof(chars));
				}
			}
			return value;
		}

		internal static Codepoint FromCharEnumerator(IEnumerator<char> chars) {
			Codepoint value;
			if (char.IsHighSurrogate(chars.Current)) {
				var high = chars.Current;
				if (!chars.MoveNext() || !char.IsLowSurrogate(chars.Current)) {
					throw new ArgumentException("Low surrogate must follow a high surrogate", nameof(chars));
				}
				var low = chars.Current;
				value = new Codepoint(char.ConvertToUtf32(high, low), false);
			} else if (char.IsLowSurrogate(chars.Current)) {
				throw new ArgumentException("Low surrogate cannot be without prior high surrogate", nameof(chars));
			} else {
				value = new Codepoint(chars.Current, false);
			}
			return value;
		}

		private readonly int value;

		internal bool IsValidSingleChar {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				return ((this.value >= 0) && (this.value <= 0xD7FF)) || ((this.value >= 0xE000) && (this.value <= 0xFFFF));
			}
		}

		internal bool IsValidDoubleChar {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				return (this.value >= 0x10000) && (this.value <= 0x10FFFF);
			}
		}

		internal Codepoint(int value, bool check) {
			if (check && !IsValidRange(value)) {
				throw new ArgumentOutOfRangeException(nameof(value));
			}
			this.value = value;
		}

		public Codepoint(int value): this(value, true) { }

		public bool FitsIntoChar => this.value < 0xFFFF;

		public int ToInt32() {
			return this.value;
		}

		public byte[] ToUtf32Bytes() {
			return BitConverter.GetBytes(this.value);
		}

		public char[] ToChars() {
			if (this.IsValidSingleChar) {
				return new[] { GetSingleCharUnchecked() };
			}
			if (this.IsValidDoubleChar) {
				return new[] { GetHighSurrogate(), GetLowSurrogate() };
			}
			throw new UnsupportedCodepointException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal char GetSingleCharUnchecked() {
			return (char)this.value;
		}

		internal char GetHighSurrogate() {
			return (char)(((this.value-0x010000)>>10)|0xD800);
		}

		internal char GetLowSurrogate() {
			return (char)(((this.value-0x010000)&0x03FF)|0xDC00);
		}

		public byte[] ToUtf8Bytes() {
			if (this.value >= 0x0) {
				if (this.value <= 0x00007F) {
					return new[] {
						(byte)this.value
					};
				}
				if (this.value <= 0x0007FF) {
					return new[] {
						(byte)(((this.value>>6)&0x1F)|0xC0),
						(byte)(0x80|(this.value&0x3F))
					};
				}
				if (this.value <= 0x00FFFF) {
					return new[] {
						(byte)(((this.value>>12)&0x0F)|0xE0),
						(byte)(((this.value>>6)&0x3F)|0x80),
						(byte)((this.value&0x3F)|0x80)
					};
				}
				if (this.value <= 0x1FFFFF) {
					return new[] {
						(byte)(0xF0|(0x07&(this.value>>18))),
						(byte)(0x80|(0x3F&(this.value>>12))),
						(byte)(0x80|(0x3F&(this.value>>6))),
						(byte)(0x80|(0x3F&this.value))
					};
				}
			}
			throw new UnsupportedCodepointException();
		}

		public void AppendTo(StringBuilder builder) {
			if (this.IsValidSingleChar) {
				builder.Append(GetSingleCharUnchecked());
			} else if (this.IsValidDoubleChar) {
				builder.Append(GetHighSurrogate());
				builder.Append(GetLowSurrogate());
			} else {
				throw new UnsupportedCodepointException();
			}
		}

		public int CompareTo(Codepoint other) {
			return this.value.CompareTo(other.value);
		}

		public bool Equals(Codepoint other) {
			return this.value == other.value;
		}

		public override bool Equals(object obj) {
			if (obj is Codepoint) {
				return this.value == ((Codepoint)obj).value;
			}
			return base.Equals(obj);
		}

		public override int GetHashCode() {
			return this.value.GetHashCode();
		}

		public override string ToString() {
			return ToString(null, null);
		}

		public string ToString(string format) {
			return ToString(format, null);
		}

		TypeCode IConvertible.GetTypeCode() {
			return TypeCode.Object;
		}

		bool IConvertible.ToBoolean(IFormatProvider provider) {
			return this.value > 0;
		}

		char IConvertible.ToChar(IFormatProvider provider) {
			return (char)this;
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider) {
			return (sbyte)this.value;
		}

		byte IConvertible.ToByte(IFormatProvider provider) {
			return (byte)this.value;
		}

		short IConvertible.ToInt16(IFormatProvider provider) {
			return (short)this.value;
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider) {
			return (ushort)this.value;
		}

		int IConvertible.ToInt32(IFormatProvider provider) {
			return this.value;
		}

		uint IConvertible.ToUInt32(IFormatProvider provider) {
			return (uint)this.value;
		}

		long IConvertible.ToInt64(IFormatProvider provider) {
			return this.value;
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider) {
			return (ulong)this.value;
		}

		float IConvertible.ToSingle(IFormatProvider provider) {
			return this.value;
		}

		double IConvertible.ToDouble(IFormatProvider provider) {
			return this.value;
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider) {
			return this.value;
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider) {
			throw new NotSupportedException();
		}

		public string ToString(IFormatProvider formatProvider) {
			return ToString(null, formatProvider);
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider) {
			if (typeof(Codepoint).IsAssignableFrom(conversionType)) {
				return this;
			}
			return ((IConvertible)this.value).ToType(conversionType, provider);
		}

		public string ToString(string format, IFormatProvider formatProvider) {
			switch (format) {
			case "x":
				return string.Format(formatProvider, "0x{0:x2}", this.value);
			case "u":
				return string.Format(formatProvider, "u+{0:x4}", this.value);
			case "U":
				return string.Format(formatProvider, "U+{0:x8}", this.value);
			case "X":
				if (this.value <= 0xFF) {
					goto case "x";
				}
				if (this.value <= 0xFFFF) {
					goto case "u";
				}
				goto case "U";
			}
			if (this.value == -1) {
				return "EOF";
			}
			if (this.IsValidSingleChar) {
				return ((char)this.value).ToString();
			}
			if (this.IsValidDoubleChar) {
				return char.ConvertFromUtf32(this.value);
			}
			return "(Invalid Codepoint)";
		}

		Codepoint IIncrementable<Codepoint>.Increment() {
			return new Codepoint(this.value+1, false);
		}

		Codepoint IIncrementable<Codepoint>.Decrement() {
			return new Codepoint(this.value-1, false);
		}
	}
}
