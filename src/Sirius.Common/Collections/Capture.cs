using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using JetBrains.Annotations;

namespace Sirius.Collections {
	/// <summary>A capture or slice of data, which can be enumerated and which has a starting index and a count.</summary>
	/// <typeparam name="T">Generic type parameter for the iteration element type.</typeparam>
	public struct Capture<T>: IReadOnlyCollection<T>, ICollection<T> {
		private readonly IEnumerable<T> data;

		/// <summary>Create a new capture.</summary>
		/// <param name="data">The enumeration of the data.</param>
		/// <param name="count">Number of items in the capture.</param>
		/// <param name="index">Zero-based index of the first item to capture.</param>
		/// <remarks>The enumeration in <paramref name="data"/> must return items starting at the <paramref name="index"/> position, and it must be able to return at least <paramref name="count"/> items. The <paramref name="index"/> is only informal for the consumer of the capture and not used to advance the enumeration.</remarks>
		public Capture([NotNull]IEnumerable<T> data, int count, long index) {
			this.data = data;
			this.Count = count;
			this.Index = index;
		}

		/// <summary>Gets the enumerator.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <returns>The enumerator.</returns>
		[Pure]
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

		/// <summary>Gets the enumerator.</summary>
		/// <returns>The enumerator.</returns>
		[Pure]
		IEnumerator IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}

		/// <summary>Gets the number of items in the capture.</summary>
		/// <value>The count.</value>
		public int Count {
			get;
		}

		/// <summary>Gets the zero-based index of the first captured item.</summary>
		/// <value>The index.</value>
		public long Index {
			get;
		}

		/// <summary>Throws a <see cref="NotSupportedException"/> because the capture is read-only.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
		void ICollection<T>.Add(T item) {
			throw new NotSupportedException();
		}

		/// <summary>Throws a <see cref="NotSupportedException"/> because the capture is read-only.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
		void ICollection<T>.Clear() {
			throw new NotSupportedException();
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <returns>true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.</returns>
		[Pure]
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

		/// <summary>
		/// 	Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
		/// </summary>
		/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from
		/// 	<paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The
		/// 	<see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		void ICollection<T>.CopyTo(T[] array, int arrayIndex) {
			if (array == null) {
				throw new ArgumentNullException(nameof(array));
			}
			foreach (var item in this) {
				array[arrayIndex++] = item;
			}
		}

		/// <summary>Throws a <see cref="NotSupportedException"/> because the capture is read-only.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
		bool ICollection<T>.Remove(T item) {
			throw new NotSupportedException();
		}

		bool ICollection<T>.IsReadOnly => true;
	}
}
