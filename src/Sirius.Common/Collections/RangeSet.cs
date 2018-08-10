using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Sirius.Collections {
	public sealed class RangeSet<T>: IEquatable<RangeSet<T>>, IRangeSet<T>
			where T: IComparable<T> {
		public static implicit operator RangeSet<T>(Range<T> range) {
			return new RangeSet<T>(range);
		}

		private static readonly Lazy<RangeSet<T>> all = new Lazy<RangeSet<T>>(() => new RangeSet<T>(Range<T>.All), LazyThreadSafetyMode.PublicationOnly);

		public static RangeSet<T> Empty {
			get;
		} = new RangeSet<T>(new Range<T>[0]);

		public static RangeSet<T> All => all.Value;

		public static IEnumerable<TResult> EnumerateRanges<TResult>(IRangeSet<T> left, IRangeSet<T> right, Func<Range<T>, int?, int?, TResult> process) {
			using (var enumLeft = left.GetEnumerator()) {
				using (var enumRight = right.GetEnumerator()) {
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

		public static IEnumerable<KeyValuePair<Range<T>, ContainedIn>> EnumerateRanges(IRangeSet<T> left, IRangeSet<T> right) {
			return EnumerateRanges(left, right, (rng, l, r) => new KeyValuePair<Range<T>, ContainedIn>(rng, l.HasValue ? r.HasValue ? ContainedIn.Both : ContainedIn.Left : ContainedIn.Right));
		}

		public static RangeSet<T> Subtract(IRangeSet<T> x, IRangeSet<T> y) {
			if (x.Count == 0) {
				return Empty;
			}
			if (y.Count == 0 && x is RangeSet<T> setX) {
				return setX;
			}
			return new RangeSet<T>(EnumerateRanges(x, y).Where(r => r.Value == ContainedIn.Left).Select(r => r.Key).NormalizeFromSorted().ToArray());
		}

		public static RangeSet<T> Difference(IRangeSet<T> x, IRangeSet<T> y) {
			if (y.Count == 0 && x is RangeSet<T> setX) {
				return setX;
			}
			if (y.Count == 0 && x is RangeSet<T> setY) {
				return setY;
			}
			return new RangeSet<T>(EnumerateRanges(x, y).Where(r => r.Value != ContainedIn.Both).Select(r => r.Key).NormalizeFromSorted().ToArray());
		}

		public static RangeSet<T> Intersection(IRangeSet<T> x, IRangeSet<T> y) {
			if (x.Count == 0 || y.Count == 0) {
				return Empty;
			}
			return new RangeSet<T>(EnumerateRanges(x, y).Where(r => r.Value == ContainedIn.Both).Select(r => r.Key).NormalizeFromSorted().ToArray());
		}

		public static RangeSet<T> Negate(IRangeSet<T> x) {
			if (x.Count == 0) {
				return All;
			}
			if (ReferenceEquals(x, All)) {
				return Empty;
			}
			return new RangeSet<T>(EnumerateRanges(x, All).Where(r => r.Value == ContainedIn.Right).Select(r => r.Key).NormalizeFromSorted().ToArray());
		}

		public static RangeSet<T> Union(IRangeSet<T> x, IRangeSet<T> y) {
			if (y.Count == 0 && x is RangeSet<T> setX) {
				return setX;
			}
			if (y.Count == 0 && x is RangeSet<T> setY) {
				return setY;
			}
			return new RangeSet<T>(EnumerateRanges(x, y).Select(r => r.Key).NormalizeFromSorted().ToArray());
		}

		public static RangeSet<T> Union(params IRangeSet<T>[] rangeSets) {
			return Union((IEnumerable<RangeSet<T>>)rangeSets);
		}

		public static RangeSet<T> Union(IEnumerable<IRangeSet<T>> rangeSets) {
			using (var enumerator = rangeSets.GetEnumerator()) {
				if (!enumerator.MoveNext()) {
					return Empty;
				}
				var result = enumerator.Current;
				while (enumerator.MoveNext()) {
					result = Union(result, enumerator.Current);
				}
				return result as RangeSet<T> ?? new RangeSet<T>(result);
			}
		}

		private readonly Range<T>[] ranges;

		public RangeSet(T item): this(new[] { new Range<T>(item, item) }) { }

		public RangeSet(Range<T> item): this(new[] { item }) { }

		public RangeSet(IEnumerable<T> items): this(items.Condense().ToArray()) { }

		public RangeSet(IEnumerable<Range<T>> ranges): this(ranges.Normalize().ToArray()) { }

		internal RangeSet(Range<T>[] normalizedPrivateRanges) {
			this.ranges = normalizedPrivateRanges;
		}

		public bool Equals(RangeSet<T> other) {
			return (other != null) && (ReferenceEquals(this, other) || this.ranges.SequenceEqual(other.ranges));
		}

		public Range<T> this[int index] => this.ranges[index];

		public IEnumerator<Range<T>> GetEnumerator() {
			return ((ICollection<Range<T>>)this.ranges).GetEnumerator();
		}

		public int Count => this.ranges.Length;

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public bool Contains(T item) {
			return this.BinarySearch(item) >= 0;
		}

		public override bool Equals(object obj) {
			return Equals(obj as RangeSet<T>);
		}

		public IEnumerable<T> Expand() {
			return this.ranges.SelectMany(r => r.Expand());
		}

		public override int GetHashCode() {
			return this.ranges.Aggregate(GetType().GetHashCode(), (hc, range) => hc^range.GetHashCode());
		}

		public override string ToString() {
			return "["+string.Join(",", this.ranges.Select(r => r.ToString()))+"]";
		}
	}
}
