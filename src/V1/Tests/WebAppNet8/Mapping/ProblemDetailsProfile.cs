using Microsoft.AspNetCore.Mvc;
using System.Net;
using ServiceBricks;

namespace WebApp
{
    public class ProblemDetailsMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<Exception, ProblemDetails>(
                (s, d) =>
                {
                    d.Detail = s.StackTrace;
                    d.Status = (int)HttpStatusCode.InternalServerError;
                    d.Type = s.GetType().FullName;
                    d.Title = s.Message;
                });
        }
    }
}