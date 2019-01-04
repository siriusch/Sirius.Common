using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal interface IPerform<TComparand, TInput, TContext>: IEquatable<IPerform<TComparand, TInput, TContext>>
			where TComparand: IEquatable<TComparand> {
		Expression Emit(StateMachineEmitter<TComparand, TInput> emitContext, Expression contextExpression, ref bool saveContext);
	}
}
