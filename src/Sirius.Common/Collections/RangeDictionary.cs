using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sirius.Collections {
	public class RangeDictionary<TKey, TValue>: IEquatable<RangeDictionary<TKey, TValue>>, IEnumerable<KeyValuePair<Range<TKey>, TValue>>
			where TKey: IComparable<TKey> {
		private class KeySet: IRangeSet<TKey> {
			private readonly IReadOnlyList<KeyValuePair<Range<TKey>, TValue>> items;

			public KeySet(IReadOnlyList<KeyValuePair<Range<TKey>, TValue>> items) {
				this.items = items;
			}

			public IEnumerator<Range<TKey>> GetEnumerator() {
				return this.items.Select(p => p.Key).GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator() {
				return GetEnumerator();
			}

			public int Count => this.items.Count;

			public Range<TKey> this[int index] => this.items[index].Key;
		}

		private class ValueList: IReadWriteList<TValue>, IReadOnlyList<TValue> {
			private readonly IList<KeyValuePair<Range<TKey>, TValue>> items;

			public ValueList(IList<KeyValuePair<Range<TKey>, TValue>> items) {
				this.items = items;
			}

			public IEnumerator<TValue> GetEnumerator() {
				return this.items.Select(p => p.Value).GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator() {
				return GetEnumerator();
			}

			public int Count => this.items.Count;

			public TValue this[int index] {
				get {
					return this.items[index].Value;
				}
				set {
					this.items[index] = new KeyValuePair<Range<TKey>, TValue>(this.items[index].Key, value);
				}
			}
		}

		public static bool operator ==(RangeDictionary<TKey, TValue> left, RangeDictionary<TKey, TValue> right) {
			return Equals(left, right);
		}

		public static bool operator !=(RangeDictionary<TKey, TValue> left, RangeDictionary<TKey, TValue> right) {
			return !Equals(left, right);
		}

		private readonly List<KeyValuePair<Range<TKey>, TValue>> items = new List<KeyValuePair<Range<TKey>, TValue>>();

		private readonly IEqualityComparer<TValue> valueEqualityComparer;

		public RangeDictionary(): this(EqualityComparer<TValue>.Default) { }

		public RangeDictionary(IEqualityComparer<TValue> valueEqualityComparer) {
			this.valueEqualityComparer = valueEqualityComparer;
			this.Keys = new KeySet(this.items);
			this.Values = new ValueList(this.items);
		}

		public RangeDictionary(IEnumerable<KeyValuePair<Range<TKey>, TValue>> items): this(items, EqualityComparer<TValue>.Default) { }

		public RangeDictionary(IEnumerable<KeyValuePair<Range<TKey>, TValue>> items, IEqualityComparer<TValue> valueEqualityComparer): this(valueEqualityComparer) {
			foreach (var item in items) {
				Add(item.Key, item.Value);
			}
		}

		public IRangeSet<TKey> Keys {
			get;
		}

		public IReadWriteList<TValue> Values {
			get;
		}

		public IEnumerator<KeyValuePair<Range<TKey>, TValue>> GetEnumerator() {
			return this.items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public bool Equals(RangeDictionary<TKey, TValue> other) {
			if (ReferenceEquals(null, other)) {
				return false;
			}
			if (ReferenceEquals(this, other)) {
				return true;
			}
			if (this.items.Count != other.items.Count) {
				return false;
			}
			for (var i = 0; i < this.items.Count; i++) {
				var x = this.items[i];
				var y = other.items[i];
				if (!x.Key.Equals(y.Key) || !this.valueEqualityComparer.Equals(x.Value, y.Value)) {
					return false;
				}
			}
			return true;
		}

		public void Add(TKey key, TValue value) {
			Add(new Range<TKey>(key, key), value);
		}

		public void Add(TKey from, TKey to, TValue value) {
			Add(new Range<TKey>(from, to), value);
		}

		public void Add(Range<TKey> range, TValue value) {
			var left = ~this.Keys.BinarySearch(range.From);
			var right = ~this.Keys.BinarySearch(range.To);
			if ((left < 0) || (right < 0) || (left != right)) {
				throw new ArgumentOutOfRangeException(nameof(range), "The range overlaps an existing range");
			}
			this.items.Insert(left, new KeyValuePair<Range<TKey>, TValue>(range, value));
			MergeIfAdjacent(left, left+1);
			MergeIfAdjacent(left-1, left);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) {
				return false;
			}
			if (ReferenceEquals(this, obj)) {
				return true;
			}
			if (obj.GetType() != GetType()) {
				return false;
			}
			return Equals((RangeDictionary<TKey, TValue>)obj);
		}

		public IEnumerable<KeyValuePair<TKey, TValue>> Expanded() {
			return this.items.SelectMany(p => p.Key.Expand().Select(k => new KeyValuePair<TKey, TValue>(k, p.Value)));
		}

		public override int GetHashCode() {
			return this.items.Aggregate(397, (hash, item) => unchecked(hash * 3+item.GetHashCode()));
		}

		private void MergeIfAdjacent(int left, int right) {
			Debug.Assert((right-left) == 1);
			if ((left < 0) || (right >= this.items.Count)) {
				return;
			}
			var leftItem = this.items[left];
			var rightItem = this.items[right];
			if (!this.valueEqualityComparer.Equals(leftItem.Value, rightItem.Value)) {
				return;
			}
			if (Incrementor<TKey>.Adjacent(leftItem.Key.To, rightItem.Key.From)) {
				this.items.RemoveAt(right);
				this.items[left] = new KeyValuePair<Range<TKey>, TValue>(new Range<TKey>(leftItem.Key.From, rightItem.Key.To), leftItem.Value);
			}
		}

		public IEnumerable<KeyValuePair<Range<TKey>, TValue>> Slice(TKey from, TKey to) {
			return Slice(new Range<TKey>(from, to));
		}

		public IEnumerable<KeyValuePair<Range<TKey>, TValue>> Slice(Range<TKey> range) {
			var left = this.Keys.BinarySearch(range.From);
			if (left < 0) {
				left = ~left;
			}
			var right = this.Keys.BinarySearch(range.From);
			if (right < 0) {
				right = ~right-1;
			}
			for (var i = left; i <= right; i++) {
				var current = this.items[i];
				var from = current.Key.From;
				var to = current.Key.To;
				yield return new KeyValuePair<Range<TKey>, TValue>(
					new Range<TKey>((i == left) && (@from.CompareTo(range.From) < 0) ? range.From : from, (i == right) && (to.CompareTo(range.To) > 0) ? range.To : to),
					current.Value);
			}
		}

		public bool TryGetValue(TKey key, out TValue value) {
			var index = this.Keys.BinarySearch(key);
			if (index >= 0) {
				value = this.items[index].Value;
				return true;
			}
			value = default(TValue);
			return false;
		}
	}
}
