using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Sirius {
	/// <summary>A symbol identifier.</summary>
	/// <remarks>This type is immutable.</remarks>
	public struct SymbolId: IEquatable<SymbolId> {
		/// <summary>Implicit cast that converts the given int to a SymbolId.</summary>
		/// <param name="id">The identifier.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static implicit operator SymbolId(int id) => new SymbolId(id);

		/// <summary>Explicit cast that converts the given SymbolId to an int.</summary>
		/// <param name="id">The identifier.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static explicit operator int(SymbolId id) => id.id;

		/// <summary>Equality operator.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator ==(SymbolId left, SymbolId right) {
			return left.id == right.id;
		}

		/// <summary>Inequality operator.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator !=(SymbolId left, SymbolId right) {
			return left.id != right.id;
		}

		/// <summary>Equality operator.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator ==(int left, SymbolId right) {
			return left == right.id;
		}

		/// <summary>Inequality operator.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator !=(int left, SymbolId right) {
			return left != right.id;
		}

		/// <summary>Equality operator.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator ==(SymbolId left, int right) {
			return left.id == right;
		}

		/// <summary>Inequality operator.</summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator !=(SymbolId left, int right) {
			return left.id != right;
		}

		/// <summary>The default End-of-File symbol.</summary>
		public const int Eof = int.MinValue;
		/// <summary>The default accept symbol.</summary>
		public const int Accept = int.MinValue + 1;
		/// <summary>The default reject symbol.</summary>
		public const int Reject = int.MinValue + 2;

		private readonly int id;

		/// <summary>Constructor.</summary>
		/// <param name="id">The identifier.</param>
		public SymbolId(int id) {
			this.id = id;
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public bool Equals(SymbolId other) {
			return this.id == other.id;
		}

		/// <summary>Indicates whether this instance and a specified object are equal.</summary>
		/// <param name="obj">The object to compare with the current instance.</param>
		/// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public override bool Equals(object obj) {
			return obj is SymbolId other && this.Equals(other);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public override int GetHashCode() {
			return this.id;
		}

		/// <summary>Converts this SymbolId to an int 32.</summary>
		/// <returns>This SymbolId as an int.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public int ToInt32() {
			return this.id;
		}

		/// <summary>Returns a textual representation of the id.</summary>
		/// <returns>A textual representation of the id.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public override string ToString() {
			return this.ToString(null);
		}

		/// <summary>Returns a textual representation of the id.</summary>
		/// <param name="resolver">The resolver to use to convert the symbol to a name.</param>
		/// <returns>A textual representation of the id.</returns>
		[Pure]
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
