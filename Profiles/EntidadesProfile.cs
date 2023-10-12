using AutoMapper;
using Entidades.Models;
using Entidades.Models.DTO;

namespace Entidades.Profiles
{
    public class EntidadesProfile : Profile
    {

        public EntidadesProfile()
        {
            CreateMap<Campo, CampoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));


            CreateMap<TiposMuestra, TipoMuestraDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));


            CreateMap<NombresVariablesMuestra, NombreVariableMuestraDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.IdTipoMuestra, opt => opt.MapFrom(src => src.IdTipoMuestra));


        }



    }
}
