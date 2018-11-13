using System;

namespace Sirius.Unicode {
	/// <summary>A base class for push.based UTF-8 decoding.</summary>
	/// <seealso cref="Utf8CharDecoder"/>
	/// <seealso cref="Utf8CodepointDecoder"/>
	public abstract class Utf8DecoderBase {
		private int current;
		private int pendingBits;

		/// <summary>Specialised default constructor for use only by derived class.</summary>
		protected Utf8DecoderBase() {
			this.pendingBits = 0;
		}

		/// <summary>Gets a value indicating whether this decoder has pending UTF-8 data.</summary>
		/// <value><c>true</c> if this decoder has pending data, <c>false</c> if not.</value>
		public bool HasPendingData => this.pendingBits > 0;

		/// <summary>Push the next UTF-8 data into the decoder.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="data">The UTF-8 byte to push.</param>
		public void Push(byte data) {
			if (this.pendingBits == 0) {
				if ((data&0x80) == 0x00) {
					this.Yield(data);
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
					this.Yield(this.current);
				}
				return;
			}
			throw new ArgumentException($"Invalid byte {data} with {this.pendingBits} pending bits");
		}

		/// <summary>Yields a UTF-32 codepoint.</summary>
		/// <param name="value">The codepoint value.</param>
		protected abstract void Yield(int value);
	}
}
