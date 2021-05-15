# TeleScope **DOCUMENTATION**

## Structure of this page

<div style="width:100%;">
<table style="border-style:solid; border-width:1px; border-color: #c3d2d9; width: 100%;" cellspacing="0">
<tr>
<td style="padding: 0px 20px 20px 20px">

### [Articles](articles)

Provides explanations of major implementations and examples - in the future. 

</td>
<td style="padding: 0px 20px 20px 20px">

### [API Reference](api)

Provides descriptions about the entire public API like classes, their methods, events, members and so on.

</td>
</tr>
</table>
</div>

## Quick Start

The TeleScope project provides lots of **[NuGet packages](https://www.nuget.org/profiles/telescope-dotnet)**.
These packages may be used within your domain specific application in different layers depending on your architectural approach. 

##### Clean Architecture Principle

The intention of this project is to support applications that follow any architectural approach.
One best practice is the *Clean Architecture Principle*.
There might be differences in the naming of your architectural layers, but the theory remains the same.

> Keeping your business logic clean, which means free from breaking changes through external dependencies or implementations.

Of course the TeleScope packages are external dependencies for other projects. Nevertheless,
TeleScope separates its entities from their implementations so that other projects may inject the code where it is needed.
Additionally, you can take advantage of the example implementations or you are also free to create your own implementation based 
on the interfaces TeleScope provides like shown below.

![TeleScope Principle](images/telescope_ca.svg)

