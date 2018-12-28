namespace Sirius.StateMachine {
	/// <summary>A state machine function.</summary>
	/// <typeparam name="TInput">Type of the input.</typeparam>
	/// <param name="input">The input.</param>
	/// <param name="state">[in,out] The state.</param>
	/// <param name="context">[in,out] The context.</param>
	/// <returns><c>true</c> if the state is valid (&gt;= 0), <c>false</c> otherwise.</returns>
	public delegate bool StateMachineFunc<in TInput>(TInput input, ref int state, ref object context);

	/// <summary>A state machine function.</summary>
	/// <typeparam name="TInput">Type of the input.</typeparam>
	/// <typeparam name="TContext">Type of the context.</typeparam>
	/// <param name="input">The input.</param>
	/// <param name="state">[in,out] The state.</param>
	/// <param name="context">[in,out] The context.</param>
	/// <returns><c>true</c> if the state is valid (&gt;= 0), <c>false</c> otherwise.</returns>
	public delegate bool StateMachineFunc<in TInput, in TContext>(TInput input, ref int state, TContext context) where TContext: class;
}
