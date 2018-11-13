using System;
using System.Drawing;
using System.IO;

namespace Sirius.Text {
	/// <summary>The RichTextWriter is an extended <see cref="TextWriter" /> which adds support for basic formatting and indentation, such as typically used for syntax highlighting.</summary>
	public abstract class RichTextWriter: TextWriter {
		/// <summary>Information about the indent.</summary>
		private class IndentInfo: IDisposable {
			/// <summary>The depth.</summary>
			private readonly int depth;
			/// <summary>The previous.</summary>
			private readonly IndentInfo previous;
			/// <summary>The owner.</summary>
			private RichTextWriter owner;

			/// <summary>Constructor.</summary>
			/// <param name="owner">The owner.</param>
			/// <param name="depth">The depth.</param>
			public IndentInfo(RichTextWriter owner, int depth) {
				this.owner = owner;
				this.depth = depth;
				this.previous = owner.indent;
			}

			/// <summary>Gets the depth.</summary>
			/// <value>The depth.</value>
			public int Depth => this.depth;

			/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
			/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
			public void Dispose() {
				if (this.owner != null) {
					try {
						if (this.owner.indent != this) {
							throw new InvalidOperationException("The indenting does not match");
						}
						this.owner.indent = this.previous;
					} finally {
						this.owner = null;
					}
				}
			}
		}

		/// <summary>The null writer.</summary>
		private static readonly NullTextWriter nullWriter = new NullTextWriter();

		/// <summary>Gets the null RichTextWriter.</summary>
		/// <value>The null.</value>
		public new static RichTextWriter Null => nullWriter;

		/// <summary>Wraps the specified writer with a RichTextWriter, adding indentation support on the way.</summary>
		/// <param name="writer">The text writer to wrap.</param>
		/// <returns>The wrapped TextWriter.</returns>
		public static RichTextWriter Wrap(TextWriter writer) {
			return (writer as RichTextWriter) ?? new DelegateTextWriter(writer, null);
		}

		private readonly IStyleProvider styleProvider;
		private IndentInfo indent;
		private string indentChars;

		/// <summary>Specialised constructor for use only by derived class.</summary>
		/// <param name="formatProvider">The format provider.</param>
		/// <param name="styleProvider">The style provider.</param>
		[CLSCompliant(false)]
		protected RichTextWriter(IFormatProvider formatProvider, IStyleProvider styleProvider): base(formatProvider) {
			this.styleProvider = styleProvider;
			this.IndentPending = true;
		}

		/// <summary>Specialised default constructor for use only by derived class.</summary>
		protected RichTextWriter() { }

		/// <summary>Gets or sets the indentation chars.</summary>
		/// <remarks>If not specified (or set to <c>null</c>), two spaces are used.</remarks>
		/// <value>The indent characters.</value>
		public string IndentChars {
			get {
				return this.indentChars ?? "  ";
			}
			set {
				this.indentChars = value;
			}
		}

		/// <summary>Gets the style provider provided in the constructor of this rich text writer.</summary>
		/// <value>The style provider.</value>
		[CLSCompliant(false)]
		public IStyleProvider StyleProvider => this.styleProvider;

		/// <summary>Gets the depth of the indent.</summary>
		/// <value>The depth of the indent.</value>
		protected int IndentDepth {
			get {
				if (this.indent == null) {
					return 0;
				}
				return this.indent.Depth;
			}
		}

		/// <summary>Gets or sets a value indicating whether the indent pending.</summary>
		/// <value><c>true</c> if indent pending, <c>false</c> if not.</value>
		protected virtual bool IndentPending {
			get;
			set;
		}

		/// <summary>Assert indentation.</summary>
		protected void AssertIndentation() {
			if (this.IndentPending) {
				this.IndentPending = false;
				this.WriteIndentation();
			}
		}

		/// <summary>Increments the indentation and returns a disposable handle.</summary>
		/// <remarks>The indentation is decremented by disposing the handle.</remarks>
		/// <returns>The indentation handle.</returns>
		public IDisposable Indent() {
			return this.Indent(1);
		}

		/// <summary>Increments the indentation by the specified depth and returns a disposable handle.</summary>
		/// <remarks>The indentation is decremented by disposing the handle.</remarks>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
		/// <param name="depth">The depth.</param>
		/// <returns>The indentation handle.</returns>
		public IDisposable Indent(int depth) {
			if (depth < 0) {
				throw new ArgumentOutOfRangeException("depth", "Negative indent is not allowed");
			}
			this.indent = new IndentInfo(this, this.IndentDepth + depth);
			return this.indent;
		}

