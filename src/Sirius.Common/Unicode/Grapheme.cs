using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sirius.Unicode {
	/// <summary>A grapheme is the smallest part of the writing system. In terms of Unicode, the grapheme is a cluster of a non-combining codepoint followed by any number of combining codepoints which may change the character (adding accents for instance). The grapheme is typically what the user expects to see as one "character" on screen.</summary>
	/// <remarks>This struct is immutable.</remarks>
	public struct Grapheme: IComparable<Grapheme>, IEquatable<Grapheme>, IEnumerable<Codepoint> {
		/// <summary>Equality operator.</summary>
		/// <param name="x">A Grapheme to compare.</param>
		/// <param name="y">A Grapheme to compare.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator ==(Grapheme x, Grapheme y) {
			return x.Equals(y);
		}

		/// <summary>Inequality operator.</summary>
		/// <param name="x">A Grapheme to process.</param>
		/// <param name="y">A Grapheme to process.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator !=(Grapheme x, Grapheme y) {
			return !x.Equals(y);
		}

		/// <summary>Less-than comparison operator.</summary>
		/// <param name="x">A Grapheme to process.</param>
		/// <param name="y">A Grapheme to process.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator <(Grapheme x, Grapheme y) {
			return x.CompareTo(y) < 0;
		}

		/// <summary>Greater-than comparison operator.</summary>
		/// <param name="x">A Grapheme to process.</param>
		/// <param name="y">A Grapheme to process.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator >(Grapheme x, Grapheme y) {
			return x.CompareTo(y) > 0;
		}

		/// <summary>Greater-than-or-equal comparison operator.</summary>
		/// <param name="x">A Grapheme to process.</param>
		/// <param name="y">A Grapheme to process.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator >=(Grapheme x, Grapheme y) {
			return x.CompareTo(y) >= 0;
		}

		/// <summary>Less-than-or-equal comparison operator.</summary>
		/// <param name="x">A Grapheme to process.</param>
		/// <param name="y">A Grapheme to process.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator <=(Grapheme x, Grapheme y) {
			return x.CompareTo(y) <= 0;
		}

		/// <summary>Implicit cast that converts the given Codepoint to a Grapheme.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operation.</returns>
		[Pure]
		public static implicit operator Grapheme(Codepoint value) {
			if (!Codepoint.IsValid(value)) {
				throw new ArgumentException("Cannot convert a surrogate to a grapheme", nameof(value));
			}
			if (Codepoint.IsCombiningMark((int)value)) {
				throw new ArgumentException("Cannot convert a combining mark to a grapheme", nameof(value));
			}
			// ReSharper disable once SpecifyACultureInStringConversionExplicitly
			return new Grapheme(value.ToString(), false);
		}

		/// <summary>Implicit cast that converts the given char to a Grapheme.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operation.</returns>
		[Pure]
		public static implicit operator Grapheme(char value) {
			if (char.IsSurrogate(value)) {
				throw new ArgumentException("Cannot convert a surrogate to a grapheme", nameof(value));
			}
			if (Codepoint.IsCombiningMark((int)value)) {
				throw new ArgumentException("Cannot convert a combining mark to a grapheme", nameof(value));
			}
			return new Grapheme(value.ToString(), false);
		}

		/// <summary>Converts an enumeration of UTF-16 chars to an enumeration of graphemes.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="chars">The input characters.</param>
		/// <returns>Graphemes based on the input characters.</returns>
		[Pure]
		public static IEnumerable<Grapheme> FromCharsMany(IEnumerable<char> chars) {
			using (var enumerator = chars.GetEnumerator()) {
				if (!enumerator.MoveNext()) {
					yield break;
				}
				var builder = new StringBuilder(16);
				do {
					var ch = enumerator.Current;
					if (Codepoint.IsCombiningMark((int)ch)) {
						if (builder.Length == 0) {
							throw new ArgumentException("A grapheme must start with a non-combining character", nameof(value));
						}
						builder.Append(ch);
					} else {
						if (builder.Length > 0) {
							yield return new Grapheme(builder.ToString(), false);
							builder.Clear();
						}
						builder.Append(ch);
						if (char.IsHighSurrogate(ch)) {
							if (!enumerator.MoveNext() || !char.IsLowSurrogate(enumerator.Current)) {
								throw new ArgumentException("A low surrogate must always follow a high surrogate", nameof(value));
							}
							builder.Append(enumerator.Current);
						} else if (char.IsLowSurrogate(ch)) {
							throw new ArgumentException("A low surrogate is not valid without a preceding high surrogate", nameof(value));
						}
					}
				} while (enumerator.MoveNext());
				if (builder.Length > 0) {
					yield return new Grapheme(builder.ToString(), false);
				}
			}
		}

		/// <summary>Converts an enumeration of UTF-16 chars to a single grapheme.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="chars">The input characters.</param>
		/// <returns>Grapheme based on the input characters.</returns>
		[Pure]
		public static Grapheme FromChars(IEnumerable<char> chars) {
			if (chars == null) {
				throw new ArgumentNullException(nameof(chars));
			}
			return new Grapheme(chars.AsString(), true);
		}

		/// <summary>Converts an enumeration of Unicode codepoints to a single grapheme.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="codepoints">The input codepoints.</param>
		/// <returns>Grapheme based on the input codepoints.</returns>
		[Pure]
		public static Grapheme FromCodepoints(IEnumerable<Codepoint> codepoints) {
			if (codepoints == null) {
				throw new ArgumentNullException(nameof(codepoints));
			}
			return new Grapheme(codepoints.AsString(), true);
		}

		/// <summary>Converts an enumeration of Unicode codepoints to an enumeration of graphemes.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="codepoints">The input codepoints.</param>
		/// <returns>Graphemes based on the input codepoints.</returns>
		[Pure]
		public static IEnumerable<Grapheme> FromCodepointsMany(IEnumerable<Codepoint> codepoints) {
			if (codepoints == null) {
				throw new ArgumentNullException(nameof(codepoints));
			}
			return FromCharsMany(codepoints.ToChars());
		}

		private readonly string value;

		/// <summary>Create a grapheme from a UTF-16 string.</summary>
		/// <param name="value">The value of the grapheme.</param>
		public Grapheme(string value): this(value, true) { }

		private Grapheme(string value, bool check) {
			if (check) {
				using (var enumerator = value.GetEnumerator()) {
					if (!enumerator.MoveNext()) {
						throw new ArgumentException("An empty string is not a valid grapheme", nameof(value));
					}
					if (Codepoint.IsCombiningMark(Codepoint.FromCharEnumerator(enumerator))) {
						throw new ArgumentException("A grapheme cannot start with a combining character", nameof(value));
					}
					while (enumerator.MoveNext()) {
						if (!Codepoint.IsCombiningMark(Codepoint.FromCharEnumerator(enumerator))) {
							throw new ArgumentException("A grapheme cannot contain more than one non-combining character", nameof(value));
						}
					}
				}
			}
			this.value = value;
		}

		internal string Value => this.value ?? "\0";

		/// <summary>Gets a value indicating whether this Grapheme is combined (e.g. has trailing combining characters).</summary>
		/// <value><c>true</c> if this Grapheme is combined, <c>false</c> if not.</value>
		public bool IsCombined => this.Value.Length > (char.IsHighSurrogate(this.Value[0]) ? 2 : 1);

		/// <summary>
		/// 	Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in
		/// 	the sort order as the other object.
		/// </summary>
		/// <param name="other">An object to compare with this instance.</param>
		/// <returns>
		/// 	A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" />
		/// 	in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the
		/// 	sort order.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public int CompareTo(Grapheme other) {
			return StringComparer.InvariantCulture.Compare(this.Value, other.Value);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public bool Equals(Grapheme other) {
			return StringComparer.InvariantCulture.Equals(this.Value, other.Value);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public override int GetHashCode() {
			return typeof(Grapheme).GetHashCode()^StringComparer.InvariantCulture.GetHashCode(this.Value);
		}

		/// <summary>Indicates whether this instance and a specified object are equal.</summary>
		/// <param name="obj">The object to compare with the current instance.</param>
		/// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public override bool Equals(object obj) {
			return obj is Grapheme && Equals((Grapheme)obj);
		}

		/// <summary>Normalizes the grapheme to the given form.</summary>
		/// <param name="form">The normalization form.</param>
		/// <returns>A Grapheme.</returns>
		/// <seealso cref="string.Normalize(System.Text.NormalizationForm)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public Grapheme Normalize(NormalizationForm form) {
			return CreateIfDifferent(this.Value.Normalize(form));
		}

		/// <summary>Query if the grapheme matches a specific normalization form.</summary>
		/// <param name="form">The normalization form.</param>
		/// <returns><c>true</c> if normalized, <c>false</c> if not.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public bool IsNormalized(NormalizationForm form) {
			return this.Value.IsNormalized(form);
		}

		/// <summary>Converts this Grapheme to upper-case.</summary>
		/// <returns>A Grapheme.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public Grapheme ToUpperInvariant() {
			return CreateIfDifferent(this.Value.ToUpperInvariant());
		}

		/// <summary>Converts this Grapheme to lower-case.</summary>
		/// <returns>A Grapheme.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public Grapheme ToLowerInvariant() {
			return CreateIfDifferent(this.Value.ToLowerInvariant());
		}

		[Pure]
		private Grapheme CreateIfDifferent(string value) {
			if (StringComparer.InvariantCulture.Equals(value, this.Value)) {
				return this;
			}
			return new Grapheme(value, false);
		}

		/// <summary>Returns an enumerator that iterates through the codepoints of the grapheme.</summary>
		/// <returns>An enumerator that can be used to iterate through the collection.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public IEnumerator<Codepoint> GetEnumerator() {
			return this.Value.ToCodepoints().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		/// <summary>Returns the characters of the grapheme as string.</summary>
		/// <returns>The grapheme characters.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public override string ToString() {
			return this.Value;
		}
	}
}
