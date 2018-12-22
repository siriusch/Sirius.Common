using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class PerformStatic<TComparand, TInput, TData>: IPerform<TComparand, TInput, TData>
			where TComparand: IEquatable<TComparand> {
		public PerformStatic(StateSwitchBuilder<TComparand, TInput, TData> target, bool yield) {
			this.Yield = yield;
			this.Target = target;
		}

		public bool Yield {
			get;
		}

		public StateSwitchBuilder<TComparand, TInput, TData> Target {
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
				body.Add(Expression.Constant(emitter.GetIdForBuilder(this.Target)));
			} else {
				body.Add(Expression.Assign(
						emitter.StateParameter,
						Expression.Constant(emitter.GetIdForBuilder(this.Target))));
				body.Add(Expression.Goto(
						emitter.StartLabel,
						typeof(int)));
			}
			return Expression.Block(body);
		}
	}
}
