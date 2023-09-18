using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface ITalentService : IGenericService<Talent>
    {
        bool VerifyTalentExists(Talent talent);

    }
}
