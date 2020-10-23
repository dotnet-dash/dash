# Data Type Declaration
Defining the _Data Type_ of an attribute is pretty straight forward, as you have seen.

Simply use the following syntax:
~~~ JSON
  "AttributeName": "[data type]([modifiers])"
~~~

where `[modifiers]` is _optional_.

An example:
~~~ JSON
  "GivenName": "String"
~~~

## Supported Data Types
The following Data Types are supported:

| Dash data type  | .NET     | SqlServer        |
|-----------------|----------|------------------|
| `string`        | String   | varchar          |
| `unicode`       | String   | nvarchar         |
| `int`           | Int      | int              |
| `bool`          | Bool     | bit              |
| `datetime`      | DateTime | datetime         |
| `guid`          | Guid     | uniqueidentifier |

For your convenience, the target data type (.NET and SqlServer) is also included.

## Constraints
Dash supports _constraints_ that let you express the limitations of your Attribute.

### Optional/Nullable
In Dash, all attributes are _required_ or _Non-Nullable_.

Similar to C#, use `?` to specify that an attribute is _optional_ or _Nullable_.

Example:
~~~ JSON
  "Nickname": "String?"
~~~

### Maximum Length
To specify that an attribute has a maximum length, use `[length]`.

Example:
~~~ JSON
  "Nickname": "String[30]"
~~~

### Default Value
To specify that an attribute has a default value, use `(=='default')`.

Example:
~~~ JSON
  "Nickname": "String(=='Foo')"
~~~

In the above example, we specified that the default value for `Nickname` is `Foo`.

### Regular Expression
To use a regular expression as a constraint, use `:[regex]`

Example:
~~~ JSON
  "Nickname": "String:[a-zA-Z0-9]{5}"
~~~

### Mixing Constraints
All constraints can be mixed. For instance, you can specify that an attribute is Nullable and has a Maximum Length of 30:

~~~ JSON
  "Nickname": "String?[30]"
~~~

You can put the constraints in any order, except for the Regular Expression constraint, which must always be placed as the final constraint.

~~~ JSON
  "Nickname": "String(=='Foo')[30]?:[a-zA-Z0-9]"
~~~