using System;
using System.Collections.Generic;
using System.Linq;

namespace Sirius.Collections {
	public class ArrayContentEqualityComparer<T>: IEqualityComparer<T[]> {
		public static readonly ArrayContentEqualityComparer<T> Default = new ArrayContentEqualityComparer<T>(8, EqualityComparer<T>.Default);

		private readonly IEqualityComparer<T> comparer;
		private readonly int hashCount;

		public ArrayContentEqualityComparer(int hashCount, IEqualityComparer<T> comparer) {
			this.hashCount = hashCount;
			this.comparer = comparer;
		}

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

		public int GetHashCode(T[] obj) {
			return obj
				.Take(this.hashCount)
				.Aggregate(GetType().GetHashCode(), (hc, item) => unchecked((hc * 3)^item.GetHashCode()));
		}
	}
}
