using System;
using System.Collections.Generic;
using System.Linq;

namespace Sirius.Collections {
	/// <summary>A set equality comparer.</summary>
	/// <typeparam name="T">Generic type parameter.</typeparam>
	public class SetEqualityComparer<T>: IEqualityComparer<IEnumerable<T>> {
		/// <summary>The default instance.</summary>
		public static readonly SetEqualityComparer<T> Default = new SetEqualityComparer<T>(EqualityComparer<T>.Default);

		private readonly IEqualityComparer<T> comparer;

		/// <summary>Constructor.</summary>
		/// <param name="comparer">The value comparer to use.</param>
		public SetEqualityComparer(IEqualityComparer<T> comparer) {
			this.comparer = comparer;
		}

		/// <summary>Determines whether the specified objects are equal.</summary>
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
			return x.SetEqual(y, this.comparer);
		}

		/// <summary>Returns a hash code for the specified object.</summary>
		/// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj" /> is a reference type and <paramref name="obj" /> is null.</exception>
		/// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		public int GetHashCode(IEnumerable<T> obj) {
			return obj.Aggregate(GetType().GetHashCode(), (hc, item) => hc^this.comparer.GetHashCode(item));
		}
	}
}
