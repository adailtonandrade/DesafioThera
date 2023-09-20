using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Service
{
    public class TalentService : GenericService<Talent>, ITalentService
    {
        private readonly ITalentRepository _talentRepository;

        private List<string> _errors = new List<string>();

        public TalentService(ITalentRepository talentRepository) : base(talentRepository)
        {
            _talentRepository = talentRepository;
        }

        public bool VerifyTalentExists(Talent talent)
        {
            if (VerifyTalentExistsByCpf(talent))
            {
                _errors.Add("O CPF já esta em uso para outro talento");
            }

            if (VerifyTalentExistsByEmail(talent))
            {
                _errors.Add("O E-mail já esta em uso para outro talento");
            }
            return _errors.Count() != 0;
        }

        private bool VerifyTalentCanBeChanged(Talent talent)
        {
            Expression<Func<Talent, bool>> filter = (Talent p) => p.Active.Equals("1") && p.Id == talent.Id;
            var result = this.Get(filter, null, "");
            return (result?.Count() > 0);
        }

        private bool VerifyTalentExistsByEmail(Talent talent)
        {
            Expression<Func<Talent, bool>> filter = (Talent p) => p.Email.ToLower().Equals(talent.Email.ToLower()) && p.Id != talent.Id;
            var result = this.Get(filter, null, "");
            return (result?.Count() > 0);
        }

        private bool VerifyTalentExistsByCpf(Talent talent)
        {
            Expression<Func<Talent, bool>> filter = (Talent p) => p.Cpf.ToLower().Equals(talent.Cpf.ToLower()) && p.Id != talent.Id;
            var result = this.Get(filter, null, "");
            return (result?.Count() > 0);
        }

        public override List<string> Validate(Talent talent)
        {
            if (!IsUniqueField(nameof(talent.Cpf), talent.Cpf, talent.Id))
                _errors.Add("O CPF já esta em uso para outro talento");
            if (!IsUniqueField(nameof(talent.Email), talent.Email, talent.Id))
                _errors.Add("O E-mail já esta em uso para outro talento");
            return _errors;
        }
    }
}
