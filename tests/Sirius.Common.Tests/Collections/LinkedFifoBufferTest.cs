using System;
using System.Linq;

using Xunit;
using Xunit.Abstractions;

namespace Sirius.Collections {
	public class LinkedFifoBufferTest {
		private static byte[] ByteRange(byte start, int count) {
			var result = new byte[count];
			for (var i = 0; i < count; i++) {
				result[i] = (byte)((start+i)&0xFF);
			}
			return result;
		}

		private readonly ITestOutputHelper output;

		public LinkedFifoBufferTest(ITestOutputHelper output) {
			this.output = output;
		}

		[Fact]
		public void Fifo() {
			// tiny buffer capacity so that we can test the different methods with buffer breakover
			var buffer = new LinkedFifoBuffer<byte>(4);
			var position = buffer.HeadPosition;
			for (byte i = 1; i <= 4; i++) {
				LinkedFifoBuffer<byte>.Write(ref buffer, i);
				// no breakover
				Assert.Same(position.Buffer, buffer);
			}
			Assert.Equal(4, position.Count());
			Assert.Equal(4, position.AvailableCount());
			LinkedFifoBuffer<byte>.Write(ref buffer, 5);
			Assert.Equal(5, position.Count());
			Assert.Equal(5, position.AvailableCount());
			Assert.NotSame(position.Buffer, buffer);
			Assert.Equal(ByteRange(1, 5), position);
			position = position.Advance(4);
			Assert.NotSame(position.Buffer, buffer);
			Assert.Equal(ByteRange(5, 1), position);
			position = position.Advance(1);
			Assert.Same(position.Buffer, buffer);
			Assert.Empty(position);
			LinkedFifoBuffer<byte>.Write(ref buffer, ByteRange(6, 3));
			Assert.Same(position.Buffer, buffer);
			LinkedFifoBuffer<byte>.Write(ref buffer, ByteRange(9, 10));
			Assert.NotSame(position.Buffer, buffer);
			Assert.Equal(ByteRange(6, 13), position);
			position = position.Advance(12);
			Assert.Same(position.Buffer, buffer);
			Assert.Equal(ByteRange(18, 1), position);
			position = position.Advance(1);
			Assert.Empty(position);
		}
	}
}
