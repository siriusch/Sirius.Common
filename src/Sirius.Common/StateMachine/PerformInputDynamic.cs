using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class PerformInputDynamic<TComparand, TInput, TData>: IPerform<TComparand, TInput, TData>
			where TComparand: IEquatable<TComparand> {
		private readonly Expression<Func<TInput, TData, StateSwitchBuilder<TComparand, TInput, TData>>> computeState;

		public PerformInputDynamic(Expression<Func<TInput, TData, StateSwitchBuilder<TComparand, TInput, TData>>> computeState, bool yield) {
			this.computeState = computeState;
			this.Yield = yield;
		}

		public bool Yield {
			get;
		}

		public Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			var computeStateInt = new ConstantReplacer<TComparand, TInput>(emitter).Modify(this.computeState);
			var varInput = computeStateInt.Parameters[0];
			var varContext = computeStateInt.Parameters[1];
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
			body.Add(Expression.Assign(varInput, emitter.InputParameter));
			if (this.Yield) {
				body.Add(computeStateInt.Body);
			} else {
				body.Add(Expression.Assign(
						emitter.StateParameter,
						computeStateInt.Body));
				body.Add(Expression.Goto(
						emitter.StartLabel,
						typeof(int)));
			}
			return Expression.Block(new[] {varInput, varContext}, body);
		}
	}
}
