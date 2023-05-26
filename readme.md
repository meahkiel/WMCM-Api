# WMCM Web API

## Requirements
- Postgre or SQL Server
- SMTP Gateway or Fake SMTP (mailtrap.io)
- Facebook API
- .Net Core 7.0
- Docker Container (optional)
- Visual Studio 2022

## Instruction

#### Setting up database connection Postgre
- Open Visual Studio Code
- Open appsettings.Development.json
- Update Connectionstring  (server=localhost;Database=wmcmdb;Port=5432;User Id=userId;Password=yourpassword;)
### **Running using Kestrel**
``` cli
dotnet run --project .\API\
```
### **Running with Docker**
```
docker run --rm -it -p 8080:80 
```

### WMCM Admin Login 
- Enter username and Password in the web app 
```
Username: bob
Password: Pa$$w0rd
```
- Click Channel 
- Add your credential in the appropriate channel


