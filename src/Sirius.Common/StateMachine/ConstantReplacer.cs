using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class ConstantReplacer<TComparand, TInput>: ExpressionVisitor
			where TComparand: IEquatable<TComparand> {
		private readonly StateMachineEmitter<TComparand, TInput> emitter;

		public ConstantReplacer(StateMachineEmitter<TComparand, TInput> emitter) {
			this.emitter = emitter;
		}

		public Expression<Func<TData, int>> Modify<TData>(Expression<Func<TData, StateSwitchBuilder<TComparand, TInput, TData>>> computeState) {
			return Expression.Lambda<Func<TData, int>>(this.Visit(computeState.Body), computeState.Parameters);
		}

		public Expression<Func<TInput, TData, int>> Modify<TData>(Expression<Func<TInput, TData, StateSwitchBuilder<TComparand, TInput, TData>>> computeState) {
			return Expression.Lambda<Func<TInput, TData, int>>(this.Visit(computeState.Body), computeState.Parameters);
		}

		protected override Expression VisitConstant(ConstantExpression node) {
			if (node.Value is StateSwitchBuilder<TComparand, TInput> state) {
				return Expression.Constant(this.emitter.GetIdForBuilder(state), typeof(int));
			}
			return base.VisitConstant(node);
		}

		protected override Expression VisitMember(MemberExpression node) {
			if (typeof(StateSwitchBuilder<TComparand, TInput>).IsAssignableFrom(node.Type)) {
				// Execute expression to get the constant state value
				var state = Expression.Lambda<Func<StateSwitchBuilder<TComparand, TInput>>>(node).Compile().Invoke();
				return Expression.Constant(this.emitter.GetIdForBuilder(state), typeof(int));
			}
			return base.VisitMember(node);
		}
	}
}
