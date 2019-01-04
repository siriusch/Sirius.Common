using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class PerformAction<TComparand, TInput, TContext>: PerformActionBase<TComparand, TInput, TContext, TContext>
			where TComparand: IEquatable<TComparand> {
		private readonly Expression<Action<TContext>> action;

		public PerformAction(Expression<Action<TContext>> action) {
			this.action = action;
		}

		public override Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			return Expression.Block(
					emitter.ReplaceBuildersByIds(this.action, contextExpression).Body,
					base.Emit(emitter, contextExpression, ref saveContext));
		}

		public override bool Equals(IPerform<TComparand, TInput, TContext> other) {
			return other is PerformAction<TComparand, TInput, TContext> otherAction && (this.action == otherAction.action) && base.Equals(other);
		}
	}
}
