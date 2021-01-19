# ![TeleScope](images/telescope-logo.svg)

## Introduction

#### Welcome

The goal of the **TeleScope** project is to create reusable nuget packages that are based strongly on clean architecture principles.

#### Status

| GitHub Actions and Status |
| ------------------------- |
| ![Build and Test](https://github.com/telescope-dotnet/telescope/workflows/Build%20and%20Test/badge.svg)
| ![Publish to NuGet](https://github.com/telescope-dotnet/telescope/workflows/Publish%20to%20NuGet/badge.svg)
| ![Publish Docu](https://github.com/telescope-dotnet/telescope/workflows/Publish%20Docu/badge.svg)

#### TOC

* [Introduction](#introduction)
* [Usage](#usage)
* [Development](#development)

## Usage

The TeleScope repository contains one solution with lots of [NuGet](https://www.nuget.org/profiles/telescope-dotnet) packages.
These packages may be used within your domain specific application in different layers depending on your architectural approach. 

#### Clean Architecture

The intention of this project is to support applications that follow the clean architecture principles. There might be differences in naming the architectural layers, but the theory remains the same.
> Keeping your business logic clean, which means free from breaking changes through external dependencies.

Of course the TeleScope packages are external dependencies to other projects. Nevertheless, TeleScope provides reusable and extensible entities and their implementations within infrastructure and presentation layers, like shown below.

![TeleScope](images/telescope-ca.svg)

#### NuGet Packages

###### Versioning

The NuGet versions follow the [Semantic Versioning](https://semver.org/) pattern.
If a version is in state `0.x.x`, this indicates that the package is not used in productive environments so far and
that feature updates also may cause breaking changes. 

<!-- Connectors -->
<table>
<thead>
<tr align="center"><th colspan="2">

[TeleScope.Connectors.*](https://www.nuget.org/packages?q=TeleScope.Connectors)

</th></tr>
</thead>
<tbody>
<tr align="center">
<td colspan="2">

[![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Abstractions.svg?label=Abstractions)](https://www.nuget.org/packages/TeleScope.Connectors.Abstractions/)

</td>
</tr>
<tr align="center">
<td>

[![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Mqtt.Abstractions.svg?label=Mqtt.Abstractions)](https://www.nuget.org/packages/TeleScope.Connectors.Mqtt.Abstractions/)


</td>
<td>

[![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Plc.Abstractions.svg?label=Plc.Abstractions)](https://www.nuget.org/packages/TeleScope.Connectors.Plc.Abstractions/)

</td>
</tr>
<tr align="center">
<td>

[![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Mqtt.svg?label=Mqtt)](https://www.nuget.org/packages/TeleScope.Connectors.Mqtt/)

</td>
<td>

[![Nuget](https://img.shields.io/nuget/v/TeleScope.Connectors.Plc.Siemens.svg?label=Plc.Siemens)](https://www.nuget.org/packages/TeleScope.Connectors.Plc.Siemens/)

</td>
</tr>
</tbody>
</table>

<!-- Persistence -->
<table>
<thead>
<tr align="center"><th colspan="3">

[TeleScope.Persistence.*](https://www.nuget.org/packages?q=TeleScope.Persistence)

</th></tr>
</thead>
<tbody>
<tr align="center">
<td colspan="3">

[![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Abstractions.svg?label=Abstractions)](https://www.nuget.org/packages/TeleScope.Persistence.Abstractions/)

</td>
</tr>

<tr align="center">
<td>

[![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Json.svg?label=Json)](https://www.nuget.org/packages/TeleScope.Persistence.Json/)

</td>
<td>

[![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Csv.svg?label=Csv)](https://www.nuget.org/packages/TeleScope.Persistence.Csv/)

</td>
<td>

[![Nuget](https://img.shields.io/nuget/v/TeleScope.Persistence.Parquet.svg?label=Parquet)](https://www.nuget.org/packages/TeleScope.Persistence.Parquet/)

</td>
</tr>
</tbody>
</table>

<!-- UI -->
<table>
<thead>
<tr align="center"><th colspan="2">

[TeleScope.UI.*](https://www.nuget.org/packages?q=TeleScope.UI)

</th></tr>
</thead>
<tbody>
<tr align="center">
<td>

[![Nuget](https://img.shields.io/nuget/v/TeleScope.UI.Cli.svg?label=Cli)](https://www.nuget.org/packages/TeleScope.UI.Cli/)

</td>
</tr>
</tbody>
</table>

<!-- Logging -->
<table>
<thead>
<tr align="center"><th colspan="2">

[TeleScope.Logging.*](https://www.nuget.org/packages?q=TeleScope.Logging)

</th></tr>
</thead>
<tbody>
<tr align="center">
<td>

[![Nuget](https://img.shields.io/nuget/v/TeleScope.Logging.svg?label=Logging)](https://www.nuget.org/packages/TeleScope.Logging/)

</td>
</tr>

<tr align="center">
<td>

[![Nuget](https://img.shields.io/nuget/v/TeleScope.Logging.Extensions.Serilog.svg?label=Logging.Extensions.Serilog)](https://www.nuget.org/packages/TeleScope.Logging.Extensions.Serilog/)

</td>
</tr>
</tbody>
</table>


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
