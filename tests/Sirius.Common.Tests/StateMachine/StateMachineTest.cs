using System;
using System.Linq.Expressions;

using Sirius.Collections;

using Xunit;
using Xunit.Abstractions;

namespace Sirius.StateMachine {
	public class StateMachineTest {
		private readonly ITestOutputHelper output;

		public StateMachineTest(ITestOutputHelper output) {
			this.output = output;
		}

		[Theory]
		[InlineData(0, "test")]
		[InlineData(1, "   te\nst")]
		[InlineData(3, "\n\n\n")]
		[InlineData(0, "")]
		public void TestCounter(int expectedCount, string input) {
			// Define the state machine
			var root = new StateSwitchBuilder<char, char, int>();
			root.On('\n').Do(i => i + 1).Yield(root);
			root.Default.Yield(root);
			// Compile the state machine
			var emitter = new StateMachineEmitter<char, char>(root, EquatableConditionEmitter<char>.Default);
			emitter.OnEnter<object>((i, d) => this.output.WriteLine("+"));
			emitter.OnLeave<object>((i, d) => this.output.WriteLine("-"));
			var stateExpr = emitter.Emit();
			var stateFn = stateExpr.Compile();
			// Run the state machine for some input
			var state = 0;
			object context = 0;
			foreach (var ch in input) {
				stateFn(ch, ref state, ref context);
			}
			// The context is now an integer with the number of newlines
			this.output.WriteLine(context.ToString());
			Assert.Equal(expectedCount, (int)context);
			Assert.True(state >= 0);
		}

		[Theory]
		[InlineData(true, "test")]
		[InlineData(true, "   test")]
		[InlineData(false, "testx")]
		[InlineData(false, "x")]
		public void TestRangeSet(bool accept, string input) {
			var root = new StateSwitchBuilder<RangeSet<char>, char, ITestOutputHelper>();
			var emitter = new StateMachineEmitter<RangeSet<char>, char>(root, RangesConditionEmitter<RangeSet<char>, char>.Default);
			root.OnSequence("test").Do(o => o.WriteLine("Done")).Yield(-2);
			root.On(new RangeSet<char>(" \t\r\n")).Yield(data => (int)root);
			var stateExpr = emitter.Emit();
			var stateFn = stateExpr.Compile();
			var state = 0;
			object context = this.output;
			foreach (var ch in input) {
				this.output.WriteLine(stateFn(ch, ref state, ref context).ToString());
			}
			Assert.Equal(accept, state == -2);
		}

		[Fact]
		public void ExpressionReplaceTest() {
			var root = new StateSwitchBuilder<bool, bool, ITestOutputHelper>();
			var emitter = new StateMachineEmitter<bool, bool>(root, EquatableConditionEmitter<bool>.Default);
			root.Default.Do(o => o.WriteLine(((int)root).ToString()));
			var stateExpr = emitter.Emit();
			var stateFn = stateExpr.Compile();
			var state = 0;
			object context = this.output;
			Assert.False(stateFn(true, ref state, ref context));
		}
	}
}
