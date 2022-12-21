using AutoMapper;

namespace Zeta.NontonFilm.Application.Common.Mappings;

public interface IMapFrom<TSource, TDestination>
{
    void Mapping(Profile profile)
    {
        profile.CreateMap<TSource, TDestination>();
    }
}
