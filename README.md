# .net-angular-dating-app

.NET Angular Dating App

## .NET API

## Entity Framework

dotnet ef migrations add InitialCreate -o Data/Migrations

- Had an issue running this command on an M1 Mac
- I used the JetBrains Entity Framework Plugin directly at `Tools/EntityFramework/AddMigration`

Similar `dotnet ef database update` command did not work either

- I had to use the Plugin directly `Tools/EntityFramework/Update`

## Running a new Database Migration with Entity Framework
- `dotnet ef migrations add <arbitrary migration name>`
  - ex. `dotnet ef migrations add ExtendedUserEntity`
- After reviewing the generated migration, if it all looks good, then run `dotnet ef database update` to apply the changes

![Users in SQLite Database](https://raw.githubusercontent.com/kawgh1/.net-angular-dating-app/main/API/Users%20in%20sql%20lite%20database.png)


- To delete a database in Entity Framework `dotnet ef database drop`

## Seeding the SQLite Database
- `Seed.cs` is a class we created that has a method `seedUsers()` which first checks if any Users already exist
  - if Users do exist, return and do nothing
  - otherwise, Seed the SQLite Database by reading from a specified file `Data/UserSeedData.json`
    - in `Program.cs`
      - this block of code is only needed for seeding dummy data in the database on startup if none exists `
      

                using var scope = app.Services.CreateScope();
                var services = scope.ServiceProvider;
                
                try
                {
                    var context = services.GetRequiredService<DatabaseContext>();
                    await context.Database.MigrateAsync();
                    await Seed.SeedUsers(context);
                }
                catch (Exception e)
                {
                    var logger = services.GetService<ILogger<Program>>();
                    logger.LogError(e, "An error occured while seeding or migrating the database");
                }
        
                `


## NuGet Packages installed
- Entity Framework
  - `Microsoft.EntityFrameworkCore.Design`
- SQLite DB implementation
  - `Microsoft.EntityFrameworkCore.Sqlite`
- For Token Service
  - `System.IdentityModal.Tokens.Jwt`
- In Program.cs
  - builder.Services.AddAuthentication(JwtBearerDefaults)
    - `Microsoft.AspNetCore.Authentication.JwtBearer`
       
        
        
              builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
      
              options.TokenValidationParameters = new TokenValidationParameters
              {
            
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
              ValidateIssuer = false, // for this to be true, the information must be passed inside the token
              ValidateAudience = false // ""
      
              });

- AutoMapper
  - `AutoMapper.Extensions.Microsoft.DependencyInjection`
    - Object Relational Mapper (ORM) for Entity Framework 
    
          `
      
              // Here we are mapping the first photo that has isMain == true to be the MemberDTO's PhotoUrl photo
              CreateMap<AppUser, MemberDTO>()
                  .ForMember(member => member.PhotoUrl,
                       options => options.MapFrom(
                            src => src.Photos.FirstOrDefault(photo => photo.IsMain).Url));
        
              CreateMap<Photo, PhotoDTO>();
          
          `
      - UserController

                `
      
                    [Authorize]
                    [HttpGet("username/{username}")]
                    
                    public async Task<ActionResult<MemberDTO>> GetUserByUsername(string username)
                    {
                        var user = await _userRepository.GetUserByUsernameAsync(username);
                        var userToReturn = _mapper.Map<MemberDTO>(user);
                        return userToReturn;
                    }
              
                `

## User Photo Management with Cloudinary
- Cloudinary is used to handle photo upload, editing, etc. cloudinary.com
- //Add `CloudinaryDotNet` using Nuget Package Manager or if using Package Manager Console:
  Install-Package CloudinaryDotNet
- Account account = new Account(
  "asdffeeag",
  "245234523453dsfgf",
  "***************************");

Cloudinary cloudinary = new Cloudinary(account);

## API/appsettings.json

    `
    {
    "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
      }
    },
    "AllowedHosts": "*",
    "CloudinarySettings": {
      "CloudName": "asdfasdf",
      "ApiKey": "1234213423",
      "ApiSecret": "asdf9s988f8d8-FFdweg-8"
      }
    }
    
    `

## Successful Cloudinary Image Upload using Dating App API `PhotoService.cs`
![image-upload-to-cloudinary](https://raw.githubusercontent.com/kawgh1/.net-angular-dating-app/main/API/successful-cloudinary-image-upload.png)

- # Angular App

    - ## Dependencies
        - `npm install font-awesome` - styling
        - `npm install ngx-bootstrap` - styling
        - `npm install mkcert` - enable HTTPS within client Angular app - github.com/FiloSottile/mkcert
            - mkcert is a simple tool for making locally-trusted development certificates. No configuration.
        - `mkcert -install`
          - will requires local machine admin privileges to run
        - ### Brew Install
            - `brew install mkcert`
- In the angular `client` folder, create a new folder called `ssl` and cd into it
                       
            `
                      
                terminal $: client % mkdir ssl
                terminal $: client % cd ssl
                terminal $: ssl % mkcert -install
                Created a new local CA 💥
                Sudo password:
                The local CA is now installed in the system trust store! ⚡️
                ERROR: no Firefox security databases found

                terminal $: ssl % mkcert localhost
                Note: the local CA is not installed in the Firefox trust store.
                Run "mkcert -install" for certificates to be trusted automatically ⚠️
            
                Created a new certificate valid for the following names 📜
            
                "localhost"
            
                The certificate is at "./localhost.pem" and the key at "./localhost-key.pem" ✅
             
                It will expire on 15 January 2026 🗓

            `
    - Above we have made certificate for 'localhost' on our local machine to enable HTTPS
      - in `angular.json` under `"serve":{...}` when we run `ng serve` we want Angular to use SSL
        - add 
            `
              
                    "serve": {
                          "options": {
                              "ssl": true,
                              "sslCert": "./ssl/localhost.pem",
                              "sslKey": "./ssl/localhost-key.pem"
                              },
            `
    - You will know this operation was successful if, upon running `ng serve` it now runs at `https://localhost:4200` instead of `http`
