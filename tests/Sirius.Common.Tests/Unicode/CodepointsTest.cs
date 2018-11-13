using System;

using Xunit;
using Xunit.Abstractions;

namespace Sirius.Unicode {
	public class CodepointsTest {
		private readonly ITestOutputHelper output;

		public CodepointsTest(ITestOutputHelper output) {
			this.output = output;
		}

		[Fact]
		public void GetValid() {
			this.output.WriteLine(Codepoints.Valid.ToString());
		}

		[Fact]
		public void GetValidBmp() {
			this.output.WriteLine(Codepoints.ValidBmp.ToString());
		}

		[Fact]
		public void GetReserved() {
			this.output.WriteLine(Codepoints.Reserved.ToString());
		}

		[Fact]
		public void GetEof() {
			this.output.WriteLine(Codepoints.EOF.ToString());
		}
	}
}
