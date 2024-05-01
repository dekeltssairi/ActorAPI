using API.SwaggerExamples;
using Application;
using Application.DTO;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ActorController : ControllerBase
    {
        private readonly ILogger<ActorController> _logger;
        private readonly IActorService _actorService;

        public ActorController(ILogger<ActorController> logger, IActorService actorService)
        {
            _logger = logger;
            _actorService = actorService;
        }

        /// <summary>
        /// Retrieves a list of all actors, with optional filtering and pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/actors?Name=Pacino&amp;RankStart=1&amp;RankEnd=200&amp;PageNumber=2&amp;PageSize=10
        /// 
        /// This request retrieves actors filtered by the name "John Doe", between ranks 1 and 200, on page 2 with 10 actors per page.
        /// </remarks>
        /// <param name="queryDto">query definition.</param>
        /// <returns>A list of actors based on the specified filters and pagination settings.</returns>
        /// <response code="200">Returns the list of actors.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet(Name = "GetActors")]
        [ProducesResponseType(typeof(IEnumerable<ActorBasicDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetActorsResponseExample))]
        public async Task<ActionResult<IEnumerable<ActorBasicDTO>>> GetActors([FromQuery] ActorQueryDTO queryDto)
        { 
            try
            {
                var actors = await _actorService.GetAllActorsAsync(queryDto);
                return Ok(actors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving actors.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// Retrieves an actor by their ID.
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/actors/{id}
        /// 
        /// Replace {id} with the actual ID of the actor you want to retrieve,
        /// For example GET /api/actors/12345678-1234-1234-1234-1234567890ab
        /// </remarks>
        /// <param name="id">The ID of the actor to retrieve.</param>
        /// <returns>Returns the requested actor or a 404 status if not found.</returns>
        /// <response code="200">Returns the requested actor.</response>
        /// <response code="404">If the actor is not found.</response>
        [ProducesResponseType(typeof(ActorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetActorByIdResponseExample))]
        [HttpGet("{id}", Name = "GetActorById")]
        public async Task<ActionResult<ActorDTO>> GetActorById(Guid id)
        {
            try
            {
                var actor = await _actorService.GetActorByIdAsync(id);
                if (actor == null)
                {
                    return NotFound($"Actor with ID {id} not found.");
                }

                return Ok(actor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving actor.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Creates a new actor.
        /// </summary>
        /// <param name="actorDto">The actor creation DTO.</param>
        /// <returns>A newly created Actor</returns>
        /// <response code="201">Returns the newly created actor.</response>
        /// <response code="409">If actor with same rank allready exist</response>
        [HttpPost(Name = "AddActor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(ActorCreateDTO), typeof(AddActorRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(AddActorResponseExample))]
        public async Task<ActionResult<ActorDTO>> AddActor([FromBody] ActorCreateDTO actorDto)
        {
            try
            {
                var addedActor = await _actorService.AddActorAsync(actorDto);
                return CreatedAtAction(nameof(GetActorById), new { id = addedActor.Id }, addedActor);
            }
            catch (UniqueConstraintViolationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding actor.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing actor.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/actors/12345678-1234-1234-1234-1234567890ab
        ///     {
        ///         "name": "John Updated",
        ///         "details": "Updated details here",
        ///         "type": "Writer",
        ///         "rank": 2,
        ///         "source": "IMDB"
        ///     }
        /// 
        /// Replace `12345678-1234-1234-1234-1234567890ab` with the actual ID of the actor you want to update.
        /// </remarks>
        /// <param name="id">The ID of the actor to update.</param>
        /// <param name="actorUpdateDto">The actor data to update.</param>
        /// <returns>Returns the updated actor or appropriate status code.</returns>
        /// <response code="200">Returns the updated actor.</response>
        /// <response code="404">If no actor is found with the given ID.</response>
        /// <response code="409">If there is a conflict, such as a duplicate rank.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpPut("{id}", Name = "UpdateActor")]
        [ProducesResponseType(typeof(ActorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UpdateActorResponseExample))]
        public async Task<ActionResult<ActorDTO>> UpdateActor(Guid id, [FromBody] ActorUpdateDTO actorUpdateDto)
        {
            try
            {
                var updatedActor = await _actorService.UpdateActorAsync(id, actorUpdateDto);
                
                if (updatedActor == null)
                {
                    return NotFound($"Actor with ID {id} not found.");
                }

                return Ok(updatedActor);
            }
            catch (UniqueConstraintViolationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating actor.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Deletes an actor.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/v1/actors/{id}
        ///
        /// Replace {id} with the actual ID of the actor you want to retrieve,
        /// For example DELETE /api/actors/12345678-1234-1234-1234-1234567890ab
        /// </remarks>
        /// <param name="id">The ID of the actor to delete.</param>
        /// <returns>Returns no content if successful, not found if the actor does not exist, or an error message if an internal error occurs.</returns>
        /// <response code="204">No content, actor successfully deleted.</response>
        /// <response code="404">If no actor is found with the given ID.</response>
        [HttpDelete("{id}", Name = "DeleteActor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteActor(Guid id)
        {
            try
            {
                ActorDTO? deletedActor = await _actorService.DeleteActorAsync(id);

                if (deletedActor == null)
                {
                    return NotFound($"Actor with ID {id} not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting actor.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
