using System;

namespace Sirius {
	public struct SymbolId: IEquatable<SymbolId> {
		public static implicit operator SymbolId(int id) => new SymbolId(id);

		public static explicit operator int(SymbolId id) => id.id;

		public static bool operator ==(SymbolId left, SymbolId right) {
			return left.Equals(right);
		}

		public static bool operator !=(SymbolId left, SymbolId right) {
			return !left.Equals(right);
		}

		private const int EofId = int.MinValue;
		private const int AcceptId = int.MinValue+1;
		private const int RejectId = int.MinValue+2;

		public static SymbolId Eof = new SymbolId(EofId);
		public static SymbolId Accept = new SymbolId(AcceptId);
		public static SymbolId Reject = new SymbolId(RejectId);

		private readonly int id;

		public SymbolId(int id) {
			this.id = id;
		}

		public bool Equals(SymbolId other) {
			return this.id == other.id;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) {
				return false;
			}
			return obj is SymbolId && Equals((SymbolId)obj);
		}

		public override int GetHashCode() {
			return this.id;
		}

		public override string ToString() {
			return ToString(null);
		}

		public string ToString(Func<SymbolId, string> resolver) {
			var result = resolver?.Invoke(this);
			if (result != null) {
				return result;
			}
			switch (this.id) {
			case AcceptId:
				return "(Accept)";
			case RejectId:
				return "(Reject)";
			case EofId:
				return "(Eof)";
			default:
				return $"Symbol:{this.id}";
			}
		}
	}
}
