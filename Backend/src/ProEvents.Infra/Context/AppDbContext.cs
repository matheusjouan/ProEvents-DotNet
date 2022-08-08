using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProEvents.Domain.Identity;
using ProEvents.Domain.Model;
using ProEvents.Infra.EntitiesConfiguration;

namespace ProEvents.Infra.Context
{
    public class AppDbContext : IdentityDbContext<User, Role, int,
        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {

        // Necessário para EF Core 6
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     if (!optionsBuilder.IsConfigured)
        //     {
        //         var mysqlConnectionStr = ConnectionStringManager.GetConnectionString();

        //         optionsBuilder.UseMySql(mysqlConnectionStr,
        //             ServerVersion.AutoDetect(mysqlConnectionStr));
        //     }
        // }

        public DbSet<Event> Events { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<SocialNetwork> SocialNetworks { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<EventSpeaker> EventsSpeakers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Necessário para configurar as Entidades do Identity
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new EventConfiguration());
            builder.ApplyConfiguration(new BatchConfiguration());
            builder.ApplyConfiguration(new EventSpeakerConfiguration());
            builder.ApplyConfiguration(new SocialNetworkConfiguration());
            builder.ApplyConfiguration(new SpeakerConfiguration());

            // Aplicando Configuração das Entidades do Identity
            builder.ApplyConfiguration(new UserRoleConfiguration());
        }
    }
}