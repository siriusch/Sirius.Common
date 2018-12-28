using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using JetBrains.Annotations;

using Sirius.Collections;

namespace Sirius.StateMachine {
	/// <summary>A state machine emitter.</summary>
	/// <typeparam name="TComparand">Type of the comparand.</typeparam>
	/// <typeparam name="TInput">Type of the input.</typeparam>
	public class StateMachineEmitter<TComparand, TInput>
			where TComparand: IEquatable<TComparand> {
		private readonly List<Expression> onEnter = new List<Expression>();
		private readonly List<Expression> onLeave = new List<Expression>();
		private readonly Dictionary<int, StateSwitchBuilder<TComparand, TInput>> switchStateIds = new Dictionary<int, StateSwitchBuilder<TComparand, TInput>>();
		private readonly Dictionary<StateSwitchBuilder<TComparand, TInput>, int> switchStates = new Dictionary<StateSwitchBuilder<TComparand, TInput>, int>(ReferenceEqualityComparer<StateSwitchBuilder<TComparand, TInput>>.Default);

		/// <summary>Constructor.</summary>
		/// <exception cref="ArgumentNullException">Thrown when one or more required arguments are <c>null</c></exception>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="root">The root.</param>
		/// <param name="conditionEmitter">The condition emitter.</param>
		public StateMachineEmitter([NotNull] StateSwitchBuilder<TComparand, TInput> root, [NotNull] IConditionEmitter<TComparand, TInput> conditionEmitter): this(root, conditionEmitter, null) { }

		/// <summary>Constructor.</summary>
		/// <exception cref="ArgumentNullException">Thrown when one or more required arguments are <c>null</c></exception>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="root">The root.</param>
		/// <param name="conditionEmitter">The condition emitter.</param>
		/// <param name="contextType">Type of the context.</param>
		public StateMachineEmitter([NotNull] StateSwitchBuilder<TComparand, TInput> root, [NotNull] IConditionEmitter<TComparand, TInput> conditionEmitter, Type contextType) {
			this.Root = root ?? throw new ArgumentNullException(nameof(root));
			this.ConditionEmitter = conditionEmitter ?? throw new ArgumentNullException(nameof(conditionEmitter));
			this.InputParameter = Expression.Parameter(typeof(TInput), "input");
			this.StateParameter = Expression.Parameter(typeof(int).MakeByRefType(), "state");
			this.ContextParameter = Expression.Parameter(contextType ?? typeof(object).MakeByRefType(), "context");
			this.StartLabel = Expression.Label("start");
			if (this.GetIdForBuilder(this.Root) != 0) {
				throw new InvalidOperationException("Internal error: Unexpected root ID");
			}
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

		/// <summary>Gets the root.</summary>
		/// <value>The root.</value>
		public StateSwitchBuilder<TComparand, TInput> Root {
			get;
		}

		/// <summary>Gets or sets the state ID of the error.</summary>
		/// <value>The error state.</value>
		public int BreakState {
			get;
			set;
		} = -1;

		internal void AssertCanChangeContext() {
			if (!this.ContextParameter.IsByRef) {
				throw new InvalidOperationException("The context cannot be reset");
			}
		}

		/// <summary>Emit the state machine function.</summary>
		/// <returns>An Expression&lt;StateMachineFunc&lt;TInput&gt;&gt;</returns>
		public Expression<StateMachineFunc<TInput>> Emit() {
			return Expression.Lambda<StateMachineFunc<TInput>>(this.EmitBody(),
					this.InputParameter,
					this.StateParameter,
					this.ContextParameter);
		}

		/// <summary>Emit the state machine body.</summary>
		protected Expression EmitBody() {
			var cases = new List<SwitchCase>();
			for (var state = 0; state < this.switchStateIds.Count; state++) {
				// Note: Additional switchStates may be added by the call to builder.Emit() during iteration!
				cases.Add(
						Expression.SwitchCase(
								this.switchStateIds[state].Emit(this),
								Expression.Constant(state)));
			}
			var body = new List<Expression>();
			body.AddRange(this.onEnter);
			body.Add(Expression.Label(this.StartLabel));
			body.Add(Expression.Assign(
					this.StateParameter,
					Expression.Switch(typeof(int),
							this.StateParameter,
							Expression.Constant(this.BreakState),
							null,
							cases)));
			body.AddRange(this.onLeave);
			body.Add(Expression.GreaterThanOrEqual(
					this.StateParameter,
					Expression.Constant(0)));
			return Expression.Block(body);
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

		private Expression InvokeIfMatchingContext<TContext>(Expression<Action<TInput, TContext>> action) {
			if (typeof(TContext) == this.ContextParameter.Type) {
				return this.ReplaceBuildersByIds(action, this.InputParameter, this.ContextParameter).Body;
			}
			var parContext = action.Parameters[1];
			if (typeof(TContext).IsValueType) {
				return Expression.IfThen(
						Expression.TypeIs(this.ContextParameter, typeof(TContext)),
						Expression.Block(new[] {parContext},
								Expression.Assign(parContext,
										Expression.Convert(this.ContextParameter, typeof(TContext))), this.ReplaceBuildersByIds(action, this.InputParameter).Body));
			}
			return Expression.Block(new[] {parContext},
					Expression.IfThen(
							Expression.NotEqual(
									Expression.Assign(
											parContext,
											Expression.TypeAs(this.ContextParameter, typeof(TContext))),
									Expression.Constant(null, typeof(TContext))), this.ReplaceBuildersByIds(action, this.InputParameter).Body));
		}

		/// <summary>Adds an action to execute for a specific context type upon entering the state machine.</summary>
		/// <typeparam name="TData">Type of the data.</typeparam>
		/// <param name="action">The action.</param>
		/// <remarks>A Goto will not trigger a second execution.</remarks>
		public void OnEnter<TData>(Expression<Action<TInput, TData>> action) {
			this.onEnter.Add(this.InvokeIfMatchingContext(action));
		}

		/// <summary>Adds an action to execute for a specific context type upon leaving the state machine (on yield).</summary>
		/// <typeparam name="TData">Type of the data.</typeparam>
		/// <param name="action">The action.</param>
		public void OnLeave<TData>(Expression<Action<TInput, TData>> action) {
			this.onLeave.Add(this.InvokeIfMatchingContext(action));
		}

		/// <summary>Replace builders by identifier.</summary>
		/// <remarks>Replaces <c>(int)someBuilder</c> with the effective ID of the builder.</remarks>
		/// <typeparam name="T">Type of the delegate to use for the expression lambda.</typeparam>
		/// <param name="lambda">The expression.</param>
		/// <param name="parameterReplacements">A variable-length parameters list containing parameter replacements.</param>
		/// <returns>An Expression&lt;T&gt;</returns>
		public Expression<T> ReplaceBuildersByIds<T>(Expression<T> lambda, params Expression[] parameterReplacements) {
			var replacer = new StateReferenceReplacer<TComparand, TInput>(this);
			for (var i = 0; i < parameterReplacements.Length; i++) {
				var replacement = parameterReplacements[i];
				if (replacement != null) {
					replacer.SubstituteParameter(lambda.Parameters[i], replacement);
				}
			}
			return Expression.Lambda<T>(replacer.Visit(lambda.Body), lambda.Parameters);
		}
	}

	/// <summary>A state machine emitter.</summary>
	/// <typeparam name="TComparand">Type of the comparand.</typeparam>
	/// <typeparam name="TInput">Type of the input.</typeparam>
	/// <typeparam name="TContext">Type of the read-only context.</typeparam>
	public class StateMachineEmitter<TComparand, TInput, TContext>: StateMachineEmitter<TComparand, TInput>
			where TComparand: IEquatable<TComparand>
			where TContext: class {
		/// <summary>Constructor.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="conditionEmitter">The condition emitter.</param>
		public StateMachineEmitter([NotNull] IConditionEmitter<TComparand, TInput> conditionEmitter): this(new StateSwitchBuilder<TComparand, TInput, TContext>(), conditionEmitter) { }

		/// <summary>Constructor.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="root">The root.</param>
		/// <param name="conditionEmitter">The condition emitter.</param>
		public StateMachineEmitter([NotNull] StateSwitchBuilder<TComparand, TInput, TContext> root, [NotNull] IConditionEmitter<TComparand, TInput> conditionEmitter): base(root, conditionEmitter, typeof(TContext)) { }

		/// <summary>Gets the root.</summary>
		/// <value>The root.</value>
		public new StateSwitchBuilder<TComparand, TInput, TContext> Root => (StateSwitchBuilder<TComparand, TInput, TContext>)base.Root;

		/// <summary>Emit the state machine function.</summary>
		/// <returns>An Expression&lt;StateMachineFunc&lt;TInput&gt;&gt;</returns>
		public new Expression<StateMachineFunc<TInput, TContext>> Emit() {
			return Expression.Lambda<StateMachineFunc<TInput, TContext>>(this.EmitBody(),
					this.InputParameter,
					this.StateParameter,
					this.ContextParameter);
		}
	}
}
