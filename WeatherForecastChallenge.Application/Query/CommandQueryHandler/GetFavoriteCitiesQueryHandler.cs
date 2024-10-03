using MediatR;
using Microsoft.EntityFrameworkCore;
using WeatherForecastChallenge.Application.Query.CommandQuery;
using WeatherForecastChallenge.Core.Entities;
using WeatherForecastChallenge.Infrastructure.Data;

namespace WeatherForecastChallenge.Application.Query.CommandQueryHandler
{
    public class GetFavoriteCitiesQueryHandler : IRequestHandler<GetFavoriteCitiesQuery, List<FavoriteCity>>
    {
        private readonly ApplicationDbContext _context;

        public GetFavoriteCitiesQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FavoriteCity>> Handle(GetFavoriteCitiesQuery request, CancellationToken cancellationToken)
        {
            var favoriteCities = await _context.FavoriteCities
                .Where(fc => fc.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            return favoriteCities;
        }
    }
}
