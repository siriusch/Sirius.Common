using System;

using Sirius.Collections;

namespace Sirius.Unicode {
	/// <summary>Predefined <see cref="Codepoint" /> values and ranges.</summary>
	public static class Codepoints {
		/// <summary>
		///     Variation selectors come after a unicode codepoint to indicate that it should be represented in a particular
		///     format.
		/// </summary>
		public static class VariationSelectors {
			/// <summary>The text symbol variant selector (15).</summary>
			public static readonly Codepoint TextSymbol = Get(15);

			/// <summary>The emoji symbol variant selector (16).</summary>
			public static readonly Codepoint EmojiSymbol = Get(16);

			/// <summary>Gets the codepoint for the given variation selector.</summary>
			/// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
			/// <param name="variationSelector">The variation selector to get (1..256).</param>
			/// <returns>A Codepoint.</returns>
			public static Codepoint Get(int variationSelector) {
				if ((variationSelector <= 0) || (variationSelector > 256)) {
					throw new ArgumentOutOfRangeException(nameof(variationSelector));
				}
				return new Codepoint(variationSelector <= 16 ? 0xFE00 : 0xE0100 + (variationSelector - 1), false);
			}
		}

		/// <summary>The byte order mark codepoint.</summary>
		public static readonly Codepoint ByteOrderMark = new Codepoint(0xFEFF, false);

		/// <summary>The 9 specials codepoints.</summary>
		public static readonly Range<Codepoint> Specials = new Range<Codepoint>(new Codepoint(0xFFF0, false), new Codepoint(0xFFF8, false));
		/// <summary>The 3 annotations codepoints.</summary>
		public static readonly Range<Codepoint> Annotations = new Range<Codepoint>(new Codepoint(0xFFF9, false), new Codepoint(0xFFFB, false));
		/// <summary>The 2 reserved BMP codepoints.</summary>
		public static readonly Range<Codepoint> Reserved = new Range<Codepoint>(new Codepoint(0xFFFE, false), new Codepoint(0xFFFF, false));
		/// <summary>The high surrogates.</summary>
		public static readonly Range<Codepoint> HighSurrogates = new Range<Codepoint>(new Codepoint(0xD800, false), new Codepoint(0xDBFF, false));
		/// <summary>The low surrogates.</summary>
		public static readonly Range<Codepoint> LowSurrogates = new Range<Codepoint>(new Codepoint(0xDC00, false), new Codepoint(0xDFFF, false));
		/// <summary>The surrogates (high and low combined).</summary>
		/// <remarks>These are not "valid" codepoints, since they are used in conjunction with alternative encodings only (e.g. UTF-16).</remarks>
		public static readonly Range<Codepoint> Surrogates = new Range<Codepoint>(new Codepoint(0xD800, false), new Codepoint(0xDFFF, false));
		/// <summary>The valid Unicode codepoints (all except for surrogates and reserved).</summary>
		public static readonly RangeSet<Codepoint> Valid = ~(Surrogates | Reserved);
		/// <summary>The valid Unicode codepoints in the BMP.</summary>
		public static readonly RangeSet<Codepoint> ValidBmp = Valid & new Range<Codepoint>(new Codepoint(0x0000, false), new Codepoint(0xFFFF, false));

/// <summary>The SMP lowercase codepoints.</summary>
		public static readonly RangeSet<Codepoint> LowercaseSmp = new RangeSet<Codepoint>(new[] {
				new Range<Codepoint>(new Codepoint(0x10428, false), new Codepoint(0x1044F, false)),
				new Range<Codepoint>(new Codepoint(0x104D8, false), new Codepoint(0x104FB, false)),
				new Range<Codepoint>(new Codepoint(0x10CC0, false), new Codepoint(0x10CF2, false)),
				new Range<Codepoint>(new Codepoint(0x118C0, false), new Codepoint(0x118DF, false)),
				new Range<Codepoint>(new Codepoint(0x16E60, false), new Codepoint(0x16E7F, false)),
				new Range<Codepoint>(new Codepoint(0x1D41A, false), new Codepoint(0x1D433, false)),
				new Range<Codepoint>(new Codepoint(0x1D44E, false), new Codepoint(0x1D454, false)),
				new Range<Codepoint>(new Codepoint(0x1D456, false), new Codepoint(0x1D467, false)),
				new Range<Codepoint>(new Codepoint(0x1D482, false), new Codepoint(0x1D49B, false)),
				new Range<Codepoint>(new Codepoint(0x1D4B6, false), new Codepoint(0x1D4B9, false)),
				new Range<Codepoint>(new Codepoint(0x1D4BB, false), new Codepoint(0x1D4BB, false)),
				new Range<Codepoint>(new Codepoint(0x1D4BD, false), new Codepoint(0x1D4C3, false)),
				new Range<Codepoint>(new Codepoint(0x1D4C5, false), new Codepoint(0x1D4CF, false)),
				new Range<Codepoint>(new Codepoint(0x1D4EA, false), new Codepoint(0x1D503, false)),
				new Range<Codepoint>(new Codepoint(0x1D51E, false), new Codepoint(0x1D537, false)),
				new Range<Codepoint>(new Codepoint(0x1D552, false), new Codepoint(0x1D56B, false)),
				new Range<Codepoint>(new Codepoint(0x1D586, false), new Codepoint(0x1D59F, false)),
				new Range<Codepoint>(new Codepoint(0x1D5BA, false), new Codepoint(0x1D5D3, false)),
				new Range<Codepoint>(new Codepoint(0x1D5EE, false), new Codepoint(0x1D607, false)),
				new Range<Codepoint>(new Codepoint(0x1D622, false), new Codepoint(0x1D63B, false)),
				new Range<Codepoint>(new Codepoint(0x1D656, false), new Codepoint(0x1D66F, false)),
				new Range<Codepoint>(new Codepoint(0x1D68A, false), new Codepoint(0x1D6A5, false)),
				new Range<Codepoint>(new Codepoint(0x1D6C2, false), new Codepoint(0x1D6DA, false)),
				new Range<Codepoint>(new Codepoint(0x1D6DC, false), new Codepoint(0x1D6E1, false)),
				new Range<Codepoint>(new Codepoint(0x1D6FC, false), new Codepoint(0x1D714, false)),
				new Range<Codepoint>(new Codepoint(0x1D716, false), new Codepoint(0x1D71B, false)),
				new Range<Codepoint>(new Codepoint(0x1D736, false), new Codepoint(0x1D74E, false)),
				new Range<Codepoint>(new Codepoint(0x1D750, false), new Codepoint(0x1D755, false)),
				new Range<Codepoint>(new Codepoint(0x1D770, false), new Codepoint(0x1D788, false)),
				new Range<Codepoint>(new Codepoint(0x1D78A, false), new Codepoint(0x1D78F, false)),
				new Range<Codepoint>(new Codepoint(0x1D7AA, false), new Codepoint(0x1D7C2, false)),
				new Range<Codepoint>(new Codepoint(0x1D7C4, false), new Codepoint(0x1D7C9, false)),
				new Range<Codepoint>(new Codepoint(0x1D7CB, false), new Codepoint(0x1D7CB, false)),
				new Range<Codepoint>(new Codepoint(0x1E922, false), new Codepoint(0x1E943, false))
		});

