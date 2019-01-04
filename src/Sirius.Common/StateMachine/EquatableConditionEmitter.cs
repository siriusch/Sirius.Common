using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Sirius.StateMachine {
	/// <summary>An equatable condition emitter.</summary>
	/// <typeparam name="TInput">Type of the input.</typeparam>
	public class EquatableConditionEmitter<TInput>: IConditionEmitter<TInput, TInput>
			where TInput: IEquatable<TInput> {
		/// <summary>The default equatable condition emitter.</summary>
		public static readonly EquatableConditionEmitter<TInput> Default = new EquatableConditionEmitter<TInput>();

		private static readonly MethodInfo meth_IEquatable_Equals = Reflect<TInput>.GetMethod(i => i.Equals(default));

		/// <summary>Emits the equatable condition.</summary>
		/// <param name="comparand">The comparand.</param>
		/// <param name="varInput">The variable input.</param>
		/// <returns>An Expression.</returns>
		public Expression Emit(TInput comparand, ParameterExpression varInput) {
			if (typeof(TInput).IsPrimitive) {
				return Expression.Equal(
						varInput,
						Expression.Constant(comparand));
			}
			return Expression.Call(
					Expression.Constant(comparand),
					meth_IEquatable_Equals,
					varInput);
		}

		/// <summary>
		/// Check if two comparands are disjoint.
		/// </summary>
		/// <param name="x">First comparand.</param>
		/// <param name="y">Second comparand.</param>
		/// <returns><c>true</c> if the comparands are disjoint, <c>false</c> otherwise.</returns>
		public bool IsDisjoint(TInput x, TInput y) {
			return !x.Equals(y);
		}
	}
}
