using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MeterReadings.Service.Swagger
{
    public class FileUploadFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            string fileUploadMimeType = "multipart/form-data";

            if (operation.RequestBody != null && operation.RequestBody.Content.Any(x => x.Key.Equals(fileUploadMimeType, StringComparison.InvariantCultureIgnoreCase)))
            {
                IEnumerable<ParameterInfo> fileParams = context.MethodInfo.GetParameters().Where(pInfo => pInfo.ParameterType == typeof(IFormFile));

                OpenApiSchema schema = new OpenApiSchema()
                {
                    Format = "binary",
                    Type = "string",
                };

                operation.RequestBody.Content[fileUploadMimeType].Schema.Properties =
                    fileParams.ToDictionary(key => key.Name, value => schema);
            }
        }
    }
}
