using Swashbuckle.AspNetCore.SwaggerGen;
using TrainRegistry.Domain.ValueObjects;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace TrainRegistry.API.SchemaFilters
{
    public class TrainStatusSchemaFilter : ISchemaFilter
    {
        void ISchemaFilter.Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type != typeof(TrainStatus)) return;

            schema.Type = "string";
            schema.Enum = TrainStatus.All
                .Select(status => (IOpenApiAny) new OpenApiString(status.Value)).ToList();
        }
    }
}
