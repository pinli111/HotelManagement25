using HotelManagement25.Models;
using HotelManagement25.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace HotelManagement25.Tests.ServicesTests;

public class RoomServiceTests
{
    private readonly Mock<HotelManagementDbContext> _dbContextMock;
    private readonly RoomService _roomService;
    public RoomServiceTests()
    {
        var options = new DbContextOptions<HotelManagementDbContext>();
        _dbContextMock = new Mock<HotelManagementDbContext>(options);
        _roomService = new RoomService(_dbContextMock.Object);
    }
    
    [Fact]
    public async Task GetNearestAvailableRoom_ReturnsRoom_WhenRoomIsAvailable()
    {
        var availableRoom = new Room { Id = Guid.NewGuid(), Status = "Available", Floor = 1, Unit = 1 };
        _dbContextMock.Setup(context => context.Room)
            .ReturnsDbSet(new List<Room> { availableRoom });

        var result = await _roomService.GetNearestAvailableRoom();

        Assert.NotNull(result);
        Assert.Equal(availableRoom.Id, result.Id);
        Assert.Equal(availableRoom.Status, result.Status);
    }

    [Fact]
    public async Task GetNearestAvailableRoom_ReturnsNull_WhenNoRoomIsAvailable()
    {
        _dbContextMock.Setup(context => context.Room)
            .ReturnsDbSet(new List<Room>());

        var result = await _roomService.GetNearestAvailableRoom();

        Assert.Null(result);
    }

    [Fact]
    public async Task GetNearestAvailableRoom_ReturnsNearestRoom_WhenMultipleRoomsAreAvailable()
    {
        var room1 = new Room { Id = Guid.NewGuid(), Status = "Available", Floor = 2, Unit = 1 };
        var room2 = new Room { Id = Guid.NewGuid(), Status = "Available", Floor = 1, Unit = 2 };
        _dbContextMock.Setup(context => context.Room)
            .ReturnsDbSet(new List<Room> { room1, room2 });

        var result = await _roomService.GetNearestAvailableRoom();

        Assert.NotNull(result);
        Assert.Equal(room2.Id, result.Id);
        Assert.Equal(room2.Floor, result.Floor);
        Assert.Equal(room2.Unit, result.Unit);
    }
}