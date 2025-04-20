namespace HotelManagement25.Models;

public class RoomAssignmentCreateDto
{
    public Guid CustomerId { get; set; }
    public List<Guid> RoomIds { get; set; } = new();
}