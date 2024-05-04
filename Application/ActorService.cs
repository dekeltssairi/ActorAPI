using Application.DTO;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;

namespace Application
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;
        private readonly IMapper _mapper;

        public ActorService(IActorRepository actorRepository, IMapper mapper)
        {
            _actorRepository = actorRepository;
            _mapper = mapper;
        }

        public async Task<ActorDTO> AddActorAsync(ActorCreateDTO actorCreateDto, CancellationToken cancellationToken)
        {
            var actorWithSameRank = await _actorRepository.GetActorByRankAsync(actorCreateDto.Rank, cancellationToken);
            
            if (actorWithSameRank != null)
            {
                throw new ConflictException($"An actor with rank {actorCreateDto.Rank} already exists.");
            }

            Actor actor = _mapper.Map<Actor>(actorCreateDto);

            await _actorRepository.AddActorAsync(actor, cancellationToken);

            ActorDTO actorDTO = _mapper.Map<ActorDTO>(actor);
            
            return actorDTO;
        }

        public async Task<ActorDTO?> DeleteActorAsync(Guid actorId, CancellationToken cancellationToken)
        {
            Actor? deletedAactor = await _actorRepository.DeleteActorAsync(actorId, cancellationToken);

            if (deletedAactor == null)
                throw new NotFoundException($"No actor found with ID {actorId}");

            return _mapper.Map<ActorDTO>(deletedAactor);
        }

        public async Task<ActorDTO?> GetActorByIdAsync(Guid actorId, CancellationToken cancellationToken)
        {
            Actor? actor = await _actorRepository.GetActorByIdAsync(actorId, cancellationToken);

            if (actor == null)
                throw new NotFoundException($"No actor found with ID {actorId}");


            return _mapper.Map<ActorDTO>(actor);
        }

        public async Task<IEnumerable<ActorBasicDTO>> GetAllActorsAsync(ActorQueryDTO queryDto, CancellationToken cancellationToken)
        {
            IEnumerable<Actor> actors = await _actorRepository.GetActorsAsync(queryDto.Name, queryDto.RankStart, queryDto.RankEnd, queryDto.PageNumber, queryDto.PageSize, cancellationToken);
            return actors.Select(a => _mapper.Map<ActorBasicDTO>(a));
        }

        public async Task <ActorDTO> UpdateActorAsync(Guid id, ActorUpdateDTO actorUpdateDto, CancellationToken cancellationToken)
        {
            Actor? existingActor = await _actorRepository.GetActorByIdAsync(id, cancellationToken);
            if (existingActor == null)
            {
                throw new NotFoundException($"No actor found with ID {id}");
            }

            var actorWithSameRank = await _actorRepository.GetActorByRankAsync(actorUpdateDto.Rank, cancellationToken);
            
            if (actorWithSameRank != null && actorWithSameRank.Id != id)
            {
                throw new ConflictException($"Another actor with rank {actorUpdateDto.Rank} already exists.");
            }

            _mapper.Map(actorUpdateDto, existingActor);

            await _actorRepository.UpdateActorAsync(existingActor, cancellationToken);

            return _mapper.Map<ActorDTO>(existingActor);
        }
    }
}
