using Application.DTO;
using Swashbuckle.AspNetCore.Filters;

namespace API.SwaggerExamples
{
    public class UpdateActorResponseExample : IExamplesProvider<ActorDTO>
    {
        public ActorDTO GetExamples()
        {
            return new ActorDTO
            {
                Id = Guid.Parse("12345678-1234-1234-1234-1234567890ab"),
                Name = "John Updated",
                Details = "Updated details here.",
                Type = "Writer",
                Rank = 2,
                Source = "IMDB"
            };
        }
    }
}
