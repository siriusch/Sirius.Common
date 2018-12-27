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
			var action = emitter.ReplaceBuildersByIds(this.action);
			var varContext = action.Parameters[0];
			return Expression.Block(
					Expression.Block(new[] {varContext},
							Expression.Assign(varContext, contextExpression),
							action.Body),
					base.Emit(emitter, contextExpression, ref saveContext));
		}
	}
}
