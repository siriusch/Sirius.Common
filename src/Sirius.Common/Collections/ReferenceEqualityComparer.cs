using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sirius.Collections {
	/// <summary>A reference equality comparer.</summary>
	/// <typeparam name="T">Generic type parameter.</typeparam>
	public class ReferenceEqualityComparer<T>: IEqualityComparer<T>
			where T: class {
		/// <summary>The default instance.</summary>
		public static readonly ReferenceEqualityComparer<T> Default = new ReferenceEqualityComparer<T>();

		/// <summary>Determines whether the specified objects are equal.</summary>
		/// <param name="x">The first object of type <typeparamref name="T" /> to compare.</param>
		/// <param name="y">The second object of type <typeparamref name="T" /> to compare.</param>
		/// <returns>true if the specified objects are equal; otherwise, false.</returns>
		public bool Equals(T x, T y) {
			return ReferenceEquals(x, y);
		}

		/// <summary>Returns a hash code for the specified object.</summary>
		/// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj" /> is a reference type and <paramref name="obj" /> is null.</exception>
		/// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		public int GetHashCode(T obj) {
			return RuntimeHelpers.GetHashCode(obj);
		}
	}
}
