using Application.ViewModels;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Service
{
    public class ProfileService : GenericService<Profile>, IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private List<string> _errors = new List<string>();

        public ProfileService(IProfileRepository profileRepository) : base(profileRepository)
        {
            _profileRepository = profileRepository;
        }

        private bool VerifyProfileExistsByName(Profile profile)
        {
            Expression<Func<Profile, bool>> filter =
                (Profile p) => p.Name.Trim().ToUpper() == profile.Name.Trim().ToUpper() && p.Id != profile.Id;
            var result = this.Get(filter, null, "");
            return (result?.Count() > 0) ? true : false;
        }

        private bool VerifyProfileCanBeChanged(Profile profile)
        {
            return (profile.Active.Equals(((int)GenericStatusEnum.Active).ToString())) ? true : false;
        }

        public bool VerifyProfileExists(Profile profile)
        {
            return VerifyProfileExistsByName(profile);
        }

        public List<Permission> GetPermissions(int idProfile)
        {
            return _profileRepository.GetPermissions(idProfile);
        }

        public new List<string> Insert(Profile profile)
        {
            _errors = Validate(profile);
            if (_errors?.Count == 0)
                this._profileRepository.Insert(profile);
            return _errors;
        }

        public new List<string> Update(Profile profile)
        {
            _errors = Validate(profile);
            if (_errors?.Count == 0)
                _profileRepository.Update(profile);
            return _errors;
        }

        public override List<string> Validate(Profile variable)
        {
            if (variable is Profile profile)
            {
                if (!VerifyProfileCanBeChanged(profile))
                    _errors.Add("O Perfil não pode ser alterado");
                if (!IsUniqueField(nameof(profile.Name), profile.Name, profile.Id))
                    _errors.Add(string.Format("O valor {0}  já foi cadastrado para o campo: {1}.", profile.Name, PropertyDescription.GetAttributeDisplayName(typeof(ProfileVM).GetProperty(nameof(profile.Name)))));
            }
            return _errors;
        }
    }
}
