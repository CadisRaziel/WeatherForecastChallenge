using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WeatherForecastChallenge.Core.Entities;

namespace WeatherForecastChallenge.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
       {

       }
        public DbSet<FavoriteCity> FavoriteCities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
                       
            builder.Entity<FavoriteCity>()
                .HasOne(fc => fc.User)
                .WithMany()  
                .HasForeignKey(fc => fc.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }    
}
