using Application.DTO;
using Swashbuckle.AspNetCore.Filters;

namespace API.SwaggerExamples
{
    public class GetActorsResponseExample : IExamplesProvider<IEnumerable<ActorBasicDTO>>
    {
        public IEnumerable<ActorBasicDTO> GetExamples()
        {
            return new List<ActorBasicDTO>
            {
                new ActorBasicDTO { Id = Guid.NewGuid(), Name = "John Doe" },
                new ActorBasicDTO { Id = Guid.NewGuid(), Name = "Jane Smith" }
            };
        }
    }
}
