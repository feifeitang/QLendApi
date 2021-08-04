# QLendAPI

dotnet user-secrets init

dotnet user-secrets set ConnectionStrings:DatabaseAlias "Server={HOST},{PORT};Database=QLendDB;User Id={USER};Password={PASSWORD}"

dotnet user-secrets set MssqlSettings:Password '{PASSWORD}'

dotnet ef dbcontext scaffold Name=ConnectionStrings:DatabaseAlias Microsoft.EntityFrameworkCore.SqlServer -o Models


## migrations

dotnet ef migrations add {description}

dotnet ef database update {description}

dotnet ef migrations list

## local run in docker

docker run -itd -p 8080:80 -e MssqlSettings:Password='{PASSWORD}' qlend:1.0.0