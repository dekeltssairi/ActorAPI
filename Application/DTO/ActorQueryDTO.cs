using System.ComponentModel.DataAnnotations;

namespace Application.DTO
{
    public class ActorQueryDTO
    {
        public string? Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Rank start must be greater than 0")]
        public int? RankStart { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Rank end must be greater than 0")]
        public int? RankEnd { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
        public int PageNumber { get; set; } = 1;  // Default to 1 if not specified

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10; 
    }

}
