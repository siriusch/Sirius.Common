using System.Collections.Generic;
using System.Linq;

using Xunit;
using Xunit.Abstractions;

namespace Sirius.Collections {
	public class RangeSetTest {
		private readonly ITestOutputHelper output;

		public RangeSetTest(ITestOutputHelper output) {
			this.output = output;
		}

		[Theory]
		[InlineData(new char[0], '0', '0', '0', '0')]
		[InlineData(new char[0], '0', '3', '0', '3')]
		[InlineData(new[] {'0', '3'}, '0', '2', '1', '3')]
		[InlineData(new[] {'0', '1', '2', '3'}, '0', '1', '2', '3')]
		[InlineData(new[] {'0', '1', '2', '4', '5', '6'}, '0', '2', '4', '6')]
		public void Difference(char[] expected, char from1, char to1, char from2, char to2) {
			var range1 = Range<char>.Create(from1, to1);
			RangeSet<char> set1 = range1;
			var range2 = Range<char>.Create(from2, to2);
			RangeSet<char> set2 = range2;
			Assert.Equal(expected, RangeOperations<char>.Difference(set1, set2).Expand());
			Assert.Equal(expected, (set1 ^ set2).Expand());
			Assert.Equal(expected, (range1 ^ range2).Expand());
			Assert.Equal(expected, (set1 ^ range2).Expand());
			Assert.Equal(expected, (range1 ^ set2).Expand());
		}

		[Theory]
		[InlineData(new[] {'0'}, '0', '0', '0', '0')]
		[InlineData(new[] {'0', '1', '2', '3'}, '0', '3', '0', '3')]
		[InlineData(new[] {'1', '2'}, '0', '2', '1', '3')]
		[InlineData(new char[0], '0', '1', '2', '3')]
		[InlineData(new char[0], '0', '2', '4', '6')]
		public void Intersect(char[] expected, char from1, char to1, char from2, char to2) {
			var range1 = Range<char>.Create(from1, to1);
			RangeSet<char> set1 = range1;
			var range2 = Range<char>.Create(from2, to2);
			RangeSet<char> set2 = range2;
			Assert.Equal(expected, RangeOperations<char>.Intersection(set1, set2).Expand());
			Assert.Equal(expected, (set1 & set2).Expand());
			Assert.Equal(expected, (range1 & range2).Expand());
			Assert.Equal(expected, (set1 & range2).Expand());
			Assert.Equal(expected, (range1 & set2).Expand());
		}

		[Theory]
		[InlineData('0', '0', '0', '0')]
		[InlineData('0', '3', '0', '3')]
		[InlineData('0', '2', '1', '3')]
		[InlineData('0', '1', '2', '3')]
		[InlineData('0', '2', '4', '6')]
		public void Negate(char from1, char to1, char from2, char to2) {
			var range1 = Range<char>.Create(from1, to1);
			var range2 = Range<char>.Create(from2, to2);
			RangeSet<char> positive = new[] {range1, range2};
			var negative = RangeOperations<char>.Negate(positive);
			this.output.WriteLine(string.Join(", ", positive.Expand()));
			this.output.WriteLine(negative.Expand().Count().ToString());
			Assert.NotEqual(positive, negative);
			Assert.Equal(RangeSet<char>.All, positive ^ negative);
			Assert.Equal(RangeSet<char>.All, positive | negative);
			Assert.Equal(RangeSet<char>.Empty, positive & negative);
			Assert.Equal(positive, positive - negative);
			Assert.Equal(negative, negative - positive);
			var negative2 = (range1 == range2) ? ~range1 : ~positive;
			Assert.Equal(negative, negative2);
		}

		[Theory]
		[InlineData(new char[0], '0', '0', '0', '0')]
		[InlineData(new char[0], '0', '3', '0', '3')]
		[InlineData(new[] {'0'}, '0', '2', '1', '3')]
		[InlineData(new[] {'3'}, '1', '3', '0', '2')]
		[InlineData(new[] {'0', '1'}, '0', '1', '2', '3')]
		[InlineData(new[] {'0', '1', '2'}, '0', '2', '4', '6')]
		public void Subtract(char[] expected, char from1, char to1, char from2, char to2) {
			var range1 = Range<char>.Create(from1, to1);
			RangeSet<char> set1 = range1;
			var range2 = Range<char>.Create(from2, to2);
			RangeSet<char> set2 = range2;
			Assert.Equal(expected, RangeOperations<char>.Subtract(set1, set2).Expand());
			Assert.Equal(expected, (set1 - set2).Expand());
			Assert.Equal(expected, (range1 - range2).Expand());
			Assert.Equal(expected, (set1 - range2).Expand());
			Assert.Equal(expected, (range1 - set2).Expand());
		}

		[Theory]
		[InlineData(new[] {'0'}, '0', '0', '0', '0')]
		[InlineData(new[] {'0', '1', '2', '3'}, '0', '3', '0', '3')]
		[InlineData(new[] {'0', '1', '2', '3'}, '0', '2', '1', '3')]
		[InlineData(new[] {'0', '1', '2', '3'}, '0', '1', '2', '3')]
		[InlineData(new[] {'0', '1', '2', '4', '5', '6'}, '0', '2', '4', '6')]
		public void Union(char[] expected, char from1, char to1, char from2, char to2) {
			var range1 = Range<char>.Create(from1, to1);
			RangeSet<char> set1 = range1;
			var range2 = Range<char>.Create(from2, to2);
			RangeSet<char> set2 = range2;
			Assert.Equal(expected, RangeOperations<char>.Union(set1, set2).Expand());
			Assert.Equal(expected, new RangeSet<char>((IEnumerable<Range<char>>)(new[] {range1, range2})).Expand());
			Assert.Equal(expected, (set1 | set2).Expand());
			Assert.Equal(expected, (range1 | range2).Expand());
			Assert.Equal(expected, (set1 | range2).Expand());
			Assert.Equal(expected, (range1 | set2).Expand());
		}
	}
}
