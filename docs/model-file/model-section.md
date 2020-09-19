## Model Object
This is where interesting things are happening. Inside the `Model` object, you can define entities, its attributes, and the relationship between various entities.

### Entity
To define an Entity, simply add a JSON object inside the `Model` object, like this:

~~~ JSON
{
    "Model": {
        "Person": {
        }
    }
}
~~~

### Attribute
To define an Attribute for an Entity, simply add a key/value pair to the Entity object like this:

~~~ JSON
{
    "Model": {
        "Person": {
            "GivenName": "Unicode",
            "FamilyName": "Unicode",
            "Age": "Int"
        }
    }
}
~~~

#### Data type syntax
The _value_ of the key/value pair defines the data type of the attribute, and must adhere to the following _Data Type Syntax_:

~~~
[dash data type][constraints]
~~~

where `[dash data type]` must be one of the following values:

| Dash data type  | .NET     | SqlServer        |
|-----------------|----------|------------------|
| `string`        | String   | varchar          |
| `unicode`       | String   | nvarchar         |
| `int`           | Int      | int              |
| `bool`          | Bool     | bit              |
| `datetime`      | DateTime | datetime         |
| `guid`          | Guid     | uniqueidentifier |

where `[constraints]` can be a mix of any of the following constraints:

| Constraint      | Description                                                                               |
|-----------------|-------------------------------------------------------------------------------------------|
| `?`             | Indicates that the attribute is Optional                                                  |
| `[length]`      | Specifies the maximum length of the attribute (only applicable to `string` and `unicode`) |
| `(=='default')` | Defines the default value of the attribute                                                |
| `:[regex]`      | A regular expression constraint                                                           |

##### Example #1
Here we define that the `Username` attribute is _Optional/Nullable_:
~~~ JSON
"Username": "string?"
~~~

##### Example #2
The `Username` attribute is defined as a string with a maximum length of 10 characters:
~~~ JSON
"Username": "string[10]"
~~~

##### Example #3
In the next example, we specify a default value `anonymous` for the `Username` attribute:
~~~ JSON
"Username": "string(=='anonymous')"
~~~

##### Example #4
In the next example, we will apply a regular expression constraint to the `Username` attribute:
~~~ JSON
"Username": "string:[a-zA-Z0-9]"
~~~

##### Example #5
Now, let's try to apply multiple constraints to the same attribute:
~~~ JSON
"Username": "string?[10](=='anonymous'):[a-zA-Z0-9]"
~~~

!!! note

    You can put the constraints in any order, except for the regular expression constraint, which must be placed as the final constraint.