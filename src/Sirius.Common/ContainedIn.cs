using System;

namespace Sirius {
	[Flags]
	public enum ContainedIn {
		None = 0,
		Left = 1,
		Right = 2,
		Both = 3
	}
}
