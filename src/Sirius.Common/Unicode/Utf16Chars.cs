using Sirius.Collections;

namespace Sirius.Unicode {
	public static class Utf16Chars {
		public const char EOF = '\uFFFF'; // this character is explicitly reserved as "not a character" in unicode and thus invalid

		private static readonly Range<char> ValidBmp1 = Range<char>.Create('\u0000', '\uD7FF');
		public static readonly Range<char> ValidHighSurrogate = Range<char>.Create('\uD800', '\uDBFF');
		public static readonly Range<char> ValidLowSurrogate = Range<char>.Create('\uDC00', '\uDFFF');
		private static readonly Range<char> ValidBmp2 = Range<char>.Create('\uE000', '\uFFFD');

		public static readonly RangeSet<char> ValidBmp = new RangeSet<char>(new [] {ValidBmp1, ValidBmp2});
		public static readonly RangeSet<char> ValidFirstChar = new RangeSet<char>(new[] { ValidBmp1, ValidHighSurrogate, ValidBmp2 });
		public static readonly RangeSet<char> ValidAll = new RangeSet<char>(new[] { ValidBmp1, ValidHighSurrogate, ValidLowSurrogate, ValidBmp2 });
	}
}