		/// <summary>Resets the style of the writer.</summary>
		public virtual void Reset() { }

		/// <summary>Sets the background color of this writer.</summary>
		/// <param name="color">The color.</param>
		public virtual void SetBackground(Color color) { }

		/// <summary>Controls the boldness of the font.</summary>
		/// <param name="bold"><c>true</c> to bold.</param>
		public virtual void SetBold(bool bold) { }

		/// <summary>Sets the foreground color of this writer.</summary>
		/// <param name="color">The color.</param>
		public virtual void SetForeground(Color color) { }

		/// <summary>Controls the italic sytle of the font.</summary>
		/// <param name="bold"><c>true</c> to bold.</param>
		public virtual void SetItalic(bool bold) { }

		/// <summary>Apply a style by using the configured <see cref="StyleProvider"/>.</summary>
		/// <remarks>This has no effect if no style provider has been set.</remarks>
		/// <typeparam name="T">The enumeration type.</typeparam>
		/// <param name="style">The enumeration value specifying the style to use.</param>
		[CLSCompliant(false)]
		public void SetStyle<T>(T style) where T: struct, IComparable, IFormattable, IConvertible {
			if (this.styleProvider != null) {
				this.styleProvider.Set(this, style);
			}
		}

		/// <summary>Writes a character to the text string or stream.</summary>
		/// <param name="value">The character to write to the text stream.</param>
		public override sealed void Write(char value) {
			this.AssertIndentation();
			this.WriteInternal(value);
		}

		/// <summary>Writes a character array to the text string or stream.</summary>
		/// <exception cref="ArgumentNullException">Thrown when one or more required arguments are <c>null</c></exception>
		/// <param name="buffer">The character array to write to the text stream.</param>
		public override sealed void Write(char[] buffer) {
			if (buffer == null) {
				throw new ArgumentNullException("buffer");
			}
			if (buffer.Length > 0) {
				this.AssertIndentation();
				this.WriteInternal(buffer, 0, buffer.Length);
			}
		}

		/// <summary>Writes a subarray of characters to the text string or stream.</summary>
		/// <param name="buffer">The character array to write data from.</param>
		/// <param name="index">The character position in the buffer at which to start retrieving data.</param>
		/// <param name="count">The number of characters to write.</param>
		public override sealed void Write(char[] buffer, int index, int count) {
			if (count > 0) {
				this.AssertIndentation();
				this.WriteInternal(buffer, index, count);
			}
		}

		/// <summary>Writes the text representation of a Boolean value to the text string or stream.</summary>
		/// <param name="value">The Boolean value to write.</param>
		public override sealed void Write(bool value) {
			base.Write(value);
		}

		/// <summary>Writes the text representation of a 4-byte signed integer to the text string or stream.</summary>
		/// <param name="value">The 4-byte signed integer to write.</param>
		public override sealed void Write(int value) {
			base.Write(value);
		}

		/// <summary>Writes the text representation of a 4-byte unsigned integer to the text string or stream.</summary>
		/// <param name="value">The 4-byte unsigned integer to write.</param>
		[CLSCompliant(false)]
		public override sealed void Write(uint value) {
			base.Write(value);
		}

		/// <summary>Writes the text representation of an 8-byte signed integer to the text string or stream.</summary>
		/// <param name="value">The 8-byte signed integer to write.</param>
		public override sealed void Write(long value) {
			base.Write(value);
		}

		/// <summary>Writes the text representation of an 8-byte unsigned integer to the text string or stream.</summary>
		/// <param name="value">The 8-byte unsigned integer to write.</param>
		[CLSCompliant(false)]
		public override sealed void Write(ulong value) {
			base.Write(value);
		}

		/// <summary>Writes the text representation of a 4-byte floating-point value to the text string or stream.</summary>
		/// <param name="value">The 4-byte floating-point value to write.</param>
		public override sealed void Write(float value) {
			base.Write(value);
		}

		/// <summary>Writes the text representation of an 8-byte floating-point value to the text string or stream.</summary>
		/// <param name="value">The 8-byte floating-point value to write.</param>
		public override sealed void Write(double value) {
			base.Write(value);
		}

