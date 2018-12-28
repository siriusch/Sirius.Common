using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal class ParameterUsageFinder: ExpressionVisitor {
		private readonly HashSet<ParameterExpression> usedParameters = new HashSet<ParameterExpression>();

		public bool IsUsed(ParameterExpression expression) {
			return this.usedParameters.Contains(expression);
		}

		protected override Expression VisitParameter(ParameterExpression node) {
			this.usedParameters.Add(node);
			return base.VisitParameter(node);
		}
	}
}
