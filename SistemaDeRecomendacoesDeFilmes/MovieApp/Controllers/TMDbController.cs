using Microsoft.AspNetCore.Mvc;
using MovieApp.Services;

namespace MovieApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TMDbController : ControllerBase
    {
        private readonly TMDbService _service;

        public TMDbController(TMDbService service)
        {
            _service = service;
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularMovies()
        {
            try
            {
                var movies = await _service.GetPopularMoviesAsync();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao buscar filmes populares.", error = ex.Message });
            }
        }
    }
}
