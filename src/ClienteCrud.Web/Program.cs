var builder = WebApplication.CreateBuilder(args);

//Configuração do Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "ClienteCrud_";
});

#region NHibernate
//Configuração do NHibernate
builder.Services.AddSingleton(provider =>
    ClienteCrud.Infra.SessionFactory.GetSessionFactory(builder.Configuration.GetConnectionString("dbClienteCrud"))
);

builder.Services.AddScoped(provider =>
    provider.GetRequiredService<NHibernate.ISessionFactory>().OpenSession()
);

builder.Services.AddScoped<ClienteCrud.Infra.Repository.ClienteRepository>();
builder.Services.AddScoped<ClienteCrud.Infra.Repository.TelefoneRepository>();
#endregion

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
// app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=cliente}/{action=index}/{id?}"
);

//Tratamento para CORS
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();
