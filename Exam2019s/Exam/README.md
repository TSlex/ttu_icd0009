# Exam

###STEPS:
1 Create database
-
1) Create needed amount of domain entities
1) Place [DATA ANNOTATIONS]
2) Register them as DbSet<Entity>'s
3) Update database

2 Generate API and MVC controllers
-
* MVC
```
dotnet aspnet-codegenerator controller -name <NAME>sController -actions -m <NAME> -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
```
* API
```
dotnet aspnet-codegenerator controller -name <NAME>sController -actions -m  <NAME> -dc ApplicationDbContext -outDir ApiControllers -api --useAsyncActions  -f
```

3 (optional)
-
1. Create required DAL, BLL and API classes
2. Create required repos interfaces, classes and factory methods
3. Create required services interfaces, classes and factory methods

4 Functionality
-
1. Create needed functionality (Database => DAL => BLL => Controllers/API)
2. Secure controllers

5 Make js-client
-

6 Validate everything
-

7 Done!
-