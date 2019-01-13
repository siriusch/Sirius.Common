using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

using Sirius.Collections;

namespace Sirius.StateMachine {
	/// <summary>The ranges condition emitter.</summary>
	/// <typeparam name="TComparand">Type of the comparand.</typeparam>
	/// <typeparam name="TInput">Type of the input.</typeparam>
	public class RangesConditionEmitter<TComparand, TInput>: IConditionEmitter<TComparand, TInput>
			where TComparand: IRangeSet<TInput>
			where TInput: IComparable<TInput> {
		/// <summary>The default ranges condition emitter.</summary>
		public static readonly RangesConditionEmitter<TComparand, TInput> Default = new RangesConditionEmitter<TComparand, TInput>();

		private static IEnumerator<Expression> EmitRangesWithExclusions(TComparand comparand, ParameterExpression varInput) {
			var exclusions = new List<TInput>();
			var decrement = Incrementor<TInput>.Decrement;
			var increment = Incrementor<TInput>.Increment;
			using (var enumerator = comparand.GetEnumerator()) {
				if (!enumerator.MoveNext()) {
					yield break;
				}
				bool hasNext;
				do {
					var currentRange = enumerator.Current;
					exclusions.Clear();
					for (hasNext = enumerator.MoveNext(); hasNext; hasNext = enumerator.MoveNext()) {
						var candidate = increment(currentRange.To);
						if (!candidate.Equals(decrement(enumerator.Current.From))) {
							break;
						}
						exclusions.Add(increment(currentRange.To));
						currentRange = Range<TInput>.Create(currentRange.From, enumerator.Current.To);
					}
					yield return exclusions.Aggregate(
							RangeConditionEmitter<TInput>.Default.Emit(currentRange, varInput),
							(expr, excl) => Expression.AndAlso(expr, RangeConditionEmitter<TInput>.EmitSingleValueCompare(varInput, excl)));
				} while (hasNext);
			}
		}

		/// <summary>Emits the condition to compare a single input against a set of ranges.</summary>
		/// <param name="comparand">The comparand.</param>
		/// <param name="varInput">The variable input.</param>
		/// <returns>An Expression.</returns>
		public Expression Emit(TComparand comparand, ParameterExpression varInput) {
			using (var enumerator = EmitRangesWithExclusions(comparand, varInput)) {
				if (!enumerator.MoveNext()) {
					return Expression.Constant(false);
				}
				var result = enumerator.Current;
				Debug.Assert(result != null);
				while (enumerator.MoveNext()) {
					var condition = enumerator.Current;
					Debug.Assert(condition != null);
					result = (condition.NodeType == ExpressionType.OrElse) && condition is BinaryExpression orElseCondition
							? Expression.OrElse(
									Expression.OrElse(result,
											orElseCondition.Left),
									orElseCondition.Right)
							: Expression.OrElse(
									result,
									condition);
				}
				return result;
			}
		}

		/// <summary>
		/// Check if two comparands are disjoint.
		/// </summary>
		/// <param name="x">First comparand.</param>
		/// <param name="y">Second comparand.</param>
		/// <returns><c>true</c> if the comparands are disjoint, <c>false</c> otherwise.</returns>
		public bool IsDisjoint(TComparand x, TComparand y) {
			return (new RangeSet<TInput>(x) & new RangeSet<TInput>(y)).IsEmpty;
		}
	}
}
