using System;
using System.Collections.Generic;

namespace Sirius.Collections {
	public interface IRangeSet<T>: IReadOnlyList<Range<T>>
			where T: IComparable<T> { }
}
