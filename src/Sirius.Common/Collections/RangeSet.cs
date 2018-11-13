using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Sirius.Collections {
	/// <summary>A range set. This class cannot be inherited.</summary>
	/// <typeparam name="T">Generic type parameter.</typeparam>
	/// <remarks>
	///     The <see cref="RangeSet{T}" /> is immutable. The contained ranges in the set are normalized (incremental and
	///     non-overlapping/non-adjacent).
	/// </remarks>
	public struct RangeSet<T>: IEquatable<RangeSet<T>>, IEquatable<IRangeSet<T>>, IRangeSet<T>
			where T: IComparable<T> {
		/// <summary>Implicit cast that converts the given <paramref name="ranges" /> to a <see cref="RangeSet{T}" />.</summary>
		/// <param name="ranges">The ranges to convert. They do not have to be normalized.</param>
		/// <returns>The <see cref="RangeSet{T}" />.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static implicit operator RangeSet<T>(Range<T>[] ranges) {
			return new RangeSet<T>((IEnumerable<Range<T>>)ranges);
		}

		/// <summary>
		///     Implicit cast that converts the given <paramref name="range" /> to a <see cref="RangeSet{T}" /> containing
		///     exactly one range.
		/// </summary>
		/// <param name="range">The range to convert.</param>
		/// <returns>The <see cref="RangeSet{T}" />.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static implicit operator RangeSet<T>(Range<T> range) {
			return new RangeSet<T>(range);
		}

		/// <summary>
		///     Implicit cast that converts the given <paramref name="value" /> to a <see cref="RangeSet{T}" /> containing
		///     exactly one single-item range.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The <see cref="RangeSet{T}" />.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static implicit operator RangeSet<T>(T value) {
			return new RangeSet<T>(value);
		}

		/// <summary>Range complement operator.</summary>
		/// <param name="set">The range set.</param>
		/// <returns>A <see cref="RangeSet{T}" /> representing the complement of the input <paramref name="set" />.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator ~(RangeSet<T> set) {
			return RangeOperations<T>.Negate(set);
		}

		/// <summary>Equality operator.</summary>
		/// <param name="left">The left <see cref="RangeSet{T}" />.</param>
		/// <param name="right">The right <see cref="RangeSet{T}" />.</param>
		/// <returns><c>true</c> if the range sets are equal; <c>false</c> otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator ==(RangeSet<T> left, RangeSet<T> right) {
			return RangeOperations<T>.Equals(left, right);
		}

		/// <summary>Inequality operator.</summary>
		/// <param name="left">The left <see cref="RangeSet{T}" />.</param>
		/// <param name="right">The right <see cref="RangeSet{T}" />.</param>
		/// <returns><c>true</c> if the range sets are not equal; <c>false</c> otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator !=(RangeSet<T> left, RangeSet<T> right) {
			return !RangeOperations<T>.Equals(left, right);
		}

		/// <summary>Range subtraction operator.</summary>
		/// <param name="left">The left range set.</param>
		/// <param name="right">The right range set.</param>
		/// <returns>
		///     A <see cref="RangeSet{T}" /> representing the <paramref name="left" /> ranges minus the
		///     <paramref name="right" /> ranges.
		/// </returns>
		/// <remarks>Unlike the other range operators, this operator is not commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator -(RangeSet<T> left, RangeSet<T> right) {
			return RangeOperations<T>.Subtract(left, right);
		}

		/// <summary>Range union operator.</summary>
		/// <param name="left">The left range set.</param>
		/// <param name="right">The right range set.</param>
		/// <returns>
		///     A <see cref="RangeSet{T}" /> representing the union of the <paramref name="left" /> and the
		///     <paramref name="right" /> ranges.
		/// </returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator |(RangeSet<T> left, RangeSet<T> right) {
			return RangeOperations<T>.Union(left, right);
		}

		/// <summary>Range intersection operator.</summary>
		/// <param name="left">The left range set.</param>
		/// <param name="right">The right range set.</param>
		/// <returns>
		///     A <see cref="RangeSet{T}" /> representing the intersection of the <paramref name="left" /> and the
		///     <paramref name="right" /> ranges.
		/// </returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator &(RangeSet<T> left, RangeSet<T> right) {
			return RangeOperations<T>.Intersection(left, right);
		}

		/// <summary>Range difference operator.</summary>
		/// <param name="left">The left range set.</param>
		/// <param name="right">The right range set.</param>
		/// <returns>
		///     A <see cref="RangeSet{T}" /> representing the difference of the <paramref name="left" /> and the
		///     <paramref name="right" /> ranges (e.g. items which are only in one of the ranges).
		/// </returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator ^(RangeSet<T> left, RangeSet<T> right) {
			return RangeOperations<T>.Difference(left, right);
		}

		/// <summary>Range intersection operator.</summary>
		/// <param name="left">The left range set.</param>
		/// <param name="right">The right range set.</param>
		/// <returns>
		///     A <see cref="RangeSet{T}" /> representing the intersection of the <paramref name="left" /> and the
		///     <paramref name="right" /> ranges.
		/// </returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator &(Range<T> left, RangeSet<T> right) {
			return RangeOperations<T>.Slice(right, left);
		}

		/// <summary>Range intersection operator.</summary>
		/// <param name="left">The left range set.</param>
		/// <param name="right">The right range set.</param>
		/// <returns>
		///     A <see cref="RangeSet{T}" /> representing the intersection of the <paramref name="left" /> and the
		///     <paramref name="right" /> ranges.
		/// </returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator &(T left, RangeSet<T> right) {
			return right.Contains(left) ? left : Empty;
		}

		/// <summary>Range intersection operator.</summary>
		/// <param name="left">The left range set.</param>
		/// <param name="right">The right range set.</param>
		/// <returns>
		///     A <see cref="RangeSet{T}" /> representing the intersection of the <paramref name="left" /> and the
		///     <paramref name="right" /> ranges.
		/// </returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator &(RangeSet<T> left, Range<T> right) {
			return RangeOperations<T>.Slice(left, right);
		}

		/// <summary>Range intersection operator.</summary>
		/// <param name="left">The left range set.</param>
		/// <param name="right">The right range set.</param>
		/// <returns>
		///     A <see cref="RangeSet{T}" /> representing the intersection of the <paramref name="left" /> and the
		///     <paramref name="right" /> ranges.
		/// </returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator &(RangeSet<T> left, T right) {
			return left.Contains(right) ? right : Empty;
		}

		/// <summary>Range union operator.</summary>
		/// <param name="left">The left range set.</param>
		/// <param name="right">The right range set.</param>
		/// <returns>
		///     A <see cref="RangeSet{T}" /> representing the union of the <paramref name="left" /> and the
		///     <paramref name="right" /> ranges.
		/// </returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator |(T left, RangeSet<T> right) {
			return right.Contains(left) ? right : RangeOperations<T>.Union(new RangeSet<T>(left), right);
		}

		/// <summary>Range union operator.</summary>
		/// <param name="left">The left range set.</param>
		/// <param name="right">The right range set.</param>
		/// <returns>
		///     A <see cref="RangeSet{T}" /> representing the union of the <paramref name="left" /> and the
		///     <paramref name="right" /> ranges.
		/// </returns>
		/// <remarks>This operator is commutative.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static RangeSet<T> operator |(RangeSet<T> left, T right) {
			return left.Contains(right) ? left : RangeOperations<T>.Union(left, new RangeSet<T>(right));
		}

		private static readonly Lazy<RangeSet<T>> all = new Lazy<RangeSet<T>>(() => new RangeSet<T>(Range<T>.All), LazyThreadSafetyMode.PublicationOnly);

		/// <summary>Gets the empty range set for the type <typeparamref name="T" />.</summary>
		/// <value>The empty range set.</value>
		public static RangeSet<T> Empty {
			get;
		} = new RangeSet<T>(new Range<T>[0]);

		/// <summary>Gets the range set containing all items for the type <typeparamref name="T" />.</summary>
		/// <value>The range set containing all items.</value>
		public static RangeSet<T> All => all.Value;

		private readonly Range<T>[] ranges;

		/// <summary>Create a range set from the given item.</summary>
		/// <param name="item">The item.</param>
		public RangeSet(T item): this(new[] {new Range<T>(item, item)}) { }

		/// <summary>Create a range set from the given range.</summary>
		/// <param name="range">The range.</param>
		public RangeSet(Range<T> range): this(new[] {range}) { }

		/// <summary>Create a range set from the given ranges.</summary>
		/// <param name="items">The items to use for the range set. They do not have to be normalized.</param>
		public RangeSet(IEnumerable<T> items): this(items.Condense().ToArray()) { }

		/// <summary>Create a range set from the given ranges.</summary>
		/// <param name="ranges">The ranges to use for the range set. They do not have to be normalized.</param>
		public RangeSet(IEnumerable<Range<T>> ranges): this(ranges.Normalize().ToArray()) { }

		internal RangeSet(Range<T>[] normalizedPrivateRanges) {
			this.ranges = (normalizedPrivateRanges != null && normalizedPrivateRanges.Length > 0) ? normalizedPrivateRanges : null;
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
		[Pure]
		public bool Equals(RangeSet<T> other) {
			var count = this.Count;
			return (count == other.Count) && (count == 0) || this.ranges.SequenceEqual(other.ranges);
		}

		/// <summary>Indexer to get ranges within this range set using array index syntax.</summary>
		/// <param name="index">Zero-based index of the range to access.</param>
		/// <returns>The range at the specified index.</returns>
		public Range<T> this[int index] {
			get {
				if (this.ranges == null) {
					throw new ArgumentOutOfRangeException(nameof(index));
				}
				return this.ranges[index];
			}
		}

		/// <summary>Gets the enumerator.</summary>
		/// <returns>The enumerator.</returns>
		[Pure]
		public IEnumerator<Range<T>> GetEnumerator() {
			return (this.ranges == null ? Enumerable.Empty<Range<T>>() : this.ranges).GetEnumerator();
		}

		/// <summary>Gets the number of ranges in the set.</summary>
		/// <value>The count.</value>
		/// <remarks>To get the total number of items, use the <see cref="GetItemCount" /> method.</remarks>
		public int Count => this.ranges == null ? 0 : this.ranges.Length;

		/// <summary>Gets the number of items in the ranges of the set.</summary>
		/// <value>The count.</value>
		/// <remarks>To get the number of ranges only, use the <see cref="Count" /> property.</remarks>
		[Pure]
		public int GetItemCount() {
			return this.ranges == null ? 0 : this.ranges.Select(r => r.Count()).Sum();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}

		/// <summary>Query if this RangeSet&lt;T&gt; contains the given item.</summary>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if the object is in this collection, <c>false</c> if not.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public bool Contains(T item) {
			return RangeOperations<T>.BinarySearch(this, item) >= 0;
		}

		/// <summary>Indicates whether this instance and a specified object are equal.</summary>
		/// <param name="obj">The object to compare with the current instance.</param>
		/// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
		[Pure]
		public override bool Equals(object obj) {
			if (obj is RangeSet<T> set) {
				return this.Equals(set);
			}
			return this.Equals(obj as IRangeSet<T>);
		}

		/// <summary>Enumerates expand in this collection.</summary>
		/// <returns>An enumerator that allows foreach to be used to process expand in this collection.</returns>
		[Pure]
		public IEnumerable<T> Expand() {
			return this.SelectMany(r => r.Expand());
		}

		[Pure]
		private IEnumerable<Range<T>> GetSamples(int maxSampleCount) {
			if (this.ranges != null) {
				var increment = Math.Max(1, this.ranges.Length / maxSampleCount);
				for (var i = 0; i < this.ranges.Length; i += increment) {
					yield return this.ranges[i];
				}
			}
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		[Pure]
		public override int GetHashCode() {
			return this.GetSamples(10).Aggregate(this.Count, (hc, range) => unchecked(hc * 397 + range.GetHashCode()));
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public bool Equals(IRangeSet<T> other) {
			return RangeOperations<T>.Equals(this, other);
		}

		/// <summary>Returns a textual representation of this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> with a textual representation of the ranges in the set.</returns>
		[Pure]
		public override string ToString() {
			return "[" + string.Join(",", this.ranges.Select(r => r.ToString())) + "]";
		}
	}
}
