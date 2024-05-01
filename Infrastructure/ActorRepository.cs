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

        public async Task<IEnumerable<Actor>> GetActorsAsync(string? name, int? rankStart, int? rankEnd, int pageNumber, int pageSize)
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
                .ToListAsync();
        }

        public async Task<Actor?> GetActorByRankAsync(int rank)
        {
            return await _actorContext.Actors.FirstOrDefaultAsync(a => a.Rank == rank);
        }

        public async Task AddActorAsync(Actor actor)
        {
            _actorContext.Actors.Add(actor);
            await _actorContext.SaveChangesAsync();
        }

        public async Task UpdateActorAsync(Actor actor)
        {
            _actorContext.Actors.Update(actor);
            await _actorContext.SaveChangesAsync();
        }

        public async Task AddActorsAsync(IEnumerable<Actor> actors)
        {
            await _actorContext.Actors.AddRangeAsync(actors);
            await _actorContext.SaveChangesAsync();
        }

        public async Task <Actor?> DeleteActorAsync(Guid actorId)
        {
            Actor? actor = _actorContext.Actors.Find(actorId);
            
           if (actor != null)
            {
                _actorContext.Actors.Remove(actor);
                await _actorContext.SaveChangesAsync();
            }

            return actor;
        }

        public async Task<Actor?> GetActorByIdAsync(Guid actorId)
        {
            return await _actorContext.Actors.FirstOrDefaultAsync(a => a.Id == actorId);
        }
    }

}
