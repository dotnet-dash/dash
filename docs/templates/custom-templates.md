# Custom Templates
You can write Custom Templates if the [Built-in Templates](built-in-templates.md) do not support your code generation needs.

## How it works
Dash parses, validates and processes a [Model File](../model-file/introduction.md) to build an _[Internal Model](#internal-model)_. This Internal Model is passed to the [Template Engine](#template-engine) and then interpreted by the Template to actually generate the (C#) code.

The process visualized:

[ insert image ]

## Internal Model
!!! warning

    To write Custom Templates, you should have a good understanding of the Internal Model.

To explain the Internal Model, let's take a look at a JSON representation of an Internal Model:

~~~ JSON
{
    "Namespace": "Foo",
    "ModelName": "MyModel",
    "Entities": [{
        "Name": "User",
        "CodeAttributes": [{
            "Name": "Id",
            "DataType": "int",
            "IsNullable": false,
            "DefaultValue": null
        }],
        "SingleReferences": [],
        "CollectionReferences": [],
        "SeedData": []
    }]
}
~~~

### Internal Model
- `Namespace` is the namespace of the application.
- `ModelName` is the name of the Model, which is inferred from the filename.
- `Entities` is an array of objects:
    - `Name` contains the value that represents the .NET class name.
    - `CodeAttributes`: is an array of objects:
        - `Name` contains the value that represents the membername of a class.
        - `DataType` is the C# data type name, e.g. `bool`, `int`.
        - `IsNullable` indicates that the member is (not) nullable.
        - `DefaultValue` contains the default value of the member (`null` if there is no default value)
    - `SingleReferences` is an array of objects:
        - `ReferenceName` represents a property name that references another class.
        - `EntityModel` contains the classname that is being referenced.
        - `IsNullable` indicates that the property is (not) nullable.
    - `CollectionReferences` is an array of objects:
        - `ReferenceName` represents a property name that holds a collection of objects
        - `EntityModel` contains the classname that is being referenced.
        - `IsNullable` indicates that the property is (not) nullable.

!!! warning

    The values of all Internal Model properties should be respected by the Template and used as-is.
    Any special logic (e.g. pluralizing nouns) should be left to the Dash processing pipeline.

    If what you want to achieve is not supported, then please visit the [Dash Github Repo](https://github.com/dotnet-dash/dash) to open an issue or see how you can contribute to the codebase.

## Template Engine
Dash uses [Scriban](https://github.com/lunet-io/scriban) as its template engine. The Scriban language is similar to [Liquid](https://shopify.github.io/liquid/). To learn more about the Scriban syntax, please visit the [documentation on the Scriban templating language](https://github.com/lunet-io/scriban/blob/master/doc/language.md).

## Hello World
The following is a simple Custom Template that iterates through all classes inside the Internal Model:
~~~
namespace {{ namespace }}
{
    {{ for e in entities }}
    public class {{ e.name }}
    {
    }
    {{ end }}
}
~~~
 
When the above [Internal Model](#-internal-model) is used as the input, the generated code will be:
~~~ csharp
namespace Foo
{
    public class User
    {
    }
}

~~~
