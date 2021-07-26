# ![TeleScope](images/telescope-logo.svg)

##### Table of Content

* [Introduction](#introduction)
* [Usage](#usage)
* [Development](#development)

## Introduction

#### Welcome

* **TeleScope** is a framework written in C# that provides loosely coupled modules for several cross-cutting concerns.
* The assemblies provide connections to external services, access to the persistence layer and helper for logging or user interactions.
* The goal of the project is to create reusable NuGet packages that are based strongly on Clean Architecture Principles.


#### Status

| GitHub Actions and Status |
| ------------------------- |
| ![Build, Test and Scan](https://github.com/telescope-dotnet/telescope/workflows/Build,%20Test%20and%20Scan/badge.svg)
| ![Publish to NuGet](https://github.com/telescope-dotnet/telescope/workflows/Publish%20to%20NuGet/badge.svg)
| ![Publish Docu](https://github.com/telescope-dotnet/telescope/workflows/Publish%20Docu/badge.svg)
| [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=telescope-dotnet_telescope&metric=alert_status)](https://sonarcloud.io/dashboard?id=telescope-dotnet_telescope)


## Usage

#### Documentation

* The public [Repository](https://github.com/telescope-dotnet/telescope).
* The official [API Documentation and Reference manual](https://telescope-dotnet.github.io/telescope/).


#### NuGet Packages

The TeleScope repository contains one solution with lots of **[NuGet packages](https://www.nuget.org/profiles/telescope-dotnet)**.
These packages may be used within your domain specific application in different layers depending on your architectural approach. 

![TeleScope](images/telescope_ca.svg)

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
| Smtp         | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Smtp.Abstractions.svg?label=Smtp.Abstractions)](https://www.nuget.org/packages/TeleScope.Connectors.Smtp.Abstractions/) [![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Smtp.svg?label=Smtp)](https://www.nuget.org/packages/TeleScope.Connectors.Smtp/)


| [TeleScope.Persistence.*](https://www.nuget.org/packages?q=TeleScope.Persistence) | Packages |
| ------------ | --- |
| Abstractions | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Abstractions.svg?label=Persistence.Abstractions)](https://www.nuget.org/packages/TeleScope.Persistence.Abstractions/)
| Json         | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Json.svg?label=Json)](https://www.nuget.org/packages/TeleScope.Persistence.Json/) 
| Yaml         | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Yaml.svg?label=Yaml)](https://www.nuget.org/packages/TeleScope.Persistence.Yaml/) 
| Csv          | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Csv.svg?label=Csv)](https://www.nuget.org/packages/TeleScope.Persistence.Csv/)
| Parquet      | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Parquet.svg?label=Parquet)](https://www.nuget.org/packages/TeleScope.Persistence.Parquet/)

| [TeleScope.UI.*](https://www.nuget.org/packages?q=TeleScope.UI) | Packages |
| ------------ | --- |
| Cli          | [![Nuget](https://img.shields.io/nuget/v/TeleScope.UI.Cli.svg?label=Cli)](https://www.nuget.org/packages/TeleScope.UI.Cli/)
| Permissions  | [![Nuget](https://img.shields.io/nuget/v/TeleScope.UI.Permissions.svg?label=Permissions)](https://www.nuget.org/packages/TeleScope.UI.Permissions/)

| [TeleScope.Logging.*](https://www.nuget.org/packages?q=TeleScope.Logging) | Packages |
| ------------ | --- |
| Logging      | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Logging.svg?label=Logging)](https://www.nuget.org/packages/TeleScope.Logging/)
| Serilog      | [![Nuget](https://img.shields.io/nuget/v/TeleScope.Logging.Extensions.Serilog.svg?label=Extensions.Serilog)](https://www.nuget.org/packages/TeleScope.Logging.Extensions.Serilog/)


## Development

`will come soon`