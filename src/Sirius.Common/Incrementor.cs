using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

using Sirius.Collections;
using Sirius.Unicode;

namespace Sirius {
	/// <summary>The <see cref="Incrementor{T}"/> class provides basic integral functionality in a generic fashion for both built-in types such as <see cref="int"/> or <see cref="char"/> as well as for custom types such as <see cref="Codepoint"/>.</summary>
	/// <typeparam name="T">Generic type parameter.</typeparam>
	/// <remarks>The <see cref="Range{T}"/> and <see cref="RangeSet{T}"/> rely on the ability to compare and shift range boundaries, which they perform using this <see cref="Incrementor{T}"/> class.</remarks>
	public static class Incrementor<T>
			where T: IComparable<T> {
		private static readonly Lazy<Func<T, T>> increment = new Lazy<Func<T, T>>(() => {
			var parameter = Expression.Parameter(typeof(T));
			var incrementableInterface = typeof(T).GetInterfaces().SingleOrDefault(t => t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(IIncrementable<>)) && (t.GetGenericArguments()[0] == typeof(T)));
			Expression incrementExpr;
			if (incrementableInterface != null) {
				incrementExpr = Expression.Call(parameter, incrementableInterface.GetMethod("Increment"));
			} else if ((typeof(T) == typeof(char)) || (typeof(T) == typeof(byte))) {
				incrementExpr = Expression.Convert(Expression.Increment(Expression.Convert(parameter, typeof(ushort))), typeof(T));
			} else if (typeof(T) == typeof(sbyte)) {
				incrementExpr = Expression.Convert(Expression.Increment(Expression.Convert(parameter, typeof(short))), typeof(T));
			} else {
				incrementExpr = Expression.Increment(parameter);
			}
			return Expression.Lambda<Func<T, T>>(incrementExpr, parameter).Compile();
		}, LazyThreadSafetyMode.PublicationOnly);

		private static readonly Lazy<T> minValue = new Lazy<T>(() => GetMemberValue("MinValue"), LazyThreadSafetyMode.PublicationOnly);

		private static readonly Lazy<T> maxValue = new Lazy<T>(() => GetMemberValue("MaxValue"), LazyThreadSafetyMode.PublicationOnly);

		private static readonly Lazy<Func<T, T>> decrement = new Lazy<Func<T, T>>(() => {
			var parameter = Expression.Parameter(typeof(T));
			var incrementableInterface = typeof(T).GetInterfaces().SingleOrDefault(t => t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(IIncrementable<>)) && (t.GetGenericArguments()[0] == typeof(T)));
			Expression decrementExpr;
			if (incrementableInterface != null) {
				decrementExpr = Expression.Call(parameter, incrementableInterface.GetMethod("Decrement"));
			} else if ((typeof(T) == typeof(char)) || (typeof(T) == typeof(byte))) {
				decrementExpr = Expression.Convert(Expression.Decrement(Expression.Convert(parameter, typeof(ushort))), typeof(T));
			} else if (typeof(T) == typeof(sbyte)) {
				decrementExpr = Expression.Convert(Expression.Decrement(Expression.Convert(parameter, typeof(short))), typeof(T));
			} else {
				decrementExpr = Expression.Decrement(parameter);
			}
			return Expression.Lambda<Func<T, T>>(decrementExpr, parameter).Compile();
		}, LazyThreadSafetyMode.PublicationOnly);

		/// <summary>Value decrementor.</summary>
		public static Func<T, T> Increment => increment.Value;

		/// <summary>Value incrementor.</summary>
		public static Func<T, T> Decrement => decrement.Value;

		/// <summary>Gets the minimum value.</summary>
		/// <value>The minimum value.</value>
		public static T MinValue => minValue.Value;

		/// <summary>Gets the maximum value.</summary>
		/// <value>The maximum value.</value>
		public static T MaxValue => maxValue.Value;

		/// <summary>Tests two values for adjacency.</summary>
		/// <param name="x">A value to process.</param>
		/// <param name="y">A value to process.</param>
		/// <returns><c>true</c> if the values are adjacent (they are different but not more than one increment/decrement apart), <c>false</c> otherwise.</returns>
		public static bool Adjacent(T x, T y) {
			if (x.CompareTo(y) > 0) {
				return Adjacent(y, x);
			}
			if (x.CompareTo(MaxValue) < 0) {
				return Increment(x).CompareTo(y) == 0;
			}
			if (y.CompareTo(MinValue) > 0) {
				return x.CompareTo(Decrement(y)) == 0;
			}
			return false;
		}

		/// <summary>Determines the maximum of the given parameters.</summary>
		/// <param name="x">A value to process.</param>
		/// <param name="y">A value to process.</param>
		/// <returns>The maximum value.</returns>
		[Pure]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Max(T x, T y) {
			return x.CompareTo(y) > 0 ? y : x;
		}

		/// <summary>Determines the minimum of the given parameters.</summary>
		/// <param name="x">A value to process.</param>
		/// <param name="y">A value to process.</param>
		/// <returns>The minimum value.</returns>
		[Pure]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Min(T x, T y) {
			return y.CompareTo(x) > 0 ? y : x;
		}

		private static T GetMemberValue(string name) {
			var memberInfo = typeof(T).GetMember(name, BindingFlags.Static|BindingFlags.Public).Single();
			var field = memberInfo as FieldInfo;
			if (field != null) {
				return (T)field.GetValue(null);
			}
			var property = memberInfo as PropertyInfo;
			if (property != null) {
				return (T)property.GetValue(null, null);
			}
			throw new NotSupportedException(string.Format("Cannot get {0} member on type {1}", name, typeof(T).FullName));
		}
	}
}
