using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal abstract class PerformActionBase<TComparand, TInput, TContextIn, TContextOut>: IPerform<TComparand, TInput, TContextIn>
			where TComparand: IEquatable<TComparand> {
		protected PerformActionBase() {
			this.Next = new StatePerformBuilder<TComparand, TInput, TContextOut>();
		}

		public StatePerformBuilder<TComparand, TInput, TContextOut> Next {
			get;
		}

		public virtual Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			return this.Next.Emit(emitter, contextExpression, ref saveContext);
		}
	}
}
