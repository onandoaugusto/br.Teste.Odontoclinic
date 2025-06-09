using Remotion.Linq.Parsing;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

//Configuração do Redis
var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
if (string.IsNullOrEmpty(redisConnectionString))
{
    throw new ApplicationException("ConnectionString da Redis está vazia no arquivo de configurações.");
}

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(redisConnectionString!)
);

#region NHibernate
//Configuração do NHibernate
var connectionString = builder.Configuration.GetConnectionString("dbClienteCrud");
if (string.IsNullOrEmpty(connectionString))
{ 
    throw new ApplicationException("ConnectionString do banco está vazia no arquivo de configurações.");
}

builder.Services.AddSingleton(provider =>
    ClienteCrud.Infra.SessionFactory.GetSessionFactory(connectionString, redisConnectionString)
);

builder.Services.AddScoped(provider =>
    provider.GetRequiredService<NHibernate.ISessionFactory>().OpenSession()
);

builder.Services.AddScoped<ClienteCrud.Infra.Repository.ClienteRepository>();
builder.Services.AddScoped<ClienteCrud.Infra.Repository.TelefoneRepository>();
#endregion

//Configuração do CORS
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
    )
);

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors();

//Pipeline de requisições http
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();

//Configuração para SPA
app.MapFallbackToFile("index.html");

app.MapControllerRoute(
    name: "default",
    pattern: "/api/{controller=cliente}/{action=index}/{id?}"
);

app.Run();
