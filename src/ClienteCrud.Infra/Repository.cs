using NHibernate;
using ClienteCrud.Core.Model;
using StackExchange.Redis;
using System.Text.Json;

namespace ClienteCrud.Infra.Repository
{
    public class ClienteRepository : RepositoryBase<Cliente>
    {
        public ClienteRepository(ISession session, IConnectionMultiplexer redisConnection) : base(session, redisConnection, "cliente") { }

        //Cache específico para telefones do cliente
        public override void Save(Cliente cliente)
        {
            base.Save(cliente);
            _redis.KeyDelete($"cliente:{cliente.Id}:telefone");
        }

        protected override object GetEntityId(Cliente entity) => entity.Id;

        public IList<Telefone> GetTelefoneByClienteId(int clienteId)
        {
            var cacheKey = $"cliente:{clienteId}:telefone";
            var cachedTelefones = _redis.StringGet(cacheKey);

            if (cachedTelefones.HasValue)
            {
                return JsonSerializer.Deserialize<List<Telefone>>(cachedTelefones);
            }

            var telefones = _session.Query<Telefone>()
                .Where(w => w.cliente.Id == clienteId)
                .ToList();

            _redis.StringSet(
                cacheKey,
                JsonSerializer.Serialize(telefones),
                redisTimeout()
            );

            return telefones;
        }
    }

    public class TelefoneRepository : RepositoryBase<Telefone>
    {
        public TelefoneRepository(ISession session, IConnectionMultiplexer redisConnection) : base(session, redisConnection, "telefone") { }

        protected override object GetEntityId(Telefone entity) => entity.Id;

        public override void Save(Telefone telefone)
        {
            if (telefone.Ativo)
            {
                var telefonesAtivos = _session.Query<Telefone>()
                    .Where(t =>
                            t.cliente.Id == telefone.cliente.Id
                        && t.Ativo
                        && t.Id != telefone.Id
                    ).ToList();

                telefonesAtivos.ForEach(tel =>
                {
                    tel.Ativo = false;
                    _session.Update(tel);
                });
            }

            base.Save(telefone);
            _redis.KeyDelete($"cliente:{telefone.cliente.Id}:telefone");
        }

        public override void Delete(object id)
        {
            var telefone = GetById(id);
            if (telefone != null)
            {
                var clienteId = telefone.cliente.Id;
                base.Delete(id);

                //Invalida cache de telefones do cliente
                _redis.KeyDelete($"cliente:{clienteId}:telefone");
            }
        }
    }
}