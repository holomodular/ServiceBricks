# 1.4.0
- Rollup Release

# 1.4.0-e
- Fixes for Azure ServiceBus Queue and Topics
- Async API methods as canonical
- New test home page

# 1.4.0-d
- Support for .Net Core 10
- Included Newtonsoft.Json back into project for use with JsonPatchDocument, need services.AddControllers().AddNewtonsoftJson() as well.
- New API method Patch to send incremental updates without a full payload. Returns a full payload object.
- New API method Validate to validate a given object.
- New API methods CreateAck, UpdateAck, PatchAck that return only a response for low latency/small packet sizes.
- New format querystring parameter (and optional header parameter) that specifies the response format either classic or modern. Server default is classic so that it matches swagger API definitions.
- Upgrades to Swagger and OpenAPI caused changes to swagger apply security operations.
- xUnit test fixes
- Added TestDataTypes microservice hard-wired to framework to allow easier testing and debugging.

# 1.3.0
- Rollup quarterly release. See previous release notes.

# 1.3.0-c prerelease
- Fixes for background timer
- Note: Net8 now uses EntityFrameworkCore 9
- Note: ServiceQuery updated to 2.2.1

# 1.3.0-b prerelease
- Update all nuget package references
- Removed Newtsonft.Json from ServiceBricks and added new project for abstraction. Uses System.Text.Json by default. Use IJsonSerializer with DI or JsonSerializer class with public static Instance for quick access and to not clutter DI.
- Added Boolean LogExceptions property to all storage providers, so logging can be disabled when exceptions expected (semaphore).
- Background timers will now change state to stop when ticking and restart when processing completes.
- Minor fixes

# 1.3.0-a prerelease
- Update all nuget package references
- Add new WorkService to replace DomainProcessQueue
- Add new test cases for workservice

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

