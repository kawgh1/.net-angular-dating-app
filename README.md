# .net-angular-dating-app

.NET Angular Dating App

## .NET API

## Entity Framework

dotnet ef migrations add InitialCreate -o Data/Migrations

- Had an issue running this command on an M1 Mac
- I used the JetBrains Entity Framework Plugin directly at `Tools/EntityFramework/AddMigration`

Similar `dotnet ef database update` command did not work either

- I had to use the Plugin directly `Tools/EntityFramework/Update`


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
    - ` builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      options.TokenValidationParameters = new TokenValidationParameters
      {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
      ValidateIssuer = false, // for this to be, the information must be passed in the token
      ValidateAudience = false // ""
      });`


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
