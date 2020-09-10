# Templates
After parsing the Model specification, Dash relies on templates to generate the code.

## Default Templates
| Template name | Description
|---------------|---------------------------------------|
| `efpoco`      | Generate EntityFramework POCO classes |
| `efcontext`   | Generate EntityFramework DbContext    |

To reference default templates inside the [Dash JSON Model](./readme.md#Basic-Anatomy-of-a-Dash-Model-JSON-file) file, simply provide the template name like in the example below:
~~~ JSON
{
    "Configuration": {
        "Templates": [
            {
                "Template": "efpoco",
                "Output": "./EntityFramework",
            },
            {
                "Template": "efcontext",
                "Output": "./EntityFramework",
            }
        ]
    }
}
~~~

## Custom Templates

### Writing Custom Templates
Dash uses [Scriban](https://github.com/lunet-io/scriban) as its templating engine.

### Placing Custom Templates
You can place custom templates anywhere inside your .NET project.

Consider the following application tree:
~~~
📦 MvcApp
 ┣ 📂 Controllers
 ┣ 📂 Models
 ┣ 📂 CustomTemplates
 ┃ ┣ 📜 GenerateControllers.dt
 ┃ ┗ 📜 GenerateModels.dt
 ┣ 📜 MyDashModel.json
 ┗ 📜 MvcApp.csproj
~~~

We have two custom Dash Templates (`.dt`) that are going to generate beautiful C# Controller and Model classes.

To reference these two custom templates inside the _Model_ file, you simply provide a relative or absolute path to the templates inside your [Configuration](./readme.md#configuration-object) Object:

~~~ JSON
{
    "Configuration": {
        "Templates": [
            {
                "Template": "./CustomTemplates/GenerateControllers.dt",
                "Output": "./Controllers",
            },
            {
                "Template": "./CustomTemplates/GenerateModels.dt",
                "Output": "./Models",
            }
        ]
    }
}
~~~

### Recommended practices
- Custom Templates should have the extension `.dt`.
- Avoid naming the templates after the default extensionless template names. If you do, the custom templates are referenced instead of the default templates.