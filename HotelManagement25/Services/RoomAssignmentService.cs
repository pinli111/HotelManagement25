using HotelManagement25.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement25.Services;

public class RoomAssignmentService
{
    private readonly HotelManagementDbContext _context;
    public RoomAssignmentService(HotelManagementDbContext context)
    {
        _context = context;
    }
    
    public async Task<(bool,RoomAssignment)> CreateRoomAssignment(RoomAssignmentCreateDto roomAssignmentCreateDto)
    {
        var customer = await _context.Customer
            .FirstOrDefaultAsync(c => c.Id == roomAssignmentCreateDto.CustomerId);
        var roomAssignments = new List<RoomAssignment>();
        var rooms = await _context.Room
            .Where(r => roomAssignmentCreateDto.RoomIds.Contains(r.Id))
            .ToListAsync();
        foreach (var room in rooms)
        {
            var roomAssignment = new RoomAssignment()
            {
                CustomerId = roomAssignmentCreateDto.CustomerId,
                //Customer = customer,
                RoomId = room.Id,
                //Room = room
            };
            roomAssignments.Add(roomAssignment);
        }
        await _context.RoomAssignment.AddRangeAsync(roomAssignments);
        return (await _context.SaveChangesAsync() > 0, roomAssignments.FirstOrDefault());
    }
    
    private bool RoomAssignmentExists(Guid id)
    {
        return _context.RoomAssignment.Any(e => e.Id == id);
    }
}