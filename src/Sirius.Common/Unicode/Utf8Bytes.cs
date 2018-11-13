using Sirius.Collections;

namespace Sirius.Unicode {
	/// <summary>Some UTF-8 byte values.</summary>
	public static class Utf8Bytes {
		/// <summary>The EOF placeholder character.</summary>
		/// <remarks>It is one of the invalid bytes in UTF-8, and thus not expected in a normal UTF-8 data stream.</remarks>
		public const byte EOF = 0xFF;

		/// <summary>The valid ASCII byte range.</summary>
		public static readonly Range<byte> ValidAscii = Range<byte>.Create(0x00, 0x7F);
		/// <summary>The valid followup byte range (2nd, 3rd and 4th bytes of an encoded non-ASCII codepoint).</summary>
		public static readonly Range<byte> ValidFollowupByte = Range<byte>.Create(0x80, 0xBF);
		/// <summary>The valid start byte range (1st byte of an encoded non-ASCII codepoint).</summary>
		public static readonly Range<byte> ValidStartByte = Range<byte>.Create(0xC2, 0xF4);

		/// <summary>The valid first byte range.</summary>
		public static readonly RangeSet<byte> ValidFirstByte = ValidAscii | ValidStartByte;

		/// <summary>The valid bytes in a UTF-8 stream.</summary>
		public static readonly RangeSet<byte> ValidAll = ValidFirstByte | ValidFollowupByte;
	}
}
