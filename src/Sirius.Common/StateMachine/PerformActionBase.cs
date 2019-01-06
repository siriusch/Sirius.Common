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

		public override int GetHashCode() {
			return unchecked((GetType().GetHashCode() ^ this.Next.Perform.GetHashCode() * 397) ^ (typeof(TContextIn).GetHashCode() * 3) ^ typeof(TContextOut).GetHashCode());
		}

		public sealed override bool Equals(object obj) {
			return this.Equals(obj as IPerform<TComparand, TInput>);
		}

		public virtual bool Equals(IPerform<TComparand, TInput> other) {
			return other is PerformActionBase<TComparand, TInput, TContextIn, TContextOut> otherAction && this.Next.Perform.Equals(otherAction.Next.Perform);
		}
	}
}
