# .net-angular-dating-app
.NET Angular Dating App


## Entity Framework
dotnet ef migrations add InitialCreate -o Data/Migrations 

- Had an issue running this command on an M1 Mac
- I used the JetBrains Entity Framework Plugin directly at `Tools/EntityFramework/AddMigration`

Similar `botnet ef database update` command did not work either
- I had to use the Plugin directly Tools/EntityFramework/Update