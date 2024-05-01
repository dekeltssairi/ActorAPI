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

        public async Task<ActorDTO> AddActorAsync(ActorCreateDTO actorCreateDto)
        {
            var actorWithSameRank = await _actorRepository.GetActorByRankAsync(actorCreateDto.Rank);
            
            if (actorWithSameRank != null)
            {
                throw new UniqueConstraintViolationException($"An actor with rank {actorCreateDto.Rank} already exists.");
            }

            Actor actor = _mapper.Map<Actor>(actorCreateDto);

            await _actorRepository.AddActorAsync(actor);

            ActorDTO actorDTO = _mapper.Map<ActorDTO>(actor);
            
            return actorDTO;
        }

        public async Task<ActorDTO?> DeleteActorAsync(Guid actorId)
        {
            Actor? deletedAactor = await _actorRepository.DeleteActorAsync(actorId);

            if (deletedAactor == null) return null;

            return _mapper.Map<ActorDTO>(deletedAactor);
        }

        public async Task<ActorDTO?> GetActorByIdAsync(Guid actorId)
        {
            Actor? actor = await _actorRepository.GetActorByIdAsync(actorId);

            if (actor == null) return null;

            return _mapper.Map<ActorDTO>(actor);
        }

        public async Task<IEnumerable<ActorBasicDTO>> GetAllActorsAsync(ActorQueryDTO queryDto)
        {
            IEnumerable<Actor> actors = await _actorRepository.GetActorsAsync(queryDto.Name, queryDto.RankStart, queryDto.RankEnd, queryDto.PageNumber, queryDto.PageSize);
            return actors.Select(a => _mapper.Map<ActorBasicDTO>(a));
        }

        public async Task <ActorDTO> UpdateActorAsync(Guid id, ActorUpdateDTO actorUpdateDto)
        {
            Actor? existingActor = await _actorRepository.GetActorByIdAsync(id);
            if (existingActor == null)
            {
                throw new KeyNotFoundException($"No actor found with ID {id}");
            }

            var actorWithSameRank = await _actorRepository.GetActorByRankAsync(actorUpdateDto.Rank);
            
            if (actorWithSameRank != null && actorWithSameRank.Id != id)
            {
                throw new UniqueConstraintViolationException($"Another actor with rank {actorUpdateDto.Rank} already exists.");
            }

            _mapper.Map(actorUpdateDto, existingActor);

            await _actorRepository.UpdateActorAsync(existingActor);

            return _mapper.Map<ActorDTO>(existingActor);
        }
    }
}
