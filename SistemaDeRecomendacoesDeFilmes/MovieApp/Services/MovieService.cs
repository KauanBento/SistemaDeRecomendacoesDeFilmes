using System.Collections.Generic;
using MovieApp.Data;
using MovieApp.Models;

namespace MovieApp.Services
{
    public class MovieService
    {
        private readonly MovieRepository _repository;

        public MovieService(MovieRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Movie> GetAllMovies() => _repository.GetAll();

        public Movie GetMovieById(int id) => _repository.GetById(id);

        public void AddMovie(Movie movie) => _repository.Add(movie);

        public void UpdateMovie(Movie movie) => _repository.Update(movie);

        public void DeleteMovie(int id) => _repository.Delete(id);
    }
}
