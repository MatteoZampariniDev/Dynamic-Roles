using Duende.IdentityServer.EntityFramework.Options;
using DynamicRoles.ServerDemo.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DynamicRoles.ServerDemo.Infrastructure.Data;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser, ApplicationRole>
{
    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
