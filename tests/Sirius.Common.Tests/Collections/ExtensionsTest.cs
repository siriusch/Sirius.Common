using System;
using System.Linq;

using Xunit;

namespace Sirius.Collections {
	public class ExtensionsTest {
		[Fact]
		public void Condense() {
			Assert.Equal(new[] { Range<char>.Create('a', 'd'), Range<char>.Create('f', 'f'), Range<char>.Create('x', 'z') }, new[] { 'a', 'x', 'b', 'c', 'd', 'f', 'x', 'y', 'f', 'a', 'z', 'c' }.Condense());
			Assert.Equal(new[] { Range<char>.Create(char.MinValue, char.MaxValue) }, Range<char>.Create(char.MinValue, char.MaxValue).Expand().Condense());
		}

		[Fact]
		public void Expand() {
			Assert.Equal(new[] { 'a', 'b', 'c' }, Range<char>.Create('a', 'c').Expand());
			Assert.Equal(65536, Range<char>.Create(char.MinValue, char.MaxValue).Expand().Distinct().Count());
		}

		[Fact]
		public void ExpandMany() {
			Assert.Equal(new[] { 'a', 'b', 'c', 'd', 'f', 'x', 'y', 'z' }, new[] { Range<char>.Create('a', 'c'), Range<char>.Create('d', 'd'), Range<char>.Create('f', 'f'), Range<char>.Create('x', 'z') }.SelectMany(r => r.Expand()));
		}
	}
}
