using Microsoft.Extensions.Configuration;
using MovieApp.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
using MovieApp.DTOs;

namespace MovieApp.Services
{
    public class TMDbService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _apiKey;
        private readonly string? _baseUrl;

        public TMDbService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TMDb:ApiKey"];
            _baseUrl = configuration["TMDb:BaseUrl"];
        }

        public async Task<List<TMDbMovie>> GetPopularMoviesAsync()
        {
            var url = $"{_baseUrl}/movie/popular?api_key={_apiKey}&language=pt-BR&page=1";

            var response = await _httpClient.GetFromJsonAsync<TMDbResponse>(url);
            return response?.Results ?? new List<TMDbMovie>();
        }

        private class TMDbResponse
        {
            public List<TMDbMovie> Results { get; set; }
        }

        public async Task<TmdbMovieDto> GetMovieByIdAsync(int movieId)
        {
            var url = $"https://api.themoviedb.org/3/movie/{movieId}?api_key={_apiKey}&language=pt-BR";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            var tmdbMovie = JsonSerializer.Deserialize<TmdbMovieDto>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return tmdbMovie;
        }

    }
}
