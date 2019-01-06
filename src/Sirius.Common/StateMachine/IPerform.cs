using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	internal interface IPerform<TComparand, TInput>: IEquatable<IPerform<TComparand, TInput>>
			where TComparand: IEquatable<TComparand> {
		Expression Emit(StateMachineEmitter<TComparand, TInput> emitContext, Expression contextExpression, ref bool saveContext);
	}

	internal interface IPerform<TComparand, TInput, TContext>: IPerform<TComparand, TInput>
			where TComparand: IEquatable<TComparand> {}
}
