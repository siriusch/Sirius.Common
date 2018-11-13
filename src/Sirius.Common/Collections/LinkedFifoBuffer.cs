using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sirius.Collections {
	/// <summary>Generic FIFO (First-In-First-Out) buffer which grows (and garbage-collects) by the means of a linked list. This class cannot be inherited.</summary>
	/// <typeparam name="T">Generic type parameter.</typeparam>
	public sealed class LinkedFifoBuffer<T> {
		/// <summary>A buffer position.</summary>
		public struct BufferPosition: IEnumerable<T>, IEquatable<BufferPosition> {
			/// <summary>Equality operator.</summary>
			/// <param name="left">The left.</param>
			/// <param name="right">The right.</param>
			/// <returns>The result of the operation.</returns>
			public static bool operator ==(BufferPosition left, BufferPosition right) {
				return left.Equals(right);
			}

			/// <summary>Inequality operator.</summary>
			/// <param name="left">The left.</param>
			/// <param name="right">The right.</param>
			/// <returns>The result of the operation.</returns>
			public static bool operator !=(BufferPosition left, BufferPosition right) {
				return !left.Equals(right);
			}

			/// <summary>Constructor.</summary>
			/// <param name="buffer">The buffer.</param>
			/// <param name="index">Zero-based index into the buffer.</param>
			public BufferPosition(LinkedFifoBuffer<T> buffer, int index) {
				this.Buffer = buffer;
				this.Index = index;
			}

			/// <summary>Gets the buffer.</summary>
			/// <value>The buffer.</value>
			public LinkedFifoBuffer<T> Buffer {
				get;
			}

			/// <summary>Gets the zero-based index of this LinkedFifoBuffer&lt;T&gt;</summary>
			/// <value>The index.</value>
			public int Index {
				get;
			}

			/// <summary>Gets a value indicating whether this LinkedFifoBuffer&lt;T&gt; has data.</summary>
			/// <value>True if this LinkedFifoBuffer&lt;T&gt; has data, false if not.</value>
			public bool HasData => this.Index < this.Buffer.head || (this.Buffer.next?.head > 0);

			/// <summary>Gets the offset (relative to the data, not this specific buffer).</summary>
			/// <value>The offset.</value>
			public long Offset => this.Buffer.offset+this.Index;

			/// <summary>Returns an enumerator that iterates through the collection.</summary>
			/// <returns>An enumerator that can be used to iterate through the collection.</returns>
			[Pure]
			public IEnumerator<T> GetEnumerator() {
				var buffer = this.Buffer;
				var index = this.Index;
				for (;;) {
					if (index < buffer.head) {
						yield return buffer.data[index++];
					} else {
						buffer = buffer.next;
						if (buffer == null) {
							yield break;
						}
						index = 0;
					}
				}
			}

			/// <summary>Gets the enumerator.</summary>
			/// <returns>The enumerator.</returns>
			IEnumerator IEnumerable.GetEnumerator() {
				return GetEnumerator();
			}

			/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
			/// <param name="other">An object to compare with this object.</param>
			/// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
			[Pure]
			public bool Equals(BufferPosition other) {
				return this.Buffer.Equals(other.Buffer) && this.Index == other.Index;
			}

			/// <summary>Advances.</summary>
			/// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
			/// <param name="count">Number of.</param>
			/// <returns>A BufferPosition representing the new position.</returns>
			[Pure]
			public BufferPosition Advance(int count) {
				if (count < 0) {
					throw new ArgumentOutOfRangeException(nameof(count));
				}
				var buffer = this.Buffer;
				var index = this.Index+count;
				while (index > buffer.data.Length) {
					index -= buffer.data.Length;
					buffer = buffer.next;
					if (buffer == null) {
						throw new ArgumentOutOfRangeException(nameof(count));
					}
				}
				if (index > buffer.head) {
					throw new ArgumentOutOfRangeException(nameof(count));
				}
				return new BufferPosition(buffer, index);
			}

			/// <summary>Gets the number of available elements.</summary>
			/// <returns>A long representing the number of items.</returns>
			[Pure]
			public long AvailableCount() {
				var buffer = this.Buffer;
				long result = buffer.head-this.Index;
				while (buffer.InstanceIsFull) {
					buffer = buffer.next;
					if (buffer == null) {
						break;
					}
					result += buffer.head;
				}
				Debug.Assert(buffer?.next == null);
				return result;
			}

			/// <summary>Copies all items to the given array.</summary>
			/// <param name="array">The array.</param>
			/// <param name="index">(Optional) Zero-based index into the array.</param>
			public void CopyTo(T[] array, int index = 0) {
				CopyTo(array, index, null);
			}

			/// <summary>Copies all items to the given array.</summary>
			/// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
			/// <param name="array">The array.</param>
			/// <param name="index">Zero-based index into the array.</param>
			/// <param name="count">Number of items to copy to the array.</param>
			public void CopyTo(T[] array, int index, int? count) {
				if (count < 0) {
					throw new ArgumentOutOfRangeException(nameof(count));
				}
				var buffer = this.Buffer;
				var bufferIndex = this.Index;
				var pendingCount = count.GetValueOrDefault(int.MaxValue);
				do {
					var copyCount = Math.Min(pendingCount, buffer.head-bufferIndex);
					if (copyCount == 0) {
						break;
					}
					Array.Copy(buffer.data, bufferIndex, array, index, copyCount);
					index += copyCount;
					pendingCount -= copyCount;
					buffer = buffer.next;
					bufferIndex = 0;
				} while (buffer != null && pendingCount > 0);
				if (count.HasValue && pendingCount > 0) {
					throw new ArgumentOutOfRangeException(nameof(count));
				}
			}

			/// <summary>Indicates whether this instance and a specified object are equal.</summary>
			/// <param name="obj">The object to compare with the current instance.</param>
			/// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
			[Pure]
			public override bool Equals(object obj) {
				if (ReferenceEquals(null, obj)) {
					return false;
				}
				return obj is BufferPosition && Equals((BufferPosition)obj);
			}

			/// <summary>Returns the hash code for this instance.</summary>
			/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
			[Pure]
			public override int GetHashCode() {
				unchecked {
					return (this.Buffer.GetHashCode() * 397)^this.Index;
				}
			}

			/// <summary>Enumerates the content of the buffer, returning the item along with a <see cref="BufferPosition"/> for each item returned.</summary>
			/// <returns>An enumerator that allows foreach to be used.</returns>
			[Pure]
			public IEnumerable<KeyValuePair<BufferPosition, T>> ReadWithPosition() {
				var buffer = this.Buffer;
				var index = this.Index;
				for (;;) {
					if (index < buffer.head) {
						yield return new KeyValuePair<BufferPosition, T>(new BufferPosition(buffer, index), buffer.data[index++]);
					} else {
						buffer = buffer.next;
						if (buffer == null) {
							yield break;
						}
						index = 0;
					}
				}
			}

			/// <summary>Returns the fully qualified type name of this instance.</summary>
			/// <returns>A <see cref="T:System.String" /> containing a fully qualified type name.</returns>
			[Pure]
			public override string ToString() {
				return this.Offset.ToString();
			}
		}

		/// <summary>The default capacity.</summary>
		/// <remarks>The default is so that about 4000 bytes are consumed (depending on the site of the type of <typeparamref name="T"/>), but at least 100 items.</remarks>
		public static readonly int DefaultCapacity = Math.Max(100, 4000 / Marshal.SizeOf<T>());

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void AssertWritableBuffer(ref LinkedFifoBuffer<T> buffer) {
			if (buffer == null) {
				buffer = new LinkedFifoBuffer<T>(DefaultCapacity, 0);
			} else if (buffer.head >= buffer.data.Length) {
				buffer.next = new LinkedFifoBuffer<T>(buffer.data.Length, buffer.offset+buffer.data.Length);
				buffer = buffer.next;
			}
		}

		/// <summary>Attempts to get a <typeparamref name="T"/> value at the given BufferPosition, and advance the buffer by one position.</summary>
		/// <param name="position">[in,out] The position.</param>
		/// <param name="value">[out] The value.</param>
		/// <returns>True if it succeeds, false if it fails.</returns>
		public static bool TryGetValue(ref BufferPosition position, out T value) {
			if (position.Index < position.Buffer.head) {
				value = position.Buffer.data[position.Index];
				position = new BufferPosition(position.Buffer, position.Index+1);
				return true;
			}
			if (position.Buffer.next?.head > 0) {
				value = position.Buffer.next.data[0];
				position = new BufferPosition(position.Buffer.next, 1);
				return true;
			}
			value = default(T);
			return false;
		}

		/// <summary>Append data to the buffer.</summary>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
		/// <param name="buffer">[in,out] The buffer.</param>
		/// <param name="data">The data.</param>
		/// <param name="index">Zero-based index of the.</param>
		/// <param name="count">Number of data items to write.</param>
		public static void Write(ref LinkedFifoBuffer<T> buffer, T[] data, int index, int count) {
			if (count == 0) {
				return;
			}
			if (count < 0) {
				throw new ArgumentOutOfRangeException(nameof(count));
			}
			do {
				AssertWritableBuffer(ref buffer);
				var size = Math.Min(count, buffer.data.Length-buffer.head);
				Debug.Assert(size > 0);
				Array.Copy(data, index, buffer.data, buffer.head, size);
				buffer.head += size;
				index += size;
				count -= size;
			} while (count > 0);
		}

		/// <summary>Append data to the buffer.</summary>
		/// <param name="buffer">[in,out] The buffer.</param>
		/// <param name="data">The data.</param>
		public static void Write(ref LinkedFifoBuffer<T> buffer, T[] data) {
			Write(ref buffer, data, 0, data.Length);
		}

		/// <summary>Append data to the buffer.</summary>
		/// <param name="buffer">[in,out] The buffer.</param>
		/// <param name="data">The data.</param>
		public static void Write(ref LinkedFifoBuffer<T> buffer, T data) {
			AssertWritableBuffer(ref buffer);
			buffer.data[buffer.head++] = data;
		}

		private readonly T[] data;
		private readonly long offset;
		private int head;
		private LinkedFifoBuffer<T> next;

		/// <summary>Default constructor. Creates a buffer at offset 0 with the <see cref="DefaultCapacity"/></summary>
		public LinkedFifoBuffer(): this(DefaultCapacity, 0) { }

		/// <summary>Creates a buffer at offset 0 with the given capacity.</summary>
		/// <param name="capacity">The capacity.</param>
		public LinkedFifoBuffer(int capacity): this(capacity, 0) { }

		internal LinkedFifoBuffer(int capacity, long offset) {
			this.offset = offset;
			this.data = new T[capacity];
		}

		/// <summary>Gets the buffer instance capacity.</summary>
		/// <value>The instance capacity.</value>
		public int InstanceCapacity => this.data.Length;

		/// <summary>Gets the total offset of the buffer head.</summary>
		/// <value>The head offset.</value>
		public long HeadOffset => this.offset+this.head;

		/// <summary>Gets the head position as <see cref="BufferPosition"/>.</summary>
		/// <value>The head position.</value>
		public BufferPosition HeadPosition => new BufferPosition(this, this.head);

		/// <summary>Gets a value indicating whether the instance is full.</summary>
		/// <value>True if instance is full, false if not.</value>
		public bool InstanceIsFull => this.head == this.data.Length;

		/// <summary>Gets a value indicating whether the instance is the buffer head.</summary>
		/// <value>True if instance is the buffer head, false if not.</value>
		public bool InstanceIsHead => this.next == null;
	}
}
