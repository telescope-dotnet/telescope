# ![TeleScope](images/telescope-logo.svg)

##### Table of Content

* [Introduction](#introduction)
* [Usage](#usage)
* [Development](#development)

## Introduction

#### Welcome

The goal of the **TeleScope** project is to create reusable NuGet packages that are based strongly on clean architecture principles.

#### Status

| GitHub Actions and Status |
| ------------------------- |
| ![Build, Test and Scan](https://github.com/telescope-dotnet/telescope/workflows/Build,%20Test%20and%20Scan/badge.svg)
| ![Publish to NuGet](https://github.com/telescope-dotnet/telescope/workflows/Publish%20to%20NuGet/badge.svg)
| ![Publish Docu](https://github.com/telescope-dotnet/telescope/workflows/Publish%20Docu/badge.svg)
| [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=telescope-dotnet_telescope&metric=alert_status)](https://sonarcloud.io/dashboard?id=telescope-dotnet_telescope)



## Usage

The TeleScope repository contains one solution with lots of [NuGet](https://www.nuget.org/profiles/telescope-dotnet) packages.
These packages may be used within your domain specific application in different layers depending on your architectural approach. 

#### Clean Architecture

The intention of this project is to support applications that follow the clean architecture principles.
There might be differences in naming the architectural layers, but the theory remains the same.
> Keeping your business logic clean, which means free from breaking changes through external dependencies.

Of course the TeleScope packages are external dependencies to other projects. Nevertheless,
TeleScope provides reusable and extensible entities and their implementations within infrastructure and presentation layers,
like shown below.

![TeleScope](images/telescope-ca.svg)

#### NuGet Packages

###### Versioning

The NuGet versions follow the [Semantic Versioning](https://semver.org/) pattern.
If a version is in state `0.x.x`, this indicates that the package is not used in productive environments so far and
that feature updates also may cause breaking changes. 

<!-- Connectors -->

| [TeleScope.Connectors.*](https://www.nuget.org/packages?q=TeleScope.Connectors) | Packages |
| ------------ | --- |
| Abstractions | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Abstractions.svg?label=Connectors.Abstractions)](https://www.nuget.org/packages/TeleScope.Connectors.Abstractions/)
| Mqtt         | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Mqtt.Abstractions.svg?label=Mqtt.Abstractions)](https://www.nuget.org/packages/TeleScope.Connectors.Mqtt.Abstractions/) [![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Mqtt.svg?label=Mqtt)](https://www.nuget.org/packages/TeleScope.Connectors.Mqtt/)
| Http         | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Http.Abstractions.svg?label=Http.Abstractions)](https://www.nuget.org/packages/TeleScope.Connectors.Http.Abstractions/) [![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Http.svg?label=Http)](https://www.nuget.org/packages/TeleScope.Connectors.Http/)
| Plc          | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Plc.Abstractions.svg?label=Plc.Abstractions)](https://www.nuget.org/packages/TeleScope.Connectors.Plc.Abstractions/) [![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Plc.Siemens.svg?label=Plc.Siemens)](https://www.nuget.org/packages/TeleScope.Connectors.Plc.Siemens/)

| [TeleScope.Persistence.*](https://www.nuget.org/packages?q=TeleScope.Persistence) | Packages |
| ------------ | --- |
| Abstractions | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Abstractions.svg?label=Persistence.Abstractions)](https://www.nuget.org/packages/TeleScope.Persistence.Abstractions/)
| Json         | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Json.svg?label=Json)](https://www.nuget.org/packages/TeleScope.Persistence.Json/) 
| Yaml         | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Yaml.svg?label=Yaml)](https://www.nuget.org/packages/TeleScope.Persistence.Yaml/) 
| Csv          | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Csv.svg?label=Csv)](https://www.nuget.org/packages/TeleScope.Persistence.Csv/)
| Parquet      | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Parquet.svg?label=Parquet)](https://www.nuget.org/packages/TeleScope.Persistence.Parquet/)

| [TeleScope.UI.*](https://www.nuget.org/packages?q=TeleScope.Persistence) | Packages |
| ------------ | --- |
| Cli          | [![Nuget](https://img.shields.io/nuget/v/TeleScope.UI.Cli.svg?label=Cli)](https://www.nuget.org/packages/TeleScope.UI.Cli/)

| [TeleScope.Logging.*](https://www.nuget.org/packages?q=TeleScope.Logging) | Packages |
| ------------ | --- |
| Logging      | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Logging.svg?label=Logging)](https://www.nuget.org/packages/TeleScope.Logging/)
| Serilog      | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Logging.Extensions.Serilog.svg?label=Extensions.Serilog)](https://www.nuget.org/packages/TeleScope.Logging.Extensions.Serilog/)

#### Documentation

* The public [Repository](https://github.com/telescope-dotnet/telescope)
* The official [API Documentation and Reference manual](https://telescope-dotnet.github.io/telescope/)

```markdown
* Mention supportes
* Create a list with links, maybe to important resources,
* to wiki pages or staging servers
```

## Development

`will come soon`

~~~markdown
Step by step explanation about how to get the project running, preferably with command line examples that can be copy-pasted by readers

#### Prerequisites
* Information about the recommended IDE
* Runtime environment and frameworks that needs to be preinstalled
* **These are just example requirements. Add, duplicate or remove as required**

#### OS X & Linux
```sh
- get_command
- install_command
- configure_command
- run_command
```

#### Windows
```sh
- get_command
- install_command
- configure_command
- run_command
```
#### Goals or TODOs
- [ ] Your major next steps and
- [ ] planned long-term features

#### Known Issues
- [ ] known bugs or
- [ ] limitations
~~~
