# QLendAPI

dotnet user-secrets init

dotnet user-secrets set ConnectionStrings:DatabaseAlias "Server={HOST},{PORT};Database=QLendDB;User Id={USER};Password={PASSWORD}"

dotnet ef dbcontext scaffold Name=ConnectionStrings:DatabaseAlias Microsoft.EntityFrameworkCore.SqlServer -o Models
