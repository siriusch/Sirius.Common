using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Sirius.Collections {
	/// <summary>A key-value dictionary where the keys are treated as normalized range sets.</summary>
	/// <typeparam name="TKey">Type of the key.</typeparam>
	/// <typeparam name="TValue">Type of the value.</typeparam>
	public class RangeDictionary<TKey, TValue>: IEnumerable<KeyValuePair<Range<TKey>, TValue>>
			where TKey: IComparable<TKey> {
		private sealed class KeySet: IRangeSet<TKey> {
			private readonly IReadOnlyList<KeyValuePair<Range<TKey>, TValue>> items;

			public KeySet(IReadOnlyList<KeyValuePair<Range<TKey>, TValue>> items) {
				this.items = items;
			}

			public IEnumerator<Range<TKey>> GetEnumerator() {
				return this.items.Select(p => p.Key).GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator() {
				return this.GetEnumerator();
			}

			public int Count => this.items.Count;

			public Range<TKey> this[int index] => this.items[index].Key;
		}

		private sealed class ValueList: IList<TValue> {
			private readonly IList<KeyValuePair<Range<TKey>, TValue>> items;

			public ValueList(IList<KeyValuePair<Range<TKey>, TValue>> items) {
				this.items = items;
			}

			public IEnumerator<TValue> GetEnumerator() {
				return this.items.Select(p => p.Value).GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator() {
				return this.GetEnumerator();
			}

			public void Add(TValue item) {
				throw new NotSupportedException();
			}

			public void Clear() {
				throw new NotSupportedException();
			}

			public bool Contains(TValue item) {
				throw new NotImplementedException();
			}

			public void CopyTo(TValue[] array, int arrayIndex) {
				throw new NotImplementedException();
			}

			public bool Remove(TValue item) {
				throw new NotSupportedException();
			}

			public int Count => this.items.Count;

			public bool IsReadOnly => false;

			public int IndexOf(TValue item) {
				return this.items.Select(i => i.Value).IndexOf(item);
			}

			public void Insert(int index, TValue item) {
				throw new NotSupportedException();
			}

			public void RemoveAt(int index) {
				throw new NotSupportedException();
			}

			public TValue this[int index] {
				get {
					return this.items[index].Value;
				}
				set {
					this.items[index] = new KeyValuePair<Range<TKey>, TValue>(this.items[index].Key, value);
				}
			}
		}

		private class VirtualDictionary: IReadOnlyDictionary<TKey, TValue> {
			private readonly RangeDictionary<TKey, TValue> owner;

			public VirtualDictionary(RangeDictionary<TKey, TValue> owner) {
				this.owner = owner;
			}

			public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
				return owner.items.SelectMany(p => p.Key.Expand().Select(k => new KeyValuePair<TKey, TValue>(k, p.Value))).GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator() {
				return this.GetEnumerator();
			}

			public int Count => owner.items.Select(i => i.Key.Count()).Sum();

			public bool ContainsKey(TKey key) {
				return RangeOperations<TKey>.BinarySearch(this.owner.Keys, key) >= 0;
			}

			public bool TryGetValue(TKey key, out TValue value) {
				return this.owner.TryGetValue(key, out value);
			}

			public TValue this[TKey key] {
				get {
					if (this.owner.TryGetValue(key, out var result)) {
						return result;
					}
					throw new KeyNotFoundException();
				}
			}

			public IEnumerable<TKey> Keys => this.owner.Keys.Expand();

			public IEnumerable<TValue> Values => this.owner.SelectMany(p => Enumerable.Repeat(p.Value, p.Key.Count()));
		}

		private readonly List<KeyValuePair<Range<TKey>, TValue>> items = new List<KeyValuePair<Range<TKey>, TValue>>();

		private readonly IEqualityComparer<TValue> valueEqualityComparer;

		/// <summary>Creates a new <see cref="RangeDictionary{TKey,TValue}" />.</summary>
		public RangeDictionary(): this(EqualityComparer<TValue>.Default) { }

		/// <summary>Creates a new <see cref="RangeDictionary{TKey,TValue}" />.</summary>
		/// <param name="valueEqualityComparer">The value equality comparer.</param>
		/// <remarks>The value equality comparer is used to determine if adjacent ranges can be merged.</remarks>
		public RangeDictionary(IEqualityComparer<TValue> valueEqualityComparer) {
			this.valueEqualityComparer = valueEqualityComparer ?? EqualityComparer<TValue>.Default;
			this.Keys = new KeySet(this.items);
			this.Values = new ValueList(this.items);
			this.Expanded = new VirtualDictionary(this);
		}

		/// <summary>Creates a new <see cref="RangeDictionary{TKey,TValue}" />.</summary>
		/// <param name="items">The items to initially add.</param>
		public RangeDictionary(IEnumerable<KeyValuePair<Range<TKey>, TValue>> items): this(items, EqualityComparer<TValue>.Default) { }

		/// <summary>Creates a new <see cref="RangeDictionary{TKey,TValue}" />.</summary>
		/// <param name="items">The items to initially add.</param>
		/// <param name="valueEqualityComparer">The value equality comparer.</param>
		/// <remarks>The value equality comparer is used to determine if adjacent ranges can be merged.</remarks>
		public RangeDictionary(IEnumerable<KeyValuePair<Range<TKey>, TValue>> items, IEqualityComparer<TValue> valueEqualityComparer): this(valueEqualityComparer) {
			foreach (var item in items) {
				this.Add(item.Key, item.Value);
			}
		}

		/// <summary>Gets the value equality comparer.</summary>
		/// <value>The value comparer.</value>
		/// <remarks>The value equality comparer is used to determine if adjacent ranges can be merged.</remarks>
		public IEqualityComparer<TValue> ValueComparer => this.valueEqualityComparer;

		/// <summary>Gets the keys as <see cref="IRangeSet{T}" />.</summary>
		/// <value>The keys.</value>
		public IRangeSet<TKey> Keys {
			get;
		}

		/// <summary>Gets the values.</summary>
		/// <value>The values.</value>
		/// <remarks>
		///     The list cannot change in size, but items can be accessed and set by index. Setting an item replaces the value
		///     for the whole range at the given index.
		/// </remarks>
		public IList<TValue> Values {
			get;
		}

		/// <summary>Gets an expanded view.</summary>
		/// <value>The expanded values.</value>
		public IReadOnlyDictionary<TKey, TValue> Expanded {
			get;
		}

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An enumerator that can be used to iterate through the collection.</returns>
		[Pure]
		public IEnumerator<KeyValuePair<Range<TKey>, TValue>> GetEnumerator() {
			return this.items.GetEnumerator();
		}

		[Pure]
		IEnumerator IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}

		/// <summary>Adds a new value at the specified key.</summary>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when the range overlaps an existing range of the dictionary.</exception>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <remarks>This operation takes O(log n) time, where n is the number of range key and value pairs)</remarks>
		public void Add(TKey key, TValue value) {
			this.Add(new Range<TKey>(key, key), value);
		}

		/// <summary>Adds a new value at the specified key range.</summary>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when the range overlaps an existing range of the dictionary.</exception>
		/// <param name="from">The lower bound of the range (inclusive).</param>
		/// <param name="to">The upper bound of the range (inclusive).</param>
		/// <param name="value">The value.</param>
		/// <remarks>This operation takes O(log n) time, where n is the number of range key and value pairs)</remarks>
		public void Add(TKey from, TKey to, TValue value) {
			this.Add(new Range<TKey>(from, to), value);
		}

		/// <summary>Adds a new value at the specified key range.</summary>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when the range overlaps an existing range of the dictionary.</exception>
		/// <param name="range">The key range.</param>
		/// <param name="value">The value.</param>
		/// <remarks>This operation takes O(log n) time, where n is the number of range key and value pairs)</remarks>
		public void Add(Range<TKey> range, TValue value) {
			var left = ~RangeOperations<TKey>.BinarySearch(this.Keys, range.From);
			var right = ~RangeOperations<TKey>.BinarySearch(this.Keys, range.To);
			if ((left < 0) || (right < 0) || (left != right)) {
				throw new ArgumentOutOfRangeException(nameof(range), "The range overlaps an existing range");
			}
			this.items.Insert(left, new KeyValuePair<Range<TKey>, TValue>(range, value));
			this.MergeIfAdjacent(left, left + 1);
			this.MergeIfAdjacent(left - 1, left);
		}

		/// <summary>Adds a new value at the specified key ranges.</summary>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when the range overlaps an existing range of the dictionary.</exception>
		/// <param name="ranges">The key range.</param>
		/// <param name="value">The value.</param>
		/// <remarks>This operation takes O(log n) time, where n is the number of range key and value pairs)</remarks>
		public void Add<TRangeSet>(TRangeSet ranges, TValue value) where TRangeSet: IRangeSet<TKey> {
			foreach (var range in ranges) {
				Add(range, value);
			}
		}

		/// <summary>Adds a new value at the specified keys.</summary>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when the range overlaps an existing range of the dictionary.</exception>
		/// <param name="keys">The key range.</param>
		/// <param name="value">The value.</param>
		/// <remarks>This operation takes O(log n) time, where n is the number of range key and value pairs)</remarks>
		public void Add(IEnumerable<TKey> keys, TValue value) {
			Add(new RangeSet<TKey>(keys), value);
		}

		/// <summary>Compares the content of the <see cref="RangeDictionary{TKey,TValue}" />.</summary>
		/// <remarks>This operation runs in O(n) time (worst), where n is the number of range key and value pairs)</remarks>
		/// <param name="other">The other <see cref="RangeDictionary{TKey,TValue}" />.</param>
		/// <param name="valueEqualityComparer">(Optional) The value equality comparer.</param>
		/// <returns><c>true</c> if it succeeds, <c>false</c> if it fails.</returns>
		[Pure]
		public bool ContentEquals(RangeDictionary<TKey, TValue> other, IEqualityComparer<TValue> valueEqualityComparer = null) {
			if (ReferenceEquals(null, other)) {
				return false;
			}
			if (ReferenceEquals(this, other)) {
				return true;
			}
			if (this.items.Count != other.items.Count) {
				return false;
			}
			if (valueEqualityComparer == null) {
				valueEqualityComparer = this.valueEqualityComparer;
			}
			for (var i = 0; i < this.items.Count; i++) {
				var x = this.items[i];
				var y = other.items[i];
				if (!x.Key.Equals(y.Key) || !valueEqualityComparer.Equals(x.Value, y.Value)) {
					return false;
				}
			}
			return true;
		}

		internal IEnumerable<KeyValuePair<Range<TKey>, TValue>> GetSamples(int maxSampleCount) {
			var increment = Math.Max(1, this.items.Count / maxSampleCount);
			for (var i = 0; i < this.items.Count; i += increment) {
				yield return this.items[i];
			}
		}

		private void MergeIfAdjacent(int left, int right) {
			Debug.Assert((right - left) == 1);
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

		/// <summary>Enumerates the range keys and values intersecting the given slice range.</summary>
		/// <param name="from">The lower bound of the range (inclusive).</param>
		/// <param name="to">The upper bound of the range (inclusive).</param>
		/// <returns>An enumerator that allows foreach to be used to process slice in this collection.</returns>
		/// <remarks>This operation runs in O(n) time (worst), where n is the number of range key and value pairs)</remarks>
		public IEnumerable<KeyValuePair<Range<TKey>, TValue>> Slice(TKey from, TKey to) {
			return this.Slice(new Range<TKey>(from, to));
		}

		/// <summary>Enumerates the range keys and values intersecting the given slice range.</summary>
		/// <param name="range">The key range.</param>
		/// <returns>An enumerator that allows foreach to be used to process slice in this collection.</returns>
		/// <remarks>This operation runs in O(n) time (worst), where n is the number of range key and value pairs)</remarks>
		[Pure]
		public IEnumerable<KeyValuePair<Range<TKey>, TValue>> Slice(Range<TKey> range) {
			switch (this.items.Count) {
			case 0:
				break;
			case 1:
				var single = this.items[0];
				var singleFrom = Incrementor<TKey>.Max(single.Key.From, range.From);
				var singleTo = Incrementor<TKey>.Min(single.Key.To, range.To);
				if (singleFrom.CompareTo(singleTo) <= 0) {
					yield return new KeyValuePair<Range<TKey>, TValue>(new Range<TKey>(singleFrom, singleTo), single.Value);
				}
				break;
			default:
				var left = RangeOperations<TKey>.BinarySearch(this.Keys, range.From);
				if (left < 0) {
					left = ~left;
				}
				var right = RangeOperations<TKey>.BinarySearch(this.Keys, range.From);
				if (right < 0) {
					right = ~right - 1;
				}
				for (var i = left; i <= right; i++) {
					var current = this.items[i];
					var from = current.Key.From;
					var to = current.Key.To;
					yield return new KeyValuePair<Range<TKey>, TValue>(
							new Range<TKey>((i == left) && (@from.CompareTo(range.From) < 0) ? range.From : from, (i == right) && (to.CompareTo(range.To) > 0) ? range.To : to),
							current.Value);
				}
				break;
			}
		}

		/// <summary>Attempts to get a value from the given key.</summary>
		/// <param name="key">The key.</param>
		/// <param name="value">[out] The value.</param>
		/// <returns><c>true</c> if it succeeds, <c>false</c> if it fails.</returns>
		/// <remarks>This operation takes O(log n) time, where n is the number of range key and value pairs)</remarks>
		[Pure]
		public bool TryGetValue(TKey key, out TValue value) {
			var index = RangeOperations<TKey>.BinarySearch(this.Keys, key);
			if (index >= 0) {
				value = this.items[index].Value;
				return true;
			}
			value = default;
			return false;
		}
	}
}
