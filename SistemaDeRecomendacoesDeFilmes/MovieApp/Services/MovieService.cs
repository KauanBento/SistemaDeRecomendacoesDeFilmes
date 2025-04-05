using System;
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

        public IEnumerable<Movie> GetAllMovies()
        {
            try
            {
                return _repository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar filmes.", ex);
            }
        }

        public Movie GetMovieById(int id)
        {
            try
            {
                var movie = _repository.GetById(id);
                if (movie == null)
                    throw new KeyNotFoundException($"Filme com ID {id} não encontrado.");

                return movie;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar filme com ID {id}.", ex);
            }
        }

        public void AddMovie(Movie movie)
        {
            if (movie == null || string.IsNullOrWhiteSpace(movie.Title))
                throw new ArgumentException("Filme inválido. O título é obrigatório.");

            try
            {
                _repository.Add(movie);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar o filme.", ex);
            }
        }

        public void UpdateMovie(Movie movie)
        {
            if (movie == null || string.IsNullOrWhiteSpace(movie.Title))
                throw new ArgumentException("Filme inválido. O título é obrigatório.");

            try
            {
                var existingMovie = _repository.GetById(movie.Id);
                if (existingMovie == null)
                    throw new KeyNotFoundException($"Filme com ID {movie.Id} não encontrado.");

                _repository.Update(movie);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar o filme com ID {movie.Id}.", ex);
            }
        }

        public void DeleteMovie(int id)
        {
            try
            {
                var existingMovie = _repository.GetById(id);
                if (existingMovie == null)
                    throw new KeyNotFoundException($"Filme com ID {id} não encontrado.");

                _repository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir o filme com ID {id}.", ex);
            }
        }
    }
}
