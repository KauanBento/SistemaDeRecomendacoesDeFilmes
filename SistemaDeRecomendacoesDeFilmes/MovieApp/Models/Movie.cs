using System.ComponentModel.DataAnnotations;

namespace MovieApp.Models
{
    public class Movie
    {
        public int Id { get; init; }

        [Required(ErrorMessage = "O título do filme é obrigatório.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "O gênero do filme é obrigatório.")]
        [StringLength(50, ErrorMessage = "O gênero não pode ter mais de 50 caracteres.")]
        public string? Genre { get; set; }

        [Required(ErrorMessage = "A nota do filme é obrigatória.")]
        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public double Rating { get; set; }
        public string? Comments { get; set; }
    }
}
