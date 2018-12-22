using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class StateReferenceReplacer<TComparand, TInput>: ExpressionVisitor
			where TComparand: IEquatable<TComparand> {
		private readonly StateMachineEmitter<TComparand, TInput> emitter;

		public StateReferenceReplacer(StateMachineEmitter<TComparand, TInput> emitter) {
			this.emitter = emitter;
		}

		protected override Expression VisitUnary(UnaryExpression node) {
			if (node.NodeType == ExpressionType.Convert && node.Type == typeof(int) && typeof(StateSwitchBuilder<TComparand, TInput>).IsAssignableFrom(node.Operand.Type)) {
				var state = Expression.Lambda<Func<StateSwitchBuilder<TComparand, TInput>>>(node.Operand).Compile().Invoke();
				return Expression.Constant(this.emitter.GetIdForBuilder(state), typeof(int));
			}
			return base.VisitUnary(node);
		}
	}
}
