using System;
using System.Collections.Generic;

namespace Sirius.Collections {
	/// <summary>Interface for a set of ranges.</summary>
	/// <typeparam name="T">Generic type parameter.</typeparam>
	/// <remarks>The ranges in the set are expected to be normalized (incremental and non-overlapping/non-adjacent).</remarks>
	public interface IRangeSet<T>: IReadOnlyList<Range<T>>
			where T: IComparable<T> { }
}
