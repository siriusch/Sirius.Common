using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Sirius {
	/// <summary>A generic, type-safe identifier structure.</summary>
	/// <typeparam name="T">Generic type parameter.</typeparam>
	public struct Id<T>: IComparable, IComparable<Id<T>>, IEquatable<Id<T>>, IIncrementable<Id<T>>
			where T: class {
		private static readonly string name = typeof(T).GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? typeof(T).Name;

		/// <summary>The minimum value.</summary>
		public static readonly Id<T> MinValue = new Id<T>(int.MinValue);

		/// <summary>The maximum value.</summary>
		public static readonly Id<T> MaxValue = new Id<T>(int.MaxValue);

		/// <summary>Equality operator.</summary>
		/// <param name="left">The left id.</param>
		/// <param name="right">The right id.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator ==(Id<T> left, Id<T> right) {
			return left.id == right.id;
		}

		/// <summary>Inequality operator.</summary>
		/// <param name="left">The left id.</param>
		/// <param name="right">The right id.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator !=(Id<T> left, Id<T> right) {
			return left.id != right.id;
		}

		/// <summary>Less-than comparison operator.</summary>
		/// <param name="left">The left id.</param>
		/// <param name="right">The right id.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator <(Id<T> left, Id<T> right) {
			return left.id < right.id;
		}

		/// <summary>Greater-than comparison operator.</summary>
		/// <param name="left">The left id.</param>
		/// <param name="right">The right id.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator >(Id<T> left, Id<T> right) {
			return left.id > right.id;
		}

		/// <summary>Less-than-or-equal comparison operator.</summary>
		/// <param name="left">The left id.</param>
		/// <param name="right">The right id.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator <=(Id<T> left, Id<T> right) {
			return left.id <= right.id;
		}

		/// <summary>Greater-than-or-equal comparison operator.</summary>
		/// <param name="left">The left id.</param>
		/// <param name="right">The right id.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator >=(Id<T> left, Id<T> right) {
			return left.id >= right.id;
		}

		private readonly int id;

		/// <summary>Constructor.</summary>
		/// <param name="id">The numeric value of the identifier.</param>
		public Id(int id) {
			this.id = id;
		}

		/// <summary>Converts this id to its numeric value.</summary>
		/// <returns>The numeric value of the id.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public int ToInt32() {
			return this.id;
		}

		/// <summary>
		///     Compares the current instance with another object of the same type and returns an integer that indicates whether
		///     the current instance precedes, follows, or occurs in the same position in
		///     the sort order as the other object.
		/// </summary>
		/// <exception cref="T:System.ArgumentException">.</exception>
		/// <param name="obj">Another instance to compare.</param>
		/// <returns>
		///     A value that indicates the relative order of the objects being compared. The return value has these meanings: Value
		///     Meaning Less than zero This instance precedes <paramref name="obj" /> in
		///     the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />.
		///     Greater than zero This instance follows <paramref name="obj" /> in the sort
		///     order.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public int CompareTo(object obj) {
			return this.CompareTo((Id<T>)obj);
		}

		/// <summary>
		///     Compares the current instance with another object of the same type and returns an integer that indicates whether
		///     the current instance precedes, follows, or occurs in the same position in
		///     the sort order as the other object.
		/// </summary>
		/// <param name="other">An object to compare with this instance.</param>
		/// <returns>
		///     A value that indicates the relative order of the objects being compared. The return value has these meanings: Value
		///     Meaning Less than zero This instance precedes <paramref name="other" />
		///     in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />.
		///     Greater than zero This instance follows <paramref name="other" /> in the
		///     sort order.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public int CompareTo(Id<T> other) {
			return this.id.CompareTo(other.id);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public bool Equals(Id<T> other) {
			return this.id == other.id;
		}

		/// <summary>Indicates whether this instance and a specified object are equal.</summary>
		/// <param name="obj">An object to compare with this object.</param>
		/// <returns>
		///     true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise,
		///     false.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public override bool Equals(object obj) {
			return (obj is Id<T> other) && this.Equals(other);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public override int GetHashCode() {
			unchecked {
				return (this.id * 389) ^ typeof(T).GetHashCode();
			}
		}

		/// <summary>Returns a textual representation of the id.</summary>
		/// <returns>A textual representation of the id.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public override string ToString() {
			return string.Format("{0}:{1}", name, this.id);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		Id<T> IIncrementable<Id<T>>.Increment() {
			return new Id<T>(this.id + 1);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		Id<T> IIncrementable<Id<T>>.Decrement() {
			return new Id<T>(this.id - 1);
		}
	}
}