		/// <summary>The SMP uppercase codepoints.</summary>
		public static readonly RangeSet<Codepoint> UppercaseSmp = new RangeSet<Codepoint>(new[] {
				new Range<Codepoint>(new Codepoint(0x10400, false), new Codepoint(0x10427, false)),
				new Range<Codepoint>(new Codepoint(0x104B0, false), new Codepoint(0x104D3, false)),
				new Range<Codepoint>(new Codepoint(0x10C80, false), new Codepoint(0x10CB2, false)),
				new Range<Codepoint>(new Codepoint(0x118A0, false), new Codepoint(0x118BF, false)),
				new Range<Codepoint>(new Codepoint(0x16E40, false), new Codepoint(0x16E5F, false)),
				new Range<Codepoint>(new Codepoint(0x1D400, false), new Codepoint(0x1D419, false)),
				new Range<Codepoint>(new Codepoint(0x1D434, false), new Codepoint(0x1D44D, false)),
				new Range<Codepoint>(new Codepoint(0x1D468, false), new Codepoint(0x1D481, false)),
				new Range<Codepoint>(new Codepoint(0x1D49C, false), new Codepoint(0x1D49C, false)),
				new Range<Codepoint>(new Codepoint(0x1D49E, false), new Codepoint(0x1D49F, false)),
				new Range<Codepoint>(new Codepoint(0x1D4A2, false), new Codepoint(0x1D4A2, false)),
				new Range<Codepoint>(new Codepoint(0x1D4A5, false), new Codepoint(0x1D4A6, false)),
				new Range<Codepoint>(new Codepoint(0x1D4A9, false), new Codepoint(0x1D4AC, false)),
				new Range<Codepoint>(new Codepoint(0x1D4AE, false), new Codepoint(0x1D4B5, false)),
				new Range<Codepoint>(new Codepoint(0x1D4D0, false), new Codepoint(0x1D4E9, false)),
				new Range<Codepoint>(new Codepoint(0x1D504, false), new Codepoint(0x1D505, false)),
				new Range<Codepoint>(new Codepoint(0x1D507, false), new Codepoint(0x1D50A, false)),
				new Range<Codepoint>(new Codepoint(0x1D50D, false), new Codepoint(0x1D514, false)),
				new Range<Codepoint>(new Codepoint(0x1D516, false), new Codepoint(0x1D51C, false)),
				new Range<Codepoint>(new Codepoint(0x1D538, false), new Codepoint(0x1D539, false)),
				new Range<Codepoint>(new Codepoint(0x1D53B, false), new Codepoint(0x1D53E, false)),
				new Range<Codepoint>(new Codepoint(0x1D540, false), new Codepoint(0x1D544, false)),
				new Range<Codepoint>(new Codepoint(0x1D546, false), new Codepoint(0x1D546, false)),
				new Range<Codepoint>(new Codepoint(0x1D54A, false), new Codepoint(0x1D550, false)),
				new Range<Codepoint>(new Codepoint(0x1D56C, false), new Codepoint(0x1D585, false)),
				new Range<Codepoint>(new Codepoint(0x1D5A0, false), new Codepoint(0x1D5B9, false)),
				new Range<Codepoint>(new Codepoint(0x1D5D4, false), new Codepoint(0x1D5ED, false)),
				new Range<Codepoint>(new Codepoint(0x1D608, false), new Codepoint(0x1D621, false)),
				new Range<Codepoint>(new Codepoint(0x1D63C, false), new Codepoint(0x1D655, false)),
				new Range<Codepoint>(new Codepoint(0x1D670, false), new Codepoint(0x1D689, false)),
				new Range<Codepoint>(new Codepoint(0x1D6A8, false), new Codepoint(0x1D6C0, false)),
				new Range<Codepoint>(new Codepoint(0x1D6E2, false), new Codepoint(0x1D6FA, false)),
				new Range<Codepoint>(new Codepoint(0x1D71C, false), new Codepoint(0x1D734, false)),
				new Range<Codepoint>(new Codepoint(0x1D756, false), new Codepoint(0x1D76E, false)),
				new Range<Codepoint>(new Codepoint(0x1D790, false), new Codepoint(0x1D7A8, false)),
				new Range<Codepoint>(new Codepoint(0x1D7CA, false), new Codepoint(0x1D7CA, false)),
				new Range<Codepoint>(new Codepoint(0x1E900, false), new Codepoint(0x1E921, false)),
				new Range<Codepoint>(new Codepoint(0x1F130, false), new Codepoint(0x1F149, false)),
				new Range<Codepoint>(new Codepoint(0x1F150, false), new Codepoint(0x1F169, false)),
				new Range<Codepoint>(new Codepoint(0x1F170, false), new Codepoint(0x1F189, false))
		});

