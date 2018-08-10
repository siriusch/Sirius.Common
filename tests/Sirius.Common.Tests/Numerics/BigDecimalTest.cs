using System;
using System.Globalization;

using Xunit;
using Xunit.Abstractions;

namespace Sirius.Numerics {
	public class BigDecimalTest {
		private readonly ITestOutputHelper output;

		public BigDecimalTest(ITestOutputHelper output) {
			this.output = output;
		}

		[Theory]
		[InlineData("0", "0")]
		[InlineData("1", "1")]
		[InlineData("1", "-1")]
		public void Abs(string result, string value) {
			var resultDecimal = BigDecimal.Parse(result, CultureInfo.InvariantCulture);
			var valueDecimal = BigDecimal.Parse(value, CultureInfo.InvariantCulture);
			Assert.Equal(resultDecimal, BigDecimal.Abs(valueDecimal));
		}

		[Theory]
		[InlineData("0", "0", "0")]
		[InlineData("1", "1", "0")]
		[InlineData("1", "0", "1")]
		[InlineData("10", "1", "10")]
		[InlineData("10", "0.1", "10")]
		[InlineData("0.1", "0.1", "-10")]
		[InlineData("10", "-0.1", "10")]
		public void Max(string result, string left, string right) {
			var resultDecimal = BigDecimal.Parse(result, CultureInfo.InvariantCulture);
			var leftDecimal = BigDecimal.Parse(left, CultureInfo.InvariantCulture);
			var rightDecimal = BigDecimal.Parse(right, CultureInfo.InvariantCulture);
			Assert.Equal(resultDecimal, BigDecimal.Max(leftDecimal, rightDecimal));
		}

		[Theory]
		[InlineData("0", "0", "0")]
		[InlineData("0", "1", "0")]
		[InlineData("0", "0", "1")]
		[InlineData("1", "1", "10")]
		[InlineData("0.1", "0.1", "10")]
		[InlineData("-10", "0.1", "-10")]
		[InlineData("-0.1", "-0.1", "10")]
		public void Min(string result, string left, string right) {
			var resultDecimal = BigDecimal.Parse(result, CultureInfo.InvariantCulture);
			var leftDecimal = BigDecimal.Parse(left, CultureInfo.InvariantCulture);
			var rightDecimal = BigDecimal.Parse(right, CultureInfo.InvariantCulture);
			Assert.Equal(resultDecimal, BigDecimal.Min(leftDecimal, rightDecimal));
		}

		[Theory]
		[InlineData("0", "0", "0")]
		[InlineData("1", "1", "0")]
		[InlineData("1", "0", "1")]
		[InlineData("11", "1", "10")]
		[InlineData("10.1", "0.1", "10")]
		[InlineData("-9.9", "0.1", "-10")]
		[InlineData("9.9", "-0.1", "10")]
		public void Add(string result, string left, string right) {
			var resultDecimal = BigDecimal.Parse(result, CultureInfo.InvariantCulture);
			var leftDecimal = BigDecimal.Parse(left, CultureInfo.InvariantCulture);
			var rightDecimal = BigDecimal.Parse(right, CultureInfo.InvariantCulture);
			Assert.Equal(resultDecimal, BigDecimal.Add(leftDecimal, rightDecimal));
		}

		[Theory]
		[InlineData("0", "0")]
		[InlineData("1", "1")]
		[InlineData("10", "10")]
		[InlineData("1", "1.1")]
		[InlineData("-1", "-1")]
		[InlineData("-1", "-1.1")]
		public void Truncate(string result, string value) {
			var resultDecimal = BigDecimal.Parse(result, CultureInfo.InvariantCulture);
			var valueDecimal = BigDecimal.Parse(value, CultureInfo.InvariantCulture);
			Assert.Equal(resultDecimal, BigDecimal.Truncate(valueDecimal));
		}

		[Theory]
		[InlineData("0", "0")]
		[InlineData("1", "1")]
		[InlineData("10", "10")]
		[InlineData("2", "1.1")]
		[InlineData("-1", "-1")]
		[InlineData("-1", "-1.1")]
		public void Ceiling(string result, string value) {
			var resultDecimal = BigDecimal.Parse(result, CultureInfo.InvariantCulture);
			var valueDecimal = BigDecimal.Parse(value, CultureInfo.InvariantCulture);
			Assert.Equal(resultDecimal, BigDecimal.Ceiling(valueDecimal));
		}

