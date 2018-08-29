using System;
using System.IO;
using System.Text;

namespace Sirius.Text {
	internal class DelegateTextWriter: RichTextWriter {
		private readonly TextWriter writer;

		public DelegateTextWriter(TextWriter writer, IStyleProvider styleProvider): base(null, styleProvider) {
			this.writer = writer;
		}

		public override Encoding Encoding => this.writer.Encoding;

		public override IFormatProvider FormatProvider => this.writer.FormatProvider;

		public override string NewLine {
			get {
				return this.writer.NewLine;
			}
			set {
				this.writer.NewLine = value;
			}
		}

		public override void Close() {
			this.writer.Close();
		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
				this.writer.Dispose();
			}
		}

		public override void Flush() {
			this.writer.Flush();
		}

		public override string ToString() {
			return this.writer.ToString();
		}

		protected override void WriteInternal(char value) {
			this.writer.Write(value);
		}

		protected override void WriteInternal(char[] buffer, int index, int count) {
			this.writer.Write(buffer, index, count);
		}

		protected override void WriteInternal(string value) {
			this.writer.Write(value);
		}
	}
}
