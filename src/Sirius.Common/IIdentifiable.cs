namespace Sirius {
	/// <summary>Interface for identifiable objects.</summary>
	/// <typeparam name="T">Generic type parameter.</typeparam>
	public interface IIdentifiable<T>
			where T: class {
		/// <summary>Gets the typed identifier of the object.</summary>
		/// <value>The <see cref="Id{T}"/>.</value>
		Id<T> Id {
			get;
		}
	}
}
