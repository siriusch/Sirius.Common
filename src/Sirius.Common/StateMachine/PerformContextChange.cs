using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class PerformContextChange<TComparand, TInput, TContextIn, TContextOut>: PerformActionBase<TComparand, TInput, TContextIn, TContextOut>
			where TComparand: IEquatable<TComparand> {
		private readonly Expression<Func<TContextIn, TContextOut>> transition;

		public PerformContextChange(Expression<Func<TContextIn, TContextOut>> transition) {
			this.transition = transition;
		}

		public override Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			emitter.AssertCanChangeContext();
			saveContext = true;
			return base.Emit(emitter, emitter.ReplaceBuildersByIds(this.transition, contextExpression).Body, ref saveContext);
		}
	}
}
