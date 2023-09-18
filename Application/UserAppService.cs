using Application.Interfaces;
using Domain.Interfaces.Data;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.ViewModels;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Enum;
using AutoMapper;

namespace Application
{
    public class UserAppService : GenericAppService, IUserAppService
    {
        private readonly IUserService _userService;
        List<string> _errors = new List<string>();
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UserAppService(IUserService userService, IUnitOfWork uow, IMapper mapper) : base(uow)
        {
            _userService = userService;
            _uow = uow;
            _mapper = mapper;
        }

        public IEnumerable<UserVM> GetAll()
        {
            return _mapper.Map<IEnumerable<User>, IEnumerable<UserVM>>(_userService.GetAll()).OrderBy(q => q.Name);
        }

        public UserVM GetById(int id)
        {
            return _mapper.Map<User, UserVM>(_userService.GetById(id));
        }

        public IEnumerable<UserVM> Get(Expression<Func<UserVM, bool>> filter = null, Expression<Func<IQueryable<UserVM>, IOrderedQueryable<UserVM>>> orderBy = null, string includeProperties = "")
        {
            var filterNew = filter != null ? _mapper.Map<Expression<Func<UserVM, bool>>, Expression<Func<User, bool>>>(filter) : null;
            var orderByNew = orderBy != null ? _mapper.Map<Expression<Func<IQueryable<UserVM>, IOrderedQueryable<UserVM>>>
                , Expression<Func<IQueryable<User>, IOrderedQueryable<User>>>>(orderBy) : null;
            return _mapper.Map<IEnumerable<User>, IEnumerable<UserVM>>(_userService.Get(filterNew, orderByNew, includeProperties));
        }

        public List<string> Insert(UserVM obj)
        {
            throw new NotImplementedException();
        }

        public List<string> Update(UserVM obj)
        {
            try
            {
                User user = _mapper.Map<UserVM, User>(obj);
                _errors.AddRange(_userService.Validate(user));
                if (_errors?.Count() == 0)
                {
                    BeginTransaction();
                    var userToBeChanged = _userService.GetById(obj.Id);
                    userToBeChanged.ProfileId = user.ProfileId;
                    userToBeChanged.Email = user.Email;
                    userToBeChanged.Cpf = user.Cpf;
                    userToBeChanged.Name = user.Name;
                    userToBeChanged.Username = user.Email;
                    userToBeChanged.NickName = user.NickName;
                    _userService.Update(user);
                    SaveChanges();
                    Commit();
                }
            }
            catch (Exception e)
            {
                _errors.Add("Erro: " + e.Message.ToString() + "\nInner Exception: " + e.InnerException.Message.ToString());
                Rollback();
            }
            return _errors;
        }

        public List<string> Delete(int id)
        {
            User user = _userService.GetById(id);
            try
            {
                if (user.Active == GenericStatusEnum.Inactive.ToString("D"))
                    _errors = Reactivate(user);
                else
                    _errors = Deactivate(user);
            }
            catch (Exception e)
            {
                if (user != null)
                    _errors.Add("Erro: " + e.Message.ToString() + "\nInner Exception: " + e.InnerException.Message.ToString());
                else
                    _errors.Add("Usuário não encontrado");
            }
            return _errors;
        }

        private List<string> Deactivate(User user)
        {
            try
            {
                BeginTransaction();
                user.Active = GenericStatusEnum.Inactive.ToString("D");
                _userService.Update(user);
                SaveChanges();
                Commit();
            }
            catch (Exception e)
            {
                _errors.Add(String.Format("Falha ao desativar o usuário {0}", user.NickName));
                Rollback();
            }
            return _errors;
        }

        private List<string> Reactivate(User user)
        {
            try
            {
                BeginTransaction();
                user.Active = GenericStatusEnum.Active.ToString("D");
                _userService.Update(user);
                SaveChanges();
                Commit();
            }
            catch (Exception e)
            {
                _errors.Add(String.Format("Falha ao reativar o usuário {0}", user.NickName));
                Rollback();
            }
            return _errors;
        }

    }
}
