using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class PerformInputAction<TComparand, TInput, TData>: PerformActionBase<TComparand, TInput, TData, TData>
			where TComparand: IEquatable<TComparand> {
		private readonly Expression<Action<TInput, TData>> action;

		public PerformInputAction(Expression<Action<TInput, TData>> action) {
			this.action = action;
		}

		public override Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			var varInput = this.action.Parameters[0];
			var varContext = this.action.Parameters[1];
			return Expression.Block(
					Expression.Block(new[] {varInput, varContext},
							Expression.Assign(varInput, emitter.InputParameter),
							Expression.Assign(varContext, contextExpression),
							this.action.Body),
					base.Emit(emitter, contextExpression, ref saveContext));
		}
	}
}
