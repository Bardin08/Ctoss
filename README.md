## Introduction

CTOSS (Customizable Table Object Sorting and Searching) is an advanced filtering library built for seamless integration with AgGrid but can also be used as a standalone tool. It is designed to consume JSON or POCO models and convert them to filtering, sorting, or pagination expressions for numbers, strings, and dates. CTOSS works with all major datatypes from the CTS (Common Type System).

<table width="100%" border="0">
<tr>
  <td valign="top">
    <img src="https://github.com/Bardin08/db-seeder/assets/67170413/7d90d3f8-c9cc-4747-a37b-2dddf49f1778" width="150px"/>
  </td>
</tr>
</table>

## Key Features

- **Seamless Integration with AgGrid**: Effortlessly add advanced filtering for server-side AgGrid data sources and the EF Core-controlled databases.
- **Standalone Usage**: This can be used independently of AgGrid for custom filtering needs for in-memory collections or databases via Entity Framework.
- **Multiple Data Types**: Supports all major data types (numbers, dates, and strings) from CTS.

## Installation

To install the CTOSS library, use the following NuGet command:

```bash
dotnet add package Ctoss
```

## Getting Started

For simple scenarios, users can start without any specific configuration when the JSON or POCO model property has the same name (case doesn't matter) as in the Entity or DTO. In this case, CTOSS can process the filtering, sorting, or pagination behind the scenes.

### Configuration

CTOSS is highly configurable. Below is an example of configuring the CTOSS settings for different entities.


```csharp
using Ctoss.Configuration;
using Ctoss.Example;
using Ctoss.Extensions;
using Ctoss.Models;
using Ctoss.Models.Enums;

CtossSettingsBuilder.Create()
    .Entity<ExampleEntity>()
        .Property("Property", x => x.Property + x.Property2, p => { p.IgnoreCase = true; })
        .Apply()
    .Entity<ExampleNumericEntity>()
        .Property("virtual", x => x.A + x.B)
        .Apply()
    .Entity<ExampleTextEntity>()
        .Property(x => x.TextField, settings => { settings.IgnoreCase = true;})
        .Apply();
```

NOTE: in the first scenario, `Property` overrides the default property mapping, and on the filter, instead of a plain `Property` value, a result of the given expression will be used. 

### Usage

CTOSS provides three extension methods and a bunch of overloads for them. 
`WithFilter` evaluates a given filter and provides a filtering expression.
`WithPagination`, which applies pagination to the result set of entities.  
`WithSorting` applies sorting to the given set of entities. It can be a chain of more than one sorting.
All methods are fully compatible with `IEnumerable`, `IQueryable`, and Entity Framework (EF).

## Contributions

Contributions are welcome! If you encounter problems or have suggestions for improvements, feel free to submit pull requests or open issues.

## License

This project is licensed under the MIT License - see the [LICENSE](./LICENSE) file for details.

---

*Made with ❤️ by Vladyslav Bardin*
