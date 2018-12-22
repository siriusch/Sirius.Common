using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class PerformInputTransition<TComparand, TInput, TDataIn, TDataOut>: PerformActionBase<TComparand, TInput, TDataIn, TDataOut>
			where TComparand: IEquatable<TComparand> {
		private readonly Expression<Func<TInput, TDataIn, TDataOut>> transition;

		public PerformInputTransition(Expression<Func<TInput, TDataIn, TDataOut>> transition) {
			this.transition = transition;
		}

		public override Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			saveContext = true;
			var varInput = this.transition.Parameters[0];
			var varContext = this.transition.Parameters[1];
			return base.Emit(emitter,
					Expression.Block(new[] {varInput, varContext},
							Expression.Assign(varInput, emitter.InputParameter),
							Expression.Assign(varContext, contextExpression),
							this.transition.Body), ref saveContext);
		}
	}
}
