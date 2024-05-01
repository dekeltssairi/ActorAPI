using Application.DTO;
using Swashbuckle.AspNetCore.Filters;

namespace API.SwaggerExamples
{
    public class AddActorRequestExample : IExamplesProvider<ActorCreateDTO>
    {
        public ActorCreateDTO GetExamples()
        {
            return new ActorCreateDTO
            {
                Name = "John Doe",
                Details = "Sample details here",
                Type = "Main",
                Rank = 1,
                Source = "IMDB"
            };
        }
    }

    public class AddActorResponseExample : IExamplesProvider<ActorDTO>
    {
        public ActorDTO GetExamples()
        {
            return new ActorDTO
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Details = "Sample details here",
                Type = "Main",
                Rank = 1,
                Source = "IMDB"
            };
        }
    }
}
