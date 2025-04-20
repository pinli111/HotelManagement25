using HotelManagement25.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement25.Services;

public class RoomService
{
    private readonly HotelManagementDbContext _context;

    public RoomService(HotelManagementDbContext context)
    {
        _context = context;
    }

    public virtual async Task<Room?> GetNearestAvailableRoom()
    {
        return _context.Room
            .Where(r => r.Status == nameof(Constants.RoomStatus.Available))
            .OrderBy(r=> r.Floor)
            .ThenBy(r => r.Unit)
            .FirstOrDefault();
    }
    
    public async Task<IEnumerable<Room>> GetAvailableRooms()
    {
        return await _context.Room
            .Where(r => r.Status == nameof(Constants.RoomStatus.Available))
            .OrderBy(r => r.Floor)
            .ThenBy(r => r.Unit)
            .ToListAsync();
    }

    public async Task<Room?> GetRoom(Guid id)
    {
        return await _context.Room.FindAsync(id);
    }

    public async Task<IEnumerable<Room>> GetRoom()
    {
        return await _context.Room.ToListAsync();
    }
    
    public async Task<bool> UpdateRoom(Room room)
    {
        var existingRoom = await _context.Room.FindAsync(room.Id);
        if (existingRoom == null)
            return false;
        if(!string.IsNullOrEmpty(room.Name))
            existingRoom.Name = room.Name;
        if(room.Floor != 0) 
            existingRoom.Floor = room.Floor;
        if(room.Unit != 0)
            existingRoom.Unit = room.Unit;
        if(!string.IsNullOrEmpty(room.Status) && Enum.TryParse<Constants.RoomStatus>(room.Status, out _))
            existingRoom.Status = room.Status;
        return await _context.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> AddRoom(Room room)
    {
        if (room.Id != null && RoomExists(room.Id))
            return false;
        await _context.Room.AddAsync(room);
        return await _context.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> DeleteRoom(Guid roomId)
    {
        var room = await _context.Room.FindAsync(roomId);
        if (room == null)
            return false;
        _context.Room.Remove(room);
        return await _context.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> UpdateStatus(Guid roomId, string status)
    {
        var room = await _context.Room.FindAsync(roomId);
        if (room == null)
            return false;
        room.Status = status;
        return await _context.SaveChangesAsync() > 0;
    }
    
    private bool RoomExists(Guid id)
    {
        return _context.Room.Any(e => e.Id == id);
    }
}