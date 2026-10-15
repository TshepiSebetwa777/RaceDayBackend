using Microsoft.EntityFrameworkCore;

using RaceDayBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Database Context

builder.Services.AddDbContext<RaceDayDBContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Add Session Management for Role-Based Authentication

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>

{

    options.IdleTimeout = TimeSpan.FromMinutes(30);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;

});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())

{

    app.UseSwagger();

    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

// 3. Enable Session tracking (MUST be placed before Authorization)

app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();