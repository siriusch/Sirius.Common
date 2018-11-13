using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using JetBrains.Annotations;

using Sirius.Collections;

namespace Sirius.Unicode {
	/// <summary>Unicode extension methods.</summary>
	public static class UnicodeExtensions {
		/// <summary>A <see cref="RangeSet{T}"/> extension method to invariantly expand all cased characters to both lower and uppercase.</summary>
		/// <param name="input">The input codepoints set.</param>
		/// <returns>A range set containing lower- and upper-case variants of each codepoint in the input set.</returns>
		[Pure]
		public static RangeSet<Codepoint> CaseInsensitive(this RangeSet<Codepoint> input) {
			var result = input;
			foreach (var c in input.Expand()) {
				if (Codepoint.IsUpper(c)) {
					result |= Codepoint.ToLowerInvariant(c);
				} else if (Codepoint.IsLower(c)) {
					result |= Codepoint.ToUpperInvariant(c);
				}
			}
			return result;
		}

		/// <summary>Converts an enumeration of codepoints to an enumeration of UTF-16 characters.</summary>
		/// <param name="that">The codepoints.</param>
		/// <returns>The UTF-16 characters.</returns>
		[Pure]
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

		/// <summary>Converts an enumeration of codepoints to an enumeration of UTF-8 bytes.</summary>
		/// <param name="that">The codepoints.</param>
		/// <returns>The UTF-8 bytes.</returns>
		[Pure]
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

		/// <summary>A StringBuilder extension method that appends a codepoint.</summary>
		/// <param name="that">The string builder.</param>
		/// <param name="codepoint">The codepoint.</param>
		/// <returns>The <paramref name="that"/> string builder.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static StringBuilder Append(this StringBuilder that, Codepoint codepoint) {
			codepoint.AppendTo(that);
			return that;
		}

		/// <summary>A StringBuilder extension method that appends an enumeration of codepoints.</summary>
		/// <param name="that">The string builder.</param>
		/// <param name="codepoints">The codepoints.</param>
		/// <returns>The <paramref name="that"/> string builder.</returns>
		[Pure]
		public static StringBuilder Append(this StringBuilder that, IEnumerable<Codepoint> codepoints) {
			foreach (var codepoint in codepoints) {
				codepoint.AppendTo(that);
			}
			return that;
		}

		/// <summary>A <see cref="IEnumerable{T}"/> extension method that converts characters to a string.</summary>
		/// <exception cref="ArgumentNullException">Thrown when one or more required arguments are <c>null</c></exception>
		/// <param name="that">The characters.</param>
		/// <returns>A string.</returns>
		[Pure]
		public static string AsString([NotNull] this IEnumerable<char> that) {
			if (that == null) {
				throw new ArgumentNullException(nameof(that));
			}
			if (that is string str) {
				return str;
			}
			var builder = new StringBuilder((that as ICollection<char>)?.Count ?? 120);
			foreach (var ch in that) {
				builder.Append(ch);
			}
			return builder.ToString();
		}

		/// <summary>A <see cref="IEnumerable{T}"/> extension method that converts codepoints to a string.</summary>
		/// <exception cref="ArgumentNullException">Thrown when one or more required arguments are <c>null</c></exception>
		/// <param name="that">The codepoints.</param>
		/// <returns>A string.</returns>
		[Pure]
		public static string AsString([NotNull] this IEnumerable<Codepoint> that) {
			if (that == null) {
				throw new ArgumentNullException(nameof(that));
			}
			var builder = new StringBuilder((that as ICollection<Codepoint>)?.Count ?? 120);
			foreach (var ch in that) {
				ch.AppendTo(builder);
			}
			return builder.ToString();
		}

		/// <summary>Convert an enumeration of UTF-16 <see cref="char"/> to an enumeration of <see cref="Codepoint"/>.</summary>
		/// <param name="that">The input characters.</param>
		/// <returns>The codepoints.</returns>
		[Pure]
		public static IEnumerable<Codepoint> ToCodepoints(this IEnumerable<char> that) {
			return Codepoint.FromCharsMany(that);
		}

		/// <summary>Convert an enumeration of UTF-16 <see cref="char"/> to an enumeration of <see cref="Grapheme"/>.</summary>
		/// <param name="that">The input characters.</param>
		/// <returns>The graphemes.</returns>
		[Pure]
		public static IEnumerable<Grapheme> ToGraphemes(this IEnumerable<char> that) {
			return Grapheme.FromCharsMany(that);
		}

		/// <summary>Convert an enumeration of <see cref="Codepoint"/> to an enumeration of <see cref="Grapheme"/>.</summary>
		/// <param name="that">The input codepoints.</param>
		/// <returns>The graphemes.</returns>
		[Pure]
		public static IEnumerable<Grapheme> ToGraphemes(this IEnumerable<Codepoint> that) {
			return Grapheme.FromCodepointsMany(that);
		}

		[Pure]
		internal static string ProcessAllPlanes([NotNull] this IEnumerable<char> that, Func<char, char> processChar, Func<Codepoint, Codepoint> processCodepoint) {
			if (that == null) {
				throw new ArgumentNullException(nameof(that));
			}
			var result = new StringBuilder((that as string)?.Length ?? (that as ICollection<char>)?.Count ?? 128);
			using (var enumerator = that.GetEnumerator()) {
				while (enumerator.MoveNext()) {
					if (char.IsSurrogate(enumerator.Current)) {
						processCodepoint(Codepoint.FromCharEnumerator(enumerator)).AppendTo(result);
					} else {
						result.Append(processChar(enumerator.Current));
					}
				}
			}
			return result.ToString();
		}
	}
}
