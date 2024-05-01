using Application.DTO;

namespace Application
{
    public  interface IActorService
    {
        Task <IEnumerable<ActorBasicDTO>> GetAllActorsAsync(ActorQueryDTO queryDto);
        Task <ActorDTO?>GetActorByIdAsync(Guid actorId);
        Task <ActorDTO>AddActorAsync(ActorCreateDTO actor);
        Task <ActorDTO>UpdateActorAsync(Guid id, ActorUpdateDTO actorUpdateDto);
        Task <ActorDTO?> DeleteActorAsync(Guid actorId);

    }
}
