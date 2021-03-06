using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

using JetBrains.Annotations;

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

		private readonly List<MatchCase<TComparand, TInput, TContext>> onMatch = new List<MatchCase<TComparand, TInput, TContext>>();

		/// <summary>The default perform chain (if none of the inputs matched).</summary>
		/// <value>The default builder.</value>
		public StatePerformBuilder<TComparand, TInput, TContext> Default {
			get;
		} = new StatePerformBuilder<TComparand, TInput, TContext>();

		internal override Expression Emit(StateMachineEmitter<TComparand, TInput> emitter) {
			var usesContextVariable = (emitter.ContextParameter.Type != typeof(TContext));
			var varContext = usesContextVariable
					? Expression.Variable(typeof(TContext), "typedContext")
					: emitter.ContextParameter;
			var cases = this.onMatch
					.ToList();
			var conditions = this.onMatch
					.Select(c => c.EmitCondition(emitter, varContext))
					.ToList();
			var mergeLimit = this.onMatch
					.Select(c => 0)
					.ToList();
			Debug.Assert(cases.Count == this.onMatch.Count && conditions.Count == this.onMatch.Count && mergeLimit.Count == this.onMatch.Count);
			var checkDefault = true;
			var result = this.Default.Emit(emitter, varContext);
			for (var i = cases.Count - 1; i >= 0; i--) {
				var matchCase = cases[i];
				var hasMatchComparand = matchCase.TryGetComparand(out var matchComparand);
				if (checkDefault && matchCase.Builder.Perform.Equals(this.Default.Perform)) {
					var useDefault = true;
					if (hasMatchComparand) {
						for (var j = i + 1; j < cases.Count; j++) {
							var checkCase = cases[j];
							if (checkCase.TryGetComparand(out var checkComparand) && !emitter.ConditionEmitter.IsDisjoint(matchComparand, checkComparand)) {
								useDefault = false;
								break;
							}
						}
					}
					if (useDefault) {
						cases.RemoveAt(i);
						conditions.RemoveAt(i);
						mergeLimit.RemoveAt(i);
						continue;
					}
				}
				if (hasMatchComparand) {
					var mergedWith = -1;
					var limit = mergeLimit[i];
					var j = i;
					while (--j > limit) {
						var checkCase = cases[j];
						if (matchCase.Builder.Perform.Equals(checkCase.Builder.Perform)) {
							// The two cases perform the same, try to optimize by merging with previous case
							if (!(checkCase.TryGetComparand(out var checkComparand) && emitter.ConditionEmitter.IsDisjoint(matchComparand, checkComparand))) {
								// Custom conditions (which may use input) or overlapping comparands are not safe to merge
								break;
							}
							if (mergedWith < 0) {
								// Not yet merged, do it
								mergedWith = j;
								conditions[j] = Expression.OrElse(
										conditions[j],
										conditions[i]);
								cases.RemoveAt(i);
								conditions.RemoveAt(i);
								mergeLimit.RemoveAt(i);
							}
						}
					}
					if (mergedWith >= 0) {
						// Keep track how far the new condition is mergeable
						mergeLimit[mergedWith] = j;
						continue;
					}
				} else {
					// Custom case disallows default usage
					checkDefault = false;
				}
				result = Expression.Condition(
						conditions[i],
						this.onMatch[i].Builder.Emit(emitter, varContext),
						result);
			}
			if (usesContextVariable) {
				var usageFinder = new ParameterUsageFinder();
				usageFinder.Visit(result);
				usesContextVariable = usageFinder.IsUsed(varContext);
			}
			return usesContextVariable
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
			return this.On(input, default(Expression<Predicate<TContext>>));
		}

		/// <summary>The perform chain when the input falls within the given range set.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="input">The input range set.</param>
		/// <param name="condition">The condition.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public StatePerformBuilder<TComparand, TInput, TContext> On(TComparand input, Expression<Predicate<TContext>> condition) {
			foreach (var matchCase in this.onMatch.OfType<MatchComparandCase<TComparand, TInput, TContext>>()) {
				if (matchCase.Comparand.Equals(input) && (matchCase.Condition == condition)) {
					return matchCase.Builder;
				}
			}
			var result = new StatePerformBuilder<TComparand, TInput, TContext>();
			this.On(input, condition, result);
			return result;
		}

		/// <summary>The perform chain when the input falls within the given range set.</summary>
		/// <param name="input">The input range set.</param>
		/// <param name="perform">The perform.</param>
		public void On(TComparand input, StatePerformBuilder<TComparand, TInput, TContext> perform) {
			this.On(input, null, perform);
		}

		/// <summary>The perform chain when the input falls within the given range set.</summary>
		/// <param name="input">The input range set.</param>
		/// <param name="condition">The condition.</param>
		/// <param name="perform">The perform.</param>
		public void On(TComparand input, Expression<Predicate<TContext>> condition, [NotNull] StatePerformBuilder<TComparand, TInput, TContext> perform) {
			if (perform == null) {
				throw new ArgumentNullException(nameof(perform));
			}
			this.onMatch.Add(new MatchComparandCase<TComparand, TInput, TContext>(input, condition, perform));
		}

		/// <summary>The perform chain when the condition evaluates to true.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="condition">The condition.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public StatePerformBuilder<TComparand, TInput, TContext> On(Expression<Func<TInput, TContext, bool>> condition) {
			var result = new StatePerformBuilder<TComparand, TInput, TContext>();
			this.On(condition, result);
			return result;
		}

		/// <summary>The perform chain when the condition evaluates to true.</summary>
		/// <param name="condition">The condition.</param>
		/// <param name="perform">The perform.</param>
		public void On([NotNull] Expression<Func<TInput, TContext, bool>> condition, [NotNull] StatePerformBuilder<TComparand, TInput, TContext> perform) {
			if (condition == null) {
				throw new ArgumentNullException(nameof(condition));
			}
			if (perform == null) {
				throw new ArgumentNullException(nameof(perform));
			}
			this.onMatch.Add(new MatchCustomCase<TComparand, TInput, TContext>(condition, perform));
		}
	}
}
