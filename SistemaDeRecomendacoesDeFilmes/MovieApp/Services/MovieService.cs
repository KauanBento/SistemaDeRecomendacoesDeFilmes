using MovieApp.Models;
using MovieApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System;

namespace MovieApp.Services
{
    public class MovieService
    {
        private readonly MovieContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _tmdbApiKey = "1575db7d563dd229314f551f145b6ebb";

        public MovieService(MovieContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<List<Movie>> GetAllAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task<Movie> AddAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task<Movie> UpdateAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return false;

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }

        // 🔍 Buscar filmes do TMDb por nome
        public async Task<IEnumerable<Movie>> SearchMoviesFromTmdbAsync(string query)
        {
            var response = await _httpClient.GetAsync(
                $"https://api.themoviedb.org/3/search/movie?api_key={_tmdbApiKey}&query={Uri.EscapeDataString(query)}"
            );

            response.EnsureSuccessStatusCode();

            var jsonDoc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            var results = jsonDoc.RootElement.GetProperty("results");

            var movies = new List<Movie>();

            foreach (var item in results.EnumerateArray())
            {
                movies.Add(new Movie
                {
                    Title = item.GetProperty("title").GetString(),
                    Overview = item.GetProperty("overview").GetString(),
                    ReleaseDate = item.GetProperty("release_date").GetString()

                });
            }

            return movies;
        }

        // ➕ Adicionar filme ao banco de dados pelo ID do TMDb
        public async Task<Movie> AddMovieFromTmdbAsync(int tmdbId)
        {
            var response = await _httpClient.GetAsync(
                $"https://api.themoviedb.org/3/movie/{tmdbId}?api_key={_tmdbApiKey}"
            );

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            var movie = new Movie
            {
                Title = json.GetProperty("title").GetString(),
                Overview = json.GetProperty("overview").GetString(),
                PosterPath = json.TryGetProperty("poster_path", out var poster) && poster.ValueKind != JsonValueKind.Null
         ? poster.GetString()
         : null,
                ReleaseDate = json.GetProperty("release_date").GetString()
            };

            return await AddAsync(movie);
        }

        public async Task<Movie> AddMovieFromTmdbAsync(string tmdbTitle)
        {
            var url = $"https://api.themoviedb.org/3/search/movie?query={Uri.EscapeDataString(tmdbTitle)}&api_key={_tmdbApiKey}&language=pt-BR";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(content);
            var results = jsonDoc.RootElement.GetProperty("results");

            if (results.GetArrayLength() == 0)
                throw new Exception("Filme não encontrado no TMDB.");

            var firstResult = results[0];

            var movie = new Movie
            {
                Title = firstResult.GetProperty("title").GetString(),
                Overview = firstResult.GetProperty("overview").GetString(),
                PosterPath = firstResult.TryGetProperty("poster_path", out var poster) && poster.ValueKind != JsonValueKind.Null
                    ? poster.GetString()
                    : null,
                ReleaseDate = firstResult.GetProperty("release_date").GetString()
            };

            return await AddAsync(movie);
        }


    }
}
