using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace WeatherForecastChallenge.Core.Entities
{
    public class FavoriteCity
    {
        [Key]
        public int Id { get; set; } 
        public string CityName { get; set; } 
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual IdentityUser User { get; set; } 
    }
}
