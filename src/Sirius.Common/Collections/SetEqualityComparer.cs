using System;
using System.Collections.Generic;
using System.Linq;

namespace Sirius.Collections {
	public class SetEqualityComparer<T>: IEqualityComparer<IEnumerable<T>> {
		public static readonly SetEqualityComparer<T> Default = new SetEqualityComparer<T>(EqualityComparer<T>.Default);

		private readonly IEqualityComparer<T> comparer;

		public SetEqualityComparer(IEqualityComparer<T> comparer) {
			this.comparer = comparer;
		}

		public bool Equals(IEnumerable<T> x, IEnumerable<T> y) {
			if (ReferenceEquals(x, y)) {
				return true;
			}
			if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) {
				return false;
			}
			return x.SetEqual(y, this.comparer);
		}

		public int GetHashCode(IEnumerable<T> obj) {
			return obj.Aggregate(GetType().GetHashCode(), (hc, item) => hc^item.GetHashCode());
		}
	}
}
