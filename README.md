# Common .NET Libraries

This repository contains class libraries that implement functionality for common, cross-cutting concerns. 

The code here is "bare-metal" by design, so the intent is to have no third-party references (unless absolutely necessary), and also to minimize the number of references to Microsoft and/or .NET System libraries. In addition, you'll notice these libraries contain no dependencies on any specific database platform, database schema, or data access library.

The code targets .NET Standard 2.0 to maximize the potential for reuse, so you should be able to use these libraries in any .NET Standard, .NET Core, or .NET Framework project: Console, Web, Windows, and/or Library.