		/// <summary>The EOF placeholder codepoint.</summary>
		/// <remarks>This is not a "valid" codepoint. It is one of the two reserved characters, and matches the <see cref="Utf16Chars.EOF"/> value.</remarks>
		public static readonly Codepoint EOF = new Codepoint(0xFFFF, false);

		/// <summary>The right-to-left mark.</summary>
		public static readonly Codepoint RLM = new Codepoint(0x200F, false);

		/// <summary>The left-to-right mark.</summary>
		public static readonly Codepoint LRM = new Codepoint(0x200E, false);

		/// <summary>ZWJ is used to combine multiple emoji codepoints into a single emoji symbol.</summary>
		public static readonly Codepoint ZWJ = new Codepoint(0x200D, false);

		/// <summary>ORC is used as a placeholder to indicate an object should replace this codepoint in the string.</summary>
		public static readonly Codepoint ObjectReplacementCharacter = new Codepoint(0xFFFC, false);

		/// <summary>
		/// 	ReplacementCharacter is the general substitute character in the Unicode Standard.It can be substituted for any “unknown” character in another encoding that cannot be mapped in terms of
		/// 	known Unicode characters.
		/// </summary>
		public static readonly Codepoint ReplacementCharacter = new Codepoint(0xFFFD, false);

		/// <summary>The "combined enclosing keycap" is used by emoji to box icons.</summary>
		public static readonly Codepoint Keycap = new Codepoint(0x20E3, false);
	}
}
