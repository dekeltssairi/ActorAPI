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
    public async Task GetActorByIdAsync_ShouldThrowNotFoundException_WhenActorDoesNotExist()
    {
        // Arrange
        var actorId = Guid.NewGuid();

        _actorRepositoryMock.Setup(repo => repo.GetActorByIdAsync(actorId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Actor?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _actorService.GetActorByIdAsync(actorId, CancellationToken.None));
    }

}