		[Theory]
		[InlineData("NaN", "0", "0")]
		[InlineData("NaN", "1", "0")]
		[InlineData("0", "0", "1")]
		[InlineData("0.1", "1", "10")]
		[InlineData("0.01", "0.1", "10")]
		[InlineData("100", "10", "0.1")]
		[InlineData("100", "-10", "-0.1")]
		[InlineData("-100", "-10", "0.1")]
		[InlineData("-100", "10", "-0.1")]
		public void Divide(string result, string left, string right) {
			var resultDecimal = BigDecimal.Parse(result, CultureInfo.InvariantCulture);
			var leftDecimal = BigDecimal.Parse(left, CultureInfo.InvariantCulture);
			var rightDecimal = BigDecimal.Parse(right, CultureInfo.InvariantCulture);
			Assert.Equal(resultDecimal, BigDecimal.Divide(leftDecimal, rightDecimal));
		}

		[Theory]
		[InlineData("NaN", "0", "0")]
		[InlineData("NaN", "1", "0")]
		[InlineData("0", "0", "1")]
		[InlineData("1", "1", "10")]
		[InlineData("0.1", "0.1", "10")]
		[InlineData("0", "10", "0.1")]
		[InlineData("0", "-10", "-0.1")]
		[InlineData("0", "-10", "0.1")]
		[InlineData("0", "10", "-0.1")]
		public void Remainder(string result, string left, string right) {
			var resultDecimal = BigDecimal.Parse(result, CultureInfo.InvariantCulture);
			var leftDecimal = BigDecimal.Parse(left, CultureInfo.InvariantCulture);
			var rightDecimal = BigDecimal.Parse(right, CultureInfo.InvariantCulture);
			Assert.Equal(resultDecimal, BigDecimal.Remainder(leftDecimal, rightDecimal));
		}

		[Theory]
		[InlineData("0", "0")]
		[InlineData("1", "1")]
		[InlineData("10", "10")]
		[InlineData("1", "1.1")]
		[InlineData("-1", "-1")]
		[InlineData("-2", "-1.1")]
		public void Floor(string result, string value) {
			var resultDecimal = BigDecimal.Parse(result, CultureInfo.InvariantCulture);
			var valueDecimal = BigDecimal.Parse(value, CultureInfo.InvariantCulture);
			Assert.Equal(resultDecimal, BigDecimal.Floor(valueDecimal));
		}

