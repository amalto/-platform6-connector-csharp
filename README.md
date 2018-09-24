# Platform 6 C# client

> [Platform 6](https://documentation.amalto.com/platform6/master/) C# client

This repository is a [Platform 6 client](https://documentation.amalto.com/platform6/master/developer-guide/platform6-clients/) aiming to help the development a [service](https://documentation.amalto.com/platform6/master/developer-guide/getting-started/) in C#.
It exposes, among others, methods to facilitate the communication with a Platform 6 instance.

## Requirements

Platform 6 client depends on:
- [Microsoft .Net Framework](https://www.microsoft.com/en-us/download/details.aspx?id=17851) (version >= 4),
- [Microsoft .Net Core](https://docs.microsoft.com/en-us/dotnet/core/) (version >= 2),
- [Platform 6](https://documentation.amalto.com/platform6/master/user-guide/getting-started/) (version `5.17.0` and higher)

## Installing the client

You can install the [Platform 6 .Net client](https://www.nuget.org/packages/Platform6.Client/0.0.1-alpha6) from NuGet repository using PackageManager:

```bash
PM> Install-Package Platform6.Client
```

## Using the client

You can find an example of a service using this library [here](https://github.com/amalto/platform6-service-csharp).

### API

You will find the documentation of the methods exposed in the section [API](https://github.com/amalto/platform6-client-csharp/blob/master/API.md).

## Release notes

Please refer to [changelog](./CHANGELOG.md) to see the descriptions of each release.

## License

MIT © [Platform 6](https://www.platform6.io/)
