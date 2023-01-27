using DbAndGrpc.Model;
using DbAndGrpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
builder.Services.AddGrpc();


var app = builder.Build();
// Configure the HTTP request pipeline.
app.MapGrpcService<UserApiService>();
app.MapGet("/",(ApplicationContext db)=> db.Users.ToList());
app.Run();
