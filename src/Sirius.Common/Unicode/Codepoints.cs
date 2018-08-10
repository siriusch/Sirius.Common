using System;

using Sirius.Collections;

namespace Sirius.Unicode {
	public static class Codepoints {
		/// <summary>
		/// Variation selectors come after a unicode codepoint to indicate that it should be represented in a particular format.
		/// </summary>
		public static class VariationSelectors {
			public static readonly Codepoint VS15 = new Codepoint(0xFE0E, false);
			public static readonly Codepoint TextSymbol = VS15;

			public static readonly Codepoint VS16 = new Codepoint(0xFE0F, false);
			public static readonly Codepoint EmojiSymbol = VS16;
		}

		public static readonly Range<Codepoint> Reserved = new Range<Codepoint>(new Codepoint(0xFFFE, false), new Codepoint(0xFFFF, false));
		public static readonly Range<Codepoint> Surrogates = new Range<Codepoint>(new Codepoint(0xD800, false), new Codepoint(0xE000, false));
		public static readonly RangeSet<Codepoint> Valid = RangeSet<Codepoint>.Subtract(RangeSet<Codepoint>.All, new RangeSet<Codepoint>(new [] {Surrogates, Reserved}));
		public static readonly RangeSet<Codepoint> ValidBmp = RangeSet<Codepoint>.Subtract(new RangeSet<Codepoint>(Range<Codepoint>.Create(new Codepoint(0x0000, false), new Codepoint(0xFFFD, false))), new RangeSet<Codepoint>(Surrogates));

		public static readonly Codepoint EOF = new Codepoint(0xFFFF, false);

		/// <summary>
		/// The right-to-left mark
		/// </summary>
		public static readonly Codepoint RLM = new Codepoint(0x200F, false);

		/// <summary>
		/// The left-to-right mark
		/// </summary>
		public static readonly Codepoint LRM = new Codepoint(0x200E, false);

		/// <summary>
		/// ZWJ is used to combine multiple emoji codepoints into a single emoji symbol.
		/// </summary>
		public static readonly Codepoint ZWJ = new Codepoint(0x200D, false);

		/// <summary>
		/// ORC is used as a placeholder to indicate an object should replace this codepoint in the string.
		/// </summary>
		public static readonly Codepoint ObjectReplacementCharacter = new Codepoint(0xFFFC, false);

		public static readonly Codepoint ORC = ObjectReplacementCharacter;

		/// <summary>
		/// The "combined enclosing keycap" is used by emoji to box icons
		/// </summary>
		public static readonly Codepoint Keycap = new Codepoint(0x20E3, false);
	}
}
