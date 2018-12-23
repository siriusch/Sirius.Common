using System;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	/// <summary>A state perform chain builder.</summary>
	/// <typeparam name="TComparand">Type of the comparand.</typeparam>
	/// <typeparam name="TInput">Type of the input.</typeparam>
	/// <typeparam name="TData">Type of the data.</typeparam>
	public class StatePerformBuilder<TComparand, TInput, TData>
			where TComparand: IEquatable<TComparand> {
		private IPerform<TComparand, TInput, TData> perform;

		internal IPerform<TComparand, TInput, TData> Perform {
			get => this.perform ?? PerformConstant<TComparand, TInput, TData>.Break;
			set {
				if (this.perform != null) {
					throw new InvalidOperationException("Cannot set perform multiple times");
				}
				this.perform = value;
			}
		}

		/// <summary>Perform the given action (independent of input).</summary>
		/// <param name="action">The action.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public StatePerformBuilder<TComparand, TInput, TData> Do(Expression<Action<TData>> action) {
			var link = new PerformAction<TComparand, TInput, TData>(action);
			this.Perform = link;
			return link.Next;
		}

		/// <summary>Perform the given action.</summary>
		/// <param name="action">The action.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public StatePerformBuilder<TComparand, TInput, TData> Do(Expression<Action<TInput, TData>> action) {
			var link = new PerformInputAction<TComparand, TInput, TData>(action);
			this.Perform = link;
			return link.Next;
		}

		/// <summary>Perform the given action.</summary>
		/// <param name="action">The action.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public StatePerformBuilder<TComparand, TInput, TData> State(Expression<Action<int, TData>> action) {
			var link = new PerformStateAction<TComparand, TInput, TData>(action);
			this.Perform = link;
			return link.Next;
		}

		/// <summary>Perform the given data transition (independent of input).</summary>
		/// <typeparam name="TDataOut">Type of the data out.</typeparam>
		/// <param name="transitionChangeData">Information describing the transition change.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public StatePerformBuilder<TComparand, TInput, TDataOut> Do<TDataOut>(Expression<Func<TData, TDataOut>> transitionChangeData) {
			var link = new PerformTransition<TComparand, TInput, TData, TDataOut>(transitionChangeData);
			this.Perform = link;
			return link.Next;
		}

		/// <summary>Perform the given data transition.</summary>
		/// <typeparam name="TDataOut">Type of the data out.</typeparam>
		/// <param name="transitionChangeData">Information describing the transition change.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public StatePerformBuilder<TComparand, TInput, TDataOut> Do<TDataOut>(Expression<Func<TInput, TData, TDataOut>> transitionChangeData) {
			var link = new PerformInputTransition<TComparand, TInput, TData, TDataOut>(transitionChangeData);
			this.Perform = link;
			return link.Next;
		}

		/// <summary>Perform the given data transition.</summary>
		/// <typeparam name="TDataOut">Type of the data out.</typeparam>
		/// <param name="transitionChangeData">Information describing the transition change.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public StatePerformBuilder<TComparand, TInput, TDataOut> State<TDataOut>(Expression<Func<int, TData, TDataOut>> transitionChangeData) {
			var link = new PerformStateTransition<TComparand, TInput, TData, TDataOut>(transitionChangeData);
			this.Perform = link;
			return link.Next;
		}

		internal Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression) {
			var saveContext = false;
			return this.Emit(emitter, contextExpression, ref saveContext);
		}

		internal Expression Emit(StateMachineEmitter<TComparand, TInput> emitter, Expression contextExpression, ref bool saveContext) {
			return this.Perform.Emit(emitter, contextExpression, ref saveContext);
		}

		/// <summary>Dynamically continue processing at the given state without waiting for new input.</summary>
		/// <param name="computeState">An <see cref="Expression" /> which returns the next state as integer.</param>
		/// <remarks>Use <see cref="StateMachineEmitter{TComparand, TInput}.GetIdForBuilder" /> to find the ID of a specific state.</remarks>
		public void Goto(Expression<Func<TData, int>> computeState) {
			this.Perform = new PerformDynamic<TComparand, TInput, TData>(computeState, false);
		}

		/// <summary>Dynamically continue processing at the given state without waiting for new input.</summary>
		/// <remarks>Use <see cref="StateMachineEmitter{TComparand, TInput}.GetIdForBuilder" /> to find the ID of a specific state.</remarks>
		/// <param name="computeState">An <see cref="Expression" /> which returns the next state as integer.</param>
		public void Goto(Expression<Func<TInput, TData, int>> computeState) {
			this.Perform = new PerformInputDynamic<TComparand, TInput, TData>(computeState, false);
		}

		/// <summary>Statically continue processing at the given state without waiting for new input.</summary>
		/// <param name="target">Target to go to.</param>
		public void Goto(StateSwitchBuilder<TComparand, TInput, TData> target) {
			this.Perform = new PerformStatic<TComparand, TInput, TData>(target, false);
		}

		/// <summary>Statically continue processing at the given state without waiting for new input.</summary>
		/// <param name="target">Target to go to.</param>
		public void Goto(int target) {
			if (target < 0) {
				throw new ArgumentOutOfRangeException(nameof(target), "A Goto() target should not be negative, consider Yield()");
			}
			this.Perform = new PerformConstant<TComparand, TInput, TData>(target, false);
		}

		/// <summary>Dynamically continue processing at the given state when new input is received.</summary>
		/// <param name="computeState">An <see cref="Expression" /> which returns the next state as integer.</param>
		/// <remarks>Use <see cref="StateMachineEmitter{TComparand, TInput}.GetIdForBuilder" /> to find the ID of a specific state.</remarks>
		public void Yield(Expression<Func<TData, int>> computeState) {
			this.Perform = new PerformDynamic<TComparand, TInput, TData>(computeState, true);
		}

		/// <summary>Dynamically continue processing at the given state when new input is received.</summary>
		/// <param name="computeState">An <see cref="Expression" /> which returns the next state as integer.</param>
		/// <remarks>Use <see cref="StateMachineEmitter{TComparand, TInput}.GetIdForBuilder" /> to find the ID of a specific state.</remarks>
		public void Yield(Expression<Func<TInput, TData, int>> computeState) {
			this.Perform = new PerformInputDynamic<TComparand, TInput, TData>(computeState, true);
		}

		/// <summary>Statically continue processing at the given state when new input is received.</summary>
		/// <param name="target">Target to go to.</param>
		public void Yield(StateSwitchBuilder<TComparand, TInput, TData> target) {
			this.Perform = new PerformStatic<TComparand, TInput, TData>(target, true);
		}

		/// <summary>Statically continue processing at the given state when new input is received.</summary>
		/// <param name="target">Target to go to.</param>
		public void Yield(int target) {
			this.Perform = new PerformConstant<TComparand, TInput, TData>(target, true);
		}

		/// <summary>Statically continue processing when new input is received.</summary>
		/// <returns>A StateSwitchBuilder&lt;TInput,TData&gt;</returns>
		public StateSwitchBuilder<TComparand, TInput, TData> Yield() {
			if (this.perform is PerformStatic<TComparand, TInput, TData> performYield && performYield.Yield) {
				return performYield.Target;
			}
			var result = new StateSwitchBuilder<TComparand, TInput, TData>();
			this.Yield(result);
			return result;
		}

		/// <summary>Statically continue processing at the given state when new input is received.</summary>
		public void Break() {
			this.Perform = new PerformConstant<TComparand, TInput, TData>(null, true);
		}
	}
}
