# API

## Create a new service

```c#
new Service(DeployParameters parameters)
```

__Argument__

```c#
public class DeployParameters {
    /** Service's identifier. */
    public string Id;
    /** Path of the endpoint's URL to get the service's client script. */
    public string Path;
    /** Base path of the endpoint's URL to get the service's client script.  */
    public string BasePath;
    /** Semver versions of all the service's components. */
    public Versions Versions;
    /** Properties of the service's entry menu. */
    public UserInterfaceProperties Ui;
}

public class Versions {
    /** Semver version of the service server. */
    public string Server;
    /** Semver version of the service client. */
    public string Client;
}

public class UserInterfaceProperties {
    /** Visibility of the entry menu. */
    public bool visible;
    /** Icon's name of the entry menu. */
    public string iconName;
    /** Position of the entry in the menu. */
    public int weight;
    /** Multi-language label for the entry menu (language: 'en-US', 'fr-FR'). */
    public Dictionary< string, string > label;
}
```

__Example__

```c#
using System;
using System.Collections.Generic;
using Library.Models;

namespace TestService {
	internal class Program {
		private const string MyServiceId = "demo.csharp";
        private static Library.Service _service;
		
        public static void Main() {
            _service = DeployService();
            _service.Deployed.ContinueWith(Console.WriteLine);
            
            Console.WriteLine("The service " + MyServiceId + " has been deployed.");
        }

        private static Library.Service DeployService() {
			var parameters = new DeployParameters {
				Id = MyServiceId,
				Path = Constants.Path,
				BasePath = Environment.GetEnvironmentVariable("EXTERNAL_URL"),
				Versions = new Versions {
					Client = "0.0.0",
					Server = Constants.ServerVersion
				},
				Ui = new UserInterfaceProperties {
					visible = true,
					iconName = "fas fa-hashtag",
					weight = 30,
					label = new Dictionary<string, string> {
						{"en-US", "C Sharp"},
						{"fr-FR", "C Sharp"}
					}
				}
			};

			return new Library.Service(parameters);
		}
    }
}
```

## Undeploy a service

```c#
Task UndeployService()
```

__Example__

```c#
using System;
using System.Collections.Generic;
using Library.Models;

namespace TestService {
	internal class Program {
		private const string MyServiceId = "demo.csharp";
        private static Library.Service _service;
        
        // {...}
		
        private static void UndeployService() {
            await _service.UndeployService();
            
            Console.WriteLine("The service has been undeployed.");
        }
    }
}
```
