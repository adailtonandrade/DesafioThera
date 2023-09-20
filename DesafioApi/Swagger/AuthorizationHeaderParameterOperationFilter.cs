using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace DesafioApi.Swagger
{
    public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();

            }
            operation.parameters.Add(new Parameter
            {
                name = "Authorization",
                @in = "header",
                description = "Insert the JWT this way: Bearer {your token}",
                required = false,
                type = "string",
                @default = "Bearer "
            });
        }
    }
}