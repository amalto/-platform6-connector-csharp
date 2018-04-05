using System;
using System.Collections.Generic;
using Library.Models;

namespace Service {
	public class Application {
		private const string MyServiceId = "demo.csharp";

		private static void Main() {
			var parameters = new DeployParameters {
				Id = MyServiceId,
				Username = "admin@amalto.com",
				Path = "/" + MyServiceId + "/api/",
				BasePath = "http://docker.for.mac.localhost:8000",
				Versions = new Versions {
					Client = "0.0.0",
					Server = "0.0.1"
				},
				Ui = new UserInterfaceProperties {
					visible = true,
					iconName = "fa-hashtag",
					weight = 30,
					label = new Dictionary<string, string> {
						{"en-US", "C Sharp"},
						{"fr-FR", "C Sharp"}
					}
				}
			};

			var service = new Library.Service(parameters);
			service.Deployed.ContinueWith(Console.WriteLine);
		}
	}
}