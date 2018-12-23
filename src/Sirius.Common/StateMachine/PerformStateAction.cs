using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class PerformStateAction<TComparand, TInput, TData>: PerformActionBase<TComparand, TInput, TData, TData>
			where TComparand: IEquatable<TComparand> {
		private readonly Expression<Action<int, TData>> action;

		public PerformStateAction(Expression<Action<int, TData>> action) {
			this.action = action;
		}

		public override Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			var action = emitter.ReplaceBuildersByIds(this.action);
			var varState = action.Parameters[0];
			var varContext = action.Parameters[1];
			return Expression.Block(
					Expression.Block(new[] {varState, varContext},
							Expression.Assign(varState, emitter.StateParameter),
							Expression.Assign(varContext, contextExpression),
							action.Body),
					base.Emit(emitter, contextExpression, ref saveContext));
		}
	}
}
