using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Sirius.Collections {
	/// <summary>A <see cref="RangeDictionary{TKey,TValue}" /> content comparer.</summary>
	/// <typeparam name="TKey">Type of the key.</typeparam>
	/// <typeparam name="TValue">Type of the value.</typeparam>
	public class RangeDictionaryComparer<TKey, TValue>: IEqualityComparer<RangeDictionary<TKey, TValue>>
			where TKey: IComparable<TKey> {
		/// <summary>The default instance.</summary>
		public static readonly RangeDictionaryComparer<TKey, TValue> Default = new RangeDictionaryComparer<TKey, TValue>();

		/// <summary>Default constructor.</summary>
		public RangeDictionaryComparer(): this(null) { }

		/// <summary>Constructor.</summary>
		/// <param name="valueEqualityComparer">The value equality comparer.</param>
		public RangeDictionaryComparer(IEqualityComparer<TValue> valueEqualityComparer) {
			this.ValueComparer = valueEqualityComparer;
		}

		/// <summary>Gets the value equality comparer.</summary>
		/// <value>The value equality comparer.</value>
		public IEqualityComparer<TValue> ValueComparer {
			get;
		}

		/// <summary>Determines whether the specified objects are equal.</summary>
		/// <param name="x">The first object of type <see cref="RangeDictionary{TKey, TValue}"/> to compare.</param>
		/// <param name="y">The second object of type <see cref="RangeDictionary{TKey, TValue}"/> to compare.</param>
		/// <returns>true if the specified objects are equal; otherwise, false.</returns>
		[Pure]
		public bool Equals(RangeDictionary<TKey, TValue> x, RangeDictionary<TKey, TValue> y) {
			if (ReferenceEquals(x, y)) {
				return true;
			}
			if ((x == null) || (y == null)) {
				return false;
			}
			return x.ContentEquals(y, this.ValueComparer);
		}

		/// <summary>Returns a hash code for the specified object.</summary>
		/// <exception cref="T:System.ArgumentNullException">
		///     The type of <paramref name="obj" /> is a reference type and
		///     <paramref name="obj" /> is null.
		/// </exception>
		/// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		[Pure]
		public int GetHashCode(RangeDictionary<TKey, TValue> obj) {
			var valueEqualityComparer = this.ValueComparer ?? obj.ValueComparer;
			return obj.GetSamples(10).Aggregate(397, (hash, item) => unchecked(hash * 3 + item.Key.From.GetHashCode() + item.Key.To.GetHashCode() + valueEqualityComparer.GetHashCode(item.Value)));
		}
	}
}
