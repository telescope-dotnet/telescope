# TeleScope **DOCUMENTATION**

> TeleScope is a framework written in C# that provides loosely coupled modules for several cross-cutting concerns.

> The assemblies provide connections to external services, access to the persistence layer and helper for logging or user interactions.

> The goal of the project is to create reusable NuGet packages that are based strongly on Clean Architecture Principles.

## Structure of this page
 
| [Articles](articles)   | [API Documentation](api)   |
| :--------------------: | :------------------------: | 
| Provides explanations of major implementations and examples...<br>**in the near in the future**.  | Provides descriptions about the entire public API like classes, their methods, events, members and so on.

## Quick Start

The TeleScope project provides lots of **[NuGet packages](https://www.nuget.org/profiles/telescope-dotnet)**.
These packages may be used within your domain specific application in different layers depending on your architectural approach. 

### Packages

#### [Connectors](#tab/connectors)

| [TeleScope.Connectors.*](https://www.nuget.org/packages?q=TeleScope.Connectors) | Packages |
| ------------ | --- |
| Abstractions | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Abstractions.svg?label=Connectors.Abstractions)](https://www.nuget.org/packages/TeleScope.Connectors.Abstractions/)
| Mqtt         | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Mqtt.Abstractions.svg?label=Mqtt.Abstractions)](https://www.nuget.org/packages/TeleScope.Connectors.Mqtt.Abstractions/) [![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Mqtt.svg?label=Mqtt)](https://www.nuget.org/packages/TeleScope.Connectors.Mqtt/)
| Http         | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Http.Abstractions.svg?label=Http.Abstractions)](https://www.nuget.org/packages/TeleScope.Connectors.Http.Abstractions/) [![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Http.svg?label=Http)](https://www.nuget.org/packages/TeleScope.Connectors.Http/)
| Plc          | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Plc.Abstractions.svg?label=Plc.Abstractions)](https://www.nuget.org/packages/TeleScope.Connectors.Plc.Abstractions/) [![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Plc.Siemens.svg?label=Plc.Siemens)](https://www.nuget.org/packages/TeleScope.Connectors.Plc.Siemens/)

#### [Persistence](#tab/persistence)

| [TeleScope.Persistence.*](https://www.nuget.org/packages?q=TeleScope.Persistence) | Packages |
| ------------ | --- |
| Abstractions | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Abstractions.svg?label=Persistence.Abstractions)](https://www.nuget.org/packages/TeleScope.Persistence.Abstractions/)
| Json         | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Json.svg?label=Json)](https://www.nuget.org/packages/TeleScope.Persistence.Json/) 
| Yaml         | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Yaml.svg?label=Yaml)](https://www.nuget.org/packages/TeleScope.Persistence.Yaml/) 
| Csv          | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Csv.svg?label=Csv)](https://www.nuget.org/packages/TeleScope.Persistence.Csv/)
| Parquet      | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Parquet.svg?label=Parquet)](https://www.nuget.org/packages/TeleScope.Persistence.Parquet/)

#### [Logging](#tab/loging)

| [TeleScope.Logging.*](https://www.nuget.org/packages?q=TeleScope.Logging) | Packages |
| ------------ | --- |
| Logging      | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Logging.svg?label=Logging)](https://www.nuget.org/packages/TeleScope.Logging/)
| Serilog      | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Logging.Extensions.Serilog.svg?label=Extensions.Serilog)](https://www.nuget.org/packages/TeleScope.Logging.Extensions.Serilog/)

#### [User Interfaces](#tab/ui)

| [TeleScope.UI.*](https://www.nuget.org/packages?q=TeleScope.UI) | Packages |
| ------------ | --- |
| Cli          | [![Nuget](https://img.shields.io/nuget/v/TeleScope.UI.Cli.svg?label=Cli)](https://www.nuget.org/packages/TeleScope.UI.Cli/)

---

### Clean Architecture Principle

The intention of this project is to support applications that follow any architectural approach.
One best practice is the *Clean Architecture Principle*.
There might be differences in the naming of your architectural layers, but the theory remains the same.

> Keeping your business logic clean, which means free from breaking changes through external dependencies or implementations.

![TeleScope Principle](images/telescope_ca.svg)

Of course the TeleScope packages are external dependencies for other projects. Nevertheless,
TeleScope separates its entities from their implementations so that other projects may inject the code where it is needed.
Additionally, you can take advantage of the example implementations or you are also free to create your own implementation based 
on the interfaces TeleScope provides.
