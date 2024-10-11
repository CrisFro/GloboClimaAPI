using Amazon.DynamoDBv2.DataModel;
using GloboClimaAPI.Models;

namespace GloboClimaAPI.Services
{
    public class FavoritesService
    {
        private readonly IDynamoDBContext _dbContext;

        public FavoritesService(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Busca os favoritos do usuário
        public async Task<List<Favorite>> GetFavoritesAsync(string userId)
        {
            return await _dbContext.QueryAsync<Favorite>(userId).GetRemainingAsync();
        }

        // Adiciona um favorito ao banco de dados
        public async Task AddFavoriteAsync(Favorite favorite)
        {
            await _dbContext.SaveAsync(favorite); // Salva no DynamoDB
        }

        // Remove um favorito
        public async Task RemoveFavoriteAsync(Favorite favorite)
        {
            await _dbContext.DeleteAsync(favorite);
        }
    }

}
