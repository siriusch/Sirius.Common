using System;

namespace Sirius {
	/// <summary>Interface for incrementable objects (native support for <see cref="Incrementor{T}"/> methods).</summary>
	/// <typeparam name="T">Generic type parameter.</typeparam>
	/// <remarks>Types implementing this interface are assumed to be immutable and integral-alike.</remarks>
	public interface IIncrementable<T>
			where T: IIncrementable<T>, IComparable<T> {
		/// <summary>Gets the decremented value of itself.</summary>
		/// <returns>The current value minus one.</returns>
		T Decrement();

		/// <summary>Gets the incremented value of itself.</summary>
		/// <returns>The current value plus one.</returns>
		T Increment();
	}
}
