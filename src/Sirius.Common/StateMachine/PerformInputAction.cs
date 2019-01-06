using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class PerformInputAction<TComparand, TInput, TContext>: PerformActionBase<TComparand, TInput, TContext, TContext>
			where TComparand: IEquatable<TComparand> {
		private readonly Expression<Action<TInput, TContext>> action;

		public PerformInputAction(Expression<Action<TInput, TContext>> action) {
			this.action = action;
		}

		public override Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			return Expression.Block(
					emitter.ReplaceBuildersByIds(this.action, emitter.InputParameter, contextExpression).Body,
					base.Emit(emitter, contextExpression, ref saveContext));
		}

		public override int GetHashCode() {
			return this.action.GetHashCode() ^ base.GetHashCode();
		}

		public override bool Equals(IPerform<TComparand, TInput> other) {
			return other is PerformInputAction<TComparand, TInput, TContext> otherAction && (this.action == otherAction.action) && base.Equals(other);
		}
	}
}
