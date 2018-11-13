using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Sirius {
	/// <summary>Helper class to provide non-interlocked operations analogous to <see cref="Interlocked"/>.</summary>
	public static class Noninterlocked {
		/// <summary>Exchanges two variable values.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="obj1">[in,out] The first value.</param>
		/// <param name="obj2">[in,out] The second value.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Exchange<T>(ref T obj1, ref T obj2) {
			var temp = obj1;
			obj1 = obj2;
			obj2 = temp;
		}
	}
}
