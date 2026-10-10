var constructor = WebApplication.CreateBuilder(args);
constructor.Services.AddReverseProxy()
    .LoadFromConfig(constructor.Configuration.GetSection("ProxyReverso"));

var aplicacion = constructor.Build();
aplicacion.MapReverseProxy();
aplicacion.Run();
