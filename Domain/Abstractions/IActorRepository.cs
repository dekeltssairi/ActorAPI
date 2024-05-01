using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IActorRepository
    {
        Task<IEnumerable<Actor>> GetActorsAsync(string? name, int? rankStart, int? rankEnd, int pageNumber, int pageSize);
        Task AddActorAsync(Actor actor);
        Task UpdateActorAsync(Actor actor);
        Task<Actor?> DeleteActorAsync(Guid actorId);
        Task<Actor?> GetActorByIdAsync(Guid actorId);
        Task<Actor?> GetActorByRankAsync(int rank);
        Task AddActorsAsync(IEnumerable<Actor> actors);
    }

}
