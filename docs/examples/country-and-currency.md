# Country and Currency
In this example, we will create a Model file called `CountryCurrency.json`.

## What we showcase in this example
- how to use the [@@Seed property](../language.md#@@seed-property) to read a CSV from an HTTP resource.
- how to build simple relationships between 2 Entities.

## The source code
~~~ JSON
{
    "Model": {
        "Currency": {
            "Code": "String[3]",
            "Name": "Unicode",
            "NumericCode": "Int",

            "@@Seed": {
                "FromCsv": {
                    "Uri": "https://raw.githubusercontent.com/datasets/currency-codes/master/data/codes-all.csv",
                    "FirstLineIsHeader": true,
                    "MapHeaders": {
                        "Code": "AlphabeticCode",
                        "Name": "Currency",
                        "NumericCode": "NumericCode"
                    }
                }
            }
        },

        "Country": {
            "CountryName": "Unicode",
            "Alpha3": "String[3]",
            "CountryCode": "int",
            
            "@@Has": {
                "OfficialCurrency": "Currency"
            },

            "@@Seed": {
                "FromCsv": {
                    "Uri": "https://raw.githubusercontent.com/lukes/ISO-3166-Countries-with-Regional-Codes/master/all/all.csv",
                    "FirstLineIsHeader": true,
                    "MapHeaders": {
                        "CountryName": "name",
                        "Alpha3": "alpha-3",
                        "CountryCode": "country-code"
                    }
                }
            }
        }
    }
}
~~~

In the following example, we:
- define an [Entity](./docs/entity.md) called `Currency` with 3 [Attributes](./docs/entity.md#attributes):
    - `Code` of the type [string](./docs/datatypes#string) with a maximum of length of 3.
    - `Name` of the type [unicode](./docs/datatypes#unicode) indicating that it is a string that also supports unicode characters.
    - `NumericCode` of the type [integer](./docs/datatypes#integer)
- use the [special @@Seed property](./docs/seed.md) to specify that `Currency` has seed data, and that this data exists inside a CSV file located in a GitHub repository.
- specify how the CSV data is mapped to each attribute using the `MapHeaders` property.