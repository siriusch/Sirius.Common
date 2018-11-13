using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Sirius.Collections {
	/// <summary>Generic operations for ranges and range sets.</summary>
	/// <typeparam name="T">Generic type parameter.</typeparam>
	public static class RangeOperations<T>
			where T: IComparable<T> {
		/// <summary>Perform a binary search for a specific <paramref name="value" /> in a <seealso cref="IRangeSet{T}" />.</summary>
		/// <typeparam name="TRangeSet">Type of the range set.</typeparam>
		/// <param name="set">The range set to act on.</param>
		/// <param name="value">The value to find.</param>
		/// <returns>
		///     The index of the range containing the specified <paramref name="value" /> in the <see cref="IRangeSet{T}" />, if
		///     <paramref name="value" /> is found. If <paramref name="value" /> is not found,
		///     the negative number returned is the bitwise complement of the index where the value would need to be inserted as
		///     range.
		/// </returns>
		[Pure]
		public static int BinarySearch<TRangeSet>(TRangeSet set, T value)
				where TRangeSet: IRangeSet<T> {
			var left = 0;
			var right = set.Count;
			while (left < right) {
				var middle = (left + right) / 2;
				var range = set[middle];
				if (value.CompareTo(range.From) < 0) {
					right = middle;
				} else if (value.CompareTo(range.To) > 0) {
					left = middle + 1;
				} else {
					return middle;
				}
			}
			Debug.Assert(left == right);
			return ~left;
		}

		/// <summary>Generically query if 'set' is empty.</summary>
		/// <remarks>
		///     This method avoids boxing when applied on a struct implementing <see cref="IRangeSet{T}" />. A <c>null</c>
		///     <paramref name="set" /> however will be seen as being empty.
		/// </remarks>
		/// <typeparam name="TRangeSet">Type of the range set.</typeparam>
		/// <param name="set">The range set.</param>
		/// <returns><c>true</c> if empty set, <c>false</c> if not.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool IsEmptySet<TRangeSet>(TRangeSet set)
				where TRangeSet: IRangeSet<T> {
			if (!typeof(TRangeSet).IsValueType && ReferenceEquals(set, null)) {
				return true;
			}
			return set.Count == 0;
		}

		/// <summary>Generically gets range enumerator.</summary>
		/// <remarks>This method avoids boxing when applied on a struct implementing <see cref="IRangeSet{T}" />.</remarks>
		/// <typeparam name="TRangeSet">Type of the range set.</typeparam>
		/// <param name="set">The range set.</param>
		/// <returns>The range enumerator.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		internal static IEnumerator<Range<T>> GetRangeEnumerator<TRangeSet>(TRangeSet set)
				where TRangeSet: IRangeSet<T> {
			if (IsEmptySet(set)) {
				return Enumerable.Empty<Range<T>>().GetEnumerator();
			}
			return set.GetEnumerator();
		}

		/// <summary>Generically check for a value in a range set.</summary>
		/// <typeparam name="TRangeSet">Type of the range set.</typeparam>
		/// <param name="set">The range set.</param>
		/// <param name="value">The value to find.</param>
		/// <returns><c>true</c> if the object is in this collection, <c>false</c> if not.</returns>
		[Pure]
		public static bool Contains<TRangeSet>(TRangeSet set, T value)
				where TRangeSet: IRangeSet<T> {
			if (IsEmptySet(set)) {
				return false;
			}
			var first = set[0];
			var last = set[set.Count - 1];
			if ((value.CompareTo(first.From) < 0) || (value.CompareTo(last.To) > 0)) {
				return false;
			}
			return BinarySearch(set, value) >= 0;
		}

		/// <summary>Generically converts a set to a range set.</summary>
		/// <typeparam name="TRangeSet">Type of the range set.</typeparam>
		/// <param name="set">The range set.</param>
		/// <returns>A RangeSet&lt;T&gt;</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		private static RangeSet<T> AsRangeSet<TRangeSet>(TRangeSet set)
				where TRangeSet: IRangeSet<T> {
			if (IsEmptySet(set)) {
				return RangeSet<T>.Empty;
			}
			if (set is RangeSet<T> rangeSet) {
				return rangeSet;
			}
			return new RangeSet<T>(set.ToArray());
		}

		/// <summary>Tests if two IRangeSet&lt;T&gt; objects are considered equal.</summary>
		/// <typeparam name="TRangeSetLeft">Type of the range set left.</typeparam>
		/// <typeparam name="TRangeSetRight">Type of the range set right.</typeparam>
		/// <param name="left">The left <see cref="RangeSet{T}" />.</param>
		/// <param name="right">The right <see cref="RangeSet{T}" />.</param>
		/// <returns><c>true</c> if the objects are considered equal, <c>false</c> if they are not.</returns>
		public static bool Equals<TRangeSetLeft, TRangeSetRight>(TRangeSetLeft left, TRangeSetRight right)
				where TRangeSetLeft: IRangeSet<T>
				where TRangeSetRight: IRangeSet<T> {
			var isLeftReference = !typeof(TRangeSetLeft).IsValueType;
			var isRightReference = !typeof(TRangeSetRight).IsValueType;
			if (isLeftReference && isRightReference && ReferenceEquals(left, right)) {
				return true;
			}
			if (isLeftReference && ReferenceEquals(left, null)) {
				return false;
			}
			if (isRightReference && ReferenceEquals(right, null)) {
				return false;
			}
			var count = left.Count;
			if (count != right.Count) {
				return false;
			}
			for (var i = 0; i < count; i++) {
				if (left[i] != right[i]) {
					return false;
				}
			}
			return true;
		}

		/// <summary>
		///     Enumerates all ranges of left and right combined, and call the <paramref name="process" /> callback for each range,
		///     passing the index into the origin left and/or right range set.
		/// </summary>
		/// <typeparam name="TRangeSetLeft">Type of the left range set.</typeparam>
		/// <typeparam name="TRangeSetRight">Type of the right range set.</typeparam>
		/// <typeparam name="TResult">Type of the result.</typeparam>
		/// <param name="left">The left <see cref="RangeSet{T}" />.</param>
		/// <param name="right">The right <see cref="RangeSet{T}" />.</param>
		/// <param name="process">The process.</param>
		/// <returns>An enumerator that allows foreach to be used to process the ranges in this collection.</returns>
		public static IEnumerable<TResult> EnumerateRanges<TRangeSetLeft, TRangeSetRight, TResult>(TRangeSetLeft left, TRangeSetRight right, Func<Range<T>, int?, int?, TResult> process)
				where TRangeSetLeft: IRangeSet<T>
				where TRangeSetRight: IRangeSet<T> {
			using (var enumLeft = GetRangeEnumerator(left)) {
				using (var enumRight = GetRangeEnumerator(right)) {
					var ixLeft = -1;
					var ixRight = -1;
					var rngLeft = enumLeft.GetNext(ref ixLeft);
					var rngRight = enumRight.GetNext(ref ixRight);
					while (rngLeft.HasValue || rngRight.HasValue) {
						if (rngLeft.HasValue && (!rngRight.HasValue || (rngLeft.Value.To.CompareTo(rngRight.Value.From) < 0))) {
							// no overlap, only in this
							yield return process(rngLeft.Value, ixLeft, null);
							rngLeft = enumLeft.GetNext(ref ixLeft);
							continue;
						}
						// if we get here then rngOther.HasValue == true
						if (!rngLeft.HasValue || (rngRight.Value.To.CompareTo(rngLeft.Value.From) < 0)) {
							// no overlap, only in other
							yield return process(rngRight.Value, null, ixRight);
							rngRight = enumRight.GetNext(ref ixRight);
							continue;
						}
						// if we get here then we have an overlap. first return any overhang on the "from" side
						if (rngLeft.Value.From.CompareTo(rngRight.Value.From) < 0) {
							Debug.Assert(rngLeft.Value.To.CompareTo(rngRight.Value.From) >= 0);
							yield return process(new Range<T>(rngLeft.Value.From, Incrementor<T>.Decrement(rngRight.Value.From)), ixLeft, null);
							rngLeft = new Range<T>(rngRight.Value.From, rngLeft.Value.To);
						} else if (rngLeft.Value.From.CompareTo(rngRight.Value.From) > 0) {
							Debug.Assert(rngRight.Value.To.CompareTo(rngLeft.Value.From) >= 0);
							yield return process(new Range<T>(rngRight.Value.From, Incrementor<T>.Decrement(rngLeft.Value.From)), null, ixRight);
							rngRight = new Range<T>(rngLeft.Value.From, rngRight.Value.To);
						}
						// next return overlapping part and fixup ranges for next iteration
						Debug.Assert(rngLeft.Value.From.CompareTo(rngRight.Value.From) == 0);
						if (rngLeft.Value.To.CompareTo(rngRight.Value.To) < 0) {
							// rngOther is longer
							yield return process(rngLeft.Value, ixLeft, ixRight);
							rngRight = new Range<T>(Incrementor<T>.Increment(rngLeft.Value.To), rngRight.Value.To);
							rngLeft = enumLeft.GetNext(ref ixLeft);
						} else if (rngLeft.Value.To.CompareTo(rngRight.Value.To) > 0) {
							// rngThis is longer
							yield return process(rngRight.Value, ixLeft, ixRight);
							rngLeft = new Range<T>(Incrementor<T>.Increment(rngRight.Value.To), rngLeft.Value.To);
							rngRight = enumRight.GetNext(ref ixRight);
						} else {
							// both equal
							yield return process(rngLeft.Value, ixLeft, ixRight);
							rngLeft = enumLeft.GetNext(ref ixLeft);
							rngRight = enumRight.GetNext(ref ixRight);
						}
					}
				}
			}
		}

		/// <summary>Enumerates all ranges which are in left and/or right, and return information about its origin.</summary>
		/// <typeparam name="TRangeSetLeft">Type of the left range set.</typeparam>
		/// <typeparam name="TRangeSetRight">Type of the right range set.</typeparam>
		/// <param name="left">The left <see cref="RangeSet{T}" />.</param>
		/// <param name="right">The right <see cref="RangeSet{T}" />.</param>
		/// <returns>An enumerator that allows foreach to be used to process the ranges as keys and their origin as values.</returns>
		public static IEnumerable<KeyValuePair<Range<T>, ContainedIn>> EnumerateRanges<TRangeSetLeft, TRangeSetRight>(TRangeSetLeft left, TRangeSetRight right)
				where TRangeSetLeft: IRangeSet<T>
				where TRangeSetRight: IRangeSet<T> {
			return EnumerateRanges(left, right, (rng, l, r) => new KeyValuePair<Range<T>, ContainedIn>(rng, l.HasValue ? r.HasValue ? ContainedIn.Both : ContainedIn.Left : ContainedIn.Right));
		}

		/// <summary>Range subtraction operation.</summary>
		/// <remarks>Unlike the other range operations, this operator is not commutative.</remarks>
		/// <typeparam name="TRangeSetLeft">Type of the range set left.</typeparam>
		/// <typeparam name="TRangeSetRight">Type of the range set right.</typeparam>
		/// <param name="left">The left range set.</param>
		/// <param name="right">The right range set.</param>
		/// <returns>
		///     A <see cref="RangeSet{T}" /> representing the <paramref name="left" /> ranges minus the
		///     <paramref name="right" /> ranges.
		/// </returns>
		public static RangeSet<T> Subtract<TRangeSetLeft, TRangeSetRight>(TRangeSetLeft left, TRangeSetRight right)
				where TRangeSetLeft: IRangeSet<T>
				where TRangeSetRight: IRangeSet<T> {
			if (IsEmptySet(left)) {
				return RangeSet<T>.Empty;
			}
			if (IsEmptySet(right)) {
				return AsRangeSet(left);
			}
			return new RangeSet<T>(EnumerateRanges(left, right).Where(r => r.Value == ContainedIn.Left).Select(r => r.Key).ToArray());
		}

		/// <summary>Range difference operation.</summary>
		/// <remarks>This operator is commutative.</remarks>
		/// <typeparam name="TRangeSetLeft">Type of the range set left.</typeparam>
		/// <typeparam name="TRangeSetRight">Type of the range set right.</typeparam>
		/// <param name="left">The left range set.</param>
		/// <param name="right">The right range set.</param>
		/// <returns>
		///     A <see cref="RangeSet{T}" /> representing the difference of the <paramref name="left" /> and the
		///     <paramref name="right" /> ranges (e.g. items which are only in one of the ranges).
		/// </returns>
		public static RangeSet<T> Difference<TRangeSetLeft, TRangeSetRight>(TRangeSetLeft left, TRangeSetRight right)
				where TRangeSetLeft: IRangeSet<T>
				where TRangeSetRight: IRangeSet<T> {
			if (IsEmptySet(right)) {
				return AsRangeSet(left);
			}
			if (IsEmptySet(left)) {
				return AsRangeSet(right);
			}
			return new RangeSet<T>(EnumerateRanges(left, right).Where(r => r.Value != ContainedIn.Both).Select(r => r.Key).NormalizeFromSorted().ToArray());
		}

		/// <summary>Range intersection operation.</summary>
		/// <remarks>This operator is commutative.</remarks>
		/// <typeparam name="TRangeSetLeft">Type of the range set left.</typeparam>
		/// <typeparam name="TRangeSetRight">Type of the range set right.</typeparam>
		/// <param name="left">The left range set.</param>
		/// <param name="right">The right range set.</param>
		/// <returns>
		///     A <see cref="RangeSet{T}" /> representing the intersection of the <paramref name="left" /> and the
		///     <paramref name="right" /> ranges.
		/// </returns>
		public static RangeSet<T> Intersection<TRangeSetLeft, TRangeSetRight>(TRangeSetLeft left, TRangeSetRight right)
				where TRangeSetLeft: IRangeSet<T>
				where TRangeSetRight: IRangeSet<T> {
			if (IsEmptySet(left) || IsEmptySet(right)) {
				return RangeSet<T>.Empty;
			}
			return new RangeSet<T>(EnumerateRanges(left, right).Where(r => r.Value == ContainedIn.Both).Select(r => r.Key).ToArray());
		}

		/// <summary>Range negation operation.</summary>
		/// <typeparam name="TRangeSet">Type of the range set.</typeparam>
		/// <param name="set">The range set.</param>
		/// <returns>A <see cref="RangeSet{T}" /> representing the complement of the input <paramref name="set" />.</returns>
		public static RangeSet<T> Negate<TRangeSet>(TRangeSet set)
				where TRangeSet: IRangeSet<T> {
			if (IsEmptySet(set)) {
				return RangeSet<T>.All;
			}
			if (Equals(RangeSet<T>.All, set)) {
				return RangeSet<T>.Empty;
			}
			return new RangeSet<T>(EnumerateRanges(set, RangeSet<T>.All).Where(r => r.Value == ContainedIn.Right).Select(r => r.Key).ToArray());
		}

		/// <summary>Range slice operation.</summary>
		/// <typeparam name="TRangeSet">Type of the range set.</typeparam>
		/// <param name="set">The range set.</param>
		/// <param name="slice">The slice range to return.</param>
		/// <returns>A <see cref="RangeSet{T}" /> representing the slice of the input <paramref name="set" />.</returns>
		public static RangeSet<T> Slice<TRangeSet>(TRangeSet set, Range<T> slice)
				where TRangeSet: IRangeSet<T> {
			if (IsEmptySet(set)) {
				return RangeSet<T>.Empty;
			}
			var first = set[0];
			var last = set[set.Count - 1];
			if ((slice.To.CompareTo(first.From) < 0) || (slice.From.CompareTo(last.To) > 0)) {
				return RangeSet<T>.Empty;
			}
			if ((slice.From.CompareTo(first.From) <= 0) && (slice.To.CompareTo(last.To) >= 0)) {
				return AsRangeSet(set);
			}
			if (slice.From.CompareTo(slice.To) == 0) {
				return BinarySearch(set, slice.From) >= 0 ? slice : RangeSet<T>.Empty;
			}
			return new RangeSet<T>(EnumerateRanges(set, new RangeSet<T>(slice)).Where(r => r.Value == ContainedIn.Both).Select(r => r.Key).ToArray());
		}

		/// <summary>Range slice operation.</summary>
		/// <typeparam name="TRangeSet">Type of the range set.</typeparam>
		/// <param name="set">The range set.</param>
		/// <param name="from">The lower bound of the slice (inclusive).</param>
		/// <param name="to">The upper bound of the slice (inclusive).</param>
		/// <returns>A <see cref="RangeSet{T}" /> representing the slice of the input <paramref name="set" />.</returns>
		public static RangeSet<T> Slice<TRangeSet>(TRangeSet set, T from, T to)
				where TRangeSet: IRangeSet<T> {
			return Slice(set, Range<T>.Create(from, to));
		}

		/// <summary>Range union operation.</summary>
		/// <remarks>This operator is commutative.</remarks>
		/// <typeparam name="TRangeSetLeft">Type of the range set left.</typeparam>
		/// <typeparam name="TRangeSetRight">Type of the range set right.</typeparam>
		/// <param name="left">The left range set.</param>
		/// <param name="right">The right range set.</param>
		/// <returns>
		///     A <see cref="RangeSet{T}" /> representing the union of the <paramref name="left" /> and the
		///     <paramref name="right" /> ranges.
		/// </returns>
		public static RangeSet<T> Union<TRangeSetLeft, TRangeSetRight>(TRangeSetLeft left, TRangeSetRight right)
				where TRangeSetLeft: IRangeSet<T>
				where TRangeSetRight: IRangeSet<T> {
			if (IsEmptySet(left)) {
				return AsRangeSet(right);
			}
			if (IsEmptySet(right)) {
				return AsRangeSet(left);
			}
			if (Equals(RangeSet<T>.All, left) || Equals(RangeSet<T>.All, right)) {
				return RangeSet<T>.All;
			}
			return new RangeSet<T>(EnumerateRanges(left, right).Select(r => r.Key).NormalizeFromSorted().ToArray());
		}

		/// <summary>Range union operation.</summary>
		/// <remarks>This operator is commutative.</remarks>
		/// <typeparam name="TRangeSet">Type of the range set.</typeparam>
		/// <param name="rangeSets">A variable-length parameters list containing range sets.</param>
		/// <returns>A <see cref="RangeSet{T}" /> representing the union of <paramref name="rangeSets" />.</returns>
		public static RangeSet<T> Union<TRangeSet>(params TRangeSet[] rangeSets)
				where TRangeSet: IRangeSet<T> {
			return Union((IEnumerable<TRangeSet>)rangeSets);
		}

		/// <summary>Range union operation.</summary>
		/// <remarks>This operator is commutative.</remarks>
		/// <typeparam name="TRangeSet">Type of the range set.</typeparam>
		/// <param name="rangeSets">An enumeration of range sets.</param>
		/// <returns>A <see cref="RangeSet{T}" /> representing the union of <paramref name="rangeSets" />.</returns>
		public static RangeSet<T> Union<TRangeSet>(IEnumerable<TRangeSet> rangeSets)
				where TRangeSet: IRangeSet<T> {
			using (var enumerator = rangeSets.GetEnumerator()) {
				if (!enumerator.MoveNext()) {
					return RangeSet<T>.Empty;
				}
				var result = AsRangeSet(enumerator.Current);
				while (enumerator.MoveNext()) {
					result = Union(result, enumerator.Current);
				}
				return result;
			}
		}
	}
}
