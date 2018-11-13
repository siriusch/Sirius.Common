using System.Collections.Generic;
using System.Linq;

namespace Sirius.Collections {
	/// <summary>A sequence equality comparer.</summary>
	/// <typeparam name="T">Generic type parameter.</typeparam>
	public class SequenceEqualityComparer<T>: IEqualityComparer<IEnumerable<T>> {
		/// <summary>The default instance with a hash count of 8.</summary>
		public static readonly SequenceEqualityComparer<T> Default = new SequenceEqualityComparer<T>(8, EqualityComparer<T>.Default);

		private readonly int hashCount;
		private readonly IEqualityComparer<T> comparer;

		/// <summary>Constructor.</summary>
		/// <param name="hashCount">Number of hashes.</param>
		/// <param name="comparer">The value comparer to use.</param>
		public SequenceEqualityComparer(int hashCount, IEqualityComparer<T> comparer) {
			this.hashCount = hashCount;
			this.comparer = comparer;
		}

		/// <summary>Determines whether the specified sequences are equal.</summary>
		/// <param name="x">The first object of type <typeparamref name="T" /> to compare.</param>
		/// <param name="y">The second object of type <typeparamref name="T" /> to compare.</param>
		/// <returns>true if the specified objects are equal; otherwise, false.</returns>
		public bool Equals(IEnumerable<T> x, IEnumerable<T> y) {
			if (ReferenceEquals(x, y)) {
				return true;
			}
			if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) {
				return false;
			}
			return x.SequenceEqual(y, this.comparer);
		}

		/// <summary>Returns a hash code for the specified object.</summary>
		/// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj" /> is a reference type and <paramref name="obj" /> is null.</exception>
		/// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		public int GetHashCode(IEnumerable<T> obj) {
			return obj.Take(this.hashCount).Aggregate(GetType().GetHashCode(), (hc, item) => unchecked((hc*397)^this.comparer.GetHashCode(item)));
		}
	}
}
