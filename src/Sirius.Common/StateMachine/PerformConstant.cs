using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class PerformConstant<TComparand, TInput, TData>: IPerform<TComparand, TInput, TData>
			where TComparand: IEquatable<TComparand> {
		public static readonly PerformConstant<TComparand, TInput, TData> Break = new PerformConstant<TComparand, TInput, TData>(null, true);

		public PerformConstant(int? target, bool yield) {
			this.Yield = yield;
			this.Target = target;
		}

		public bool Yield {
			get;
		}

		public int? Target {
			get;
		}

		public Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			var body = new List<Expression>();
			if (saveContext) {
				body.Add(Expression.Assign(
						emitter.ContextParameter,
						Expression.Convert(contextExpression, typeof(object))));
			}
			if (this.Yield) {
				body.Add(Expression.Constant(this.Target.GetValueOrDefault(emitter.BreakState)));
			} else {
				body.Add(Expression.Assign(
						emitter.StateParameter,
						Expression.Constant(this.Target.GetValueOrDefault(emitter.BreakState))));
				body.Add(Expression.Goto(
						emitter.StartLabel,
						typeof(int)));
			}
			return Expression.Block(body);
		}
	}
}
