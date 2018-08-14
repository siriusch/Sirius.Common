using System;

using Xunit;
using Xunit.Abstractions;

namespace Sirius.Unicode {
	public class CodepointTest {
		private readonly ITestOutputHelper output;

		public CodepointTest(ITestOutputHelper output) {
			this.output = output;
		}

		[Theory]
		[InlineData('\uFFFE')]
		[InlineData('\uFFFF')]
		public void InvalidCodepoint(char c) {
			Assert.Throws<ArgumentOutOfRangeException>(() => new Codepoint(c));
			output.WriteLine(((Codepoint)c).ToString());
		}
	}
}
