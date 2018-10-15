using System;
using System.Linq;

using Xunit;

namespace Sirius.Collections {
	public class CaptureTest {
		[Fact]
		public void EnumerationError() {
			var capture = new Capture<int>(Enumerable.Range(1, 1), 2, 1);
			Assert.Throws<InvalidOperationException>(() => capture.ToList());
		}

		[Fact]
		public void Exact() {
			var capture = new Capture<int>(Enumerable.Range(1, 10), 10, 5);
			Assert.Equal(Enumerable.Range(1, 10), capture);
			Assert.Equal(5, capture.Index);
			Assert.Equal(10, capture.Count);
		}

		[Fact]
		public void Subset() {
			var capture = new Capture<int>(Enumerable.Range(5, 10), 5, 5);
			Assert.Equal(Enumerable.Range(5, 5), capture);
			Assert.Equal(5, capture.Index);
			Assert.Equal(5, capture.Count);
		}
	}
}
