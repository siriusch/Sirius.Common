using System;

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
		[InlineData(true, "test")]
		[InlineData(true, "   test")]
		[InlineData(false, "testx")]
		[InlineData(false, "x")]
		public void TestRangeSet(bool accept, string input) {
			var root = new StateSwitchBuilder<RangeSet<char>, char, ITestOutputHelper>();
			var emitter = new StateMachineEmitter<RangeSet<char>, char>(root, RangesConditionEmitter<RangeSet<char>, char>.Default);
			root.OnSequence("test").Do(o => o.WriteLine("Done")).Yield(-2);
			root.On(new RangeSet<char>(" \t\r\n")).Yield(data => root);
			var stateExpr = emitter.Emit();
			var stateFn = stateExpr.Compile();
			var state = 0;
			object context = this.output;
			foreach (var ch in input) {
				this.output.WriteLine(stateFn(ch, ref state, ref context).ToString());
			}
			Assert.Equal(accept, state == -2);
		}
	}
}
