using System.Linq.Expressions;

namespace Sirius.StateMachine {
	public interface IConditionEmitter<TComparand, TInput> {
		Expression Emit(TComparand comparand, ParameterExpression varInput);
	}
}
