using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CaliCamp.Helpers
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileUploadMember = context.MethodInfo.GetParameters()
                .Where(p => p.ParameterType == typeof(IFormFile))
                .Select(p => p.Name)
                .ToList();

            if (fileUploadMember.Any())
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        {
                            "multipart/form-data", new OpenApiMediaType
                            {
                                Schema = new OpenApiSchema
                                {
                                    Type = "object",
                                    Properties = new Dictionary<string, OpenApiSchema>
                                    {
                                        { fileUploadMember.First(), new OpenApiSchema { Type = "string", Format = "binary" } },
                                        { "campingSpotId", new OpenApiSchema { Type = "integer" } },
                                        { "description", new OpenApiSchema { Type = "string" } },
                                        { "uploadedBy", new OpenApiSchema { Type = "integer" } }
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }
    }
}
