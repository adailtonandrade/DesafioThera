using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.ViewModels;
using Domain.Interfaces.Services;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces.Data;
using Application.Interfaces;
using Domain.Interfaces.Repositories;
using AutoMapper.Extensions.ExpressionMapping;

namespace Application
{
    public class ProfileAppService : GenericAppService, IProfileAppService
    {
        private readonly IProfileService _profileService;
        private readonly AutoMapper.IMapper _mapper;
        private readonly IComposedKeyRepository<ProfilePermission> _profilePermissionRepository;
        List<string> _errors = new List<string>();

        public ProfileAppService(IProfileService profileService, AutoMapper.IMapper mapper,
            IUnitOfWork uow, IComposedKeyRepository<ProfilePermission> profilePermissionRepository) : base(uow)
        {
            _profileService = profileService;
            _mapper = mapper;
            _profilePermissionRepository = profilePermissionRepository;
        }

        public List<string> Delete(int id)
        {
            Profile profile = _profileService.GetById(id);
            try
            {
                if (profile.Active.Equals(((int)GenericStatusEnum.Inactive).ToString("D")))
                    _errors = Reactivate(profile);
                else
                    _errors = Deactivate(profile);
            }
            catch (Exception e)
            {
                if (profile != null)
                    _errors.Add("Erro: " + e.Message.ToString() + "\nInner Exception: " + e.InnerException.Message.ToString());
                else
                    _errors.Add("O Perfil não foi encontrado");
            }
            return _errors;
        }

        private List<string> Deactivate(Profile profile)
        {
            try
            {
                var users = _profileService.Get(p => p.Id == profile.Id && p.UserList.Any(u => u.Active == ((int)GenericStatusEnum.Active).ToString()));
                if (users == null || users.Count() == 0)
                {
                    BeginTransaction();
                    profile.Active = ((int)GenericStatusEnum.Inactive).ToString();
                    _profileService.Update(profile);
                    SaveChanges();
                    Commit();
                }
                else
                    _errors.Add(String.Format("O Perfil {0} não pode ser desativado pois está associado a um ou mais usuários", profile.Name));
            }
            catch (Exception e)
            {
                _errors.Add(String.Format("Ocorreu um erro ao desativar o Perfil"));
                Rollback();
            }
            return _errors;
        }

        private List<string> Reactivate(Profile profile)
        {
            try
            {
                BeginTransaction();
                profile.Active = ((int)GenericStatusEnum.Active).ToString();
                _profileService.Update(profile);
                SaveChanges();
                Commit();
            }
            catch (Exception e)
            {
                _errors.Add(String.Format("Ocorreu uma falha ao reativar o Perfil"));
                Rollback();
            }
            return _errors;
        }

        public IEnumerable<ProfileVM> Get(Expression<Func<ProfileVM, bool>> filter = null, Expression<Func<IQueryable<ProfileVM>, IOrderedQueryable<ProfileVM>>> orderBy = null, string includeProperties = "")
        {
            var filterNew = filter != null ? _mapper.MapExpression<Expression<Func<ProfileVM, bool>>, Expression<Func<Profile, bool>>>(filter) : null;
            var orderByNew = orderBy != null ? _mapper.MapExpression<Expression<Func<IQueryable<ProfileVM>, IOrderedQueryable<ProfileVM>>>
                , Expression<Func<IQueryable<Profile>, IOrderedQueryable<Profile>>>>(orderBy) : null;
            return _mapper.Map<IEnumerable<Profile>, IEnumerable<ProfileVM>>(_profileService.Get(filterNew, orderByNew, includeProperties));
        }

        public IEnumerable<ProfileVM> GetAll()
        {
            return _mapper.Map<IEnumerable<Profile>, IEnumerable<ProfileVM>>(_profileService.GetAll());
        }

        public ProfileVM GetById(int id)
        {
            return _mapper.Map<Profile, ProfileVM>(_profileService.GetById(id));
        }

        public List<string> Insert(ProfileVM obj)
        {
            List<ProfilePermission> accessInserted = new List<ProfilePermission>();
            try
            {
                Profile profile = _mapper.Map<ProfileVM, Profile>(obj);
                profile.UserList = null;
                profile.ProfilePermissions = null;
                profile.Active = ((int)GenericStatusEnum.Active).ToString();
                _errors = _profileService.Validate(profile);
                if (_errors?.Count == 0)
                {
                    BeginTransaction();
                    _profileService.Insert(profile);
                    SaveChanges();
                    if (_errors?.Count() == 0)
                    {
                        if (obj.SelectedPermissionIdList != null)
                        {
                            foreach (var item in obj.SelectedPermissionIdList)
                            {
                                ProfilePermission newAccess = new ProfilePermission()
                                {
                                    PermissionId = item,
                                    ProfileId = profile.Id
                                };
                                _profilePermissionRepository.Insert(newAccess);
                                accessInserted.Add(newAccess);
                            }
                        }
                        SaveChanges();
                        Commit();
                    }
                }
            }
            catch (Exception e)
            {
                _errors.Add("Erro: " + e.Message.ToString() + "\nInner Exception: " + e.InnerException.Message.ToString());
                Rollback();
            }
            return _errors;
        }

        public List<string> Update(ProfileVM obj)
        {
            List<ProfilePermission> accessInserted = new List<ProfilePermission>();
            try
            {
                Profile profile = _mapper.Map<ProfileVM, Profile>(obj);
                _errors = _profileService.Validate(profile);
                if (_errors?.Count == 0)
                {
                    BeginTransaction();
                    _profileService.Update(profile);
                    if (_errors?.Count() == 0)
                    {
                        var permissionIds = _profileService.GetPermissions(profile.Id).Select(p => p.Id).ToList();
                        foreach (var item in permissionIds)
                        {
                            ProfilePermission deleteAccess = new ProfilePermission() { PermissionId = item, ProfileId = profile.Id };
                            if (obj.SelectedPermissionIdList != null)
                            {
                                if (!obj.SelectedPermissionIdList.Contains(item))
                                {
                                    _profilePermissionRepository.Delete(deleteAccess);
                                }
                            }
                            else
                            {
                                _profilePermissionRepository.Delete(deleteAccess);
                            }
                        }
                        if (obj.SelectedPermissionIdList != null)
                        {
                            foreach (var item in obj.SelectedPermissionIdList)
                            {
                                if (!permissionIds.Contains(item))
                                {
                                    ProfilePermission newAccess = new ProfilePermission() { PermissionId = item, ProfileId = profile.Id };
                                    _profilePermissionRepository.Insert(newAccess);
                                    accessInserted.Add(newAccess);
                                }
                            }
                        }
                        SaveChanges();
                        Commit();
                    }
                }
            }
            catch (Exception e)
            {
                _errors.Add("Erro: " + e.Message.ToString() + "\nInner Exception: " + e.InnerException.Message.ToString());
                Rollback();
            }
            return _errors;
        }

        public List<PermissionVM> GetPermissions(int idProfile)
        {
            Expression<Func<Permission, bool>> filter = (Permission p) => p.Id == idProfile;
            var PermissionsMapped = _mapper.Map<IEnumerable<Permission>, IEnumerable<PermissionVM>>(_profileService.GetPermissions(idProfile));
            return PermissionsMapped.ToList();
        }
    }
}
