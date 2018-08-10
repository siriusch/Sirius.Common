using System;

namespace Sirius {
	public interface IIncrementable<T>
			where T: IIncrementable<T>, IComparable<T> {
		T Decrement();

		T Increment();
	}
}
