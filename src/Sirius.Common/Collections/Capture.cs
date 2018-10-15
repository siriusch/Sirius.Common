using System;
using System.Collections;
using System.Collections.Generic;

namespace Sirius.Collections {
	public struct Capture<T>: IReadOnlyCollection<T>, ICollection<T> {
		private readonly IEnumerable<T> data;

		public Capture(IEnumerable<T> data, int count, long index) {
			this.data = data;
			this.Count = count;
			this.Index = index;
		}

		public IEnumerator<T> GetEnumerator() {
			using (var enumerator = this.data.GetEnumerator()) {
				for (var count = this.Count; count > 0; count--) {
					if (!enumerator.MoveNext()) {
						throw new InvalidOperationException("Unexpected end of capture data");
					}
					yield return enumerator.Current;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}

		public int Count {
			get;
		}

		public long Index {
			get;
		}

		void ICollection<T>.Add(T item) {
			throw new NotSupportedException();
		}

		void ICollection<T>.Clear() {
			throw new NotSupportedException();
		}

		bool ICollection<T>.Contains(T item) {
			using (var enumerator = this.data.GetEnumerator()) {
				for (var count = this.Count; count > 0; count--) {
					if (!enumerator.MoveNext()) {
						throw new InvalidOperationException("Unexpected end of capture data");
					}
					if (EqualityComparer<T>.Default.Equals(item, enumerator.Current)) {
						return true;
					}
				}
			}
			return false;
		}

		void ICollection<T>.CopyTo(T[] array, int arrayIndex) {
			foreach (var item in this) {
				array[arrayIndex++] = item;
			}
		}

		bool ICollection<T>.Remove(T item) {
			throw new NotSupportedException();
		}

		bool ICollection<T>.IsReadOnly => true;
	}
}
