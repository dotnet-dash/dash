# Dash Documentation

## Important notes
If you are not familiar with JSON, we recommend you to first learn the basics of JSON before proceeding.

## Basic Rules
- In Dash, you define Entities, Attributes, and Relationships.
- The order and scope in which Entities are defined in the source code **do not matter**.
- Always use singular nouns for naming Entities.
- Dash uses Convention Over Configuration.
- Dash is case-insensitive.

Let's go through a series of examples to learn write Dash Models.
The generated POCO is included as a reference.

## Basic Anatomy of a Dash Model file
The Dash JSON file is very simple.

At its root level, there are only 2 JSON Objects: the _optional_ `Configuration` and `Model`:

~~~ JSON
{
    "Configuration": {
    },

    "Model": {
    }
}
~~~

## Configuration Object
Inside the `Configuration` object, you define properties related to code generation.

If this object or any of its properties are not defined, then default values will be used instead. Let's take a look at an example:

~~~ JSON
{
    "Configuration": {
        "Language": "cs",
        "AutogenSuffix": ".generated",
        "Header": "(c) 2020 Dash",
        "DefaultNamespace": "MyApplication",
        "Templates": [
            {
                "Template": "./MyTemplate/EfContext",
                "Output": "./Ef",
            },
            {
                "Template": "./MyTemplates/Poco",
                "Output": "./Poco"
            }
        ]
    }
}
~~~

| Property         | Data type                     | Default value | Description                                      |
|------------------|-------------------------------|---------------|--------------------------------------------------|
| Language         | `string`                      | `cs`          | Generated code will be in the specified language |
| AutogenSuffix    | `string`                      | `.generated`  | The value that will be appended after the filename, and prepended before the extension to indicate that the file was auto-generated |
| Header           | `string`                      | _Empty_       | The header that will be injected as comments in every generated file |
| DefaultNamespace | `string`                      | _Inferred from nearest .NET project_ | This is the default namespace |
| Templates        | Array of `string` values, or `Template` objects | `["Poco", "Ef"]` | Specify the templates which will be used for code generation |

## Template Object
The `Template` object is important as it decides which templates should be used for code generation.

You can use the [Default Templates](./templates.md#default-templates), or you can write your own [Custom Templates](./templates.md#custom-templates).

~~~ JSON
{
    "Template": "Poco",
    "Output": "./Poco"
}
~~~
| Property | Data type       | Default value       | Description              |
|----------|-----------------|---------------------|--------------------------|
| Template | string          | _No default value_  | The name of the template |
| Output   | string          | `.`                 | The absolute output file location, or relative to the Model file |

## Model Object
This is where interesting things are happening. Inside the `Model` property, you can define entities, its attributes, and the relationship between various entities.