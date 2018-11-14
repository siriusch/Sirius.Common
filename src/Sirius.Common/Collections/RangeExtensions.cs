using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Sirius.Collections {
	/// <summary>Range extension methods.</summary>
	public static class RangeExtensions {
		/// <summary>Condense an unsorted enumeration of comparable values into an enumeration of ranges.</summary>
		/// <param name="items">The items to act on.</param>
		/// <returns>An enumeration.</returns>
		[Pure]
		public static IEnumerable<Range<T>> Condense<T>(this IEnumerable<T> items)
				where T: IComparable<T> {
			var sortedItems = items.ToList();
			sortedItems.Sort();
			return CondenseSorted(sortedItems);
		}

		[Pure]
		internal static IEnumerable<Range<T>> CondenseSorted<T>(IList<T> sortedItems)
				where T: IComparable<T> {
			var increment = Incrementor<T>.Increment;
			var i = 0;
			while (i < sortedItems.Count) {
				var from = sortedItems[i++];
				var to = @from;
				while (i < sortedItems.Count) {
					var next = increment(to);
					var diff = sortedItems[i].CompareTo(next);
					if (diff == 0) {
						to = next;
					} else if (diff > 0) {
						break;
					}
					i++;
				}
				yield return new Range<T>(from, to);
			}
		}

		/// <summary>
		///     Normalize an enumeration of ranges. When normalized, adjacent and overlapping (or duplicate) ranges are
		///     merged, and ranges are returned in order.
		/// </summary>
		/// <param name="items">The items to act on.</param>
		/// <returns>The normalized range enumeration.</returns>
		[Pure]
		public static IEnumerable<Range<T>> Normalize<T>(this IEnumerable<Range<T>> items)
				where T: IComparable<T> {
			return items as IRangeSet<T> ?? NormalizeFromSorted(items.OrderBy(r => r.From));
		}

		[Pure]
		internal static IEnumerable<Range<T>> NormalizeFromSorted<T>(this IEnumerable<Range<T>> items) // Items MUST be sorted by `From` property
				where T: IComparable<T> {
			using (var enumerator = items.GetEnumerator()) {
				if (enumerator.MoveNext()) {
					var range = enumerator.Current;
					var from = range.From;
					var to = range.To;
					while ((to.CompareTo(Incrementor<T>.MaxValue) < 0) && enumerator.MoveNext()) {
						range = enumerator.Current;
						if (range.From.CompareTo(Incrementor<T>.Increment(to)) > 0) {
							yield return new Range<T>(@from, to);
							@from = range.From;
							to = range.To;
						} else if (range.To.CompareTo(to) > 0) {
							to = range.To;
						}
					}
					yield return new Range<T>(@from, to);
				}
			}
		}
	}
}
