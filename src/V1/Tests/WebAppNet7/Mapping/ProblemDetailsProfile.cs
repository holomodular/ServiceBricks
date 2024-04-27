using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace WebApp.Mapping
{
    public class ProblemDetailsProfile : Profile
    {
        public ProblemDetailsProfile()
        {
            CreateMap<Exception, ProblemDetails>()
                .ForMember(x => x.Detail, y => y.MapFrom(z => JsonConvert.SerializeObject(z)))
                .ForMember(x => x.Status, y => y.MapFrom(z => (int)HttpStatusCode.InternalServerError))
                .ForMember(x => x.Type, y => y.MapFrom(z => z.GetType().FullName))
                .ForMember(x => x.Title, y => y.MapFrom(z => z.Message))
                .ForMember(x => x.Instance, y => y.Ignore())
                .ForMember(x => x.Extensions, y => y.Ignore());
        }
    }
}