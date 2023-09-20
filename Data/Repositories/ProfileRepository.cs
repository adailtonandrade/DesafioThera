using System.Collections.Generic;
using Data.Context;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Dapper;
using System.Linq;

namespace Data.Repositories
{
    public class ProfileRepository : GenericRepository<Profile>, IProfileRepository
    {
        private ModelContext _context;

        public ProfileRepository(ModelContext context) : base(context)
        {
            _context = context;
        }

        public List<Permission> GetPermissions(int idProfile)
        {
            var connection = _context.Database.Connection;
            //var transaction = _context.Database.CurrentTransaction;
            var sql = string.Format(@"SELECT P.CLAIMTYPE AS ClaimType, P.CLAIMVALUE AS ClaimValue, P.ID AS Id FROM PERMISSION P
                    INNER JOIN PROFILEPERMISSION A ON A.PERMISSIONID = P.ID
                    WHERE A.PROFILEID = {0}", idProfile);
            List<Permission> list = connection.Query<Permission>(sql).ToList();
            return list;
        }
    }
}
