using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sirius {
	public sealed class SymbolIdSequence: IReadOnlyList<SymbolId>, IEquatable<SymbolIdSequence> {
		public static implicit operator SymbolIdSequence(SymbolId[] symbols) {
			return new SymbolIdSequence(symbols);
		}

		private readonly int hash;
		private readonly IReadOnlyList<SymbolId> symbolIds;

		public SymbolIdSequence(IEnumerable<SymbolId> symbols) {
			this.symbolIds = symbols.ToArray();
			this.hash = this.symbolIds.Aggregate(23, (agg, id) => unchecked(agg * 397+id.GetHashCode()));
		}

		public bool Equals(SymbolIdSequence other) {
			if (ReferenceEquals(other, null)) {
				return false;
			}
			if (ReferenceEquals(this, other)) {
				return true;
			}
			return (this.hash == other.hash) && this.symbolIds.SequenceEqual(other.symbolIds);
		}

		public int Count => this.symbolIds.Count;

		public IEnumerator<SymbolId> GetEnumerator() {
			return this.symbolIds.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public SymbolId this[int index] => this.symbolIds[index];

		public override bool Equals(object obj) {
			return Equals(obj as SymbolIdSequence);
		}

		public override int GetHashCode() {
			return this.hash;
		}

		public override string ToString() {
			return ToString(id => id.ToString());
		}

		public string ToString(Func<SymbolId, string> resolver) {
			return string.Join(" ", this.symbolIds.Select(resolver));
		}
	}
}
