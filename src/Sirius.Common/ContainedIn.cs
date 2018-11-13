using System;

namespace Sirius {
	/// <summary>A bit-field of flags for specifying which set (left and/or right) an item is contained in.</summary>
	[Flags]
	public enum ContainedIn {
		/// <summary>Contained in none of the sets.</summary>
		None = 0,
		/// <summary>Contained in the left set.</summary>
		Left = 1,
		/// <summary>Contained in the right set.</summary>
		Right = 2,
		/// <summary>Contained in the left and right sets.</summary>
		Both = 3
	}
}
