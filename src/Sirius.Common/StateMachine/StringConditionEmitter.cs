using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Sirius.StateMachine {
	/// <summary>A string condition emitter.</summary>
	public class StringConditionEmitter: IConditionEmitter<string, string> {
		/// <summary>Current culture string condition emitter.</summary>
		public static readonly StringConditionEmitter CurrentCulture = new StringConditionEmitter(StringComparison.CurrentCulture);

		/// <summary>Current culture (ignoring case) string condition emitter.</summary>
		public static readonly StringConditionEmitter CurrentCultureIgnoreCase = new StringConditionEmitter(StringComparison.CurrentCultureIgnoreCase);

		/// <summary>Invariant string condition emitter.</summary>
		public static readonly StringConditionEmitter InvariantCulture = new StringConditionEmitter(StringComparison.InvariantCulture);

		/// <summary>Invariant (ignoring case) string condition emitter.</summary>
		public static readonly StringConditionEmitter InvariantCultureIgnoreCase = new StringConditionEmitter(StringComparison.InvariantCultureIgnoreCase);

		/// <summary>Ordinal string condition emitter.</summary>
		public static readonly StringConditionEmitter Ordinal = new StringConditionEmitter(StringComparison.Ordinal);

		/// <summary>Ordinal (ignoring case) string condition emitter.</summary>
		public static readonly StringConditionEmitter OrdinalIgnoreCase = new StringConditionEmitter(StringComparison.OrdinalIgnoreCase);

		private static readonly MethodInfo meth_String_Equals = Reflect.GetStaticMethod(() => string.Equals(default, default, default));

		private readonly StringComparison comparison;

		/// <summary>Constructor.</summary>
		/// <param name="comparison">The comparison.</param>
		public StringConditionEmitter(StringComparison comparison) {
			this.comparison = comparison;
		}

		/// <summary>Emits the condition for the string.</summary>
		/// <param name="comparand">The comparand.</param>
		/// <param name="varInput">The variable input.</param>
		/// <returns>An Expression.</returns>
		public Expression Emit(string comparand, ParameterExpression varInput) {
			return Expression.Call(
					meth_String_Equals,
					Expression.Constant(comparand),
					varInput,
					Expression.Constant(this.comparison));
		}
	}
}
