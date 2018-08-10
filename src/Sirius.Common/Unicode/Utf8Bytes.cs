using Sirius.Collections;

namespace Sirius.Unicode {
	public static class Utf8Bytes {
		public const byte EOF = 0xFF;

		public static readonly Range<byte> ValidAscii = Range<byte>.Create(0x00, 0x7F);
		public static readonly Range<byte> ValidFollowupByte = Range<byte>.Create(0x80, 0xBF);
		public static readonly Range<byte> ValidStartByte = Range<byte>.Create(0xC2, 0xF4);

		public static readonly RangeSet<byte> ValidFirstByte = new RangeSet<byte>(new [] {ValidAscii, ValidStartByte });
		public static readonly RangeSet<byte> ValidAll = new RangeSet<byte>(new [] {ValidAscii, ValidFollowupByte, ValidStartByte });
	}
}