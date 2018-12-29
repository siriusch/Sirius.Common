using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class MatchCustomCase<TComparand, TInput, TContext>: MatchCase<TComparand, TInput, TContext>
			where TComparand: IEquatable<TComparand> {
		public Expression<Func<TInput, TContext, bool>> Condition {
			get;
		}

		public MatchCustomCase(Expression<Func<TInput, TContext, bool>> condition, StatePerformBuilder<TComparand, TInput, TContext> builder): base(builder) {
			Debug.Assert(condition != null);
			this.Condition = condition;
		}

		public override Expression EmitCondition(StateMachineEmitter<TComparand, TInput> emitter, ParameterExpression varContext) {
			return emitter.ReplaceBuildersByIds(this.Condition, emitter.InputParameter, varContext).Body;
		}
	}
}