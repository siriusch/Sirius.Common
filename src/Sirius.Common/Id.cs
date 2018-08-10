using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Sirius {
	public struct Id<T>: IComparable, IComparable<Id<T>>, IEquatable<Id<T>>, IIncrementable<Id<T>>
			where T: class {
		private static readonly string name = GetName();

		private static string GetName() {
			var displayNameAttribute = typeof(T).GetCustomAttribute<DisplayNameAttribute>();
			if (displayNameAttribute != null) {
				return displayNameAttribute.DisplayName;
			}
			return typeof(T).Name;
		}

		public static readonly Id<T> MinValue = new Id<T>(int.MinValue);
		public static readonly Id<T> MaxValue = new Id<T>(int.MaxValue);

		[Pure]
		public static bool operator ==(Id<T> left, Id<T> right) {
			return left.id == right.id;
		}

		[Pure]
		public static bool operator !=(Id<T> left, Id<T> right) {
			return left.id != right.id;
		}

		[Pure]
		public static bool operator <(Id<T> left, Id<T> right) {
			return left.id < right.id;
		}

		[Pure]
		public static bool operator >(Id<T> left, Id<T> right) {
			return left.id > right.id;
		}

		[Pure]
		public static bool operator <=(Id<T> left, Id<T> right) {
			return left.id <= right.id;
		}

		[Pure]
		public static bool operator >=(Id<T> left, Id<T> right) {
			return left.id >= right.id;
		}

		private readonly int id;

		public Id(int id) {
			this.id = id;
		}

		[Pure]
		public int ToInt32() {
			return this.id;
		}

		[Pure]
		public int CompareTo(object other) {
			return CompareTo((Id<T>)other);
		}

		[Pure]
		public int CompareTo(Id<T> other) {
			return this.id.CompareTo(other.id);
		}

		[Pure]
		public bool Equals(Id<T> other) {
			return this.id == other.id;
		}

		[Pure]
		public override bool Equals(object other) {
			return (other is Id<T>) && Equals((Id<T>)other);
		}

		[Pure]
		public override int GetHashCode() {
			unchecked {
				return (this.id * 389)^typeof(T).GetHashCode();
			}
		}

		[Pure]
		public override string ToString() {
			return string.Format("{0}:{1}", name, this.id);
		}

		[Pure]
		Id<T> IIncrementable<Id<T>>.Increment() {
			return new Id<T>(this.id+1);
		}

		[Pure]
		Id<T> IIncrementable<Id<T>>.Decrement() {
			return new Id<T>(this.id-1);
		}
	}
}
