using Application.ViewModels;

namespace Application.Interfaces
{
    public interface ITalentAppService : IGenericAppService<TalentVM>
    {
        TalentDetailsVM GetDetailsById(int id);
        TalentResumeVM GetResumeByTalentId(int id);
    }
}
