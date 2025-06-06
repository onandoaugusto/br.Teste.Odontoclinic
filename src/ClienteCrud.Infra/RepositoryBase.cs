using System.Text.Json;
using NHibernate;
using StackExchange.Redis;

namespace ClienteCrud.Infra.Repository
{
    public abstract class RepositoryBase<T> where T : class
    {
        protected readonly ISession _session;
        protected readonly IDatabase _redis;
        private readonly string _entityName;

        protected RepositoryBase(ISession session, IConnectionMultiplexer redisConnection, string entityName)
        {
            _session = session;
            _redis = redisConnection.GetDatabase();
            _entityName = entityName;
        }

        protected virtual string GetCacheKey(object id) => $"{_entityName}: {id}";
        protected virtual string GetAllCacheKey() => $"{_entityName}:all";

        public virtual void Save(T entity)
        {
            using var transaction = _session.BeginTransaction();
            try
            {
                _session.SaveOrUpdate(entity);
                transaction.Commit();

                //Atualiza cache individual
                var entityId = GetEntityId(entity);
                UpdateCache(entity, entityId);

                //Invalida cache da lista completa
                _redis.KeyDelete(GetAllCacheKey());
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        protected abstract object GetEntityId(T entity);

        public virtual T GetById(object id)
        {
            var cacheKey = GetCacheKey(id);
            var cachedEntity = _redis.StringGet(cacheKey);

            if (cachedEntity.HasValue)
            {
                return JsonSerializer.Deserialize<T>(cachedEntity);
            }

            var entity = _session.Get<T>(id);

            if (entity != null)
            {
                _redis.StringSet(
                    cacheKey,
                    JsonSerializer.Serialize(entity),
                    redisTimeout()
                );
            }

            return entity;
        }

        public virtual IList<T> GetAll()
        {
            var cacheKey = GetAllCacheKey();
            var CachedEntities = _redis.StringGet(cacheKey);

            if (CachedEntities.HasValue)
            {
                return JsonSerializer.Deserialize<List<T>>(CachedEntities);
            }

            var entities = _session.Query<T>().ToList();

            _redis.StringSet(
                cacheKey,
                JsonSerializer.Serialize(entities),
                redisTimeout(15)
            );

            return entities;
        }

        public virtual void Delete(object id)
        {
            using var transaction = _session.BeginTransaction();
            try
            {
                var entity = _session.Get<T>(id);
                if (entity != null)
                {
                    _session.Delete(entity);
                    transaction.Commit();

                    //Limpeza dos caches
                    _redis.KeyDelete(GetCacheKey(id));
                    _redis.KeyDelete(GetAllCacheKey());
                }
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        protected virtual void UpdateCache(T entity, object entityId)
        {
            _redis.StringSet(
                GetCacheKey(entityId),
                JsonSerializer.Serialize(entity),
                redisTimeout()
            );
        }

        protected virtual TimeSpan redisTimeout(int min = 30) => TimeSpan.FromMinutes(min);
    }
}