		/// <summary>Writes the text representation of a decimal value to the text string or stream.</summary>
		/// <param name="value">The decimal value to write.</param>
		public override sealed void Write(decimal value) {
			base.Write(value);
		}

		/// <summary>Writes a string to the text string or stream.</summary>
		/// <param name="value">The string to write.</param>
		public override sealed void Write(string value) {
			if (!string.IsNullOrEmpty(value)) {
				this.AssertIndentation();
				this.WriteInternal(value);
			}
		}

		/// <summary>Writes the text representation of an object to the text string or stream by calling the ToString method on that object.</summary>
		/// <param name="value">The object to write.</param>
		public override sealed void Write(object value) {
			base.Write(value);
		}

		/// <summary>Writes a formatted string to the text string or stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object)" /> method.</summary>
		/// <param name="format">A composite format string (see Remarks).</param>
		/// <param name="arg0">The object to format and write.</param>
		public override sealed void Write(string format, object arg0) {
			base.Write(format, arg0);
		}

		/// <summary>
		/// 	Writes a formatted string to the text string or stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object,System.Object)" /> method.
		/// </summary>
		/// <param name="format">A composite format string (see Remarks).</param>
		/// <param name="arg0">The first object to format and write.</param>
		/// <param name="arg1">The second object to format and write.</param>
		public override sealed void Write(string format, object arg0, object arg1) {
			base.Write(format, arg0, arg1);
		}

		/// <summary>
		/// 	Writes a formatted string to the text string or stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object,System.Object,System.Object)" /> method.
		/// </summary>
		/// <param name="format">A composite format string (see Remarks).</param>
		/// <param name="arg0">The first object to format and write.</param>
		/// <param name="arg1">The second object to format and write.</param>
		/// <param name="arg2">The third object to format and write.</param>
		public override sealed void Write(string format, object arg0, object arg1, object arg2) {
			base.Write(format, arg0, arg1, arg2);
		}

		// ReSharper disable MethodOverloadWithOptionalParameter
		/// <summary>Writes a formatted string to the text string or stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object[])" /> method.</summary>
		/// <param name="format">A composite format string (see Remarks).</param>
		/// <param name="arg">An object array that contains zero or more objects to format and write.</param>
		public override sealed void Write(string format, params object[] arg) {
			base.Write(format, arg);
		}
		// ReSharper restore MethodOverloadWithOptionalParameter

		/// <summary>Writes the indentation.</summary>
		protected virtual void WriteIndentation() {
			var depth = this.IndentDepth;
			for (var i = 0; i < depth; i++) {
				this.WriteInternal(this.IndentChars);
			}
		}

		/// <summary>Writes an internal.</summary>
		/// <param name="value">The character to write to the text stream.</param>
		protected abstract void WriteInternal(char value);

		/// <summary>Writes an internal.</summary>
		/// <param name="value">The string to write. If <paramref name="value" /> is null, only the line terminator is written.</param>
		protected virtual void WriteInternal(string value) {
			if (!string.IsNullOrEmpty(value)) {
				var buffer = value.ToCharArray();
				this.WriteInternal(buffer, 0, buffer.Length);
			}
		}

		/// <summary>Writes an internal.</summary>
		/// <exception cref="ArgumentNullException">Thrown when one or more required arguments are <c>null</c></exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="buffer">The character array from which data is read.</param>
		/// <param name="index">The character position in <paramref name="buffer" /> at which to start reading data.</param>
		/// <param name="count">The maximum number of characters to write.</param>
		protected virtual void WriteInternal(char[] buffer, int index, int count) {
			if (buffer == null) {
				throw new ArgumentNullException("buffer");
			}
			if (index < 0) {
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0) {
				throw new ArgumentOutOfRangeException("count");
			}
			var end = index + count;
			if (end > buffer.Length) {
				throw new ArgumentException("Count characters after indesx exceeds buffer length");
			}
			while (index < end) {
				this.WriteInternal(buffer[index++]);
			}
		}

		/// <summary>Writes a line terminator to the text string or stream.</summary>
		public override sealed void WriteLine() {
			this.WriteInternal(this.NewLine);
			this.IndentPending = true;
		}

