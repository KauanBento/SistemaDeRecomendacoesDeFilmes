using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using MovieApp.Models;
using MovieApp.Services;

namespace MovieApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieService _service;

        public MovieController(MovieService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetMovies()
        {
            try
            {
                return Ok(_service.GetAllMovies());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao buscar filmes.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Movie> GetMovie(int id)
        {
            try
            {
                return Ok(_service.GetMovieById(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao buscar o filme.", error = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult<Movie> CreateMovie(Movie movie)
        {
            try
            {
                _service.AddMovie(movie);
                return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao adicionar o filme.", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMovie(int id, Movie movie)
        {
            if (id != movie.Id)
                return BadRequest(new { message = "O ID do filme não corresponde ao ID da URL." });

            try
            {
                _service.UpdateMovie(movie);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao atualizar o filme.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id)
        {
            try
            {
                _service.DeleteMovie(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao excluir o filme.", error = ex.Message });
            }
        }
    }
}
