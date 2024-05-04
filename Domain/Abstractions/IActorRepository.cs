using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IActorRepository
    {
        Task<IEnumerable<Actor>> GetActorsAsync(string? name, int? rankStart, int? rankEnd, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task AddActorAsync(Actor actor, CancellationToken cancellationToken);
        Task UpdateActorAsync(Actor actor, CancellationToken cancellationToken);
        Task<Actor?> DeleteActorAsync(Guid actorId, CancellationToken cancellationToken);
        Task<Actor?> GetActorByIdAsync(Guid actorId, CancellationToken cancellationToken);
        Task<Actor?> GetActorByRankAsync(int rank, CancellationToken cancellationToken);
        Task AddActorsAsync(IEnumerable<Actor> actors);
    }

}
