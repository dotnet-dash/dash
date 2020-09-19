# Reverse Engineered Entities

The default way to define Entities is to manually craft them by defining Attributes and Relationships.

However, Dash also supports far more advanced scenarios that we call **Reverse Engineered Entities**

## Database schema
You can use the database schema to define :

``` JSON
{
    "Configuration": {
        "Databases": [
            {
                "Name": "My Local SqlServer",
                "Type": "SqlServer",
                "ConnectionString": "server=localhost; integrated security=true;"
            }
        ]
    },

    "Model": {
        "Currency": {
            "@@Reverse Engineer From Database": {
                "Name": "My Local SqlServer",
                "Table": "Foo.dbo.Currencies"
            }
        }
    }
}
```

In the above example, we tell Dash to create the entities `Currency` and `Country`. Dash is smart enough to convert this to singular.

## JSON
You can use 