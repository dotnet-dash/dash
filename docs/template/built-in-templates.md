# Built-in Templates
Dash provides several built-in templates to generate C# code for common use cases.

You can write your own [Custom Templates](custom-templates.md) for more specific use cases.

## EntityFramework
The built-in EF templates will generate code to be used with EF Core.

### efpoco

**template name**: `dash://efpoco`

This template generates POCO objects to be used with EntityFramework.

### efcontext

**template name**: `dash://efcontext`

This template generates a `DbContext` class.