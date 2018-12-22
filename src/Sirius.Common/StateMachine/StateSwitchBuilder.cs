using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sirius.StateMachine {
	/// <summary>A state switch builder.</summary>
	/// <remarks>This abstract base class is only used as non-generic common base.</remarks>
	/// <typeparam name="TComparand">Type of the comparand.</typeparam>
	/// <typeparam name="TInput">Type of the input.</typeparam>
	public abstract class StateSwitchBuilder<TComparand, TInput>
			where TComparand: IEquatable<TComparand> {
		/// <summary>Emits the state machine fragment as <see cref="Expression" />.</summary>
		/// <param name="emitter">The emitter.</param>
		/// <returns>An Expression.</returns>
		internal abstract Expression Emit(StateMachineEmitter<TComparand, TInput> emitter);
	}

	/// <summary>A state switch builder.</summary>
	/// <typeparam name="TComparand">Type of the comparand.</typeparam>
	/// <typeparam name="TInput">Type of the input.</typeparam>
	/// <typeparam name="TData">Type of the data.</typeparam>
	public sealed class StateSwitchBuilder<TComparand, TInput, TData>: StateSwitchBuilder<TComparand, TInput>
			where TComparand: IEquatable<TComparand> {
		private readonly StatePerformBuilder<TComparand, TInput, TData> @default = new StatePerformBuilder<TComparand, TInput, TData>();
		private readonly List<KeyValuePair<TComparand, StatePerformBuilder<TComparand, TInput, TData>>> onMatch = new List<KeyValuePair<TComparand, StatePerformBuilder<TComparand, TInput, TData>>>();

		/// <summary>The default perform chain (if none of the inputs matched).</summary>
		/// <value>The default builder.</value>
		public StatePerformBuilder<TComparand, TInput, TData> Default {
			get => this.@default;
		}

		internal override Expression Emit(StateMachineEmitter<TComparand, TInput> emitter) {
			var varContext = Expression.Variable(typeof(TData), "typedContext");
			var result = this.@default.Emit(emitter, varContext);
			foreach (var pair in this.onMatch) {
				result = Expression.Condition(
						emitter.ConditionEmitter.Emit(pair.Key, emitter.InputParameter),
						pair.Value.Emit(emitter, varContext),
						result);
			}
			return Expression.Block(new[] {varContext},
					Expression.Assign(
							varContext,
							Expression.Convert(emitter.ContextParameter, typeof(TData))),
					result);
		}

		/// <summary>The perform chain when the input falls within the given range set.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="input">The input range set.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public StatePerformBuilder<TComparand, TInput, TData> On(TComparand input) {
			foreach (var pair in this.onMatch) {
				if (pair.Key.Equals(input)) {
					return pair.Value;
				}
			}
			var result = new StatePerformBuilder<TComparand, TInput, TData>();
			this.On(input, result);
			return result;
		}

		/// <summary>The perform chain when the input falls within the given range set.</summary>
		/// <param name="input">The input range set.</param>
		/// <param name="perform">The perform.</param>
		public void On(TComparand input, StatePerformBuilder<TComparand, TInput, TData> perform) {
			this.onMatch.Add(new KeyValuePair<TComparand, StatePerformBuilder<TComparand, TInput, TData>>(input, perform));
		}
	}
}
