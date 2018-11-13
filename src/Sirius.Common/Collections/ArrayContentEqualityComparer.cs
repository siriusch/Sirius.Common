using System;
using System.Collections.Generic;
using System.Linq;

namespace Sirius.Collections {
	/// <summary>An array content equality comparer.</summary>
	/// <typeparam name="T">Generic type parameter for the array element type.</typeparam>
	public class ArrayContentEqualityComparer<T>: IEqualityComparer<T[]> {
		/// <summary>The default instance with a hash count of 8 elements.</summary>
		public static readonly ArrayContentEqualityComparer<T> Default = new ArrayContentEqualityComparer<T>(8, EqualityComparer<T>.Default);

		private readonly IEqualityComparer<T> comparer;
		private readonly int hashCount;

		/// <summary>Create a new instance.</summary>
		/// <param name="hashCount">Number of array element hashes to take into account.</param>
		/// <param name="comparer">The comparer to use for the array elements, or <c>null</c> for the default equality comparer.</param>
		public ArrayContentEqualityComparer(int hashCount, IEqualityComparer<T> comparer) {
			this.hashCount = hashCount;
			this.comparer = comparer ?? EqualityComparer<T>.Default;
		}

		/// <summary>Determines whether the specified objects are equal.</summary>
		/// <param name="x">The first object of type <typeparamref name="T" /> to compare.</param>
		/// <param name="y">The second object of type <typeparamref name="T" /> to compare.</param>
		/// <returns>true if the specified objects are equal; otherwise, false.</returns>
		public bool Equals(T[] x, T[] y) {
			if (ReferenceEquals(x, y)) {
				return true;
			}
			if ((x == null) || (y == null)) {
				return false;
			}
			if (x.Length != y.Length) {
				return false;
			}
			for (var i = 0; i < x.Length; i++) {
				if (!this.comparer.Equals(x[i], y[i])) {
					return false;
				}
			}
			return true;
		}

		/// <summary>Returns a hash code for the specified object.</summary>
		/// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj" /> is a reference type and <paramref name="obj" /> is null.</exception>
		/// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		public int GetHashCode(T[] obj) {
			return obj
				.Take(this.hashCount)
				.Aggregate(GetType().GetHashCode(), (hc, item) => unchecked((hc * 3)^this.comparer.GetHashCode(item)));
		}
	}
}
