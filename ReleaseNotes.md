# 1.2.0
- Support .NET 9.0
- Support both sync and async querying with new ServiceQuery release
- Upgrade all packages references to latest version
- Fix for ApiQueryAfterEvent event to include initial request
- Fix for .NET 6 to use 6 libraries instead of 7
- Fix for MongoDB to add MongoDatabaseSettings and MongoCollectionSettings
- Fix for EntityFrameworkCore and MongoDB providers to add ServiceQueryOptions
- Fix for Postgres, Sqlite and SqlServer providers to add async methods to migration and ensurecreated rules
- Removed try/catch blocks in ServiceBricksStart method
- Removed old resources in webapp projects

# 1.1.3
- Fix for Azure ServiceBus Topic and Queue providers to use IBusinessRuleService from scoped IServiceProvider instead of dependency injection.

# 1.1.2
- Fix for MongoDB to create Guid serializer rule

# 1.1.0
- Official Production Release

# 1.0.11
- Update all package references to latest version
- Add cleanup logic to delete objects for unit and integration tests

# 1.0.10
- Bug fixes

# 1.0.9
- Automated release testing

# 1.0.8
- Automated release testing

# 1.0.7
- Bug fixes for appsettings, ApiClient, cleanup webapps, update nuget packages

# 1.0.6
- Bug fixes for example projects

# 1.0.5
- Bug fixes for ApiClient and BearerTokenClient to support security microservice
- Added new ServiceBricks.Xunit.Debug project

# 1.0.4
- Full source code release with V1.0.4

