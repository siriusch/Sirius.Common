using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Sirius {
	/// <summary>A state key, where the state is a <see cref="int"/>.</summary>
	/// <typeparam name="T">Generic type parameter.</typeparam>
	/// <remarks>This immutable struct is optimized to be used as key of sets or dictionaries.</remarks>
	public struct StateKey<T>: IEquatable<StateKey<T>> where T: IEquatable<T> {
		/// <summary>Constructor.</summary>
		/// <param name="state">The state.</param>
		/// <param name="value">The value.</param>
		public StateKey(int state, T value) {
			this.State = state;
			this.Value = value;
		}

		/// <summary>Gets the state.</summary>
		/// <value>The state.</value>
		public int State {
			get;
		}

		/// <summary>Gets the value.</summary>
		/// <value>The value.</value>
		public T Value {
			get;
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public bool Equals(StateKey<T> other) {
			return (this.State == other.State) && this.Value.Equals(other.Value);
		}

		/// <summary>Indicates whether this instance and a specified object are equal.</summary>
		/// <param name="obj">The object to compare with the current instance.</param>
		/// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
		[Pure]
		public override bool Equals(object obj) {
			return obj is StateKey<T> other && Equals(other);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		[Pure]
		public override int GetHashCode() {
			unchecked {
				return (this.State * 397)^this.Value.GetHashCode();
			}
		}

		/// <summary>Returns a textual representation of this key.</summary>
		/// <returns>A textual representation.</returns>
		[Pure]
		public override string ToString() {
			return $"{this.State}:{this.Value}";
		}
	}
}
