using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class PerformStateAction<TComparand, TInput, TContext>: PerformActionBase<TComparand, TInput, TContext, TContext>
			where TComparand: IEquatable<TComparand> {
		private readonly Expression<Action<int, TContext>> action;

		public PerformStateAction(Expression<Action<int, TContext>> action) {
			this.action = action;
		}

		public override Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			return Expression.Block(
					emitter.ReplaceBuildersByIds(this.action, emitter.StateParameter, contextExpression).Body,
					base.Emit(emitter, contextExpression, ref saveContext));
		}

		public override bool Equals(IPerform<TComparand, TInput, TContext> other) {
			return other is PerformStateAction<TComparand, TInput, TContext> otherAction && (this.action == otherAction.action) && base.Equals(other);
		}
	}
}
