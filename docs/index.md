<div align="center">
    <img alt="dotnet-dash" src="./img/logo-w-text.png" width="38%" />
</div>

Dash is a .NET command-line tool for fast model-driven code generation.

## How Dash Works
- As a Developer, you will be writing a [Model File](/user-guide/model-file.md) using JSON to describe Domain Entities and its relationship with eachother.
- Dash has several generic [Built-in Templates]() to generate code based on your Model File.
- You can also write [Custom Templates](/user-guide/custom-templates) to generate code to cover more specific use cases.
- Run the [Dash CLI](/user-guide/dash-cli) tool to generate the code.

Here is a visual representation of the above:

![](./img/how-dash-works.png)

## Installation
Installation is pretty simple, just copy and paste the following command:

~~~
dotnet tool install --global dotnet-dash --version 0.1.1-alpha
~~~

If you have successfully installed Dash, the following command will bring up the help screen:

~~~
dotnet dash --help
~~~