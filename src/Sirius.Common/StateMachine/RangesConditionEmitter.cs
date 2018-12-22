using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Sirius.Collections;

namespace Sirius.StateMachine {
	/// <summary>The ranges condition emitter.</summary>
	/// <typeparam name="TComparand">Type of the comparand.</typeparam>
	/// <typeparam name="TInput">Type of the input.</typeparam>
	public class RangesConditionEmitter<TComparand, TInput>: IConditionEmitter<TComparand, TInput>
			where TComparand: IEnumerable<Range<TInput>>
			where TInput: IComparable<TInput> {
		/// <summary>The default ranges condition emitter.</summary>
		public static readonly RangesConditionEmitter<TComparand, TInput> Default = new RangesConditionEmitter<TComparand, TInput>();

		/// <summary>Emits the condition to compare a single input against a set of ranges.</summary>
		/// <param name="comparand">The comparand.</param>
		/// <param name="varInput">The variable input.</param>
		/// <returns>An Expression.</returns>
		public Expression Emit(TComparand comparand, ParameterExpression varInput) {
			using (var enumerator = comparand.GetEnumerator()) {
				if (!enumerator.MoveNext()) {
					return Expression.Constant(false);
				}
				var result = RangeConditionEmitter<TInput>.Default.Emit(enumerator.Current, varInput);
				while (enumerator.MoveNext()) {
					result = Expression.OrElse(
							result,
							RangeConditionEmitter<TInput>.Default.Emit(enumerator.Current, varInput));
				}
				return result;
			}
		}
	}
}
