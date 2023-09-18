using Application.Interfaces;
using Domain.Interfaces.Data;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.ViewModels;
using System.Linq.Expressions;
using Domain.Entities;
using AutoMapper;

namespace Application
{
    public class PermissionAppService : GenericAppService, IPermissionAppService
    {
        private readonly IGenericService<Permission> _permissionService;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PermissionAppService(IGenericService<Permission> permissionService, IUnitOfWork uow, IMapper mapper) : base(uow)
        {
            _permissionService = permissionService;
            _uow = uow;
            _mapper = mapper;
        }

        public List<string> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PermissionVM> Get(Expression<Func<PermissionVM, bool>> filter = null, Expression<Func<IQueryable<PermissionVM>, IOrderedQueryable<PermissionVM>>> orderBy = null, string includeProperties = "")
        {
            var filterNew = filter != null ? _mapper.Map<Expression<Func<PermissionVM, bool>>, Expression<Func<Permission, bool>>>(filter) : null;
            var orderByNew = orderBy != null ? _mapper.Map<Expression<Func<IQueryable<PermissionVM>, IOrderedQueryable<PermissionVM>>>
                , Expression<Func<IQueryable<Permission>, IOrderedQueryable<Permission>>>>(orderBy) : null;
            return _mapper.Map<IEnumerable<Permission>, IEnumerable<PermissionVM>>(_permissionService.Get(filterNew, orderByNew, includeProperties));
        }

        public IEnumerable<PermissionVM> GetAll()
        {
            return _mapper.Map<IEnumerable<Permission>, IEnumerable<PermissionVM>>(_permissionService.GetAll());
        }

        public PermissionVM GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<string> Insert(PermissionVM obj)
        {
            throw new NotImplementedException();
        }

        public List<string> Update(PermissionVM obj)
        {
            throw new NotImplementedException();
        }
    }
}
