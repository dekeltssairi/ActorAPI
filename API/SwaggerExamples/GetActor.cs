using Application.DTO;
using Swashbuckle.AspNetCore.Filters;

namespace API.SwaggerExamples
{
    public class GetActorByIdResponseExample : IExamplesProvider<ActorDTO>
    {
        public ActorDTO GetExamples()
        {
            return new ActorDTO
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Details = "A well-known actor in drama and action films.",
                Type = "Main",
                Rank = 1,
                Source = "Imdb"
            };
        }
    }
}
