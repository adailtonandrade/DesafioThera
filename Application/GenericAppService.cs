using Domain.Interfaces.Data;


namespace Application
{
    public class GenericAppService
    {
        private readonly IUnitOfWork _uow;

        public GenericAppService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void BeginTransaction()
        {
            _uow.BeginTransaction();
        }
        public void Commit(bool dispose = true)
        {
            _uow.Commit(dispose);
        }

        public void Rollback()
        {
            _uow.Rollback();
        }

        public void SaveChanges()
        {
            _uow.SaveChanges();
        }
    }
}