		[Theory]
		[InlineData("0", "0", 0, MidpointRounding.ToEven)]
		[InlineData("1", "1.4", 0, MidpointRounding.ToEven)]
		[InlineData("1", "1.4", 0, MidpointRounding.AwayFromZero)]
		[InlineData("2", "1.5", 0, MidpointRounding.ToEven)]
		[InlineData("2", "1.5", 0, MidpointRounding.AwayFromZero)]
		[InlineData("2", "1.6", 0, MidpointRounding.ToEven)]
		[InlineData("2", "1.6", 0, MidpointRounding.AwayFromZero)]
		[InlineData("2", "2.4", 0, MidpointRounding.ToEven)]
		[InlineData("2", "2.4", 0, MidpointRounding.AwayFromZero)]
		[InlineData("2", "2.5", 0, MidpointRounding.ToEven)]
		[InlineData("3", "2.5", 0, MidpointRounding.AwayFromZero)]
		[InlineData("3", "2.6", 0, MidpointRounding.ToEven)]
		[InlineData("3", "2.6", 0, MidpointRounding.AwayFromZero)]
		[InlineData("-1", "-1.4", 0, MidpointRounding.ToEven)]
		[InlineData("-1", "-1.4", 0, MidpointRounding.AwayFromZero)]
		[InlineData("-2", "-1.5", 0, MidpointRounding.ToEven)]
		[InlineData("-2", "-1.5", 0, MidpointRounding.AwayFromZero)]
		[InlineData("-2", "-1.6", 0, MidpointRounding.ToEven)]
		[InlineData("-2", "-1.6", 0, MidpointRounding.AwayFromZero)]
		[InlineData("-2", "-2.4", 0, MidpointRounding.ToEven)]
		[InlineData("-2", "-2.4", 0, MidpointRounding.AwayFromZero)]
		[InlineData("-2", "-2.5", 0, MidpointRounding.ToEven)]
		[InlineData("-3", "-2.5", 0, MidpointRounding.AwayFromZero)]
		[InlineData("-3", "-2.6", 0, MidpointRounding.ToEven)]
		[InlineData("-3", "-2.6", 0, MidpointRounding.AwayFromZero)]
		[InlineData("1.44", "1.444", 2, MidpointRounding.ToEven)]
		[InlineData("1.44", "1.444", 2, MidpointRounding.AwayFromZero)]
		[InlineData("1.56", "1.555", 2, MidpointRounding.ToEven)]
		[InlineData("1.56", "1.555", 2, MidpointRounding.AwayFromZero)]
		[InlineData("1.67", "1.666", 2, MidpointRounding.ToEven)]
		[InlineData("1.67", "1.666", 2, MidpointRounding.AwayFromZero)]
		[InlineData("2.44", "2.444", 2, MidpointRounding.ToEven)]
		[InlineData("2.44", "2.444", 2, MidpointRounding.AwayFromZero)]
		[InlineData("2.56", "2.555", 2, MidpointRounding.ToEven)]
		[InlineData("2.56", "2.555", 2, MidpointRounding.AwayFromZero)]
		[InlineData("2.67", "2.666", 2, MidpointRounding.ToEven)]
		[InlineData("2.67", "2.666", 2, MidpointRounding.AwayFromZero)]
		public void Round(string result, string value, int decimals, MidpointRounding mode) {
			var resultDecimal = BigDecimal.Parse(result, CultureInfo.InvariantCulture);
			var valueDecimal = BigDecimal.Parse(value, CultureInfo.InvariantCulture);
			Assert.Equal(resultDecimal, BigDecimal.Round(valueDecimal, decimals, mode));
		}

		[Theory]
		[InlineData("0", "0", "0")]
		[InlineData("0", "1", "0")]
		[InlineData("0", "0", "1")]
		[InlineData("10", "1", "10")]
		[InlineData("1", "0.1", "10")]
		[InlineData("-1", "0.1", "-10")]
		[InlineData("-1", "-0.1", "10")]
		public void Multiply(string result, string left, string right) {
			var resultDecimal = BigDecimal.Parse(result, CultureInfo.InvariantCulture);
			var leftDecimal = BigDecimal.Parse(left, CultureInfo.InvariantCulture);
			var rightDecimal = BigDecimal.Parse(right, CultureInfo.InvariantCulture);
			Assert.Equal(resultDecimal, BigDecimal.Multiply(leftDecimal, rightDecimal));
		}

		[Theory]
		[InlineData("0", "0")]
		[InlineData("1", "-1")]
		[InlineData("-1", "1")]
		public void Negate(string result, string value) {
			var resultDecimal = BigDecimal.Parse(result, CultureInfo.InvariantCulture);
			var valueDecimal = BigDecimal.Parse(value, CultureInfo.InvariantCulture);
			Assert.Equal(resultDecimal, BigDecimal.Negate(valueDecimal));
		}

		[Theory]
		[InlineData("0", "0", "0")]
		[InlineData("1", "1", "0")]
		[InlineData("-1", "0", "1")]
		[InlineData("-9", "1", "10")]
		[InlineData("-9.9", "0.1", "10")]
		[InlineData("10.1", "0.1", "-10")]
		[InlineData("-10.1", "-0.1", "10")]
		public void Subtract(string result, string left, string right) {
			var resultDecimal = BigDecimal.Parse(result, CultureInfo.InvariantCulture);
			var leftDecimal = BigDecimal.Parse(left, CultureInfo.InvariantCulture);
			var rightDecimal = BigDecimal.Parse(right, CultureInfo.InvariantCulture);
			Assert.Equal(resultDecimal, BigDecimal.Subtract(leftDecimal, rightDecimal));
		}
	}
}
