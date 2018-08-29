using System;

namespace Sirius.Text {
	/// <summary>
	/// A style provider applies formatting styles to a <see cref="RichTextWriter"/> based on enumeration values.
	/// </summary>
	[CLSCompliant(false)]
	public interface IStyleProvider {
		/// <summary>
		/// Resets the style of the specified rich text writer.
		/// </summary>
		/// <param name="writer">The rich text writer.</param>
		void Reset(RichTextWriter writer);

		/// <summary>
		/// Sets the style on the rich text writer for a specific enumeration value.
		/// </summary>
		/// <typeparam name="T">The enumeration type.</typeparam>
		/// <param name="writer">The rich text writer.</param>
		/// <param name="style">The enumeration value to use for applying the style.</param>
		void Set<T>(RichTextWriter writer, T style) where T: struct, IComparable, IFormattable, IConvertible;
	}
}
