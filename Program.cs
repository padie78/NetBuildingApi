var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(); // <-- This registers the core authorization services
builder.Services.AddAuthentication();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthentication();
//app.UseAuthorization();
//app.UseHttpsRedirection();

app.Run();
