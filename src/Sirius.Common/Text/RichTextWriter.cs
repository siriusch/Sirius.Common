using System;
using System.Drawing;
using System.IO;

namespace Sirius.Text {
	/// <summary>
	/// The RichTextWriter is an extended <see cref="TextWriter" /> which adds support for basic formatting and indentation, such as typically used for syntax highlighting.
	/// </summary>
	public abstract class RichTextWriter: TextWriter {
		private class IndentInfo: IDisposable {
			private readonly int depth;
			private readonly IndentInfo previous;
			private RichTextWriter owner;

			public IndentInfo(RichTextWriter owner, int depth) {
				this.owner = owner;
				this.depth = depth;
				this.previous = owner.indent;
			}

			public int Depth => this.depth;

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

		private static readonly NullTextWriter nullWriter = new NullTextWriter();

		/// <summary>
		/// Gets the null RichTextWriter.
		/// </summary>
		public new static RichTextWriter Null => nullWriter;

		/// <summary>
		/// Wraps the specified writer with a RichTextWriter, adding indentation support on the way.
		/// </summary>
		/// <param name="writer">The text writer to wrap.</param>
		/// <returns>The wrapped TextWriter.</returns>
		public static RichTextWriter Wrap(TextWriter writer) {
			return (writer as RichTextWriter) ?? new DelegateTextWriter(writer, null);
		}

		private readonly IStyleProvider styleProvider;
		private IndentInfo indent;
		private string indentChars;

		[CLSCompliant(false)]
		protected RichTextWriter(IFormatProvider formatProvider, IStyleProvider styleProvider): base(formatProvider) {
			this.styleProvider = styleProvider;
			this.IndentPending = true;
		}

		protected RichTextWriter() { }

		/// <summary>
		/// Gets or sets the indentation chars.
		/// </summary>
		/// <remarks>If not specified (or set to <c>null</c>), two spaces are used.</remarks>
		public string IndentChars {
			get {
				return this.indentChars ?? "  ";
			}
			set {
				this.indentChars = value;
			}
		}

		/// <summary>
		/// Gets the style provider provided in the constructor of this rich text writer.
		/// </summary>
		/// <value>
		/// The style provider.
		/// </value>
		[CLSCompliant(false)]
		public IStyleProvider StyleProvider => this.styleProvider;

		protected int IndentDepth {
			get {
				if (this.indent == null) {
					return 0;
				}
				return this.indent.Depth;
			}
		}

		protected virtual bool IndentPending {
			get;
			set;
		}

		protected void AssertIndentation() {
			if (this.IndentPending) {
				this.IndentPending = false;
				this.WriteIndentation();
			}
		}

		/// <summary>
		/// Increments the indentation and returns a disposable handle.
		/// </summary>
		/// <remarks>The indentation is decremented by disposing the handle.</remarks>
		/// <returns>The indentation handle.</returns>
		public IDisposable Indent() {
			return this.Indent(1);
		}

		/// <summary>
		/// Increments the indentation by the specified depth and returns a disposable handle.
		/// </summary>
		/// <remarks>The indentation is decremented by disposing the handle.</remarks>
		/// <returns>The indentation handle.</returns>
		public IDisposable Indent(int depth) {
			if (depth < 0) {
				throw new ArgumentOutOfRangeException("depth", "Negative indent is not allowed");
			}
			this.indent = new IndentInfo(this, this.IndentDepth + depth);
			return this.indent;
		}

		/// <summary>
		/// Resets the style of the writer.
		/// </summary>
		public virtual void Reset() { }

		/// <summary>
		/// Sets the background color of this writer.
		/// </summary>
		/// <param name="color">The color.</param>
		public virtual void SetBackground(Color color) { }

		/// <summary>
		/// Controls the boldness of the font.
		/// </summary>
		public virtual void SetBold(bool bold) { }

		/// <summary>
		/// Sets the foreground color of this writer.
		/// </summary>
		/// <param name="color">The color.</param>
		public virtual void SetForeground(Color color) { }

		/// <summary>
		/// Controls the italic sytle of the font.
		/// </summary>
		public virtual void SetItalic(bool bold) { }

		/// <summary>
		/// Apply a style by using the configured <see cref="StyleProvider"/>.
		/// </summary>
		/// <typeparam name="T">The enumeration type.</typeparam>
		/// <param name="style">The enumeration value specifying the style to use.</param>
		/// <remarks>This has no effect if no style provider has been set.</remarks>
		[CLSCompliant(false)]
		public void SetStyle<T>(T style) where T: struct, IComparable, IFormattable, IConvertible {
			if (this.styleProvider != null) {
				this.styleProvider.Set(this, style);
			}
		}

