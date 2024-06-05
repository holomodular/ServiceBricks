using Microsoft.AspNetCore.Mvc;
using ServiceQuery;

namespace ServiceBricks
{
    public interface IApiController<TDto>
    {
        ActionResult Create(TDto dto);

        Task<ActionResult> CreateAsync(TDto dto);

        ActionResult Update(TDto dto);

        Task<ActionResult> UpdateAsync(TDto dto);

        ActionResult Delete(string storageKey);

        Task<ActionResult> DeleteAsync(string storageKey);

        ActionResult Get(string storageKey);

        Task<ActionResult> GetAsync(string storageKey);

        ActionResult Query(ServiceQueryRequest request);

        Task<ActionResult> QueryAsync(ServiceQueryRequest request);

        ActionResult GetErrorResponse(IResponse response);
    }
}