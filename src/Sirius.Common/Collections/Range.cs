using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Sirius.Collections {
	/// <summary>A generic range (<see cref="From"/>..<see cref="To"/>).</summary>
	/// <typeparam name="T">Generic type parameter, must implement <see cref="IComparable{T}"/>.</typeparam>
	/// <remarks>This struct is immutable.</remarks>
	public struct Range<T>: IEquatable<Range<T>>
			where T: IComparable<T> {
		private static readonly Lazy<Range<T>> all = new Lazy<Range<T>>(() => new Range<T>(Incrementor<T>.MinValue, Incrementor<T>.MaxValue));

		/// <summary>Gets a range representing all items of the type <typeparamref name="T"/>.</summary>
		/// <value>A range.</value>
		public static Range<T> All => all.Value;

		/// <summary>Range complement operator.</summary>
		/// <param name="range">The range.</param>
		/// <returns>A <see cref="RangeSet{T}"/> representing the complement of the input <paramref name="range"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator ~(Range<T> range) {
			return RangeOperations<T>.Negate(new RangeSet<T>(range));
		}

		/// <summary>Range equality operator.</summary>
		/// <param name="left">The left range.</param>
		/// <param name="right">The right range.</param>
		/// <returns><c>true</c> if the ranges are equal, <c>false</c> otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator ==(Range<T> left, Range<T> right) {
			return left.Equals(right);
		}

		/// <summary>Range inequality operator.</summary>
		/// <param name="left">The left range.</param>
		/// <param name="right">The right range.</param>
		/// <returns><c>false</c> if the ranges are equal, <c>true</c> otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator !=(Range<T> left, Range<T> right) {
			return !left.Equals(right);
		}

		/// <summary>Range subtraction operator.</summary>
		/// <param name="left">The left range.</param>
		/// <param name="right">The right range.</param>
		/// <returns>A <see cref="RangeSet{T}"/> representing the <paramref name="left"/> range minus the <paramref name="right"/> range.</returns>
		/// <remarks>Unlike the other range operators, this operator is not commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator -(Range<T> left, Range<T> right) {
			return RangeOperations<T>.Subtract(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		/// <summary>Range union operator.</summary>
		/// <param name="left">The left range.</param>
		/// <param name="right">The right range.</param>
		/// <returns>A <see cref="RangeSet{T}"/> representing the union of the <paramref name="left"/> and the <paramref name="right"/> ranges.</returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator |(Range<T> left, Range<T> right) {
			return RangeOperations<T>.Union(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		/// <summary>Range intersection operator.</summary>
		/// <param name="left">The left range.</param>
		/// <param name="right">The right range.</param>
		/// <returns>A <see cref="RangeSet{T}"/> representing the intersection of the <paramref name="left"/> and the <paramref name="right"/> ranges.</returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator &(Range<T> left, Range<T> right) {
			return RangeOperations<T>.Slice(new RangeSet<T>(left), right);
		}

		/// <summary>Range difference operator.</summary>
		/// <param name="left">The left range.</param>
		/// <param name="right">The right range.</param>
		/// <returns>A <see cref="RangeSet{T}"/> representing the difference of the <paramref name="left"/> and the <paramref name="right"/> ranges (e.g. items which are only in one of the ranges).</returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator ^(Range<T> left, Range<T> right) {
			return RangeOperations<T>.Difference(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		/// <summary>Range subtraction operator.</summary>
		/// <param name="left">The left range.</param>
		/// <param name="right">The right range.</param>
		/// <returns>A <see cref="RangeSet{T}"/> representing the <paramref name="left"/> range minus the <paramref name="right"/> range.</returns>
		/// <remarks>Unlike the other range operators, this operator is not commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator -(T left, Range<T> right) {
			return RangeOperations<T>.Subtract(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		/// <summary>Range union operator.</summary>
		/// <param name="left">The left range.</param>
		/// <param name="right">The right range.</param>
		/// <returns>A <see cref="RangeSet{T}"/> representing the union of the <paramref name="left"/> and the <paramref name="right"/> ranges.</returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator |(T left, Range<T> right) {
			return RangeOperations<T>.Union(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		/// <summary>Range intersection operator.</summary>
		/// <param name="left">The left range.</param>
		/// <param name="right">The right range.</param>
		/// <returns>A <see cref="RangeSet{T}"/> representing the intersection of the <paramref name="left"/> and the <paramref name="right"/> ranges.</returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator &(T left, Range<T> right) {
			return right.Contains(left) ? new RangeSet<T>(left) : RangeSet<T>.Empty;
		}

		/// <summary>Range difference operator.</summary>
		/// <param name="left">The left range.</param>
		/// <param name="right">The right range.</param>
		/// <returns>A <see cref="RangeSet{T}"/> representing the difference of the <paramref name="left"/> and the <paramref name="right"/> ranges (e.g. items which are only in one of the ranges).</returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator ^(T left, Range<T> right) {
			return RangeOperations<T>.Difference(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		/// <summary>Range subtraction operator.</summary>
		/// <param name="left">The left range.</param>
		/// <param name="right">The right range.</param>
		/// <returns>A <see cref="RangeSet{T}"/> representing the <paramref name="left"/> range minus the <paramref name="right"/> range.</returns>
		/// <remarks>Unlike the other range operators, this operator is not commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator -(Range<T> left, T right) {
			return RangeOperations<T>.Subtract(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		/// <summary>Range union operator.</summary>
		/// <param name="left">The left range.</param>
		/// <param name="right">The right range.</param>
		/// <returns>A <see cref="RangeSet{T}"/> representing the union of the <paramref name="left"/> and the <paramref name="right"/> ranges.</returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator |(Range<T> left, T right) {
			return RangeOperations<T>.Union(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		/// <summary>Range intersection operator.</summary>
		/// <param name="left">The left range.</param>
		/// <param name="right">The right range.</param>
		/// <returns>A <see cref="RangeSet{T}"/> representing the intersection of the <paramref name="left"/> and the <paramref name="right"/> ranges.</returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator &(Range<T> left, T right) {
			return left.Contains(right) ? new RangeSet<T>(right) : RangeSet<T>.Empty;
		}

		/// <summary>Range difference operator.</summary>
		/// <param name="left">The left range.</param>
		/// <param name="right">The right range.</param>
		/// <returns>A <see cref="RangeSet{T}"/> representing the difference of the <paramref name="left"/> and the <paramref name="right"/> ranges (e.g. items which are only in one of the ranges).</returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator ^(Range<T> left, T right) {
			return RangeOperations<T>.Difference(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		/// <summary>Implicit cast that converts the given item to a <see cref="Range{T}"/>.</summary>
		/// <param name="single">The single item of the range.</param>
		/// <returns>The range with the single item.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static implicit operator Range<T>(T single) {
			return new Range<T>(single, single);
		}

		/// <summary>Creates a new <see cref="Range{T}"/> representing a single item.</summary>
		/// <param name="single">The single.</param>
		/// <returns>A <see cref="Range{T}"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static Range<T> Create(T single) {
			return Create(single, single);
		}

		/// <summary>Creates a new <see cref="Range{T}"/> representing the items between <paramref name="from"/> and <paramref name="to"/> (both inclusive).</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="from">The lower bound of the range (inclusive).</param>
		/// <param name="to">The upper bound of the range (inclusive).</param>
		/// <returns>A <see cref="Range{T}"/></returns>
		[Pure]
		public static Range<T> Create(T from, T to) {
			if (from.CompareTo(to) > 0) {
				throw new ArgumentException("'from <= to' expected");
			}
			return new Range<T>(from, to);
		}

		internal Range(T from, T to) {
			this.From = from;
			this.To = to;
		}

		/// <summary>Gets the lower bound of the range.</summary>
		/// <value>The lower bound of the range (inclusive).</value>
		public T From {
			get;
		}

		/// <summary>Gets the upper bound of the range.</summary>
		/// <value>The upper bound of the range (inclusive).</value>
		public T To {
			get;
		}

		/// <summary>Gets the number of items in this range.</summary>
		/// <remarks>A range has always a count of one or more since <see cref="From"/> and <see cref="To"/> are both inclusive.</remarks>
		[Pure]
		public int Count() {
			if (typeof(IConvertible).IsAssignableFrom(typeof(T))) {
				return (int)(((IConvertible)this.To).ToInt64(CultureInfo.InvariantCulture) - ((IConvertible)this.From).ToInt64(CultureInfo.InvariantCulture)) + 1;
			}
			var result = 1;
			var current = this.From;
			while (current.CompareTo(this.To) < 0) {
				result++;
				current = Incrementor<T>.Increment(current);
			}
			return result;
		}

		/// <summary>Checks whether the given item is in the range.</summary>
		/// <param name="key">The item to check.</param>
		/// <returns><c>true</c> if the item is in this range, <c>false</c> if not.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public bool Contains(T key) {
			return (this.From.CompareTo(key) <= 0) && (this.To.CompareTo(key) >= 0);
		}

		/// <summary>Checks whether the given range is in the range.</summary>
		/// <param name="range">The range to check.</param>
		/// <returns><c>true</c> if the given range is fully contained in this range, <c>false</c> if not.</returns>
		/// <seealso cref="IsContainedIn"/>
		/// <seealso cref="Intersects"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public bool Contains(Range<T> range) {
			return (this.From.CompareTo(range.From) <= 0) && (this.To.CompareTo(range.To) >= 0);
		}

		/// <summary>Checks whether the range is in the given range.</summary>
		/// <param name="range">The range to check.</param>
		/// <returns><c>true</c> if the given range full contains this range, <c>false</c> if not.</returns>
		/// <seealso cref="Contains(Range{T})"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public bool IsContainedIn(Range<T> range) {
			return range.Contains(this);
		}

		/// <summary>Checks whether the range intersects with the given range.</summary>
		/// <param name="range">The range to check.</param>
		/// <returns><c>true</c> if the given range intersects this range, <c>false</c> if not.</returns>
		/// <seealso cref="Contains(Range{T})"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public bool Intersects(Range<T> range) {
			return (this.From.CompareTo(range.To) <= 0) && (range.From.CompareTo(this.To) <= 0);
		}

		/// <summary>Expands the range to an enumeration of all the contained items.</summary>
		/// <returns>An enumeration of all items of this range.</returns>
		[Pure]
		public IEnumerable<T> Expand() {
			var increment = Incrementor<T>.Increment;
			var current = this.From;
			yield return current;
			while (current.CompareTo(this.To) != 0) {
				current = increment(current);
				yield return current;
			}
		}

		/// <summary>Returns a textual representation of the range.</summary>
		/// <returns>A <see cref="T:System.String" /> containing a textual representation of the range.</returns>
		[Pure]
		public override string ToString() {
			return this.From.CompareTo(this.To) == 0 ? this.From.ToString() : $"{this.From}..{this.To}";
		}

		/// <summary>Indicates whether the current range is equal to another range.</summary>
		/// <param name="other">A range to compare to this range.</param>
		/// <returns><c>true</c> if the current range is equal to the <paramref name="other" /> range; otherwise, <c>false</c>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public bool Equals(Range<T> other) {
			return this.From.CompareTo(other.From) == 0 && this.To.CompareTo(other.To) == 0;
		}

		/// <summary>Indicates whether the current range and a specified object are equal.</summary>
		/// <param name="obj">The object to compare with the current range.</param>
		/// <returns><c>true</c> if <paramref name="obj" /> is an equal range; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj) {
			if (!(obj is Range<T>)) {
				return false;
			}
			return this.Equals((Range<T>)obj);
		}

		/// <summary>Returns the hash code for this range.</summary>
		/// <returns>A 32-bit signed integer that is the hash code for this range.</returns>
		public override int GetHashCode() {
			return unchecked((this.From.GetHashCode() * 397) ^ this.To.GetHashCode());
		}
	}
}
