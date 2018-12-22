using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class PerformTransition<TComparand, TInput, TDataIn, TDataOut>: PerformActionBase<TComparand, TInput, TDataIn, TDataOut>
			where TComparand: IEquatable<TComparand> {
		private readonly Expression<Func<TDataIn, TDataOut>> transition;

		public PerformTransition(Expression<Func<TDataIn, TDataOut>> transition) {
			this.transition = transition;
		}

		public override Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			saveContext = true;
			var transition = emitter.ReplaceBuildersByIds(this.transition);
			var varContext = transition.Parameters[0];
			return base.Emit(emitter,
					Expression.Block(new[] {varContext},
							Expression.Assign(varContext, contextExpression),
							transition.Body), ref saveContext);
		}
	}
}
