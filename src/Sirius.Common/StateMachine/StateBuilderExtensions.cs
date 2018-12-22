using System;
using System.Collections.Generic;
using System.Linq;

using Sirius.Collections;

namespace Sirius.StateMachine {
	/// <summary>State builder extension methods.</summary>
	public static class StateBuilderExtensions {
		/// <summary>When a given input is matched, consume it and proceed.</summary>
		/// <typeparam name="TComparand">Type of the comparand.</typeparam>
		/// <typeparam name="TInput">Type of the input.</typeparam>
		/// <typeparam name="TData">Type of the data.</typeparam>
		/// <param name="that">The builder to act on.</param>
		/// <param name="input">The input.</param>
		/// <returns>A StateSwitchBuilder&lt;TInput,TData&gt;</returns>
		public static StateSwitchBuilder<TComparand, TInput, TData> Take<TComparand, TInput, TData>(this StateSwitchBuilder<TComparand, TInput, TData> that, TComparand input)
				where TComparand: IEquatable<TComparand> {
			return that.On(input).Yield();
		}

		/// <summary>When a given input sequence is matched, consume the input except for the last one.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <typeparam name="TComparand">Type of the comparand.</typeparam>
		/// <typeparam name="TInput">Type of the input.</typeparam>
		/// <typeparam name="TData">Type of the data.</typeparam>
		/// <param name="that">The builder to act on.</param>
		/// <param name="inputs">The inputs.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public static StatePerformBuilder<TComparand, TInput, TData> OnSequence<TComparand, TInput, TData>(this StateSwitchBuilder<TComparand, TInput, TData> that, IEnumerable<TComparand> inputs)
				where TComparand: IEquatable<TComparand> {
			using (var enumerator = inputs.GetEnumerator()) {
				if (!enumerator.MoveNext()) {
					throw new InvalidOperationException("Empty input not allowed");
				}
				var result = that.On(enumerator.Current);
				while (enumerator.MoveNext()) {
					result = result.Yield().On(enumerator.Current);
				}
				return result;
			}
		}

		/// <summary>When a given input sequence is matched, consume the input except for the last one.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <typeparam name="TInput">Type of the input.</typeparam>
		/// <typeparam name="TData">Type of the data.</typeparam>
		/// <param name="that">The builder to act on.</param>
		/// <param name="inputs">The inputs.</param>
		/// <returns>A StatePerformBuilder&lt;TInput,TData&gt;</returns>
		public static StatePerformBuilder<RangeSet<TInput>, TInput, TData> OnSequence<TInput, TData>(this StateSwitchBuilder<RangeSet<TInput>, TInput, TData> that, IEnumerable<TInput> inputs)
				where TInput: IComparable<TInput> {
			return OnSequence(that, inputs.Select(i => new RangeSet<TInput>(i)));
		}
	}
}
