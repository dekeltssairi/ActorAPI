using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ActorRepository : IActorRepository
    {
        private readonly ActorContext _actorContext;

        public ActorRepository(ActorContext actorContext)
        {
            _actorContext = actorContext;
        }

        public async Task<IEnumerable<Actor>> GetActorsAsync(string? name, int? rankStart, int? rankEnd, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            IQueryable<Actor> query = _actorContext.Actors;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(a => a.Name.Contains(name));
            }

            if (rankStart.HasValue && rankEnd.HasValue)
            {
                query = query.Where(a => a.Rank >= rankStart && a.Rank <= rankEnd);
            }

            return await query
                .OrderBy(a => a.Rank) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Actor?> GetActorByRankAsync(int rank, CancellationToken cancellationToken)
        {
            return await _actorContext.Actors.SingleOrDefaultAsync(a => a.Rank == rank, cancellationToken);
        }

        public async Task AddActorAsync(Actor actor, CancellationToken cancellationToken)
        {
            _actorContext.Actors.Add(actor);
            await _actorContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateActorAsync(Actor actor, CancellationToken cancellationToken)
        {
            _actorContext.Actors.Update(actor);
            await _actorContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddActorsAsync(IEnumerable<Actor> actors)
        {
            await _actorContext.Actors.AddRangeAsync(actors);
            await _actorContext.SaveChangesAsync();
        }

        public async Task <Actor?> DeleteActorAsync(Guid actorId, CancellationToken cancellationToken)
        {
            Actor? actor = await _actorContext.Actors
                .Where(a => a.Id == actorId)
                .SingleOrDefaultAsync(cancellationToken);

            if (actor != null)
            {
                _actorContext.Actors.Remove(actor);
                await _actorContext.SaveChangesAsync();
            }

            return actor;
        }

        public async Task<Actor?> GetActorByIdAsync(Guid actorId, CancellationToken cancellationToken)
        {
            return await _actorContext.Actors.SingleOrDefaultAsync(a => a.Id == actorId);
        }
    }

}
