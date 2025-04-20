using System.Text.Json.Serialization;
using HotelManagement25.Models;
using HotelManagement25.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Constants = HotelManagement25.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<HotelManagementDbContext>(opt
 //   => opt.UseInMemoryDatabase("HotelManagement"));
 builder.Services.AddDbContext<HotelManagementDbContext>(opt
 => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<RoomAssignmentService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HotelManagementDbContext>();
    SeedData(context);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void SeedData(HotelManagementDbContext context)
{
    //Need to create tables first
    int floor = 4;
    List<char> room = new List<char> { 'A', 'B', 'C', 'D', 'E' };
    List<(int, char)> rooms = new();
    List<Room> roomList = new();
    for(int i=1; i<= floor; i++)
    {
        if(i % 2 == 1)
        {
            for (int j = 0; j < room.Count; j++)
            {
                var newRoom = new Room()
                {
                    Floor = i,
                    Unit = j,
                    Name = $"{i}{room[j]}",
                    Status = Constants.RoomStatus.Occupied.ToString()
                };
                rooms.Add((i, room[j]));
                roomList.Add(newRoom);
            }
        }
        else
        {
            var size = room.Count - 1;
            for (int j = size; j >= 0 ; j--)
            {
                var newRoom = new Room()
                {
                    Floor = i,
                    Unit = -(j - size),
                    Name = $"{i}{room[j]}",
                    Status = Constants.RoomStatus.Available.ToString()
                };
                rooms.Add((i, room[j]));
                roomList.Add(newRoom);
            }
        }
    }
    
    if (!context.Room.Any())
    {
        context.Room.AddRange(roomList);
        context.SaveChanges();
    }

    var customerList = new List<Customer>
    {
        new Customer()
        {
            Name = "John Doe"
        },
        new Customer()
        {
            Name = "Jane Smith"
        },
        new Customer()
        {
            Name = "Alice Johnson"
        },
        new Customer()
        {
            Name = "Bob Brown"
        }
    };
    if (!context.Customer.Any())
    {
        context.Customer.AddRange(customerList);
        context.SaveChanges();
    }
}

