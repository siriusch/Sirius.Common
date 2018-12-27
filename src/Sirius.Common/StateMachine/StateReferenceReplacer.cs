using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class StateReferenceReplacer<TComparand, TInput>: ExpressionVisitor
			where TComparand: IEquatable<TComparand> {
		private readonly StateMachineEmitter<TComparand, TInput> emitter;
		private readonly Dictionary<ParameterExpression, Expression> replacements = new Dictionary<ParameterExpression, Expression>();

		public StateReferenceReplacer(StateMachineEmitter<TComparand, TInput> emitter) {
			this.emitter = emitter;
		}

		public void SubstituteParameter(ParameterExpression when, Expression then) {
			if (!when.Type.IsAssignableFrom(then.Type)) {
				throw new ArgumentException($"The parameter {when.Name} of type {when.Type} cannot be substituted with an expression of type {then.Type}", nameof(then));
			}
			this.replacements.Add(when, then);
		}

		protected override Expression VisitParameter(ParameterExpression node) {
			return this.replacements.TryGetValue(node, out var replacement) ? replacement : node;
		}

		protected override Expression VisitUnary(UnaryExpression node) {
			if ((node.NodeType == ExpressionType.Convert) && (node.Type == typeof(int)) && typeof(StateSwitchBuilder<TComparand, TInput>).IsAssignableFrom(node.Operand.Type)) {
				var state = Expression.Lambda<Func<StateSwitchBuilder<TComparand, TInput>>>(node.Operand).Compile().Invoke();
				return Expression.Constant(this.emitter.GetIdForBuilder(state), typeof(int));
			}
			return base.VisitUnary(node);
		}
	}
}
