using Application.ViewModels;
using AutoMapper;
using Domain.Entities;
using AutoMapper.Extensions.ExpressionMapping;

namespace Application.Mapper
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                #region Profile
                cfg.CreateMap<Domain.Entities.Profile, ProfileVM>()
                .ForMember(p => p.ProfilePermissions, opt => opt.MapFrom(p => p.ProfilePermissions)).ReverseMap().PreserveReferences()
                .ForMember(p => p.UserList, opt => opt.MapFrom(p => p.UserList)).ReverseMap().PreserveReferences()
                .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Name.Trim())).ReverseMap();
                #endregion
                #region User
                cfg.CreateMap<UserVM, User>().ReverseMap();
                #endregion
                #region Talent
                cfg.CreateMap<TalentVM, Talent>().ReverseMap();
                cfg.CreateMap<Talent, TalentDetailsVM>().ReverseMap();
                #endregion
                #region Permission
                cfg.CreateMap<Permission, PermissionVM>().ReverseMap();
                #endregion
                #region  ProfilePermission 
                cfg.CreateMap<ProfilePermission, ProfilePermissionVM>().ReverseMap();
                #endregion
            });
        }
    }
}
