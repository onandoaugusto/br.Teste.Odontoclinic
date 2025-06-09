using System.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Caches.StackExchangeRedis;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;

namespace ClienteCrud.Infra;

public static class SessionFactory
{
    private static ISessionFactory _sessionFactory;
    private static readonly object _lock = new object();

    public static ISessionFactory GetSessionFactory(string connectionString, string redisConnString)
    {
        if (_sessionFactory == null)
        {
            lock (_lock)
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = BuildSessionFactory(connectionString, redisConnString);
                }
            }
        }

        return _sessionFactory;
    }

    private static ISessionFactory BuildSessionFactory(string connectionString, string redisConnString)
    {
        try
        {
            return Fluently
                .Configure()
                .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(connectionString)
                    .Driver<MicrosoftDataSqlClientDriver>()
                    .ShowSql()
                )
                .Mappings(m => m.FluentMappings
                    .AddFromAssembly(typeof(ClienteMap).Assembly)
                    .AddFromAssembly(typeof(TelefoneMap).Assembly)
                    .Conventions.Add(FluentNHibernate.Conventions.Helpers.DefaultLazy.Never())
                )
                .Cache(c => c
                    .UseQueryCache()
                    .UseSecondLevelCache()
                    .ProviderClass<RedisCacheProvider>()
                )
                .ExposeConfiguration(cfg => {
                    // Configurações do Redis
                    cfg.SetProperty("cache.provider_configuration", redisConnString);
                    
                    new SchemaUpdate(cfg).Execute(false, true);
                    cfg.CurrentSessionContext<ThreadStaticSessionContext>();
                })
                .BuildSessionFactory();
        }
        catch (Exception ex)
        {
            throw new Exception("Falha ao criar SessionFactory", ex);
        }
    }

    public static ISession OpenSession()
    {
        return GetSessionFactory(ConfigurationManager.ConnectionStrings["dbClienteCrud"].ConnectionString, ConfigurationManager.ConnectionStrings["Redis"].ConnectionString).OpenSession();
    }
}
