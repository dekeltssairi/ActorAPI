using Xunit;
using Moq;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Numerics;
using Domain.Abstractions;
using Application;
using Application.DTO;
using Domain.Entities;
using Domain.Exceptions;

public class ActorServiceTests
{
    private readonly Mock<IActorRepository> _actorRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ActorService _actorService;

    public ActorServiceTests()
    {
        _actorRepositoryMock = new Mock<IActorRepository>();
        _mapperMock = new Mock<IMapper>();
        _actorService = new ActorService(_actorRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task AddActorAsync_ShouldAddActor_WhenActorWithSameRankDoesNotExist()
    {
        // Arrange
        var actorCreateDto = new ActorCreateDTO { Rank = 1 };
        var actor = new Actor { Id = Guid.NewGuid(), Rank = 1 };
        var actorDto = new ActorDTO { Id = actor.Id, Rank = actor.Rank };

        _actorRepositoryMock.Setup(repo => repo.GetActorByRankAsync(actorCreateDto.Rank, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Actor?)null);
        _mapperMock.Setup(mapper => mapper.Map<Actor>(actorCreateDto)).Returns(actor);
        _actorRepositoryMock.Setup(repo => repo.AddActorAsync(actor, It.IsAny<CancellationToken>()))
                            .Returns(Task.CompletedTask);
        _mapperMock.Setup(mapper => mapper.Map<ActorDTO>(actor)).Returns(actorDto);

        // Act
        var result = await _actorService.AddActorAsync(actorCreateDto, CancellationToken.None);

        // Assert
        Assert.Equal(actorDto, result);
    }

    [Fact]
    public async Task AddActorAsync_ShouldThrowConflictException_WhenActorWithSameRankExists()
    {
        // Arrange
        var actorCreateDto = new ActorCreateDTO { Rank = 1 };
        var existingActor = new Actor { Id = Guid.NewGuid(), Rank = 1 };

        _actorRepositoryMock.Setup(repo => repo.GetActorByRankAsync(actorCreateDto.Rank, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(existingActor);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _actorService.AddActorAsync(actorCreateDto, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteActorAsync_ShouldDeleteActor_WhenActorExists()
    {
        // Arrange
        var actorId = Guid.NewGuid();
        var actor = new Actor { Id = actorId };
        var actorDto = new ActorDTO { Id = actorId };

        _actorRepositoryMock.Setup(repo => repo.DeleteActorAsync(actorId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(actor);
        _mapperMock.Setup(mapper => mapper.Map<ActorDTO>(actor)).Returns(actorDto);

        // Act
        var result = await _actorService.DeleteActorAsync(actorId, CancellationToken.None);

        // Assert
        Assert.Equal(actorDto, result);
    }

    [Fact]
    public async Task DeleteActorAsync_ShouldThrowNotFoundException_WhenActorDoesNotExist()
    {
        // Arrange
        var actorId = Guid.NewGuid();

        _actorRepositoryMock.Setup(repo => repo.DeleteActorAsync(actorId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Actor?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _actorService.DeleteActorAsync(actorId, CancellationToken.None));
    }

    [Fact]
    public async Task GetActorByIdAsync_ShouldReturnActor_WhenActorExists()
    {
        // Arrange
        var actorId = Guid.NewGuid();
        var actor = new Actor { Id = actorId };
        var actorDto = new ActorDTO { Id = actorId };

        _actorRepositoryMock.Setup(repo => repo.GetActorByIdAsync(actorId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(actor);
        _mapperMock.Setup(mapper => mapper.Map<ActorDTO>(actor)).Returns(actorDto);

        // Act
        var result = await _actorService.GetActorByIdAsync(actorId, CancellationToken.None);

        // Assert
        Assert.Equal(actorDto, result);
    }

    [Fact]
    public async Task GetActorByIdAsync_ShouldThrowNotFoundException_WhenActorDoesNotExist()
    {
        // Arrange
        var actorId = Guid.NewGuid();

        _actorRepositoryMock.Setup(repo => repo.GetActorByIdAsync(actorId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Actor?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _actorService.GetActorByIdAsync(actorId, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateActorAsync_ShouldUpdateActor_WhenActorExistsAndNoConflict()
    {
        // Arrange
        var actorId = Guid.NewGuid();
        var actorUpdateDto = new ActorUpdateDTO { Rank = 2 };
        var existingActor = new Actor { Id = actorId, Rank = 1 };
        var updatedActor = new Actor { Id = actorId, Rank = 2 };
        var updatedActorDto = new ActorDTO { Id = actorId, Rank = 2 };

        _actorRepositoryMock.Setup(repo => repo.GetActorByIdAsync(actorId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(existingActor);
        _actorRepositoryMock.Setup(repo => repo.GetActorByRankAsync(actorUpdateDto.Rank, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Actor?)null);
        _mapperMock.Setup(mapper => mapper.Map(actorUpdateDto, existingActor)).Returns(updatedActor);
        _actorRepositoryMock.Setup(repo => repo.UpdateActorAsync(updatedActor, It.IsAny<CancellationToken>()))
                            .Returns(Task.CompletedTask);
        _mapperMock.Setup(mapper => mapper.Map<ActorDTO>(updatedActor)).Returns(updatedActorDto);

        // Act
        var result = await _actorService.UpdateActorAsync(actorId, actorUpdateDto, CancellationToken.None);

        // Assert
        Assert.Equal(updatedActorDto, result);
    }

    [Fact]
    public async Task UpdateActorAsync_ShouldThrowConflictException_WhenActorWithSameRankExists()
    {
        // Arrange
        var actorId = Guid.NewGuid();
        var actorUpdateDto = new ActorUpdateDTO { Rank = 2 };
        var existingActor = new Actor { Id = actorId, Rank = 1 };
        var conflictingActor = new Actor { Id = Guid.NewGuid(), Rank = 2 };

        _actorRepositoryMock.Setup(repo => repo.GetActorByIdAsync(actorId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(existingActor);
        _actorRepositoryMock.Setup(repo => repo.GetActorByRankAsync(actorUpdateDto.Rank, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(conflictingActor);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _actorService.UpdateActorAsync(actorId, actorUpdateDto, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateActorAsync_ShouldThrowNotFoundException_WhenActorDoesNotExist()
    {
        // Arrange
        var actorId = Guid.NewGuid();
        var actorUpdateDto = new ActorUpdateDTO { Rank = 2 };

        _actorRepositoryMock.Setup(repo => repo.GetActorByIdAsync(actorId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Actor?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _actorService.UpdateActorAsync(actorId, actorUpdateDto, CancellationToken.None));
    }

    [Fact]
    public async Task GetAllActorsAsync_ShouldReturnAllActors()
    {
        // Arrange
        var actors = new List<Actor>
    {
        new Actor { Id = Guid.NewGuid(), Rank = 1, Name = "Dekel1" },
        new Actor { Id = Guid.NewGuid(), Rank = 2, Name = "Dekel2" }
    };
        var actorQueryDto = new ActorQueryDTO { Name = "", RankStart = 1, RankEnd = 5, PageNumber = 1, PageSize = 10 };
        var actorBasicDtos = actors.Select(actor => new ActorBasicDTO { Id = actor.Id, Name = actor.Name }).ToList();

        _actorRepositoryMock.Setup(repo => repo.GetActorsAsync(actorQueryDto.Name, actorQueryDto.RankStart, actorQueryDto.RankEnd, actorQueryDto.PageNumber, actorQueryDto.PageSize, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(actors);
        _mapperMock.Setup(mapper => mapper.Map<ActorBasicDTO>(It.IsAny<Actor>()))
                   .Returns((Actor actor) => new ActorBasicDTO { Id = actor.Id, Name = actor.Name });

        // Act
        var result = await _actorService.GetAllActorsAsync(actorQueryDto, CancellationToken.None);

        // Assert
        Assert.Collection(result,
            item =>
            {
                Assert.Equal(actorBasicDtos[0].Id, item.Id);
                Assert.Equal(actorBasicDtos[0].Name, item.Name);
            },
            item =>
            {
                Assert.Equal(actorBasicDtos[1].Id, item.Id);
                Assert.Equal(actorBasicDtos[1].Name, item.Name);
            });
    }

}
