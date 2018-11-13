using System;
using System.Drawing;
using System.Text;

namespace Sirius.Text {
	/// <summary>
	/// A <see cref="RichTextWriter"/> implementation for writing to the console. Supports indentation and output colors.
	/// </summary>
	public class ConsoleTextWriter: RichTextWriter {
		internal static ConsoleColor GetConsoleColor(Color color) {
			if (color.GetSaturation() < 0.5) {
				// we have a grayish color
				switch ((int)(color.GetBrightness() * 3.5)) {
				case 0:
					return ConsoleColor.Black;
				case 1:
					return ConsoleColor.DarkGray;
				case 2:
					return ConsoleColor.Gray;
				default:
					return ConsoleColor.White;
				}
			}
			var hue = (int)Math.Round(color.GetHue() / 60, MidpointRounding.AwayFromZero);
			if (color.GetBrightness() < 0.4) {
				// dark color
				switch (hue) {
				case 1:
					return ConsoleColor.DarkYellow;
				case 2:
					return ConsoleColor.DarkGreen;
				case 3:
					return ConsoleColor.DarkCyan;
				case 4:
					return ConsoleColor.DarkBlue;
				case 5:
					return ConsoleColor.DarkMagenta;
				default:
					return ConsoleColor.DarkRed;
				}
			}
			// bright color
			switch (hue) {
			case 1:
				return ConsoleColor.Yellow;
			case 2:
				return ConsoleColor.Green;
			case 3:
				return ConsoleColor.Cyan;
			case 4:
				return ConsoleColor.Blue;
			case 5:
				return ConsoleColor.Magenta;
			default:
				return ConsoleColor.Red;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConsoleTextWriter"/> class bound to the console.
		/// </summary>
		public ConsoleTextWriter(): this(null) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ConsoleTextWriter"/> class bound to the console, using the specified style provider.
		/// </summary>
		/// <param name="styleProvider">The style provider.</param>
		[CLSCompliant(false)]
		public ConsoleTextWriter(IStyleProvider styleProvider): base(null, styleProvider) { }

		/// <summary>When overridden in a derived class, returns the character encoding in which the output is written.</summary>
		/// <value>The character encoding in which the output is written.</value>
		public override Encoding Encoding => Console.OutputEncoding;

		/// <summary>Gets or sets the line terminator string used by the current TextWriter.</summary>
		/// <value>The line terminator string for the current TextWriter.</value>
		public override string NewLine {
			get => Console.Out.NewLine;
			set => Console.Out.NewLine = value;
		}

		/// <summary>Gets or sets a value indicating whether the indent pending.</summary>
		/// <value><c>true</c> if indent pending, <c>false</c> if not.</value>
		protected override bool IndentPending {
			get => base.IndentPending || (Console.CursorLeft == 0);
			set => base.IndentPending = value;
		}

		/// <summary>Resets the style of the writer.</summary>
		public override void Reset() {
			Console.ResetColor();
		}

		/// <summary>Sets the background color of this writer.</summary>
		/// <param name="color">The color.</param>
		public override void SetBackground(Color color) {
			Console.BackgroundColor = GetConsoleColor(color);
		}

		/// <summary>Sets the foreground color of this writer.</summary>
		/// <param name="color">The color.</param>
		public override void SetForeground(Color color) {
			Console.ForegroundColor = GetConsoleColor(color);
		}

		/// <summary>Char writing implementation.</summary>
		/// <param name="value">The value.</param>
		protected override void WriteInternal(char value) {
			Console.Write(value);
		}

		/// <summary>String writing implementation.</summary>
		/// <param name="value">The value.</param>
		protected override void WriteInternal(string value) {
			Console.Write(value);
		}

		/// <summary>Char array writing implementation.</summary>
		/// <param name="buffer">The character buffer.</param>
		/// <param name="index">Zero-based index of the start position in the buffer.</param>
		/// <param name="count">Number of characters to write.</param>
		protected override void WriteInternal(char[] buffer, int index, int count) {
			Console.Write(buffer, index, count);
		}
	}
}
