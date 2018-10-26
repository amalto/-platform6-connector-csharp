# Platform 6 C# connector

> [Platform 6](https://documentation.amalto.com/platform6/master/) C# connector

This repository is a [Platform 6 connector](https://documentation.amalto.com/platform6/master/develop-app/custom-service/platform6-cmb-connectors/) aiming to help the development a [service](https://documentation.amalto.com/platform6/master/developer-guide/getting-started/) in C#.
It exposes, among others, methods to facilitate the communication with a Platform 6 instance.

## Requirements

Platform 6 connector depends on:
- [Microsoft .Net Framework](https://www.microsoft.com/en-us/download/details.aspx?id=17851) (version >= 4),
- [Microsoft .Net Core](https://docs.microsoft.com/en-us/dotnet/core/) (version >= 2),
- [Platform 6](https://documentation.amalto.com/platform6/master/user-guide/getting-started/) (version `5.17.0` and higher)

## Installing the connector

You can install the [Platform 6 .Net connector](https://www.nuget.org/packages/Platform6.Client/0.0.1-alpha6) from the NuGet repository using PackageManager:

```bash
PM> Install-Package Platform6.Client
```

## Using the connector

You can find an example of a service using this library [here](https://github.com/amalto/platform6-service-csharp).

### API

You will find the documentation of the methods exposed in the section [API](https://github.com/amalto/platform6-client-csharp/blob/master/Platform6/API.md).

## Build the project

Install [protocol buffer](https://developers.google.com/protocol-buffers/docs/csharptutorial):

> For Mac OS X, use: `brew install protobuf`

Run the compiler.

```
$ cd Platform6/Messages/
$ mkdir ProtocolBuffers && protoc -I=. --csharp_out=ProtocolBuffers common_message_proto_buff.proto
```

It will generate the `CommonMessageProtoBuff.cs` in the `ProtocolBuffers` folder.

Compile the C# classes to create the `.exe` in the folder `bin/Debug/`.

```
$ msbuild Platform6/Library/Library.csproj 
```

## Release notes

Please refer to [changelog](./Platform6/CHANGELOG.md) to see the descriptions of each release.

## License

MIT © [Platform 6](https://www.platform6.io/)
