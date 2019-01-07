using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal abstract class MatchCase<TComparand, TInput, TContext>
			where TComparand: IEquatable<TComparand> {
		public StatePerformBuilder<TComparand, TInput, TContext> Builder {
			get;
		}

		protected MatchCase(StatePerformBuilder<TComparand, TInput, TContext> builder) {
			Debug.Assert(builder != null);
			this.Builder = builder;
		}

		public abstract Expression EmitCondition(StateMachineEmitter<TComparand, TInput> emitter, ParameterExpression varContext);

		public abstract bool TryGetComparand(out TComparand comparand);
	}
}
