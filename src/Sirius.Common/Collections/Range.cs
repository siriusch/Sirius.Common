using System;
using System.Collections.Generic;
using System.Globalization;

namespace Sirius.Collections {
	public struct Range<T>: IEquatable<Range<T>>
			where T: IComparable<T> {
		private static readonly Lazy<Range<T>> all = new Lazy<Range<T>>(() => new Range<T>(Incrementor<T>.MinValue, Incrementor<T>.MaxValue));

		public static Range<T> All => all.Value;

		public static RangeSet<T> operator ~(Range<T> range) {
			return RangeSet<T>.Negate(new RangeSet<T>(range));
		}

		public static bool operator ==(Range<T> left, Range<T> right) {
			return left.Equals(right);
		}

		public static bool operator !=(Range<T> left, Range<T> right) {
			return !left.Equals(right);
		}

		public static RangeSet<T> operator -(Range<T> left, Range<T> right) {
			return RangeSet<T>.Subtract(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		public static RangeSet<T> operator |(Range<T> left, Range<T> right) {
			return RangeSet<T>.Union(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		public static RangeSet<T> operator &(Range<T> left, Range<T> right) {
			return RangeSet<T>.Intersection(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		public static RangeSet<T> operator ^(Range<T> left, Range<T> right) {
			return RangeSet<T>.Difference(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		public static RangeSet<T> operator -(T left, Range<T> right) {
			return RangeSet<T>.Subtract(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		public static RangeSet<T> operator |(T left, Range<T> right) {
			return RangeSet<T>.Union(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		public static RangeSet<T> operator &(T left, Range<T> right) {
			return RangeSet<T>.Intersection(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		public static RangeSet<T> operator ^(T left, Range<T> right) {
			return RangeSet<T>.Difference(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		public static RangeSet<T> operator -(Range<T> left, T right) {
			return RangeSet<T>.Subtract(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		public static RangeSet<T> operator |(Range<T> left, T right) {
			return RangeSet<T>.Union(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		public static RangeSet<T> operator &(Range<T> left, T right) {
			return RangeSet<T>.Intersection(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		public static RangeSet<T> operator ^(Range<T> left, T right) {
			return RangeSet<T>.Difference(new RangeSet<T>(left), new RangeSet<T>(right));
		}

		public static Range<T> Create(T single) {
			return Create(single, single);
		}

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

		public T From {
			get;
		}

		public T To {
			get;
		}

		public int Count() {
			if (typeof(IConvertible).IsAssignableFrom(typeof(T))) {
				return (((IConvertible)this.To).ToInt32(CultureInfo.InvariantCulture) - ((IConvertible)this.From).ToInt32(CultureInfo.InvariantCulture)) + 1;
			}
			int result = 0;
			T current = this.From;
			while (current.CompareTo(this.To) < 0) {
				result++;
				current = Incrementor<T>.Increment(current);
			}
			return result;
		}

		public bool Contains(T key) {
			return (this.From.CompareTo(key) <= 0) && (this.To.CompareTo(key) >= 0);
		}

		public bool Contains(Range<T> range) {
			return (this.From.CompareTo(range.From) <= 0) && (this.To.CompareTo(range.To) >= 0);
		}

		public bool IsContainedIn(Range<T> range) {
			return range.Contains(this);
		}

		public bool Intersects(Range<T> range) {
			return (this.From.CompareTo(range.To) <= 0) && (range.From.CompareTo(this.To) <= 0);
		}

		public IEnumerable<T> Expand() {
			var increment = Incrementor<T>.Increment;
			for (var current = this.From;; current = increment(current)) {
				yield return current;
				if (current.CompareTo(this.To) == 0) {
					yield break;
				}
			}
		}

		public override string ToString() {
			return this.From.CompareTo(this.To) == 0 ? this.From.ToString() : string.Format("{0}..{1}", this.From, this.To);
		}

		public bool Equals(Range<T> other) {
			return EqualityComparer<T>.Default.Equals(this.From, other.From) && EqualityComparer<T>.Default.Equals(this.To, other.To);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) {
				return false;
			}
			return obj is Range<T> && this.Equals((Range<T>)obj);
		}

		public override int GetHashCode() {
			unchecked {
				return (EqualityComparer<T>.Default.GetHashCode(this.From) * 397) ^ EqualityComparer<T>.Default.GetHashCode(this.To);
			}
		}
	}
}
