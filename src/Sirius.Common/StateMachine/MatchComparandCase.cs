using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class MatchComparandCase<TComparand, TInput, TContext>: MatchCase<TComparand, TInput, TContext>
			where TComparand: IEquatable<TComparand> {
		public TComparand Comparand {
			get;
		}

		public Expression<Predicate<TContext>> Condition {
			get;
		}

		public MatchComparandCase(TComparand comparand, Expression<Predicate<TContext>> condition, StatePerformBuilder<TComparand, TInput, TContext> builder): base(builder) {
			this.Comparand = comparand;
			this.Condition = condition;
		}

		public override Expression EmitCondition(StateMachineEmitter<TComparand, TInput> emitter, ParameterExpression varContext) {
			var condition = emitter.ConditionEmitter.Emit(this.Comparand, emitter.InputParameter);
			if (this.Condition == null) {
				return condition;
			}
			return Expression.AndAlso(
					condition,
					emitter.ReplaceBuildersByIds(this.Condition, varContext).Body);
		}

		public override bool TryGetComparand(out TComparand comparand) {
			comparand = this.Comparand;
			return true;
		}
	}
}
