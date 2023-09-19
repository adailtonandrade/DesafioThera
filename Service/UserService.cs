using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Service
{
    public class UserService : GenericService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;

        private List<string> _errors = new List<string>();

        public UserService(IUserRepository userRepository) : base(userRepository)
        {
            _userRepository = userRepository;
        }

        public bool VerifyUserExists(User user)
        {
            if (VerifyUserExistsByCpf(user))
            {
                _errors.Add("O CPF já esta em uso");
            }

            if (VerifyUserExistsByEmail(user))
            {
                _errors.Add("O E-mail já esta em uso");
            }

            return _errors.Count() != 0;
        }

        private bool VerifyUserCanBeChanged(User user)
        {
            Expression<Func<User, bool>> filter = (User p) => p.Active.Equals("1") && p.Id == user.Id;
            var result = this.Get(filter, null, "");
            return (result?.Count() > 0);
        }

        private bool VerifyUserExistsByEmail(User user)
        {
            Expression<Func<User, bool>> filter = (User p) => p.Email.ToLower().Equals(user.Email.ToLower()) && p.Id != user.Id;
            var result = this.Get(filter, null, "");
            return (result?.Count() > 0);
        }

        private bool VerifyUserExistsByCpf(User user)
        {
            Expression<Func<User, bool>> filter = (User p) => p.Cpf.ToLower().Equals(user.Cpf.ToLower()) && p.Id != user.Id;
            var result = this.Get(filter, null, "");
            return (result?.Count() > 0);
        }

        public override List<string> Validate(User user)
        {
            if (!VerifyUserCanBeChanged(user))
            {
                _errors.Add("Usuário não pode ser alterado");
                return _errors;
            }
            VerifyUserExists(user);
            return _errors;
        }
    }
}
