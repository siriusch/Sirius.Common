using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sirius {
	/// <summary>A symbol identifier sequence. This class cannot be inherited.</summary>
	/// <remarks>This immutable class can be used as key in sets or dictionaries.</remarks>
	public sealed class SymbolIdSequence: IReadOnlyList<SymbolId>, IEquatable<SymbolIdSequence> {
		/// <summary>Implicit cast that converts the given SymbolId[] to a SymbolIdSequence.</summary>
		/// <param name="symbols">The symbols.</param>
		/// <returns>The result of the operation.</returns>
		public static implicit operator SymbolIdSequence(SymbolId[] symbols) {
			return new SymbolIdSequence(symbols);
		}

		private readonly int hash;
		private readonly IReadOnlyList<SymbolId> symbolIds;

		/// <summary>Constructor.</summary>
		/// <param name="symbols">The symbols.</param>
		public SymbolIdSequence(IEnumerable<SymbolId> symbols) {
			this.symbolIds = symbols.ToArray();
			this.hash = this.symbolIds.Aggregate(23, (agg, id) => unchecked(agg * 397+id.GetHashCode()));
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
		public bool Equals(SymbolIdSequence other) {
			if (ReferenceEquals(other, null)) {
				return false;
			}
			if (ReferenceEquals(this, other)) {
				return true;
			}
			return (this.hash == other.hash) && this.symbolIds.SequenceEqual(other.symbolIds);
		}

		/// <summary>Gets the number of symbols.</summary>
		/// <value>The symbol count.</value>
		public int Count => this.symbolIds.Count;

		/// <summary>Gets the enumerator.</summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<SymbolId> GetEnumerator() {
			return this.symbolIds.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		/// <summary>Gets the symbol at the specified index in the read-only list.</summary>
		/// <param name="index">The zero-based index of the symbol to get.</param>
		/// <returns>The symbol at the specified index.</returns>
		public SymbolId this[int index] => this.symbolIds[index];

		/// <summary>Determines whether the specified object is equal to the current object.</summary>
		/// <param name="obj">The object to compare with the current object.</param>
		/// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
		public override bool Equals(object obj) {
			return Equals(obj as SymbolIdSequence);
		}

		/// <summary>Serves as the default hash function.</summary>
		/// <returns>A hash code for the current object.</returns>
		public override int GetHashCode() {
			return this.hash;
		}

		/// <summary>Returns a string that represents the current object.</summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString() {
			return ToString(id => id.ToString());
		}

		/// <summary>Returns a string that represents the current object.</summary>
		/// <param name="resolver">The resolver.</param>
		/// <returns>A string that represents the current object.</returns>
		public string ToString(Func<SymbolId, string> resolver) {
			return string.Join(" ", this.symbolIds.Select(resolver));
		}
	}
}
