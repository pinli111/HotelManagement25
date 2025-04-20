namespace HotelManagement25.Models;

public class Room
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Status { get; set; } = Constants.RoomStatus.Available.ToString();
    public int Floor { get; set; }
    public int Unit { get; set; }
}