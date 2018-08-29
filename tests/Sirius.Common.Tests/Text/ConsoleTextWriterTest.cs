using System;
using System.Drawing;

using Xunit;

namespace Sirius.Text {
	public class ConsoleTextWriterTest {
		[Theory]
		[InlineData(ConsoleColor.Black, KnownColor.Black)]
		[InlineData(ConsoleColor.Blue, KnownColor.Blue)]
		[InlineData(ConsoleColor.Cyan, KnownColor.Cyan)]
		[InlineData(ConsoleColor.DarkBlue, KnownColor.DarkBlue)]
		[InlineData(ConsoleColor.DarkCyan, KnownColor.DarkCyan)]
		[InlineData(ConsoleColor.DarkGray, KnownColor.Gray)]
		[InlineData(ConsoleColor.DarkGreen, KnownColor.Green)]
		[InlineData(ConsoleColor.DarkMagenta, KnownColor.DarkMagenta)]
		[InlineData(ConsoleColor.DarkRed, KnownColor.DarkRed)]
		[InlineData(ConsoleColor.DarkYellow, KnownColor.Olive)]
		[InlineData(ConsoleColor.Gray, KnownColor.LightGray)]
		[InlineData(ConsoleColor.Green, KnownColor.Lime)]
		[InlineData(ConsoleColor.Magenta, KnownColor.Magenta)]
		[InlineData(ConsoleColor.Red, KnownColor.Red)]
		[InlineData(ConsoleColor.White, KnownColor.White)]
		[InlineData(ConsoleColor.Yellow, KnownColor.Yellow)]
		public void ColorMapping(ConsoleColor expected, KnownColor input) {
			Assert.Equal(expected, ConsoleTextWriter.GetConsoleColor(Color.FromKnownColor(input)));
		}
	}
}
