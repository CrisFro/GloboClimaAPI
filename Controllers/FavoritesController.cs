using GloboClimaAPI.Models;
using GloboClimaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GloboClimaAPI.Controllers
{

    [Authorize]
    [Route("api/favorites")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly FavoritesService _favoritesService;

        public FavoritesController(FavoritesService favoritesService)
        {
            _favoritesService = favoritesService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Favorite> favorites = await _favoritesService.GetFavoritesAsync(userId);
            return Ok(favorites); 
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToFavorites([FromBody] FavoriteDto favoriteDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            var favorite = new Favorite
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Country = favoriteDto.Country,
                City = favoriteDto.City
            };

            if (string.IsNullOrEmpty(favorite.Country) && string.IsNullOrEmpty(favorite.City))
            {
                return BadRequest("Você deve fornecer uma cidade ou um país.");
            }

            // Verifica se o favorito já existe
            var existingFavorites = await _favoritesService.GetFavoritesAsync(userId);
            bool alreadyFavorited = existingFavorites.Any(f =>
                (f.City == favorite.City && favorite.City != null) ||
                (f.Country == favorite.Country && favorite.Country != null));

            if (alreadyFavorited)
            {
                return BadRequest("Você já adicionou este favorito.");
            }

            await _favoritesService.AddFavoriteAsync(favorite);
            return Ok(new { message = "Favorito adicionado com sucesso!" });
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFavorite([FromBody] FavoriteIdDto favoriteIdDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (await _favoritesService.RemoveFavoriteAsync(favoriteIdDto.Id, userId))
            {
                return Ok(new { message = "Favorito removido com sucesso!" });
            }

            return Unauthorized("Você só pode remover seus próprios favoritos.");
        }

        public class FavoriteIdDto
        {
            public string? Id { get; set; }
        }

        public class FavoriteDto
        {
            public string? Country { get; set; }
            public string? City { get; set; }
        }
    }
}
