using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sirius.Collections {
	public static class LinqExtensions {
		private static readonly ConcurrentDictionary<Type, MethodInfo> indexerGetter = new ConcurrentDictionary<Type, MethodInfo>();

		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> that, T itemToPrepend) {
			yield return itemToPrepend;
			foreach (var item in that) {
				yield return item;
			}
		}

		public static IEnumerable<T> Append<T>(this IEnumerable<T> that, T itemToAppend) {
			foreach (var item in that) {
				yield return item;
			}
			yield return itemToAppend;
		}

		public static IEnumerable<T> ExceptAt<T>(this IEnumerable<T> that, int index) {
			foreach (var item in that) {
				if (index != 0) {
					yield return item;
				}
				index--;
			}
		}

		public static int IndexOf<T>(this IEnumerable<T> that, T value) {
			return that.IndexOf(value, 0, int.MaxValue, null);
		}

		public static int IndexOf<T>(this IEnumerable<T> that, T value, int startIndex) {
			return that.IndexOf(value, startIndex, int.MaxValue, null);
		}

		public static int IndexOf<T>(this IEnumerable<T> that, T value, int startIndex, int count) {
			return that.IndexOf(value, startIndex, count, null);
		}

		public static int IndexOf<T>(this IEnumerable<T> that, T value, IEqualityComparer<T> comparer) {
			return that.IndexOf(value, 0, int.MaxValue, comparer);
		}

		public static int IndexOf<T>(this IEnumerable<T> that, T value, int startIndex, IEqualityComparer<T> comparer) {
			return that.IndexOf(value, startIndex, int.MaxValue, comparer);
		}

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

		public static int LastIndexOf<T>(this IEnumerable<T> that, T value) {
			return that.LastIndexOf(value, 0, int.MaxValue, null);
		}

		public static int LastIndexOf<T>(this IEnumerable<T> that, T value, int startIndex) {
			return that.LastIndexOf(value, startIndex, int.MaxValue, null);
		}

		public static int LastIndexOf<T>(this IEnumerable<T> that, T value, int startIndex, int count) {
			return that.LastIndexOf(value, startIndex, count, null);
		}

		public static int LastIndexOf<T>(this IEnumerable<T> that, T value, IEqualityComparer<T> comparer) {
			return that.LastIndexOf(value, 0, int.MaxValue, comparer);
		}

		public static int LastIndexOf<T>(this IEnumerable<T> that, T value, int startIndex, IEqualityComparer<T> comparer) {
			return that.LastIndexOf(value, startIndex, int.MaxValue, comparer);
		}

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

		public static bool InstanceEquals<T>(this T that, object other, Func<T, T, bool> equals)
				where T: class {
			if (ReferenceEquals(that, other)) {
				return true;
			}
			if (ReferenceEquals(that, null) || ReferenceEquals(other, null) || (that.GetType() != other.GetType())) {
				return false;
			}
			return equals(that, (T)other);
		}

		public static bool SetEqual<T>(this IEnumerable<T> x, IEnumerable<T> y) {
			return x.SetEqual(y, EqualityComparer<T>.Default);
		}

		public static bool SetEqual<T>(this IEnumerable<T> x, IEnumerable<T> y, IEqualityComparer<T> comparer) {
			if (ReferenceEquals(x, y)) {
				return true;
			}
			if ((x == null) || (y == null)) {
				return false;
			}
			if (x is HashSet<T> xSet) {
				return xSet.SetEquals(y);
			}
			if (y is HashSet<T> ySet) {
				return ySet.SetEquals(x);
			}
			using (var xEnum = x.GetEnumerator()) {
				using (var yEnum = y.GetEnumerator()) {
					var hasX = xEnum.MoveNext();
					var hasY = yEnum.MoveNext();
					if (hasX != hasY) {
						// exactly one of the sets is empty
						return false;
					}
					if (!hasX) {
						// both sets are emtpy
						return true;
					}
					var currX = xEnum.Current;
					var currY = yEnum.Current;
					hasX = xEnum.MoveNext();
					hasY = yEnum.MoveNext();
					if (!hasX && !hasY) {
						// both sets have exactly one item, compare it
						return comparer.Equals(currX, currY);
					}
					xSet = new HashSet<T> {
							currX
					};
					if (hasX) {
						do {
							xSet.Add(xEnum.Current);
						} while (xEnum.MoveNext());
					}
					ySet = new HashSet<T> {
							currY
					};
					if (hasY) {
						do {
							ySet.Add(yEnum.Current);
						} while (yEnum.MoveNext());
					}
				}
			}
			return (xSet.Count == ySet.Count) && xSet.SetEquals(ySet);
		}

		public static bool StructEquals<T>(this T that, object other, Func<T, T, bool> equals)
				where T: struct {
			if (ReferenceEquals(other, null) || (other.GetType() != typeof(T))) {
				return false;
			}
			return equals(that, (T)other);
		}

		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items) {
			return items.ToDictionary(g => g.Key, g => g.Value);
		}

		public static T? GetNext<T>(this IEnumerator<T> that, ref int index)
				where T: struct {
			if (that.MoveNext()) {
				index++;
				return that.Current;
			}
			return default(T?);
		}

		public static T? GetNext<T>(this IEnumerator<T> that)
				where T: struct {
			if (that.MoveNext()) {
				return that.Current;
			}
			return default(T?);
		}

		public static IEnumerable<T> Yield<T>(this T that) {
			yield return that;
		}

		public static TValue? GetValueOrNull<TKey, TValue>(this IDictionary<TKey, TValue> that, TKey key)
				where TValue: struct {
			TValue value;
			if (that.TryGetValue(key, out value)) {
				return value;
			}
			return null;
		}

		public static TValue? GetValueOrNull<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> that, TKey key)
				where TValue: struct {
			TValue value;
			if (that.TryGetValue(key, out value)) {
				return value;
			}
			return null;
		}

		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> that, TKey key, TValue @default = default(TValue)) {
			TValue value;
			if (that.TryGetValue(key, out value)) {
				return value;
			}
			return @default;
		}

		public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> that, TKey key, TValue @default = default(TValue)) {
			TValue value;
			if (that.TryGetValue(key, out value)) {
				return value;
			}
			return @default;
		}

		public static bool AddRange<T>(this ICollection<T> that, IEnumerable<T> items) {
			var count = that.Count;
			var set = that as ISet<T>;
			if (set != null) {
				set.UnionWith(items);
			} else {
				foreach (var item in items) {
					that.Add(item);
				}
			}
			return that.Count > count;
		}

		public static Func<TKey, TValue> CreateGetter<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> that) {
			var indexerGetter = LinqExtensions.indexerGetter.GetOrAdd(typeof(IReadOnlyDictionary<TKey, TValue>), type => Reflect<IReadOnlyDictionary<TKey, TValue>>.GetProperty(d => d[default(TKey)]).GetMethod);
			return (Func<TKey, TValue>)Delegate.CreateDelegate(typeof(Func<TKey, TValue>), that, indexerGetter, true);
		}

		public static Func<TKey, TValue> CreateGetter<TKey, TValue>(this IDictionary<TKey, TValue> that) {
			var indexerGetter = LinqExtensions.indexerGetter.GetOrAdd(typeof(IDictionary<TKey, TValue>), type => Reflect<IDictionary<TKey, TValue>>.GetProperty(d => d[default(TKey)]).GetMethod);
			return (Func<TKey, TValue>)Delegate.CreateDelegate(typeof(Func<TKey, TValue>), that, indexerGetter, true);
		}

		public static Func<int, T> CreateGetter<T>(this IReadOnlyList<T> that) {
			var indexerGetter = LinqExtensions.indexerGetter.GetOrAdd(typeof(IReadOnlyList<T>), type => Reflect<IReadOnlyList<T>>.GetProperty(d => d[default(int)]).GetMethod);
			return (Func<int, T>)Delegate.CreateDelegate(typeof(Func<int, T>), that, indexerGetter, true);
		}

		public static Func<int, T> CreateGetter<T>(this IList<T> that) {
			var indexerGetter = LinqExtensions.indexerGetter.GetOrAdd(typeof(IList<T>), type => Reflect<IList<T>>.GetProperty(d => d[default(int)]).GetMethod);
			return (Func<int, T>)Delegate.CreateDelegate(typeof(Func<int, T>), that, indexerGetter, true);
		}
	}
}
