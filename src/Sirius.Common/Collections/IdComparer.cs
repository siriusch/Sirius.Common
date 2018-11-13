using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Sirius.Collections {
	/// <summary>A comparer for <see cref="IIdentifiable{T}"/> objects.</summary>
	/// <typeparam name="T">Generic type parameter, must be a class implementing <see cref="IIdentifiable{T}"/>.</typeparam>
	public class IdComparer<T>: IEqualityComparer<IIdentifiable<T>>, IComparer<IIdentifiable<T>>
			where T: class {
		/// <summary>The default instance.</summary>
		public static readonly IdComparer<T> Default = new IdComparer<T>();

		/// <summary>Compares identities of two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		/// 	A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is
		/// 	less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
		/// </returns>
		[Pure]
		public int Compare(IIdentifiable<T> x, IIdentifiable<T> y) {
			if (ReferenceEquals(x, y)) {
				return 0;
			}
			if (x == null) {
				return -1;
			}
			if (y == null) {
				return 1;
			}
			return x.Id.CompareTo(y.Id);
		}

		/// <summary>Determines whether the specified object identities are equal.</summary>
		/// <param name="x">The first object of type <typeparamref name="T" /> to compare.</param>
		/// <param name="y">The second object of type <typeparamref name="T" /> to compare.</param>
		/// <returns>true if the specified objects are equal; otherwise, false.</returns>
		[Pure]
		public bool Equals(IIdentifiable<T> x, IIdentifiable<T> y) {
			if (ReferenceEquals(x, y)) {
				return true;
			}
			if ((x == null) || (y == null)) {
				return false;
			}
			return x.Id.Equals(y.Id);
		}

		/// <summary>Returns a hash code for the specified object identity.</summary>
		/// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj" /> is a reference type and <paramref name="obj" /> is null.</exception>
		/// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		[Pure]
		public int GetHashCode(IIdentifiable<T> obj) {
			return obj.Id.GetHashCode();
		}
	}
}
