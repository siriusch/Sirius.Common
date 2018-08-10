using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sirius.Collections {
	public sealed class LinkedFifoBuffer<T> {
		public struct BufferPosition: IEnumerable<T>, IEquatable<BufferPosition> {
			public static bool operator ==(BufferPosition left, BufferPosition right) {
				return left.Equals(right);
			}

			public static bool operator !=(BufferPosition left, BufferPosition right) {
				return !left.Equals(right);
			}

			public BufferPosition(LinkedFifoBuffer<T> buffer, int index) {
				this.Buffer = buffer;
				this.Index = index;
			}

			public LinkedFifoBuffer<T> Buffer {
				get;
			}

			public int Index {
				get;
			}

			public bool HasData => this.Index < this.Buffer.head || (this.Buffer.next?.head > 0);

			public long Offset => this.Buffer.offset+this.Index;

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

			IEnumerator IEnumerable.GetEnumerator() {
				return GetEnumerator();
			}

			public bool Equals(BufferPosition other) {
				return this.Buffer.Equals(other.Buffer) && this.Index == other.Index;
			}

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

			public void CopyTo(T[] array, int index = 0) {
				CopyTo(array, index, null);
			}

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

			public override bool Equals(object obj) {
				if (ReferenceEquals(null, obj)) {
					return false;
				}
				return obj is BufferPosition && Equals((BufferPosition)obj);
			}

			public override int GetHashCode() {
				unchecked {
					return (this.Buffer.GetHashCode() * 397)^this.Index;
				}
			}

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

			public override string ToString() {
				return this.Offset.ToString();
			}
		}

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

		public static void Write(ref LinkedFifoBuffer<T> buffer, T[] data) {
			Write(ref buffer, data, 0, data.Length);
		}

		public static void Write(ref LinkedFifoBuffer<T> buffer, T data) {
			AssertWritableBuffer(ref buffer);
			buffer.data[buffer.head++] = data;
		}

		private readonly T[] data;
		private readonly long offset;
		private int head;
		private LinkedFifoBuffer<T> next;

		public LinkedFifoBuffer(): this(DefaultCapacity, 0) { }

		public LinkedFifoBuffer(int capacity): this(capacity, 0) { }

		internal LinkedFifoBuffer(int capacity, long offset) {
			this.offset = offset;
			this.data = new T[capacity];
		}

		public int InstanceCapacity => this.data.Length;

		public long HeadOffset => this.offset+this.head;

		public BufferPosition HeadPosition => new BufferPosition(this, this.head);

		public bool InstanceIsFull => this.head == this.data.Length;

		public bool InstanceIsHead => this.next == null;
	}
}