		public override sealed void Write(char value) {
			this.AssertIndentation();
			this.WriteInternal(value);
		}

		public override sealed void Write(char[] buffer) {
			if (buffer == null) {
				throw new ArgumentNullException("buffer");
			}
			if (buffer.Length > 0) {
				this.AssertIndentation();
				this.WriteInternal(buffer, 0, buffer.Length);
			}
		}

		public override sealed void Write(char[] buffer, int index, int count) {
			if (count > 0) {
				this.AssertIndentation();
				this.WriteInternal(buffer, index, count);
			}
		}

		public override sealed void Write(bool value) {
			base.Write(value);
		}

		public override sealed void Write(int value) {
			base.Write(value);
		}

		[CLSCompliant(false)]
		public override sealed void Write(uint value) {
			base.Write(value);
		}

		public override sealed void Write(long value) {
			base.Write(value);
		}

		[CLSCompliant(false)]
		public override sealed void Write(ulong value) {
			base.Write(value);
		}

		public override sealed void Write(float value) {
			base.Write(value);
		}

		public override sealed void Write(double value) {
			base.Write(value);
		}

		public override sealed void Write(decimal value) {
			base.Write(value);
		}

		public override sealed void Write(string value) {
			if (!string.IsNullOrEmpty(value)) {
				this.AssertIndentation();
				this.WriteInternal(value);
			}
		}

		public override sealed void Write(object value) {
			base.Write(value);
		}

		public override sealed void Write(string format, object arg0) {
			base.Write(format, arg0);
		}

		public override sealed void Write(string format, object arg0, object arg1) {
			base.Write(format, arg0, arg1);
		}

		public override sealed void Write(string format, object arg0, object arg1, object arg2) {
			base.Write(format, arg0, arg1, arg2);
		}

		// ReSharper disable MethodOverloadWithOptionalParameter
		public override sealed void Write(string format, params object[] arg) {
			base.Write(format, arg);
		}
		// ReSharper restore MethodOverloadWithOptionalParameter

		protected virtual void WriteIndentation() {
			int depth = this.IndentDepth;
			for (int i = 0; i < depth; i++) {
				this.WriteInternal(this.IndentChars);
			}
		}

		protected abstract void WriteInternal(char value);

		protected virtual void WriteInternal(string value) {
			if (!string.IsNullOrEmpty(value)) {
				char[] buffer = value.ToCharArray();
				this.WriteInternal(buffer, 0, buffer.Length);
			}
		}

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
			int end = index + count;
			if (end > buffer.Length) {
				throw new ArgumentException("Count characters after indesx exceeds buffer length");
			}
			while (index < end) {
				this.WriteInternal(buffer[index++]);
			}
		}

		public override sealed void WriteLine() {
			this.WriteInternal(this.NewLine);
			this.IndentPending = true;
		}

		public override sealed void WriteLine(char value) {
			this.Write(value);
			this.WriteLine();
		}

		public override sealed void WriteLine(char[] buffer) {
			this.Write(buffer);
			this.WriteLine();
		}

		public override sealed void WriteLine(char[] buffer, int index, int count) {
			this.Write(buffer, index, count);
			this.WriteLine();
		}

		public override sealed void WriteLine(bool value) {
			base.WriteLine(value);
		}

		public override sealed void WriteLine(int value) {
			base.WriteLine(value);
		}

		[CLSCompliant(false)]
		public override sealed void WriteLine(uint value) {
			base.WriteLine(value);
		}

		public override sealed void WriteLine(long value) {
			base.WriteLine(value);
		}

		[CLSCompliant(false)]
		public override sealed void WriteLine(ulong value) {
			base.WriteLine(value);
		}

		public override sealed void WriteLine(float value) {
			base.WriteLine(value);
		}

		public override sealed void WriteLine(double value) {
			base.WriteLine(value);
		}

		public override sealed void WriteLine(decimal value) {
			base.WriteLine(value);
		}

		public override sealed void WriteLine(string value) {
			this.Write(value);
			this.WriteLine();
		}

		public override sealed void WriteLine(object value) {
			base.WriteLine(value);
		}

		public override sealed void WriteLine(string format, object arg0) {
			base.WriteLine(format, arg0);
		}

		public override sealed void WriteLine(string format, object arg0, object arg1) {
			base.WriteLine(format, arg0, arg1);
		}

		public override sealed void WriteLine(string format, object arg0, object arg1, object arg2) {
			base.WriteLine(format, arg0, arg1, arg2);
		}

		public override sealed void WriteLine(string format, params object[] arg) {
			base.WriteLine(format, arg);
		}
	}
}
