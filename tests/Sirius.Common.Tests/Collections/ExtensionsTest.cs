using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Sirius.Collections {
	public class ExtensionsTest {
		[Fact]
		public void Condense() {
			Assert.Equal(new[] {Range<char>.Create('a', 'd'), Range<char>.Create('f', 'f'), Range<char>.Create('x', 'z')}, new[] {'a', 'x', 'b', 'c', 'd', 'f', 'x', 'y', 'f', 'a', 'z', 'c'}.Condense());
			Assert.Equal(new[] {Range<char>.Create(char.MinValue, char.MaxValue)}, Range<char>.Create(char.MinValue, char.MaxValue).Expand().Condense());
		}

		[Fact]
		public void Expand() {
			Assert.Equal(new[] {'a', 'b', 'c'}, Range<char>.Create('a', 'c').Expand());
			Assert.Equal(65536, Range<char>.Create(char.MinValue, char.MaxValue).Expand().Distinct().Count());
		}

		[Fact]
		public void ExpandMany() {
			Assert.Equal(new[] {'a', 'b', 'c', 'd', 'f', 'x', 'y', 'z'}, new[] {Range<char>.Create('a', 'c'), Range<char>.Create('d', 'd'), Range<char>.Create('f', 'f'), Range<char>.Create('x', 'z')}.SelectMany(r => r.Expand()));
		}

		[Theory]
		[InlineData(-1, 'z', 0, int.MaxValue, "abcde")]
		[InlineData(-1, 'c', 0, 2, "abcde")]
		[InlineData(-1, 'c', 3, int.MaxValue, "abcde")]
		[InlineData(2, 'c', 0, int.MaxValue, "abcde")]
		[InlineData(2, 'c', 2, int.MaxValue, "abcde")]
		[InlineData(2, 'c', 0, 3, "abcde")]
		[InlineData(2, 'c', 0, int.MaxValue, "abcdeabcde")]
		public void IndexOf(int expected, char value, int startIndex, int count, IEnumerable<char> data) {
			Assert.Equal(expected, data.IndexOf(value, startIndex, count));
		}

		[Theory]
		[InlineData(-1, 'z', 0, int.MaxValue, "abcde")]
		[InlineData(-1, 'c', 0, 2, "abcde")]
		[InlineData(-1, 'c', 3, int.MaxValue, "abcde")]
		[InlineData(2, 'c', 0, int.MaxValue, "abcde")]
		[InlineData(2, 'c', 2, int.MaxValue, "abcde")]
		[InlineData(2, 'c', 0, 3, "abcde")]
		[InlineData(2, 'c', 0, 7, "abcdeabcde")]
		[InlineData(7, 'c', 0, int.MaxValue, "abcdeabcde")]
		[InlineData(-1, 'c', 3, 4, "abcdeabcde")]
		[InlineData(7, 'c', 3, 5, "abcdeabcde")]
		public void LastIndexOf(int expected, char value, int startIndex, int count, IEnumerable<char> data) {
			Assert.Equal(expected, data.LastIndexOf(value, startIndex, count));
		}
	}
}
