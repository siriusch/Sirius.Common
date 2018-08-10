using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Sirius.Unicode {
	public struct Grapheme: IComparable<Grapheme>, IEquatable<Grapheme>, IEnumerable<Codepoint> {
		public static bool operator ==(Grapheme x, Grapheme y) {
			return x.Equals(y);
		}

		public static bool operator !=(Grapheme x, Grapheme y) {
			return !x.Equals(y);
		}

		public static bool operator <(Grapheme x, Grapheme y) {
			return x.CompareTo(y) < 0;
		}

		public static bool operator >(Grapheme x, Grapheme y) {
			return x.CompareTo(y) > 0;
		}

		public static bool operator >=(Grapheme x, Grapheme y) {
			return x.CompareTo(y) >= 0;
		}

		public static bool operator <=(Grapheme x, Grapheme y) {
			return x.CompareTo(y) <= 0;
		}

		public static implicit operator Grapheme(char value) {
			if (char.IsSurrogate(value)) {
				throw new ArgumentException("Cannot convert a surrogate to a grapheme", nameof(value));
			}
			if (Codepoint.IsCombiningMark((int)value)) {
				throw new ArgumentException("Cannot convert a combining mark to a grapheme", nameof(value));
			}
			return new Grapheme(value.ToString(), false);
		}

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
							throw new ArgumentException("A grapheme cannot contain more than one non-combining character", nameof(value));
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
								throw new ArgumentException("A low surrogate must follow a high surrogate", nameof(value));
							}
							builder.Append(enumerator.Current);
						} else if (char.IsLowSurrogate(ch)) {
							throw new ArgumentException("A low surrogate must follow a high surrogate", nameof(value));
						}
					}
				} while (enumerator.MoveNext());
				if (builder.Length > 0) {
					yield return new Grapheme(builder.ToString(), false);
				}
			}
		}

		public static Grapheme FromChars(IEnumerable<char> chars) {
			if (chars == null) {
				throw new ArgumentNullException(nameof(chars));
			}
			return new Grapheme(chars.AsString(), true);
		}

		private readonly string value;

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

		public Grapheme(IEnumerable<Codepoint> codepoints): this(codepoints.AsString(), true) { }

		internal string Value => this.value ?? "\0";

		public bool IsCombined => this.Value.Length > (char.IsHighSurrogate(this.Value[0]) ? 2 : 1);

		public int CompareTo(Grapheme other) {
			return StringComparer.Ordinal.Compare(this.Value, other.Value);
		}

		public bool Equals(Grapheme other) {
			return StringComparer.Ordinal.Equals(this.Value, other.Value);
		}

		public override int GetHashCode() {
			return typeof(Grapheme).GetHashCode()^StringComparer.Ordinal.GetHashCode(this.Value);
		}

		public override bool Equals(object obj) {
			return obj is Grapheme && Equals((Grapheme)obj);
		}

		public Grapheme Normalize(NormalizationForm form) {
			return CreateIfDifferent(this.Value.Normalize(form));
		}

		public bool IsNormalized(NormalizationForm form) {
			return this.Value.IsNormalized(form);
		}

		public Grapheme ToUpperInvariant() {
			return CreateIfDifferent(this.Value.ToUpperInvariant());
		}

		public Grapheme ToLowerInvariant() {
			return CreateIfDifferent(this.Value.ToLowerInvariant());
		}

		private Grapheme CreateIfDifferent(string value) {
			if (StringComparer.Ordinal.Equals(value, this.Value)) {
				return this;
			}
			return new Grapheme(value, false);
		}

		public IEnumerator<Codepoint> GetEnumerator() {
			return this.Value.ToCodepoints().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public override string ToString() {
			return this.Value;
		}
	}
}
