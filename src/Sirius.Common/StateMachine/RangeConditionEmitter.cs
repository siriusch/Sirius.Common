using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Sirius.Collections;

namespace Sirius.StateMachine {
	/// <summary>A range condition emitter.</summary>
	/// <typeparam name="TInput">Type of the input.</typeparam>
	public class RangeConditionEmitter<TInput>: IConditionEmitter<Range<TInput>, TInput>
			where TInput: IComparable<TInput> {
		/// <summary>The default range condition emitter.</summary>
		public static readonly RangeConditionEmitter<TInput> Default = new RangeConditionEmitter<TInput>();

		private static readonly MethodInfo meth_IComparable_Compare = Reflect<IComparable<TInput>>.GetMethod(v => v.CompareTo(default));

		internal static BinaryExpression EmitSingleValueCompare(ParameterExpression varInput, TInput value, bool equal) {
			var left = typeof(TInput).IsPrimitive
					? (Expression)varInput
					: Expression.Call(varInput, meth_IComparable_Compare, Expression.Constant(value));
			var right = typeof(TInput).IsPrimitive
					? Expression.Constant(value)
					: Expression.Constant(0);
			return equal
					? Expression.Equal(
							left,
							right)
					: Expression.NotEqual(
							left,
							right);
		}

		/// <summary>Emits the condition to compare a single input against a range.</summary>
		/// <param name="comparand">The comparand.</param>
		/// <param name="varInput">The variable input.</param>
		/// <returns>An Expression.</returns>
		public Expression Emit(Range<TInput> comparand, ParameterExpression varInput) {
			switch (comparand.Expand().Take(3).Count()) {
			case 1:
				return EmitSingleValueCompare(varInput, comparand.From, true);
			case 2:
				return typeof(TInput).IsPrimitive
						? Expression.OrElse(
								Expression.Equal(
										varInput,
										Expression.Constant(comparand.From)),
								Expression.Equal(
										varInput,
										Expression.Constant(comparand.To)))
						: Expression.OrElse(
								Expression.Equal(
										Expression.Call(varInput, meth_IComparable_Compare, Expression.Constant(comparand.From)),
										Expression.Constant(0)),
								Expression.Equal(
										Expression.Call(varInput, meth_IComparable_Compare, Expression.Constant(comparand.To)),
										Expression.Constant(0)));
			default:
				return typeof(TInput).IsPrimitive
						? Expression.AndAlso(
								Expression.GreaterThanOrEqual(
										varInput,
										Expression.Constant(comparand.From)),
								Expression.LessThanOrEqual(
										varInput,
										Expression.Constant(comparand.To)))
						: Expression.AndAlso(
								Expression.GreaterThanOrEqual(
										Expression.Call(varInput, meth_IComparable_Compare, Expression.Constant(comparand.From)),
										Expression.Constant(0)),
								Expression.LessThanOrEqual(
										Expression.Call(varInput, meth_IComparable_Compare, Expression.Constant(comparand.To)),
										Expression.Constant(0)));
			}
		}

		public bool IsDisjoint(Range<TInput> x, Range<TInput> y) {
			return (x & y).IsEmpty;
		}
	}
}
