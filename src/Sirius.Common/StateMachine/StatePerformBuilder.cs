using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	/// <summary>A state perform chain builder.</summary>
	/// <typeparam name="TComparand">Type of the comparand.</typeparam>
	/// <typeparam name="TInput">Type of the input.</typeparam>
	/// <typeparam name="TContext">Type of the context.</typeparam>
	public class StatePerformBuilder<TComparand, TInput, TContext>
			where TComparand: IEquatable<TComparand> {
		private IPerform<TComparand, TInput, TContext> perform;

		internal IPerform<TComparand, TInput, TContext> Perform {
			get => this.perform ?? PerformConstant<TComparand, TInput, TContext>.Break;
			set {
				if (this.perform != null) {
					throw new InvalidOperationException("Cannot set perform multiple times");
				}
				this.perform = value;
			}
		}

		/// <summary>Statically continue processing at the given state when new input is received.</summary>
		public void Break() {
			this.Perform = new PerformConstant<TComparand, TInput, TContext>(null, true);
		}

		/// <summary>Perform the given action (independent of input).</summary>
		/// <param name="action">The action.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public StatePerformBuilder<TComparand, TInput, TContext> Do(Expression<Action<TContext>> action) {
			var link = new PerformAction<TComparand, TInput, TContext>(action);
			this.Perform = link;
			return link.Next;
		}

		/// <summary>Perform the given action.</summary>
		/// <param name="action">The action.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public StatePerformBuilder<TComparand, TInput, TContext> Do(Expression<Action<TInput, TContext>> action) {
			var link = new PerformInputAction<TComparand, TInput, TContext>(action);
			this.Perform = link;
			return link.Next;
		}

		/// <summary>Perform the given data transition (independent of input).</summary>
		/// <typeparam name="TDataOut">Type of the data out.</typeparam>
		/// <param name="transitionChangeData">Information describing the transition change.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public StatePerformBuilder<TComparand, TInput, TDataOut> Do<TDataOut>(Expression<Func<TContext, TDataOut>> transitionChangeData) {
			var link = new PerformContextChange<TComparand, TInput, TContext, TDataOut>(transitionChangeData);
			this.Perform = link;
			return link.Next;
		}

		/// <summary>Perform the given data transition.</summary>
		/// <typeparam name="TDataOut">Type of the data out.</typeparam>
		/// <param name="transitionChangeData">Information describing the transition change.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public StatePerformBuilder<TComparand, TInput, TDataOut> Do<TDataOut>(Expression<Func<TInput, TContext, TDataOut>> transitionChangeData) {
			var link = new PerformInputContextChange<TComparand, TInput, TContext, TDataOut>(transitionChangeData);
			this.Perform = link;
			return link.Next;
		}

		internal Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, ParameterExpression contextExpression) {
			var saveContext = false;
			return this.Emit(emitter, contextExpression, ref saveContext);
		}

		internal Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			return this.Perform.Emit(emitter, contextExpression, ref saveContext);
		}

		/// <summary>Dynamically continue processing at the given state without waiting for new input.</summary>
		/// <param name="computeState">An <see cref="Expression" /> which returns the next state as integer.</param>
		/// <remarks>Use <see cref="StateMachineEmitter{TComparand, TInput}.GetIdForBuilder" /> to find the ID of a specific state.</remarks>
		public void Goto(Expression<Func<TContext, int>> computeState) {
			this.Perform = new PerformDynamic<TComparand, TInput, TContext>(computeState, false);
		}

		/// <summary>Dynamically continue processing at the given state without waiting for new input.</summary>
		/// <remarks>Use <see cref="StateMachineEmitter{TComparand, TInput}.GetIdForBuilder" /> to find the ID of a specific state.</remarks>
		/// <param name="computeState">An <see cref="Expression" /> which returns the next state as integer.</param>
		public void Goto(Expression<Func<TInput, TContext, int>> computeState) {
			this.Perform = new PerformInputDynamic<TComparand, TInput, TContext>(computeState, false);
		}

		/// <summary>Statically continue processing at the given state without waiting for new input.</summary>
		/// <param name="target">Target to go to.</param>
		public void Goto(StateSwitchBuilder<TComparand, TInput, TContext> target) {
			this.Perform = new PerformStatic<TComparand, TInput, TContext>(target, false);
		}

		/// <summary>Statically continue processing at the given state without waiting for new input.</summary>
		/// <param name="target">Target to go to.</param>
		public void Goto(int target) {
			if (target < 0) {
				throw new ArgumentOutOfRangeException(nameof(target), "A Goto() target should not be negative, consider Yield()");
			}
			this.Perform = new PerformConstant<TComparand, TInput, TContext>(target, false);
		}

		/// <summary>Perform the given action.</summary>
		/// <param name="action">The action.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public StatePerformBuilder<TComparand, TInput, TContext> State(Expression<Action<int, TContext>> action) {
			var link = new PerformStateAction<TComparand, TInput, TContext>(action);
			this.Perform = link;
			return link.Next;
		}

		/// <summary>Perform the given data transition.</summary>
		/// <typeparam name="TDataOut">Type of the data out.</typeparam>
		/// <param name="transitionChangeData">Information describing the transition change.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public StatePerformBuilder<TComparand, TInput, TDataOut> State<TDataOut>(Expression<Func<int, TContext, TDataOut>> transitionChangeData) {
			var link = new PerformStateTransition<TComparand, TInput, TContext, TDataOut>(transitionChangeData);
			this.Perform = link;
			return link.Next;
		}

		/// <summary>Dynamically continue processing at the given state when new input is received.</summary>
		/// <param name="computeState">An <see cref="Expression" /> which returns the next state as integer.</param>
		/// <remarks>Use <see cref="StateMachineEmitter{TComparand, TInput}.GetIdForBuilder" /> to find the ID of a specific state.</remarks>
		public void Yield(Expression<Func<TContext, int>> computeState) {
			this.Perform = new PerformDynamic<TComparand, TInput, TContext>(computeState, true);
		}

		/// <summary>Dynamically continue processing at the given state when new input is received.</summary>
		/// <param name="computeState">An <see cref="Expression" /> which returns the next state as integer.</param>
		/// <remarks>Use <see cref="StateMachineEmitter{TComparand, TInput}.GetIdForBuilder" /> to find the ID of a specific state.</remarks>
		public void Yield(Expression<Func<TInput, TContext, int>> computeState) {
			this.Perform = new PerformInputDynamic<TComparand, TInput, TContext>(computeState, true);
		}

		/// <summary>Statically continue processing at the given state when new input is received.</summary>
		/// <param name="target">Target to go to.</param>
		public void Yield(StateSwitchBuilder<TComparand, TInput, TContext> target) {
			this.Perform = new PerformStatic<TComparand, TInput, TContext>(target, true);
		}

		/// <summary>Statically continue processing at the given state when new input is received.</summary>
		/// <param name="target">Target to go to.</param>
		public void Yield(int target) {
			this.Perform = new PerformConstant<TComparand, TInput, TContext>(target, true);
		}

		/// <summary>Statically continue processing when new input is received.</summary>
		/// <returns>A StateSwitchBuilder&lt;TInput,TData&gt;</returns>
		public StateSwitchBuilder<TComparand, TInput, TContext> Yield() {
			if (this.perform is PerformStatic<TComparand, TInput, TContext> performYield && performYield.Yield) {
				return performYield.Target;
			}
			var result = new StateSwitchBuilder<TComparand, TInput, TContext>();
			this.Yield(result);
			return result;
		}
	}
}
