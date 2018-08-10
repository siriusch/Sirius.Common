using System;

namespace Sirius.Unicode {
	/// <summary>
	///     Push-based converter from UTF8 byte stream to UTF16 character stream
	/// </summary>
	public sealed class Utf8CharDecoder {
		private readonly Action<char> sink;
		private int current;
		private int pendingBits;

		public Utf8CharDecoder(Action<char> sink) {
			this.sink = sink;
			this.pendingBits = 0;
		}

		public bool HasPendingData => this.pendingBits > 0;

		public void Push(byte data) {
			if (this.pendingBits == 0) {
				if ((data&0x80) == 0x00) {
					Yield(data);
					return;
				}
				if ((data&0xE0) == 0xC0) {
					this.pendingBits = 6;
					this.current = (data&0x1F)<<6;
					return;
				}
				if ((data&0xF0) == 0xE0) {
					this.pendingBits = 12;
					this.current = (data&0x0F)<<12;
					return;
				}
				if ((data&0xF8) == 0xF0) {
					this.pendingBits = 18;
					this.current = (data&0x07)<<18;
					return;
				}
			} else if ((data&0xC0) == 0x80) {
				this.pendingBits -= 6;
				this.current = this.current|((data&0x3F)<<this.pendingBits);
				if (this.pendingBits == 0) {
					Yield(this.current);
				}
				return;
			}
			throw new ArgumentException($"Invalid byte {data} with {this.pendingBits} pending bits");
		}

		private void Yield(int value) {
			if (value <= 0xFFFF) {
				this.sink((char)value);
			} else if ((value >= 0x10000) && (value <= 0x10FFFF)) {
				var valueOffset = value-0x010000;
				this.sink((char)((valueOffset>>10)|0xD800));
				this.sink((char)((valueOffset&0x03FF)|0xDC00));
			} else {
				throw new InvalidOperationException("Invalid codepoint");
			}
		}
	}
}
