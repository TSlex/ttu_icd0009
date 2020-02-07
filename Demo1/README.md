## Demo1

~~~
dotnet aspnet-codegenerator controller -name PersonsController -actions -m Person -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ContactsController -actions -m Contact -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ContactTypesController -actions -m ContactType -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
~~~

~~~
dotnet aspnet-codegenerator controller -name PersonsController -actions -m Person -dc ApplicationDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name ContactsController -actions -m Contact -dc ApplicationDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name ContactTypesController -actions -m ContactType -dc ApplicationDbContext -outDir ApiControllers -api --useAsyncActions  -f
~~~