
using System.ComponentModel.DataAnnotations;

namespace Application.DTO
{
    public class ActorCreateDTO
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public string Type { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Rank must be greater than 0")]
        public int Rank { get; set; }
        public string Source { get; set; }
    }
}
