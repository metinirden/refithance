using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Refithance;
using Refithance.Configuration;
using Refithance.Debugger.Apis;
using Refithance.Generator;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRefithance();

var app = builder.Build();

app.Services.UseRefithanceValidationWatcher();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/config", ([FromServices] IOptionsSnapshot<RefithanceOptions> optionsSnapshot) => Results.Ok(optionsSnapshot.Value)).WithOpenApi();
app.MapGet("/todos/{id}", async ([FromServices] ITodoApi todoApi, int id) => Results.Ok(await todoApi.GetTask(id))).WithOpenApi();
app.MapGet("/users/{id}", async ([FromServices] IUserApi userApi, int id) => Results.Ok(await userApi.GetUser(id))).WithOpenApi();
app.Run();
