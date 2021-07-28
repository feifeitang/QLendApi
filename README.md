# QLendAPI

dotnet user-secrets init

dotnet user-secrets set ConnectionStrings:DatabaseAlias "Server={HOST},{PORT};Database=QLendDB;User Id={USER};Password={PASSWORD}"

dotnet user-secrets set MssqlSettings:Password '{PASSWORD}'

dotnet ef dbcontext scaffold Name=ConnectionStrings:DatabaseAlias Microsoft.EntityFrameworkCore.SqlServer -o Models


## migrations

dotnet ef migrations add InitialCreate

dotnet ef database update InitialCreate
