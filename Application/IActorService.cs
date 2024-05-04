using Application.DTO;

namespace Application
{
    public  interface IActorService
    {
        Task <IEnumerable<ActorBasicDTO>> GetAllActorsAsync(ActorQueryDTO queryDto, CancellationToken cancellationToken);
        Task <ActorDTO?>GetActorByIdAsync(Guid actorId, CancellationToken cancellationToken);
        Task <ActorDTO>AddActorAsync(ActorCreateDTO actor, CancellationToken cancellationToken);
        Task <ActorDTO>UpdateActorAsync(Guid id, ActorUpdateDTO actorUpdateDto, CancellationToken cancellationToken);
        Task <ActorDTO?> DeleteActorAsync(Guid actorId, CancellationToken cancellationToken);

    }
}
