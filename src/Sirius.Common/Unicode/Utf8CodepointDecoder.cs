using System;

namespace Sirius.Unicode {
	/// <summary>Push-based converter from UTF-8 byte stream to codepoint stream.</summary>
	public sealed class Utf8CodepointDecoder: Utf8DecoderBase {
		private readonly Action<Codepoint> sink;

		/// <summary>Constructor.</summary>
		/// <param name="sink">The sink which receives the codepoints.</param>
		public Utf8CodepointDecoder(Action<Codepoint> sink): base() {
			this.sink = sink;
		}

		/// <summary>Yields a UTF-32 codepoint.</summary>
		/// <param name="value">The codepoint value.</param>
		protected override void Yield(int value) {
			this.sink(new Codepoint(value));
		}
	}
}
