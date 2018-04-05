namespace csharp.Models {
	public interface IDeployParameters {
		/** Email address of the caller. */
		string Username { get; }
		/** Service's identifier. */
		string Id { get; }
		/** Path of the endpoint's URL to get the service's client script. */
		string Path { get; }
		/** Base path of the endpoint's URL to get the service's client script.  */
		string BasePath { get; }
		/** Semver versions of all the service's components. */
		IVersions Versions { get; }
		/** Properties of the service's entry menu. */
		UserInterfaceProperties Ui { get; }
	}
}