		/// <summary>Writes a character followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The character to write to the text stream.</param>
		public override sealed void WriteLine(char value) {
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes an array of characters followed by a line terminator to the text string or stream.</summary>
		/// <param name="buffer">The character array from which data is read.</param>
		public override sealed void WriteLine(char[] buffer) {
			this.Write(buffer);
			this.WriteLine();
		}

		/// <summary>Writes a subarray of characters followed by a line terminator to the text string or stream.</summary>
		/// <param name="buffer">The character array from which data is read.</param>
		/// <param name="index">The character position in <paramref name="buffer" /> at which to start reading data.</param>
		/// <param name="count">The maximum number of characters to write.</param>
		public override sealed void WriteLine(char[] buffer, int index, int count) {
			this.Write(buffer, index, count);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of a Boolean value followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The Boolean value to write.</param>
		public override sealed void WriteLine(bool value) {
			base.WriteLine(value);
		}

		/// <summary>Writes the text representation of a 4-byte signed integer followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The 4-byte signed integer to write.</param>
		public override sealed void WriteLine(int value) {
			base.WriteLine(value);
		}

		/// <summary>Writes the text representation of a 4-byte unsigned integer followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The 4-byte unsigned integer to write.</param>
		[CLSCompliant(false)]
		public override sealed void WriteLine(uint value) {
			base.WriteLine(value);
		}

		/// <summary>Writes the text representation of an 8-byte signed integer followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The 8-byte signed integer to write.</param>
		public override sealed void WriteLine(long value) {
			base.WriteLine(value);
		}

		/// <summary>Writes the text representation of an 8-byte unsigned integer followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The 8-byte unsigned integer to write.</param>
		[CLSCompliant(false)]
		public override sealed void WriteLine(ulong value) {
			base.WriteLine(value);
		}

		/// <summary>Writes the text representation of a 4-byte floating-point value followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The 4-byte floating-point value to write.</param>
		public override sealed void WriteLine(float value) {
			base.WriteLine(value);
		}

		/// <summary>Writes the text representation of a 8-byte floating-point value followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The 8-byte floating-point value to write.</param>
		public override sealed void WriteLine(double value) {
			base.WriteLine(value);
		}

		/// <summary>Writes the text representation of a decimal value followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The decimal value to write.</param>
		public override sealed void WriteLine(decimal value) {
			base.WriteLine(value);
		}

		/// <summary>Writes a string followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The string to write. If <paramref name="value" /> is null, only the line terminator is written.</param>
		public override sealed void WriteLine(string value) {
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of an object by calling the ToString method on that object, followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The object to write. If <paramref name="value" /> is null, only the line terminator is written.</param>
		public override sealed void WriteLine(object value) {
			base.WriteLine(value);
		}

		/// <summary>
		/// 	Writes a formatted string and a new line to the text string or stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object)" /> method.
		/// </summary>
		/// <param name="format">A composite format string (see Remarks).</param>
		/// <param name="arg0">The object to format and write.</param>
		public override sealed void WriteLine(string format, object arg0) {
			base.WriteLine(format, arg0);
		}

		/// <summary>
		/// 	Writes a formatted string and a new line to the text string or stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object,System.Object)" />
		/// 	method.
		/// </summary>
		/// <param name="format">A composite format string (see Remarks).</param>
		/// <param name="arg0">The first object to format and write.</param>
		/// <param name="arg1">The second object to format and write.</param>
		public override sealed void WriteLine(string format, object arg0, object arg1) {
			base.WriteLine(format, arg0, arg1);
		}

		/// <summary>Writes out a formatted string and a new line, using the same semantics as <see cref="M:System.String.Format(System.String,System.Object)" />.</summary>
		/// <param name="format">A composite format string (see Remarks).</param>
		/// <param name="arg0">The first object to format and write.</param>
		/// <param name="arg1">The second object to format and write.</param>
		/// <param name="arg2">The third object to format and write.</param>
		public override sealed void WriteLine(string format, object arg0, object arg1, object arg2) {
			base.WriteLine(format, arg0, arg1, arg2);
		}

		/// <summary>Writes out a formatted string and a new line, using the same semantics as <see cref="M:System.String.Format(System.String,System.Object)" />.</summary>
		/// <param name="format">A composite format string (see Remarks).</param>
		/// <param name="arg">An object array that contains zero or more objects to format and write.</param>
		public override sealed void WriteLine(string format, params object[] arg) {
			base.WriteLine(format, arg);
		}
	}
}
