namespace ServiceBricks.TestDataTypes
{
    /// <summary>
    /// This is a REST API client for the TestDto.
    /// </summary>
    public partial interface ITestApiClient : IApiClient<TestDto>, ITestApiService
    {
    }
}
