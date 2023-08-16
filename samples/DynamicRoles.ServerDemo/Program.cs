using NSwag.Generation.Processors.Security;
using NSwag;
using DynamicRoles.ServerDemo.Infrastructure.Data;
using DynamicRoles.ServerDemo.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using DynamicRoles.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("DynamicRolesDb"));
builder.Services.AddScoped<ApplicationDbContextInitializer>();

builder.Services.AddDefaultIdentityWithDynamicRoles<ApplicationUser,ApplicationRole, ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddOpenApiDocument(configure =>
{
    configure.Title = "DynamicRoles API";
    configure.AddSecurity("JWT", Enumerable.Empty<string>(),
        new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Type into the textbox: Bearer {your JWT token}."
        });

    configure.OperationProcessors.Add(
        new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

var app = builder.Build();

// Initialise and seed the database
using (var scope = app.Services.CreateScope())
{
    try
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database initialization.");

        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseSwaggerUi3(configure =>
{
    configure.DocumentPath = "/api/v1/openapi.json";
});

app.UseReDoc(configure =>
{
    configure.Path = "/redoc";
    configure.DocumentPath = "/api/v1/openapi.json";
});

app.UseRouting();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
