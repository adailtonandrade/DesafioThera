using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces.Services;
using Application.ViewModels;
using Domain.Interfaces.Data;
using AutoMapper;

namespace Application
{
    public class ProfilePermissionAppService : GenericAppService, IProfilePermissionAppService
    {
        private readonly IUnitOfWork _uow;
        private readonly IProfilePermissionService _profilePermissionService;
        private readonly IMapper _mapper;

        public ProfilePermissionAppService(IProfilePermissionService profilePermissionService, IMapper mapper, IUnitOfWork uow) : base(uow)
        {
            _profilePermissionService = profilePermissionService;
            _uow = uow;
            _mapper = mapper;
        }
        public ProfilePermissionVM GetByComposedKey(int param1, int param2)
        {
            return _mapper.Map<ProfilePermission, ProfilePermissionVM>(_profilePermissionService.GetByComposedKey(param1, param2));
        }
    }
}
