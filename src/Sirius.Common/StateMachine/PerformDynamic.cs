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
			var computeState = emitter.ReplaceBuildersByIds(this.computeState);
			var varContext = computeState.Parameters[0];
			var body = new List<Expression>();
			if (saveContext) {
				body.Add(Expression.Assign(
						emitter.ContextParameter,
						Expression.Convert(
								Expression.Assign(varContext, contextExpression),
								typeof(object))));
			} else {
				body.Add(Expression.Assign(varContext, contextExpression));
			}
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
			return Expression.Block(new[] {varContext}, body);
		}
	}
}
