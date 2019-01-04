using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class PerformDynamic<TComparand, TInput, TContext>: IPerform<TComparand, TInput, TContext>
			where TComparand: IEquatable<TComparand> {
		private readonly Expression<Func<TContext, int>> computeState;

		public PerformDynamic(Expression<Func<TContext, int>> computeState, bool yield) {
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
			var computeState = emitter.ReplaceBuildersByIds(this.computeState, contextExpression);
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

		public bool Equals(IPerform<TComparand, TInput, TContext> other) {
			return other is PerformDynamic<TComparand, TInput, TContext> otherDynamic && this.computeState == otherDynamic.computeState && this.Yield == otherDynamic.Yield;
		}
	}
}
