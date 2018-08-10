using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sirius.Collections {
	public static class RangeExtensions {
		public static IEnumerable<Range<T>> Condense<T>(this IEnumerable<T> items)
				where T: IComparable<T> {
			var sortedItems = items.ToList();
			sortedItems.Sort();
			return CondenseSorted(sortedItems);
		}

		internal static IEnumerable<Range<T>> CondenseSorted<T>(IList<T> sortedItems)
				where T: IComparable<T> {
			var increment = Incrementor<T>.Increment;
			var i = 0;
			while (i < sortedItems.Count) {
				var from = sortedItems[i++];
				var to = from;
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

		public static IEnumerable<Range<T>> Normalize<T>(this IEnumerable<Range<T>> items)
				where T: IComparable<T> {
			return NormalizeFromSorted(items.OrderBy(r => r.From));
		}

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
							yield return new Range<T>(from, to);
							from = range.From;
							to = range.To;
						} else if (range.To.CompareTo(to) > 0) {
							to = range.To;
						}
					}
					yield return new Range<T>(from, to);
				}
			}
		}

		public static int BinarySearch<T>(this IRangeSet<T> that, T item)
				where T: IComparable<T> {
			var left = 0;
			var right = that.Count;
			while (left < right) {
				var middle = (left+right) / 2;
				var range = that[middle];
				if (item.CompareTo(range.From) < 0) {
					right = middle;
				} else if (item.CompareTo(range.To) > 0) {
					left = middle+1;
				} else {
					return middle;
				}
			}
			Debug.Assert(left == right);
			return ~left;
		}
	}
}
