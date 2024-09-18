using AutoMapper;
using ZadElealam.Core.Dto;
using ZadElealam.Core.Models;

namespace ZadElealam.Apis.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDto, Category>();
            CreateMap< YouTubeVideo, YouTubeVideoDto>();
            CreateMap<Feedback, FeedbackDto>().ReverseMap();
        }
    }
}
