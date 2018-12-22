using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal abstract class PerformActionBase<TComparand, TInput, TDataIn, TDataOut>: IPerform<TComparand, TInput, TDataIn>
			where TComparand: IEquatable<TComparand> {
		protected PerformActionBase() {
			this.Next = new StatePerformBuilder<TComparand, TInput, TDataOut>();
		}

		public StatePerformBuilder<TComparand, TInput, TDataOut> Next {
			get;
		}

		public virtual Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			return this.Next.Emit(emitter, contextExpression, ref saveContext);
		}
	}
}
