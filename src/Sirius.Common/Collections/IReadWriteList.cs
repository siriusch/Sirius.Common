using System.Collections.Generic;

namespace Sirius.Collections {
	public interface IReadWriteList<T>: IReadOnlyCollection<T> {
		T this[int index] {
			get;
			set;
		}
	}
}
