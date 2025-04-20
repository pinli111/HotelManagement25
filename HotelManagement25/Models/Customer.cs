namespace HotelManagement25.Models;

public class Customer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } 
    public ICollection<RoomAssignment> RoomAssignments { get; set; }
}