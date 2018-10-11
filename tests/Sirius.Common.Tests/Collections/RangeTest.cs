using System;

using Xunit;
using Xunit.Abstractions;

namespace Sirius.Collections {
	public class RangeTest {
		private readonly ITestOutputHelper output;

		public RangeTest(ITestOutputHelper output) {
			this.output = output;
		}

		[Theory]
		[InlineData(new[] {'0'}, '0', '0')]
		[InlineData(new[] {'0', '1', '2', '3'}, '0', '3')]
		public void ExpandChar(char[] expected, char from, char to) {
			Assert.Equal(expected, Range<char>.Create(from, to).Expand());
			if (from == to) {
				Assert.Equal(expected, Range<char>.Create(from).Expand());
			}
		}

		[Theory]
		[InlineData(new[] {0}, 0, 0)]
		[InlineData(new[] {0, 1, 2, 3}, 0, 3)]
		public void ExpandInt(int[] expected, int from, int to) {
			Assert.Equal(expected, Range<int>.Create(from, to).Expand());
			if (from == to) {
				Assert.Equal(expected, Range<int>.Create(from).Expand());
			}
		}
	}
}
