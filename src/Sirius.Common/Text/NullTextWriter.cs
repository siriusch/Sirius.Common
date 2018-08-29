using System.Text;

namespace Sirius.Text {
	internal class NullTextWriter: RichTextWriter {
		public override Encoding Encoding => Encoding.UTF8;

		protected override void WriteInternal(char value) { }

		protected override void WriteInternal(char[] buffer, int index, int count) { }

		protected override void WriteInternal(string value) { }
	}
}
