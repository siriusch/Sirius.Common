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
	/// <typeparam name="TContext">Type of the context.</typeparam>
	public sealed class StateSwitchBuilder<TComparand, TInput, TContext>: StateSwitchBuilder<TComparand, TInput>
			where TComparand: IEquatable<TComparand> {
		/// <summary>Explicit cast that converts the given StateSwitchBuilder&lt;TComparand,TInput,TData&gt; to an int.</summary>
		/// <exception cref="NotSupportedException">Thrown when the requested operation is not supported.</exception>
		/// <param name="builder">The builder.</param>
		/// <returns>The result of the operation.</returns>
		/// <remarks>This operator is only for usage in lambda expressions, it is not supported at runtime.</remarks>
		public static explicit operator int(StateSwitchBuilder<TComparand, TInput, TContext> builder) {
			throw new NotSupportedException("This conversion is not supported at runtime");
		}

		private readonly List<KeyValuePair<TComparand, StatePerformBuilder<TComparand, TInput, TContext>>> onMatch = new List<KeyValuePair<TComparand, StatePerformBuilder<TComparand, TInput, TContext>>>();

		/// <summary>The default perform chain (if none of the inputs matched).</summary>
		/// <value>The default builder.</value>
		public StatePerformBuilder<TComparand, TInput, TContext> Default {
			get;
		} = new StatePerformBuilder<TComparand, TInput, TContext>();

		internal override Expression Emit(StateMachineEmitter<TComparand, TInput> emitter) {
			var varContext = Expression.Variable(typeof(TContext), "typedContext");
			var result = this.Default.Emit(emitter, varContext);
			foreach (var pair in this.onMatch) {
				result = Expression.Condition(
						emitter.ConditionEmitter.Emit(pair.Key, emitter.InputParameter),
						pair.Value.Emit(emitter, varContext),
						result);
			}
			var usageFinder = new ParameterUsageFinder();
			usageFinder.Visit(result);
			return usageFinder.IsUsed(varContext)
					? Expression.Block(new[] {varContext},
							Expression.Assign(
									varContext,
									Expression.Convert(emitter.ContextParameter, typeof(TContext))),
							result)
					: result;
		}

		/// <summary>The perform chain when the input falls within the given range set.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="input">The input range set.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public StatePerformBuilder<TComparand, TInput, TContext> On(TComparand input) {
			foreach (var pair in this.onMatch) {
				if (pair.Key.Equals(input)) {
					return pair.Value;
				}
			}
			var result = new StatePerformBuilder<TComparand, TInput, TContext>();
			this.On(input, result);
			return result;
		}

		/// <summary>The perform chain when the input falls within the given range set.</summary>
		/// <param name="input">The input range set.</param>
		/// <param name="perform">The perform.</param>
		public void On(TComparand input, StatePerformBuilder<TComparand, TInput, TContext> perform) {
			this.onMatch.Add(new KeyValuePair<TComparand, StatePerformBuilder<TComparand, TInput, TContext>>(input, perform));
		}
	}
}
