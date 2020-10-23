# Introduction
Dash uses the JSON format to describe the domain models. Each JSON file is called a _(Dash) Model File_.

Model Files are written according to Dash's own JSON specification which, throughout this documentation, is referred to as _Dash JSON_.

!!! warning

    If you are not familiar with JSON, we recommend you to first learn the basics of JSON before proceeding.
    You need to be at least familiar with the JSON syntax and the supported data types.

## Basic Rules of Dash JSON
- In Dash JSON, key/value pairs are used to define:
    1. [Sections](#sections)
    1. [Entities](#entities)
    1. [Attributes](#attributes)
    1. [Relationships](defining-relationships.md)
    1. [Actions](#actions)
- The order in which the key/value pairs are defined in the Model File **do not matter**.
- Always use **singular nouns** for naming Entities.
- Dash uses Convention Over Configuration.
- Dash is case-insensitive.

## Sections
At its root level, Dash JSON requires sections to be defined.

Only two sections are supported: the _optional_ `Configuration` and the _required_ `Model` object:

~~~ JSON
{
    "Configuration": {
    },

    "Model": {
    }
}
~~~

The [`Configuration`](configuration-section.md) object contains key/value pairs that allows you to configure the code generation.

The `Model` object contains the key/value pairs that represent Entities.

## Entities
Entities are defined using key/value pairs directly under the `Model` object, like this:

~~~ JSON
{
    "Model": {
        "Person": {
        }
    }
}
~~~

In the above example, we defined the entity `Person` with 0 attributes.

## Attributes
An attribute consists of a _Name_ and a _[Data Type Declaration](./data-type-declaration.md)_, and is defined using a key/value pair placed directly under an Entity object, like this:

~~~ JSON
{
    "Model": {
        "Person": {
            "Name": "string",
            "Age": "int"
        }
    }
}
~~~

In the above example, we have added the 2 attributes to the `Person` entity:

1. `Name` of the data type `string`
1. `Age` of the data type `int`

!!! note

    You can add constraints to the Data Type Declaration to limit the length, or enforce a [regular expression](to-do).
    Please visit the [Data Type Declaration](./data-type-declaration.md) to read more about this subject.

!!! important

    Dash will automatically add an `Id` attribute for every Entity.
    Please visit the [Attributes documentation](attributes.md) for more information.

## Relationships
Dash supports 3 types of relationships:

| Relationship type | Dash Key/Value Pair         |
|-------------------|-----------------------------|
| 1 to 1            | `@@Has`                     |
| 1 to Many         | `@@Has Many`                |
| Many to Many      | `@@Has Many And Belongs To` |

### 1 to 1
To define a 1:1 relationship, simply add a `@@Has` object to your Entity.

#### Reference the Entity using an Array of Strings
This is the most concise way of defining a relationship. However, its limitation is that you cannot have multiple references to the same Entity.
~~~ JSON
{
    "Model": {
        "Person": {
            "@@Has": ["Address"]
        },

        "Address": {
        },
    }
}
~~~

#### Reference the Entity using an Object and Key/Value pairs.
This is a bit more verbose but lets you define multiple relationships with the same Entity, and also naming the relationship.
~~~ JSON
{
    "Model": {
        "Person": {
            "@@Has": {
                "BillingAddress": "Address",
                "ShippingAddress": "Address"
            }
        },

        "Address": {
        }
    }
}
~~~

### 1 to Many
To define a 1 to Many relationship, add a `@@Has Many` object to your Entity.

#### Reference the Entity using an Array of Strings
~~~ JSON
{
    "Model": {
        "Person": {
            "@@Has Many": ["Address"]
        },

        "Address": {
        }        
    }
}
~~~

#### Reference the Entity using an Object and Key/Value pairs.
~~~ JSON
{
    "Model": {
        "Person": {
            "@@Has Many": {
                "Address": "Address"
            }
        },

        "Address": {
        }
    }
}
~~~

### Many to Many
To define a Many to Many relationship, add a `@@Has Many And Belongs To` object to your Entity.

#### Reference the Entity using an Array of Strings
~~~ JSON
{
    "Model": {
        "Person": {
            "@@Has Many And Belongs To": {
                "Address": "Address"
            }
        },

        "Address": {
        }
    }
}
~~~

#### Reference the Entity using an Object and Key/Value pairs.
~~~ JSON
{
    "Model": {
        "Person": {
            "@@Has Many And Belongs To": {
                "Address": "Address"
            }
        },

        "Address": {
        }
    }
}
~~~

## Actions