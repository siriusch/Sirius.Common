using System.Linq.Expressions;

namespace Sirius.StateMachine {
	/// <summary>
	/// Interface which implements an emitter for a specific comparand type
	/// </summary>
	/// <typeparam name="TComparand">Comparant type.</typeparam>
	/// <typeparam name="TInput">Input type.</typeparam>
	public interface IConditionEmitter<TComparand, TInput> {
		/// <summary>
		/// Emit an expression which checks if the input expression (of type <typeparamref name="TInput"/>) is in the comparand.
		/// </summary>
		/// <param name="comparand">The effective comparand.</param>
		/// <param name="varInput">The input parameter expression.</param>
		/// <returns>An expression of type <see cref="bool"/> which computes to <c>true</c> if the input matches the comparand.</returns>
		Expression Emit(TComparand comparand, ParameterExpression varInput);

		/// <summary>
		/// Check if two comparands are disjoint.
		/// </summary>
		/// <param name="x">First comparand.</param>
		/// <param name="y">Second comparand.</param>
		/// <returns><c>true</c> if the comparands are disjoint, <c>false</c> otherwise.</returns>
		bool IsDisjoint(TComparand x, TComparand y);
	}
}
