using AutoMapper;
using ZadElealam.Core.Dto;
using ZadElealam.Core.Models;

namespace ZadElealam.Apis.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap< YouTubeVideo, YouTubeVideoDto>().ReverseMap();
            CreateMap<Feedback, FeedbackDto>().ReverseMap();
            CreateMap<Feedback, UpdateFeedbackDto>().ReverseMap();
            CreateMap<Notification, NotificationDto>().ReverseMap();
        }
    }
}
