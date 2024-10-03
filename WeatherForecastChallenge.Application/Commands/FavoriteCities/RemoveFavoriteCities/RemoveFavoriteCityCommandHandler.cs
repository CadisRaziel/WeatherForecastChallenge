using MediatR;
using Microsoft.EntityFrameworkCore;
using WeatherForecastChallenge.Core.Response;
using WeatherForecastChallenge.Infrastructure.Data;

namespace WeatherForecastChallenge.Application.Commands.FavoriteCities.RemoveFavoriteCities
{
    public class RemoveFavoriteCityCommandHandler : IRequestHandler<RemoveFavoriteCityCommand, FavoriteCityResponse>
    {
        private readonly ApplicationDbContext _context;

        public RemoveFavoriteCityCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FavoriteCityResponse> Handle(RemoveFavoriteCityCommand request, CancellationToken cancellationToken)
        {
            var favoriteCity = await _context.FavoriteCities
                .FirstOrDefaultAsync(fc => fc.CityName == request.CityName && fc.UserId == request.UserId, cancellationToken);

            if (favoriteCity == null)
            {
                return new FavoriteCityResponse
                {
                    CityName = request.CityName,
                    Message = "Cidade não encontrada nos favoritos."
                };
            }

            _context.FavoriteCities.Remove(favoriteCity);
            await _context.SaveChangesAsync(cancellationToken);

            return new FavoriteCityResponse
            {
                CityName = request.CityName,
                Message = "Cidade removida com sucesso!"
            };
        }
    }
}
