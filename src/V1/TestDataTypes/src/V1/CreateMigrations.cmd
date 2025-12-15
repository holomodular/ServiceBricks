cd ServiceBricks.TestDataTypes.Postgres
dotnet ef migrations add TestDataTypesV1 --context TestDataTypesPostgresContext --startup-project ../Tests/MigrationsHost
cd ..\ServiceBricks.TestDataTypes.Sqlite
dotnet ef migrations add TestDataTypesV1 --context TestDataTypesSqliteContext --startup-project ../Tests/MigrationsHost
cd ..\ServiceBricks.TestDataTypes.SqlServer
dotnet ef migrations add TestDataTypesV1 --context TestDataTypesSqlServerContext --startup-project ../Tests/MigrationsHost
