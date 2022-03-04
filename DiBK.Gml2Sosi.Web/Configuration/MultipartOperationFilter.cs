using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DiBK.Gml2Sosi.Web.Configuration
{
    public class MultipartOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var mediaType = new OpenApiMediaType()
            {
                Schema = new OpenApiSchema()
                {
                    Type = "object",
                    Properties =
                    {
                        ["gmlFile"] = new OpenApiSchema
                        {
                            Type = "file",
                            Format = "binary"
                        }
                    },
                    Required = new HashSet<string>() { "gmlFile" }
                }
            };
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = { ["multipart/form-data"] = mediaType }
            };
        }
    }
}
