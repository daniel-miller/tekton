# Tekton

Tekton is a library of general-purpose components that implement functionality for common, cross-cutting concerns. Its primary purpose is reuse in other applications and platforms. The name **Tekton** (abbreviated **Tek**) comes from the Greek word meaning "toolmaker" or "craftsman". 

Tekton has these parts:

* 3 low-level class libraries
* 1 high-level class library
* 1 terminal (console) application
* 1 application programming interface (API)
* 1 web user interface (React client)

The dependencies between these parts looks like this:

![image](https://github.com/user-attachments/assets/09d1d214-0457-4240-a771-e54d245d510e)

## Tek.Contract

This low-level class library defines and implements settings, rules, and constraints to help enforce consistency, validate inputs, and/or specify obligations and guarantees between different parts of the code. In effect, the Contract library:

* Defines interfaces.
* Implements classes for configuration settings.
* Implements classes for request/response data types (i.e., input and output classes).
* Targets .NET Standard 2.0 for maximum reusability.
* Contains zero dependencies on third-party libraries.

Ideally, classes in this library are plain old class objects (POCO) and data transfer objects (DTO) only. Some constant literals and enumeration types are also defined here, which improves readability and reuse of the code in this library.

The code here is bare-metal by design, and the intent is to have zero third-party references, and also to minimize the number of references to Microsoft and/or .NET System libraries. Notice, for example, this library contains no dependencies to any specific database platform, database schema, data access library, or serialization library.

The code targets .NET Standard 2.0 to maximize the potential for reuse, so it can be used in any .NET Standard, .NET Core, or .NET Framework project, including multi-platform Console, Web, and Windows applications.

## Tek.Common

This low-level class library implements basic general-purpose functionality where third-party dependencies are not permitted. The Common library:

* Targets .NET Standard 2.0 for maximum reusability.
* Contains zero dependencies on third-party libraries.

## Tek.Toolbox

This low-level class library implements more advanced general-purpose functionality with third-party dependencies. The Toolbox library:

* Targets .NET Standard 2.0 for maximum reusability.
* Contains dependencies on third-party libraries.

Notice the JSON serialization library referenced by this library is [Newtonsoft.JSON](https://www.newtonsoft.com/json). If you might want to use a different library for JSON serialization (e.g. System.Text.Json, for example), then you can replace this reference and revise the dependent code.

## Tek.Service

This high-level class library implements application features required by Tek.API and Tek.UI projects. It is tightly coupled to the [PostgreSQL](https://www.postgresql.org) database platform, and depends directly on the schema of the tekton database hosted in PostgreSQL. The Service library:

* Targets .NET Core 9.0 for maximum power and flexibility.
* Contain dependencies on third-party libraries.

## Tek.Terminal

This console application implements a terminal to access services provided by the Tekton platform.

Use this command to confirm the terminal is operational:

`dotnet run hello`

Use this command to upgrade the database schema to match the current version of the source code:

`dotnet run metadata upgrade-database`

## Tek.API

This .NET (Core) Web API implements access to services provided by the Tekton platform.

Use this HTTP request to confirm the API is operational:

`GET /api/version`

Use this HTTP request to obtain an JSON Web Token (JWT) for authorization to send requests to API methods available in the platform:

`POST /api/token`
