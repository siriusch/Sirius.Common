using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class PerformInputContextChange<TComparand, TInput, TContextIn, TContextOut>: PerformActionBase<TComparand, TInput, TContextIn, TContextOut>
			where TComparand: IEquatable<TComparand> {
		private readonly Expression<Func<TInput, TContextIn, TContextOut>> transition;

		public PerformInputContextChange(Expression<Func<TInput, TContextIn, TContextOut>> transition) {
			this.transition = transition;
		}

		public override Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			emitter.AssertCanChangeContext();
			saveContext = true;
			return base.Emit(emitter, emitter.ReplaceBuildersByIds(this.transition, emitter.InputParameter, contextExpression).Body, ref saveContext);
		}
	}
}
