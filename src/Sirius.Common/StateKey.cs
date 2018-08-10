using System;

namespace Sirius {
	public struct StateKey<T>: IEquatable<StateKey<T>> where T: IEquatable<T> {
		public StateKey(int state, T value) {
			this.State = state;
			this.Value = value;
		}

		public int State {
			get;
		}

		public T Value {
			get;
		}

		public bool Equals(StateKey<T> other) {
			return (this.State == other.State) && this.Value.Equals(other.Value);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) {
				return false;
			}
			return obj is StateKey<T> && Equals((StateKey<T>)obj);
		}

		public override int GetHashCode() {
			unchecked {
				return (this.State * 397)^this.Value.GetHashCode();
			}
		}

		public override string ToString() {
			return $"{this.State}:{this.Value}";
		}
	}
}