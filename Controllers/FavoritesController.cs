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
        private readonly ILogger<FavoritesController> _logger;

        public FavoritesController(FavoritesService favoritesService, ILogger<FavoritesController> logger)
        {
            _favoritesService = favoritesService;
            _logger = logger;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToFavorites([FromBody] Favorite favorite)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            favorite.Id = Guid.NewGuid().ToString();
            favorite.UserId = userId;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _favoritesService.AddFavoriteAsync(favorite);
                return Ok(new { message = "Favorito adicionado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao adicionar favorito: {ex.Message}");
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListFavorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            var favorites = await _favoritesService.GetFavoritesAsync(userId);
            return Ok(favorites);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFavorite([FromBody] Favorite favorite)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            if (favorite.UserId != userId)
            {
                return Unauthorized("Você só pode remover seus próprios favoritos.");
            }

            await _favoritesService.RemoveFavoriteAsync(favorite);
            return Ok(new { message = "Favorito removido com sucesso!" });
        }
    }
}
