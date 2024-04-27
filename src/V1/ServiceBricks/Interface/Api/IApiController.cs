using Microsoft.AspNetCore.Mvc;
using ServiceQuery;

namespace ServiceBricks
{
    public interface IApiController<TDto>
    {
        ActionResult Create([FromBody] TDto dto);

        Task<ActionResult> CreateAsync([FromBody] TDto dto);

        ActionResult Update([FromBody] TDto dto);

        Task<ActionResult> UpdateAsync([FromBody] TDto dto);

        ActionResult Delete([FromQuery] string storageKey);

        Task<ActionResult> DeleteAsync([FromQuery] string storageKey);

        ActionResult Get([FromQuery] string storageKey);

        Task<ActionResult> GetAsync([FromQuery] string storageKey);

        ActionResult Query([FromBody] ServiceQueryRequest request);

        Task<ActionResult> QueryAsync([FromBody] ServiceQueryRequest request);

        ActionResult GetErrorResponse(IResponse response);
    }
}