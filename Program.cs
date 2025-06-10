using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using NetBuilding.Data;
using NetBuilding.Data.Users;
using NetBuilding.Middleware;
using NetBuilding.models;
using NetBuilding.Profiles;
using NetBuilding.Token;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));


builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers( opt =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();   
    opt.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization(); // <-- This registers the core authorization services

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new BuildingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var builderSecurity = builder.Services.AddIdentityCore<User>();
var identityBuilder = new IdentityBuilder(builderSecurity.UserType, builder.Services);
identityBuilder.AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
identityBuilder.AddSignInManager<SignInManager<User>>();
builder.Services.AddScoped<IJwtBuilder, JwtBuilder>();

var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("my key word"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                                    AddJwtBearer(opt =>
                                    {
                                        opt.TokenValidationParameters = new TokenValidationParameters
                                        {
                                            ValidateIssuerSigningKey = true,
                                            IssuerSigningKey = key,
                                            ValidateAudience = false,
                                            ValidateIssuer = false
                                        };
                                    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("corsapp",
        builder => builder.WithOrigins("*")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ManagerMiddleware>();
app.UseAuthentication();
app.UseCors("corsapp");
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
        await LoadDataBase.InsertData(context, userManager);
    }
    catch (Exception ex)
    {
        // Handle exceptions during migration or initialization
        Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
    }
}

app.Run();