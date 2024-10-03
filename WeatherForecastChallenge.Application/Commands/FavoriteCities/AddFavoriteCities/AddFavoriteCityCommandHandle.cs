using MediatR;
using WeatherForecastChallenge.Application.Commands.FavoriteCities.AddFavoriteCities;
using WeatherForecastChallenge.Core.Entities;
using WeatherForecastChallenge.Core.Response;
using WeatherForecastChallenge.Infrastructure.Data;

public class AddFavoriteCityCommandHandler : IRequestHandler<AddFavoriteCityCommand, FavoriteCityResponse>
{
    private readonly ApplicationDbContext _context;

    public AddFavoriteCityCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FavoriteCityResponse> Handle(AddFavoriteCityCommand request, CancellationToken cancellationToken)
    {
        var favoriteCity = new FavoriteCity
        {
            CityName = request.CityName,
            UserId = request.UserId
        };

        _context.FavoriteCities.Add(favoriteCity);
        await _context.SaveChangesAsync(cancellationToken);

        return new FavoriteCityResponse
        {
            CityName = request.CityName,
            Message = "Cidade adicionada com sucesso!"
        };
    }
}
