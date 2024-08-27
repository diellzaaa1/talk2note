using talk2note.Domain.Entities;
using talk2note.Application.DTO.Note;
using talk2note.Application.DTO.User;
using AutoMapper;

namespace talk2note.Application.Mappings
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Note, NoteDTO>().ReverseMap();

            CreateMap<User, UserSignUp>().ReverseMap();
            CreateMap<User, UserSignIn>().ReverseMap();

        }
    }
}
