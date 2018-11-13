using Sirius.Collections;

namespace Sirius.Unicode {
	/// <summary>Some UTF-16 character values.</summary>
	public static class Utf16Chars {
		/// <summary>The EOF placeholder character.</summary>
		/// <remarks>It is one of the two reserved characters, and thus not a "valid" character in a normal UTF-16 data stream.</remarks>
		public const char EOF = '\uFFFF'; // this character is explicitly reserved as "not a character" in unicode and thus invalid

		private static readonly Range<char> ValidBmp1 = Range<char>.Create('\u0000', '\uD7FF');
		/// <summary>The high surrogate range.</summary>
		public static readonly Range<char> HighSurrogate = Range<char>.Create('\uD800', '\uDBFF');
		/// <summary>The low surrogate range.</summary>
		public static readonly Range<char> LowSurrogate = Range<char>.Create('\uDC00', '\uDFFF');
		private static readonly Range<char> ValidBmp2 = Range<char>.Create('\uE000', '\uFFFD');

		/// <summary>The valid BMP range (without surrogates).</summary>
		public static readonly RangeSet<char> ValidBmp = ValidBmp1 | ValidBmp2;
		/// <summary>The valid first character range.</summary>
		public static readonly RangeSet<char> ValidFirstChar = ValidBmp | HighSurrogate;
		/// <summary>The valid characters in a UTF-16 stream.</summary>
		public static readonly RangeSet<char> ValidAll = ValidFirstChar | LowSurrogate;
	}
}
