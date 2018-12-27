using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class PerformStateTransition<TComparand, TInput, TContextIn, TContextOut>: PerformActionBase<TComparand, TInput, TContextIn, TContextOut>
			where TComparand: IEquatable<TComparand> {
		private readonly Expression<Func<int, TContextIn, TContextOut>> transition;

		public PerformStateTransition(Expression<Func<int, TContextIn, TContextOut>> transition) {
			this.transition = transition;
		}

		public override Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			saveContext = true;
			var transition = emitter.ReplaceBuildersByIds(this.transition);
			var varState = transition.Parameters[0];
			var varContext = transition.Parameters[1];
			return base.Emit(emitter,
					Expression.Block(new[] {varState, varContext},
							Expression.Assign(varState, emitter.StateParameter),
							Expression.Assign(varContext, contextExpression),
							transition.Body), ref saveContext);
		}
	}
}
