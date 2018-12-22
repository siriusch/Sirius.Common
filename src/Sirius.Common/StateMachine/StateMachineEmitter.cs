using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Sirius.Collections;

namespace Sirius.StateMachine {
	/// <summary>A state machine emitter.</summary>
	/// <typeparam name="TComparand">Type of the comparand.</typeparam>
	/// <typeparam name="TInput">Type of the input.</typeparam>
	public class StateMachineEmitter<TComparand, TInput>
			where TComparand: IEquatable<TComparand> {
		private readonly Dictionary<int, StateSwitchBuilder<TComparand, TInput>> switchStateIds = new Dictionary<int, StateSwitchBuilder<TComparand, TInput>>();
		private readonly Dictionary<StateSwitchBuilder<TComparand, TInput>, int> switchStates = new Dictionary<StateSwitchBuilder<TComparand, TInput>, int>(ReferenceEqualityComparer<StateSwitchBuilder<TComparand, TInput>>.Default);

		/// <summary>Constructor.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="root">The root.</param>
		/// <param name="conditionEmitter">The condition emitter.</param>
		public StateMachineEmitter(StateSwitchBuilder<TComparand, TInput> root, IConditionEmitter<TComparand, TInput> conditionEmitter) {
			this.InputParameter = Expression.Parameter(typeof(TInput), "input");
			this.StateParameter = Expression.Parameter(typeof(int).MakeByRefType(), "state");
			this.ContextParameter = Expression.Parameter(typeof(object).MakeByRefType(), "context");
			this.StartLabel = Expression.Label("start");
			this.Root = root;
			this.ConditionEmitter = conditionEmitter;
			if (this.GetIdForBuilder(root) != 0) {
				throw new InvalidOperationException("Internal error: Unexpected root ID");
			}
		}

		/// <summary>Gets the root.</summary>
		/// <value>The root.</value>
		public StateSwitchBuilder<TComparand, TInput> Root {
			get;
		}

		/// <summary>Gets the condition emitter.</summary>
		/// <value>The condition emitter.</value>
		public IConditionEmitter<TComparand, TInput> ConditionEmitter {
			get;
		}

		internal ParameterExpression InputParameter {
			get;
		}

		internal ParameterExpression ContextParameter {
			get;
		}

		internal ParameterExpression StateParameter {
			get;
		}

		internal LabelTarget StartLabel {
			get;
		}

		/// <summary>Gets or sets the state ID of the error.</summary>
		/// <value>The error state.</value>
		public int BreakState {
			get;
			set;
		} = -1;

		/// <summary>Emit the state machine.</summary>
		/// <returns>An Expression&lt;StateMachineFunc&lt;TInput&gt;&gt;</returns>
		public Expression<StateMachineFunc<TInput>> Emit() {
			var cases = new List<SwitchCase>();
			for (var state = 0; state < this.switchStateIds.Count; state++) {
				// Note: Additional switchStates may be added by the call to builder.Emit() during iteration!
				cases.Add(
						Expression.SwitchCase(
								this.switchStateIds[state].Emit(this),
								Expression.Constant(state)));
			}
			return Expression.Lambda<StateMachineFunc<TInput>>(
					Expression.Block(
							Expression.Label(this.StartLabel),
							Expression.GreaterThanOrEqual(
									Expression.Assign(
											this.StateParameter,
											Expression.Switch(typeof(int),
													this.StateParameter,
													Expression.Constant(this.BreakState),
													null,
													cases)),
									Expression.Constant(0))),
					this.InputParameter,
					this.StateParameter,
					this.ContextParameter);
		}

		/// <summary>Gets the state ID associated with a builder.</summary>
		/// <param name="switchState">Builder to look-up.</param>
		/// <returns>The state ID of the builder.</returns>
		public int GetIdForBuilder(StateSwitchBuilder<TComparand, TInput> switchState) {
			if (!this.switchStates.TryGetValue(switchState, out var value)) {
				value = this.switchStates.Count;
				this.switchStates.Add(switchState, value);
				this.switchStateIds.Add(value, switchState);
			}
			return value;
		}
	}
}
