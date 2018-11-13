using System;

namespace Sirius.Unicode {
	/// <summary>Push-based converter from UTF-8 byte stream to UTF-16 character stream.</summary>
	public sealed class Utf8CharDecoder: Utf8DecoderBase {
		private readonly Action<char> sink;

		/// <summary>Constructor.</summary>
		/// <param name="sink">The sink which receives the UTF-16 characters.</param>
		public Utf8CharDecoder(Action<char> sink): base() {
			this.sink = sink;
		}

		/// <summary>Yields a UTF-32 codepoint.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="value">The codepoint value.</param>
		protected override void Yield(int value) {
			if (value <= 0xFFFF) {
				this.sink((char)value);
			} else if ((value >= 0x10000) && (value <= 0x10FFFF)) {
				var valueOffset = value - 0x010000;
				this.sink((char)((valueOffset >> 10) | 0xD800));
				this.sink((char)((valueOffset & 0x03FF) | 0xDC00));
			} else {
				throw new InvalidOperationException("Invalid codepoint");
			}
		}
	}
}
