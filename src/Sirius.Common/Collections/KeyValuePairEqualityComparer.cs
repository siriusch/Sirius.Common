using System.Collections.Generic;

namespace Sirius.Collections {
	/// <summary>An equality comparer for <see cref="KeyValuePair{TKey,TValue}"/> structs.</summary>
	/// <typeparam name="TKey">Generic type parameter of the key.</typeparam>
	/// <typeparam name="TValue">Generic type parameter of the value.</typeparam>
	public class KeyValuePairEqualityComparer<TKey, TValue>: IEqualityComparer<KeyValuePair<TKey, TValue>> {
		private readonly EqualityComparer<TKey> keyComparer;
		private readonly EqualityComparer<TValue> valueComparer;

		/// <summary>The default instance.</summary>
		public static readonly KeyValuePairEqualityComparer<TKey, TValue> Default = new KeyValuePairEqualityComparer<TKey, TValue>(null, null);

		/// <summary>Constructor.</summary>
		public KeyValuePairEqualityComparer(EqualityComparer<TKey> keyComparer, EqualityComparer<TValue> valueComparer) {
			this.keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
			this.valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;
		}

		/// <summary>Determines whether the specified objects are equal.</summary>
		/// <param name="x">The first object of type <see cref="KeyValuePair{TKey,TValue}"/> to compare.</param>
		/// <param name="y">The second object of type <see cref="KeyValuePair{TKey,TValue}"/> to compare.</param>
		/// <returns>true if the specified objects are equal; otherwise, false.</returns>
		public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y) {
			return this.keyComparer.Equals(x.Key, y.Key) && this.valueComparer.Equals(x.Value, y.Value);
		}

		/// <summary>Returns a hash code for the specified object.</summary>
		/// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj" /> is a reference type and <paramref name="obj" /> is null.</exception>
		/// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		public int GetHashCode(KeyValuePair<TKey, TValue> obj) {
			return unchecked(this.keyComparer.GetHashCode(obj.Key) * 7 ^ this.valueComparer.GetHashCode(obj.Value));
		}
	}
}
