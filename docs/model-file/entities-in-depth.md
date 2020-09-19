# Entities In-Depth
This document will go more in-depth into the possibilities and limitations of Entities.

## Base Entity
You cannot name your Entities `Base` because this name has already been reserved by Dash. It is used for the Ultimate Base Entity. All Entities that you define will inherit from `Base`.

To show you how the default `Base` Entity is defined internally by Dash, we will show you it in Dash JSON:
~~~ JSON
{
    "Model": {
        "Base": {
            "@@Abstract": true,
            "Id": "Int"
        }
    }
}
~~~

In the above, we see that:
- all Entities will inherit an `Id` attribute of the type `Int`
- the `"@@Abstract": true` tells Dash **NOT** to generate code for this Entity.

### Overriding the Base Entity
In your Model File, you can override or extend the default `Base` Entity by providing a new definition:

~~~ JSON
{
    "Model": {
        "Base": {
            "Id": "Guid",
            "DateCreated": "DateTime"
        }
    }
}
~~~

In the above example, we override the `Id` attribute, and extend the `Base` Entity with a `DateCreated` attribute.

## Inheritance
By default, every Entity inherits from `Base`. You can also explicitly specify a custom inheritance by using `@@Inherits`:

~~~ JSON
{
    "Model": {
        "MetaData": {
            "@@Abstract": true,
            "Key": "String",
            "Value": "String"
        },

        "PersonMetaData": {
            "@@Inherits": "MetaData"
        }
    }
}
~~~

!!! warning

    Only attributes are inherited, not relationships.

!!! note

    Inheritance is only meant to reduce redundancy in the Model File.
    
    Dash will generate the same code whether you use inheritance or simply define all attributes without the help of inheritance.

