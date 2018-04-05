namespace csharp.Models {
	public interface IVersions {
		/** Semver version of the service server. */
		string Server { get; }
		/** Semver version of the service client. */
		string Client { get; }
	}
}