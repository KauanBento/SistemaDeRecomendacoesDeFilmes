using System.ComponentModel.DataAnnotations;

namespace MovieApp.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [MaxLength(255, ErrorMessage = "O título não pode ter mais que 255 caracteres")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000, ErrorMessage = "A descrição é muito longa")]
        public string? Overview { get; set; }

        [Url(ErrorMessage = "O caminho do pôster deve ser uma URL válida")]
        public string? PosterPath { get; set; }

        [DataType(DataType.Date, ErrorMessage = "A data de lançamento deve estar em formato de data")]
        public string? ReleaseDate { get; set; }
    }
}
