using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class PerformInputDynamic<TComparand, TInput, TContext>: IPerform<TComparand, TInput, TContext>
			where TComparand: IEquatable<TComparand> {
		private readonly Expression<Func<TInput, TContext, int>> computeState;

		public PerformInputDynamic(Expression<Func<TInput, TContext, int>> computeState, bool yield) {
			this.computeState = computeState;
			this.Yield = yield;
		}

		public bool Yield {
			get;
		}

		public Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			var body = new List<Expression>();
			if (saveContext) {
				body.Add(Expression.Assign(
						emitter.ContextParameter,
						Expression.Convert(
								contextExpression,
								typeof(object))));
			}
			var computeState = emitter.ReplaceBuildersByIds(this.computeState, emitter.InputParameter, contextExpression);
			if (this.Yield) {
				body.Add(computeState.Body);
			} else {
				body.Add(Expression.Assign(
						emitter.StateParameter,
						computeState.Body));
				body.Add(Expression.Goto(
						emitter.StartLabel,
						typeof(int)));
			}
			return body.Count == 1 ? body[0] : Expression.Block(body);
		}

		public override int GetHashCode() {
			return unchecked(GetType().GetHashCode() ^ (this.computeState.GetHashCode() * 397) ^ (this.Yield.GetHashCode() * 122959073) ^ typeof(TContext).GetHashCode());
		}

		public sealed override bool Equals(object obj) {
			return this.Equals(obj as IPerform<TComparand, TInput>);
		}

		public bool Equals(IPerform<TComparand, TInput> other) {
			return other is PerformInputDynamic<TComparand, TInput, TContext> otherDynamic && this.computeState == otherDynamic.computeState && this.Yield == otherDynamic.Yield;
		}
	}
}
