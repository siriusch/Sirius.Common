using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Sirius.Collections {
	/// <summary>Extension methods for enumerations.</summary>
	public static class LinqExtensions {
		private static readonly ConcurrentDictionary<Type, MethodInfo> indexerGetter = new ConcurrentDictionary<Type, MethodInfo>();

		/// <summary>Prepends an element to the enumeration.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="itemToPrepend">The item to prepend.</param>
		/// <returns>An enumeration with the given item prepended.</returns>
		[Pure]
		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> that, T itemToPrepend) {
			yield return itemToPrepend;
			foreach (var item in that) {
				yield return item;
			}
		}

		/// <summary>Appends an element to the enumeration.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="itemToAppend">The item to append.</param>
		/// <returns>An enumeration with the given item appended.</returns>
		[Pure]
		public static IEnumerable<T> Append<T>(this IEnumerable<T> that, T itemToAppend) {
			foreach (var item in that) {
				yield return item;
			}
			yield return itemToAppend;
		}

		/// <summary>Enumeration with an item at a specific index skipped.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="index">Zero-based index of the item to skip.</param>
		/// <returns>An enumeration.</returns>
		[Pure]
		public static IEnumerable<T> ExceptAt<T>(this IEnumerable<T> that, int index) {
			foreach (var item in that) {
				if (index != 0) {
					yield return item;
				}
				index--;
			}
		}

		/// <summary>Searches for the index of the first match in an enumeration.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="value">The value to search for.</param>
		/// <returns>The zero-based index of the value in the collection if found, -1 otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static int IndexOf<T>(this IEnumerable<T> that, T value) {
			return that.IndexOf(value, 0, int.MaxValue, null);
		}

		/// <summary>Searches for the index of the first match in an enumeration.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="startIndex">The start index.</param>
		/// <returns>The zero-based index of the value in the collection if found, -1 otherwise.</returns>
		/// <remarks>Unlike <see cref="Array.IndexOf(System.Array,object,int)"/>, the <paramref name="startIndex"/> may exceed the number of items in the enumeration.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static int IndexOf<T>(this IEnumerable<T> that, T value, int startIndex) {
			return that.IndexOf(value, startIndex, int.MaxValue, null);
		}

		/// <summary>Searches for the index of the first match in an enumeration.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="count">Number of items to search.</param>
		/// <returns>The zero-based index of the value in the collection if found, -1 otherwise.</returns>
		/// <remarks>Unlike <see cref="Array.IndexOf(System.Array,object,int,int)"/>, the <paramref name="startIndex"/> and <paramref name="count"/> may exceed the number of items in the enumeration.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static int IndexOf<T>(this IEnumerable<T> that, T value, int startIndex, int count) {
			return that.IndexOf(value, startIndex, count, null);
		}

		/// <summary>Searches for the index of the first match in an enumeration.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="comparer">The comparer to use.</param>
		/// <returns>The zero-based index of the value in the collection if found, -1 otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static int IndexOf<T>(this IEnumerable<T> that, T value, IEqualityComparer<T> comparer) {
			return that.IndexOf(value, 0, int.MaxValue, comparer);
		}

		/// <summary>Searches for the index of the first match in an enumeration.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="comparer">The comparer to use.</param>
		/// <returns>The zero-based index of the value in the collection if found, -1 otherwise.</returns>
		/// <remarks>Unlike <see cref="Array.IndexOf(System.Array,object,int)"/>, the <paramref name="startIndex"/> may exceed the number of items in the enumeration.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static int IndexOf<T>(this IEnumerable<T> that, T value, int startIndex, IEqualityComparer<T> comparer) {
			return that.IndexOf(value, startIndex, int.MaxValue, comparer);
		}

		/// <summary>Searches for the index of the first match in an enumeration.</summary>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="count">Number of items to search.</param>
		/// <param name="comparer">The comparer to use.</param>
		/// <returns>The zero-based index of the value in the collection if found, -1 otherwise.</returns>
		/// <remarks>Unlike <see cref="Array.IndexOf(System.Array,object,int,int)"/>, the <paramref name="startIndex"/> and <paramref name="count"/> may exceed the number of items in the enumeration.</remarks>
		[Pure]
		public static int IndexOf<T>(this IEnumerable<T> that, T value, int startIndex, int count, IEqualityComparer<T> comparer) {
			if (startIndex < 0) {
				throw new ArgumentOutOfRangeException(nameof(startIndex));
			}
			if (count < 0) {
				throw new ArgumentOutOfRangeException(nameof(count));
			}
			if (comparer == null) {
				comparer = EqualityComparer<T>.Default;
			}
			using (var enumerator = that.Skip(startIndex).GetEnumerator()) {
				while (count-- > 0 && enumerator.MoveNext()) {
					if (comparer.Equals(value, enumerator.Current)) {
						return startIndex;
					}
					startIndex++;
				}
			}
			return -1;
		}

		/// <summary>Searches for the index of the last match in an enumeration.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="value">The value to search for.</param>
		/// <returns>The zero-based index of the value in the collection if found, -1 otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static int LastIndexOf<T>(this IEnumerable<T> that, T value) {
			return that.LastIndexOf(value, 0, int.MaxValue, null);
		}

		/// <summary>Searches for the index of the last match in an enumeration.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="startIndex">The start index.</param>
		/// <returns>The zero-based index of the value in the collection if found, -1 otherwise.</returns>
		/// <remarks>Unlike <see cref="Array.LastIndexOf(System.Array,object,int)"/>, the <paramref name="startIndex"/> may exceed the number of items in the enumeration.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static int LastIndexOf<T>(this IEnumerable<T> that, T value, int startIndex) {
			return that.LastIndexOf(value, startIndex, int.MaxValue, null);
		}

		/// <summary>Searches for the index of the last match in an enumeration.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="count">Number of items to search.</param>
		/// <returns>The zero-based index of the value in the collection if found, -1 otherwise.</returns>
		/// <remarks>Unlike <see cref="Array.LastIndexOf(System.Array,object,int,int)"/>, the <paramref name="startIndex"/> and <paramref name="count"/> may exceed the number of items in the enumeration.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static int LastIndexOf<T>(this IEnumerable<T> that, T value, int startIndex, int count) {
			return that.LastIndexOf(value, startIndex, count, null);
		}

		/// <summary>Searches for the index of the last match in an enumeration.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="comparer">The comparer to use.</param>
		/// <returns>The zero-based index of the value in the collection if found, -1 otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static int LastIndexOf<T>(this IEnumerable<T> that, T value, IEqualityComparer<T> comparer) {
			return that.LastIndexOf(value, 0, int.MaxValue, comparer);
		}

		/// <summary>Searches for the index of the last match in an enumeration.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="comparer">The comparer to use.</param>
		/// <returns>The zero-based index of the value in the collection if found, -1 otherwise.</returns>
		/// <remarks>Unlike <see cref="Array.LastIndexOf(System.Array,object,int)"/>, the <paramref name="startIndex"/> may exceed the number of items in the enumeration.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static int LastIndexOf<T>(this IEnumerable<T> that, T value, int startIndex, IEqualityComparer<T> comparer) {
			return that.LastIndexOf(value, startIndex, int.MaxValue, comparer);
		}

		/// <summary>Searches for the index of the last match in an enumeration.</summary>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="count">Number of items to search.</param>
		/// <param name="comparer">The comparer to use.</param>
		/// <returns>The zero-based index of the value in the collection if found, -1 otherwise.</returns>
		/// <remarks>Unlike <see cref="Array.LastIndexOf(System.Array,object,int,int)"/>, the <paramref name="startIndex"/> and <paramref name="count"/> may exceed the number of items in the enumeration.</remarks>
		[Pure]
		public static int LastIndexOf<T>(this IEnumerable<T> that, T value, int startIndex, int count, IEqualityComparer<T> comparer) {
			if (startIndex < 0) {
				throw new ArgumentOutOfRangeException(nameof(startIndex));
			}
			if (count < 0) {
				throw new ArgumentOutOfRangeException(nameof(count));
			}
			if (comparer == null) {
				comparer = EqualityComparer<T>.Default;
			}
			var result = -1;
			using (var enumerator = that.Skip(startIndex).GetEnumerator()) {
				while (count-- > 0 && enumerator.MoveNext()) {
					if (comparer.Equals(value, enumerator.Current)) {
						result = startIndex;
					}
					startIndex++;
				}
			}
			return result;
		}

		/// <summary>Performs an equality comparison where the object passed into <paramref name="other"/> must be of the same type.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="other">The other.</param>
		/// <param name="equalityComparison">The equality comparison.</param>
		/// <returns>True if it succeeds, false if it fails.</returns>
		[Pure]
		public static bool InstanceEquals<T>(this T that, object other, Func<T, T, bool> equalityComparison = null)
				where T: class {
			if (ReferenceEquals(that, other)) {
				return true;
			}
			if (ReferenceEquals(that, null) || ReferenceEquals(other, null) || (that.GetType() != other.GetType())) {
				return false;
			}
			if (equalityComparison == null) {
				return EqualityComparer<T>.Default.Equals(that, (T)other);
			}
			return equalityComparison(that, (T)other);
		}

		/// <summary>Performs an equality comparison where the object passed into <paramref name="other"/> must be of the same type.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="other">The other.</param>
		/// <param name="equalityComparison">The equality comparison.</param>
		/// <returns>True if it succeeds, false if it fails.</returns>
		[Pure]
		public static bool StructEquals<T>(this T that, object other, Func<T, T, bool> equalityComparison)
				where T : struct {
			if (ReferenceEquals(other, null) || (other.GetType() != typeof(T))) {
				return false;
			}
			if (equalityComparison == null) {
				return EqualityComparer<T>.Default.Equals(that, (T)other);
			}
			return equalityComparison(that, (T)other);
		}

		/// <summary>Check if two enumerations contain the same set of items (unordered, ignoring duplicates).</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="other">The enumeration to compare against.</param>
		/// <returns>True if the enumerations contain the same items, false otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool SetEqual<T>(this IEnumerable<T> that, IEnumerable<T> other) {
			return that.SetEqual(other, null);
		}

		/// <summary>Check if two enumerations contain the same set of items (unordered, ignoring duplicates).</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumeration to act on.</param>
		/// <param name="other">The enumeration to compare against.</param>
		/// <param name="equalityComparer">The equality comparer to use.</param>
		/// <returns>True if the enumerations contain the same items, false otherwise.</returns>
		[Pure]
		public static bool SetEqual<T>(this IEnumerable<T> that, IEnumerable<T> other, IEqualityComparer<T> equalityComparer) {
			if (ReferenceEquals(that, other)) {
				return true;
			}
			if ((that == null) || (other == null)) {
				return false;
			}
			if (equalityComparer == null) {
				equalityComparer = EqualityComparer<T>.Default;
			}
			if (that is HashSet<T> xSet && equalityComparer.Equals(xSet.Comparer)) {
				return xSet.SetEquals(other);
			}
			if (other is HashSet<T> ySet && equalityComparer.Equals(ySet.Comparer)) {
				return ySet.SetEquals(that);
			}
			using (var xEnum = that.GetEnumerator()) {
				using (var yEnum = other.GetEnumerator()) {
					var hasX = xEnum.MoveNext();
					var hasY = yEnum.MoveNext();
					if (hasX != hasY) {
						// exactly one of the sets is empty
						return false;
					}
					if (!hasX) {
						// both sets are empty
						return true;
					}
					var currentX = xEnum.Current;
					var currentY = yEnum.Current;
					hasX = xEnum.MoveNext();
					hasY = yEnum.MoveNext();
					if (!hasX && !hasY) {
						// both sets have exactly one item, compare it
						return equalityComparer.Equals(currentX, currentY);
					}
					if (!hasX) {
						// we have a single x but multiple y
						do {
							if (!equalityComparer.Equals(currentX, yEnum.Current)) {
								return false;
							}
						} while (yEnum.MoveNext());
						return true;
					}
					if (!hasY) {
						// we have multiple x but a single y
						do {
							if (!equalityComparer.Equals(xEnum.Current, currentX)) {
								return false;
							}
						} while (xEnum.MoveNext());
						return true;
					}
					xSet = new HashSet<T>(equalityComparer) {
							currentX
					};
					do {
						xSet.Add(xEnum.Current);
					} while (xEnum.MoveNext());
					if (!xSet.Remove(currentY)) {
						return false;
					}
					ySet = new HashSet<T>(equalityComparer) {
							currentY
					};
					do {
						currentY = yEnum.Current;
						if (xSet.Remove(currentY)) {
							ySet.Add(currentY);
						} else if (!ySet.Contains(currentY)) {
							return false;
						}
					} while (yEnum.MoveNext());
					return xSet.Count == 0;
				}
			}
		}

		/// <summary>
		/// 	Creates a <see cref="T:System.Collections.Generic.Dictionary`2" /> from an <see cref="T:System.Collections.Generic.IEnumerable`1" /> containing <see cref="T:KeyValuePair`2"/>.
		/// </summary>
		/// <typeparam name="TKey">Type of the key.</typeparam>
		/// <typeparam name="TValue">Type of the value.</typeparam>
		/// <param name="items">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to create a <see cref="T:System.Collections.Generic.Dictionary`2" /> from.</param>
		/// <returns>Items as a Dictionary&lt;TKey,TValue&gt;</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items) {
			return items.ToDictionary(g => g.Key, g => g.Value);
		}

		/// <summary>
		/// 	Creates a <see cref="T:System.Collections.Generic.Dictionary`2" /> from an <see cref="T:System.Collections.Generic.IEnumerable`1" /> containing <see cref="T:KeyValuePair`2"/>.
		/// </summary>
		/// <typeparam name="TKey">Type of the key.</typeparam>
		/// <typeparam name="TValue">Type of the value.</typeparam>
		/// <param name="items">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to create a <see cref="T:System.Collections.Generic.Dictionary`2" /> from.</param>
		/// <param name="equalityComparer">The equality comparer to use.</param>
		/// <returns>Items as a Dictionary&lt;TKey,TValue&gt;</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items, IEqualityComparer<TKey> equalityComparer) {
			return items.ToDictionary(g => g.Key, g => g.Value, equalityComparer);
		}

		/// <summary>Get the next value type value from an enumerator, and update the index.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumerator to act on.</param>
		/// <param name="index">[in,out] Zero-based index of the item. The index will only be increased if an item is found.</param>
		/// <returns>The next value, or <c>null</c> if these is no next item.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T? GetNext<T>(this IEnumerator<T> that, ref int index)
				where T: struct {
			if (that.MoveNext()) {
				index++;
				return that.Current;
			}
			return default(T?);
		}

		/// <summary>Get the next value type value from an enumerator.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The enumerator to act on.</param>
		/// <returns>The next value, or <c>null</c> if these is no next item.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T? GetNext<T>(this IEnumerator<T> that)
				where T: struct {
			if (that.MoveNext()) {
				return that.Current;
			}
			return default(T?);
		}

		/// <summary>Returns the item as an enumeration.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The item to yield.</param>
		/// <returns>A read-only list containing this one item.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static IReadOnlyList<T> Yield<T>(this T that) {
			return new [] {that};
		}

		/// <summary>Safely get a value for a key from a <see cref="IDictionary{TKey,TValue}"/>, returning <c>null</c> if the key does not exist.</summary>
		/// <typeparam name="TKey">Type of the key.</typeparam>
		/// <typeparam name="TValue">Type of the value.</typeparam>
		/// <param name="that">The dictionary to act on.</param>
		/// <param name="key">The key.</param>
		/// <returns>The value or <c>null</c>.</returns>
		[Pure]
		public static TValue? GetValueOrNull<TKey, TValue>(this IDictionary<TKey, TValue> that, TKey key)
				where TValue: struct {
			if (that.TryGetValue(key, out var value)) {
				return value;
			}
			return null;
		}

		/// <summary>Safely get a value for a key from a <see cref="IReadOnlyDictionary{TKey,TValue}"/>, returning <c>null</c> if the key does not exist.</summary>
		/// <typeparam name="TKey">Type of the key.</typeparam>
		/// <typeparam name="TValue">Type of the value.</typeparam>
		/// <param name="that">The dictionary to act on.</param>
		/// <param name="key">The key.</param>
		/// <returns>The value or <c>null</c>.</returns>
		[Pure]
		public static TValue? GetValueOrNull<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> that, TKey key)
				where TValue: struct {
			if (that.TryGetValue(key, out var value)) {
				return value;
			}
			return null;
		}

		/// <summary>Safely get a value for a key from a <see cref="IDictionary{TKey,TValue}"/>, returning <paramref name="default"/> if the key does not exist.</summary>
		/// <typeparam name="TKey">Type of the key.</typeparam>
		/// <typeparam name="TValue">Type of the value.</typeparam>
		/// <param name="that">The dictionary to act on.</param>
		/// <param name="key">The key.</param>
		/// <param name="default">(Optional) The default value.</param>
		/// <returns>The value or the default value.</returns>
		[Pure]
		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> that, TKey key, TValue @default = default(TValue)) {
			if (that.TryGetValue(key, out var value)) {
				return value;
			}
			return @default;
		}

		/// <summary>Safely get a value for a key from a <see cref="IDictionary{TKey,TValue}"/>, returning <paramref name="default"/> if the key does not exist.</summary>
		/// <typeparam name="TKey">Type of the key.</typeparam>
		/// <typeparam name="TValue">Type of the value.</typeparam>
		/// <param name="that">The dictionary to act on.</param>
		/// <param name="key">The key.</param>
		/// <param name="default">(Optional) The default value.</param>
		/// <returns>The value or the default value.</returns>
		[Pure]
		public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> that, TKey key, TValue @default = default(TValue)) {
			if (that.TryGetValue(key, out var value)) {
				return value;
			}
			return @default;
		}

		/// <summary>Add a range of items to any writable <see cref="ICollection{T}"/>.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The collection to act on.</param>
		/// <param name="items">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to add to the collection.</param>
		/// <returns>True if items have been added, false otherwise.</returns>
		/// <remarks>If the collection ignores duplicate entries (such as a set), the result may be false even if the <paramref name="items"/> enumeration is not empty.</remarks>
		public static bool AddRange<T>(this ICollection<T> that, IEnumerable<T> items) {
			var count = that.Count;
			if (that is ISet<T> set) {
				set.UnionWith(items);
			} else {
				foreach (var item in items) {
					that.Add(item);
				}
			}
			return that.Count > count;
		}

		/// <summary>Create a delegate bound to the getter of the given <see cref="IReadOnlyDictionary{TKey,TValue}"/>.</summary>
		/// <typeparam name="TKey">Type of the key.</typeparam>
		/// <typeparam name="TValue">Type of the value.</typeparam>
		/// <param name="that">The dictionary to act on.</param>
		/// <returns>A getter function.</returns>
		[Pure]
		public static Func<TKey, TValue> CreateGetter<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> that) {
			var indexerGetter = LinqExtensions.indexerGetter.GetOrAdd(typeof(IReadOnlyDictionary<TKey, TValue>), type => Reflect<IReadOnlyDictionary<TKey, TValue>>.GetProperty(d => d[default(TKey)]).GetMethod);
			return (Func<TKey, TValue>)Delegate.CreateDelegate(typeof(Func<TKey, TValue>), that, indexerGetter, true);
		}

		/// <summary>Create a delegate bound to the getter of the given <see cref="IDictionary{TKey,TValue}"/>.</summary>
		/// <typeparam name="TKey">Type of the key.</typeparam>
		/// <typeparam name="TValue">Type of the value.</typeparam>
		/// <param name="that">The dictionary to act on.</param>
		/// <returns>A getter function.</returns>
		[Pure]
		public static Func<TKey, TValue> CreateGetter<TKey, TValue>(this IDictionary<TKey, TValue> that) {
			var indexerGetter = LinqExtensions.indexerGetter.GetOrAdd(typeof(IDictionary<TKey, TValue>), type => Reflect<IDictionary<TKey, TValue>>.GetProperty(d => d[default(TKey)]).GetMethod);
			return (Func<TKey, TValue>)Delegate.CreateDelegate(typeof(Func<TKey, TValue>), that, indexerGetter, true);
		}

		/// <summary>Create a delegate bound to the getter of the given <see cref="IReadOnlyList{T}"/>.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The dictionary to act on.</param>
		/// <returns>A getter function.</returns>
		[Pure]
		public static Func<int, T> CreateGetter<T>(this IReadOnlyList<T> that) {
			var indexerGetter = LinqExtensions.indexerGetter.GetOrAdd(typeof(IReadOnlyList<T>), type => Reflect<IReadOnlyList<T>>.GetProperty(d => d[default(int)]).GetMethod);
			return (Func<int, T>)Delegate.CreateDelegate(typeof(Func<int, T>), that, indexerGetter, true);
		}

		/// <summary>Create a delegate bound to the getter of the given <see cref="IList{T}"/>.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The dictionary to act on.</param>
		/// <returns>A getter function.</returns>
		[Pure]
		public static Func<int, T> CreateGetter<T>(this IList<T> that) {
			var indexerGetter = LinqExtensions.indexerGetter.GetOrAdd(typeof(IList<T>), type => Reflect<IList<T>>.GetProperty(d => d[default(int)]).GetMethod);
			return (Func<int, T>)Delegate.CreateDelegate(typeof(Func<int, T>), that, indexerGetter, true);
		}
	}
}
