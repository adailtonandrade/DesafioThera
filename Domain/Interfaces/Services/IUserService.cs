using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IUserService : IGenericService<User>
    {
        bool VerifyUserExists(User user);
    }
}
