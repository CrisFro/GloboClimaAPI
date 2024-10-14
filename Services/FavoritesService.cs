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

        public async Task<List<Favorite>> GetFavoritesAsync(string userId)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition(nameof(Favorite.UserId), Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, userId)
            };

            var search = _dbContext.ScanAsync<Favorite>(conditions);
            return await search.GetNextSetAsync();
        }

        public async Task<Favorite> GetFavoriteByIdAsync(string favoriteId, string userId)
        {
            var favorite = await _dbContext.LoadAsync<Favorite>(favoriteId);
            return (favorite != null && favorite.UserId == userId) ? favorite : null;
        }

        public async Task AddFavoriteAsync(Favorite favorite)
        {
            await _dbContext.SaveAsync(favorite);
        }

        public async Task<bool> RemoveFavoriteAsync(string favoriteId, string userId)
        {
            var favorite = await GetFavoriteByIdAsync(favoriteId, userId);
            if (favorite != null)
            {
                await _dbContext.DeleteAsync(favorite);
                return true;
            }
            return false;
        }
    }

}
