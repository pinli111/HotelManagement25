using System.Text.Json.Serialization;

namespace HotelManagement25.Models;

public class RoomAssignment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    [JsonIgnore] 
    public Customer Customer { get; set; }
    public Guid RoomId { get; set; }
    [JsonIgnore] 
    public Room Room { get; set; }
}