using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using TrainRegistry.API.Swagger;
using TrainRegistry.Application.Common.Exceptions;
using TrainRegistry.Application.Trains;
using TrainRegistry.Application.Trains.Behaviors;
using TrainRegistry.Application.Trains.Queries.GetTrainById;
using TrainRegistry.Infrastructure.Persistence;
using TrainRegistry.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// MediatR (scan Application assembly)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetTrainByIdQuery).Assembly));


builder.Services.AddDbContext<TrainDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("TrainDb")));

// Repositories
builder.Services.AddScoped<ITrainRepository, TrainRepository>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


// Swagger (optional but recommended)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Logging.AddConsole();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<GetTrainByIdQuery>();


var app = builder.Build();
app.UseHttpsRedirection();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        switch (exception)
        {
            case ValidationException validationException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var errors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                await context.Response.WriteAsJsonAsync(new { errors });
                break;

            case NotFoundException notFound:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(new { error = notFound.Message });
                break;

            case UnauthorizedAccessException:
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Unauthorized" });
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
                break;
        }
    });
});


// Exception middleware
//app.UseMiddleware<ExceptionsHandlingMiddleware>();


// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
    }
});



app.MapControllers();
app.Run();