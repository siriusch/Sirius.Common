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
			return base.Emit(emitter, emitter.ReplaceBuildersByIds(this.transition, emitter.StateParameter, contextExpression).Body, ref saveContext);
		}
	}
}
