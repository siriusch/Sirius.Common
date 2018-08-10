using System.Collections.Generic;
using System.Linq;

namespace Sirius.Collections {
	public class SequenceEqualityComparer<T>: IEqualityComparer<IEnumerable<T>> {
		public static readonly SequenceEqualityComparer<T> Default = new SequenceEqualityComparer<T>(8, EqualityComparer<T>.Default);

		private readonly int hashCount;
		private readonly IEqualityComparer<T> comparer;

		public SequenceEqualityComparer(int hashCount, IEqualityComparer<T> comparer) {
			this.hashCount = hashCount;
			this.comparer = comparer;
		}

		public bool Equals(IEnumerable<T> x, IEnumerable<T> y) {
			if (ReferenceEquals(x, y)) {
				return true;
			}
			if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) {
				return false;
			}
			return x.SequenceEqual(y, this.comparer);
		}

		public int GetHashCode(IEnumerable<T> obj) {
			return obj.Take(this.hashCount).Aggregate(GetType().GetHashCode(), (hc, item) => unchecked((hc*397)^item.GetHashCode()));
		}
	}
}