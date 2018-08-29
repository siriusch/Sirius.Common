using System;

namespace Sirius {
	public struct SymbolId: IEquatable<SymbolId> {
		public static implicit operator SymbolId(int id) => new SymbolId(id);

		public static explicit operator int(SymbolId id) => id.id;

		public static bool operator ==(SymbolId left, SymbolId right) {
			return left.id == right.id;
		}

		public static bool operator !=(SymbolId left, SymbolId right) {
			return left.id != right.id;
		}

		public static bool operator ==(int left, SymbolId right) {
			return left == right.id;
		}

		public static bool operator !=(int left, SymbolId right) {
			return left != right.id;
		}

		public static bool operator ==(SymbolId left, int right) {
			return left.id == right;
		}

		public static bool operator !=(SymbolId left, int right) {
			return left.id != right;
		}

		public const int Eof = int.MinValue;
		public const int Accept = int.MinValue + 1;
		public const int Reject = int.MinValue + 2;

		private readonly int id;

		public SymbolId(int id) {
			this.id = id;
		}

		public bool Equals(SymbolId other) {
			return this.id == other.id;
		}

		public override bool Equals(object obj) {
			return obj is SymbolId && this.Equals((SymbolId)obj);
		}

		public override int GetHashCode() {
			return this.id;
		}

		public int ToInt32() {
			return this.id;
		}

		public override string ToString() {
			return this.ToString(null);
		}

		public string ToString(Func<SymbolId, string> resolver) {
			var result = resolver?.Invoke(this);
			if (result != null) {
				return result;
			}
			switch (this.id) {
			case Accept:
				return "(Accept)";
			case Reject:
				return "(Reject)";
			case Eof:
				return "(Eof)";
			default:
				return $"Symbol:{this.id}";
			}
		}
	}
}
