using NHibernate;
using ClienteCrud.Core.Model;

namespace ClienteCrud.Infra.Repository
{
    public class ClienteRepository : IDisposable
    {
        private readonly ISession _session;
        public ClienteRepository(ISession session) => _session = session;

        public void Save(Cliente cliente)
        {
            using var transaction = _session.BeginTransaction();
            try
            {
                _session.SaveOrUpdate(cliente);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public Cliente getById(int id)
        {
            return _session.Get<Cliente>(id);
        }

        public IList<Cliente> GetAll()
        {
            return _session.Query<Cliente>().ToList();
        }

        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}