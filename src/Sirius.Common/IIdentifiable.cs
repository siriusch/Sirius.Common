namespace Sirius {
	public interface IIdentifiable<T>
			where T: class {
		Id<T> Id {
			get;
		}
	}
}
