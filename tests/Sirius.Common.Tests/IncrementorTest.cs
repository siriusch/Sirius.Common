using System;
using System.Numerics;

using Sirius.Numerics;

using Xunit;

namespace Sirius {
	public class IncrementorTest {
		private struct Incrementable: IIncrementable<Incrementable>, IComparable<Incrementable> {
			public static readonly Incrementable MinValue = new Incrementable(-1);
			public static readonly Incrementable MaxValue = new Incrementable(1);

			public Incrementable(int value) {
				if (Math.Sign(value) != value) {
					throw new ArgumentOutOfRangeException(nameof(value));
				}
				this.Value = value;
			}

			public int Value {
				get;
			}

			public Incrementable Increment() {
				return new Incrementable(this.Value+1);
			}

			public Incrementable Decrement() {
				return new Incrementable(this.Value-1);
			}

			public int CompareTo(Incrementable other) {
				return this.Value.CompareTo(other.Value);
			}
		}

		[Fact]
		public void BigDecimalDecrement() {
			Assert.Equal(BigDecimal.MinusOne, Incrementor<BigDecimal>.Decrement(BigDecimal.Zero));
		}

		[Fact]
		public void BigDecimalIncrement() {
			Assert.Equal(BigDecimal.One, Incrementor<BigDecimal>.Increment(BigDecimal.Zero));
		}

		[Fact]
		public void DecimalDecrement() {
			Assert.Equal(-1M, Incrementor<decimal>.Decrement(0M));
		}

		[Fact]
		public void DecimalIncrement() {
			Assert.Equal(1M, Incrementor<decimal>.Increment(0M));
		}

		[Fact]
		public void DecimalMaxValue() {
			Assert.Equal(decimal.MaxValue, Incrementor<decimal>.MaxValue);
		}

		[Fact]
		public void DecimalMinValue() {
			Assert.Equal(decimal.MinValue, Incrementor<decimal>.MinValue);
		}

		[Fact]
		public void DoubleDecrement() {
			Assert.Equal(-1d, Incrementor<double>.Decrement(0d));
		}

		[Fact]
		public void DoubleIncrement() {
			Assert.Equal(1d, Incrementor<double>.Increment(0d));
		}

		[Fact]
		public void DoubleMaxValue() {
			Assert.Equal(double.MaxValue, Incrementor<double>.MaxValue);
		}

		[Fact]
		public void DoubleMinValue() {
			Assert.Equal(double.MinValue, Incrementor<double>.MinValue);
		}

		[Fact]
		public void BigIntegerDecrement() {
			Assert.Equal(BigInteger.MinusOne, Incrementor<BigInteger>.Decrement(BigInteger.Zero));
		}

		[Fact]
		public void BigIntegerIncrement() {
			Assert.Equal(BigInteger.One, Incrementor<BigInteger>.Increment(BigInteger.Zero));
		}

		[Fact]
		public void CharDecrement() {
			Assert.Equal('a', Incrementor<char>.Decrement('b'));
		}

		[Fact]
		public void CharIncrement() {
			Assert.Equal('c', Incrementor<char>.Increment('b'));
		}

		[Fact]
		public void CharMaxValue() {
			Assert.Equal(char.MaxValue, Incrementor<char>.MaxValue);
		}

		[Fact]
		public void CharMinValue() {
			Assert.Equal(char.MinValue, Incrementor<char>.MinValue);
		}

		[Fact]
		public void IncrementableDecrement() {
			Assert.Equal(Incrementable.MinValue, Incrementor<Incrementable>.Decrement(new Incrementable()));
		}

		[Fact]
		public void IncrementableIncrement() {
			Assert.Equal(Incrementable.MaxValue, Incrementor<Incrementable>.Increment(new Incrementable()));
		}

		[Fact]
		public void IncrementableMaxValue() {
			Assert.Equal(Incrementable.MaxValue, Incrementor<Incrementable>.MaxValue);
		}

		[Fact]
		public void IncrementableMinValue() {
			Assert.Equal(Incrementable.MinValue, Incrementor<Incrementable>.MinValue);
		}

		[Fact]
		public void Int32Decrement() {
			Assert.Equal(-1, Incrementor<int>.Decrement(0));
		}

		[Fact]
		public void Int32Increment() {
			Assert.Equal(1, Incrementor<int>.Increment(0));
		}

		[Fact]
		public void Int32MaxValue() {
			Assert.Equal(int.MaxValue, Incrementor<int>.MaxValue);
		}

		[Fact]
		public void Int32MinValue() {
			Assert.Equal(int.MinValue, Incrementor<int>.MinValue);
		}

		[Fact]
		public void ByteDecrement() {
			Assert.Equal((byte)0, Incrementor<byte>.Decrement((byte)1));
		}

		[Fact]
		public void ByteIncrement() {
			Assert.Equal((byte)1, Incrementor<byte>.Increment((byte)0));
		}

		[Fact]
		public void ByteMaxValue() {
			Assert.Equal(byte.MaxValue, Incrementor<byte>.MaxValue);
		}

		[Fact]
		public void ByteMinValue() {
			Assert.Equal(byte.MinValue, Incrementor<byte>.MinValue);
		}
	}
}
