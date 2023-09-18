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
            var transaction = _context.Database.CurrentTransaction;
            var sql = string.Format(@"SELECT P.CLAIMTYPE AS ClaimType, P.CLAIMVALUE AS ClaimValue, P.ID AS Id FROM PERMISSION P
                    INNER JOIN ACCESS A ON A.IDPERMISSION = P.ID
                    WHERE A.IDPROFILE = {0}", idProfile);
            List<Permission> list = connection.Query<Permission>(sql, null, transaction != null ? transaction.UnderlyingTransaction : null).ToList();
            return list;
        }
    }
}
