using System;
using System.Collections.Generic;

namespace Sirius.Collections {
	public class IdComparer<T>: IEqualityComparer<IIdentifiable<T>>, IComparer<IIdentifiable<T>>
			where T: class {
		public static readonly IdComparer<T> Default = new IdComparer<T>();

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

		public bool Equals(IIdentifiable<T> x, IIdentifiable<T> y) {
			if (ReferenceEquals(x, y)) {
				return true;
			}
			if ((x == null) || (y == null)) {
				return false;
			}
			return x.Id.Equals(y.Id);
		}

		public int GetHashCode(IIdentifiable<T> obj) {
			return obj.Id.GetHashCode();
		}
	}
}
