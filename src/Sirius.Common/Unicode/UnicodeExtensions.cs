using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sirius.Collections;

namespace Sirius.Unicode {
	public static class UnicodeExtensions {
		private static bool TryCaseExpand(IEnumerable<Codepoint> letters, out ICollection<Codepoint> expanded) {
			var otherCase = new HashSet<Codepoint>();
			var allLetters = new HashSet<Codepoint>(letters);
			foreach (var c in allLetters) {
				if (Codepoint.IsUpper(c)) {
					otherCase.Add(Codepoint.ToLowerInvariant(c));
				} else if (Codepoint.IsLower(c)) {
					otherCase.Add(Codepoint.ToUpperInvariant(c));
				}
			}
			if (otherCase.All(allLetters.Contains)) {
				expanded = null;
				return false;
			}
			allLetters.UnionWith(otherCase);
			expanded = allLetters;
			return true;
		}

		public static RangeSet<Codepoint> CaseInsensitive(this RangeSet<Codepoint> input) {
			ICollection<Codepoint> output;
			if (TryCaseExpand(input.Expand(), out output)) {
				return new RangeSet<Codepoint>(output);
			}
			return input;
		}

		public static IEnumerable<char> ToChars(this IEnumerable<Codepoint> that) {
			foreach (var codepoint in that) {
				if (codepoint.FitsIntoChar) {
					yield return (char)codepoint;
				} else {
					yield return codepoint.GetHighSurrogate();
					yield return codepoint.GetLowSurrogate();
				}
			}
		}

		public static IEnumerable<byte> ToUtf8Bytes(this IEnumerable<Codepoint> that) {
			foreach (var codepoint in that.Select(c => c.ToInt32())) {
				if (codepoint <= 0x0000007F) {
					yield return (byte)codepoint;
				} else if (codepoint <= 0x000007FF) {
					yield return (byte)((codepoint>>6)|0xC0);
					yield return (byte)((codepoint&0x3F)|0x80);
				} else if (codepoint <= 0x0000FFFF) {
					yield return (byte)((codepoint>>12)|0xE0);
					yield return (byte)(((codepoint>>6)&0x3F)|0x80);
					yield return (byte)((codepoint&0x3F)|0x80);
				} else if (codepoint <= 0x0010FFFF) {
					yield return (byte)((codepoint>>18)|0xF0);
					yield return (byte)(((codepoint>>12)&0x3F)|0x80);
					yield return (byte)(((codepoint>>6)&0x3F)|0x80);
					yield return (byte)((codepoint&0x3F)|0x80);
				} else {
					throw new UnsupportedCodepointException();
				}
			}
		}

		public static string AsString(this IEnumerable<char> that) {
			var builder = new StringBuilder();
			foreach (var ch in that) {
				builder.Append(ch);
			}
			return builder.ToString();
		}

		public static string AsString(this IEnumerable<Codepoint> that) {
			return that.ToChars().AsString();
		}

		public static IEnumerable<Codepoint> ToCodepoints(this IEnumerable<char> that) {
			return Codepoint.FromCharsMany(that);
		}

		public static IEnumerable<Grapheme> ToGraphemes(this IEnumerable<char> that) {
			return Grapheme.FromCharsMany(that);
		}
	}
}
