using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace JwtLiftoff.Common.Swagger
{
    public class JwtAuthorizationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var allowAnonymous = filterPipeline.Select(info => info.Filter).Any(filter => filter is IAllowAnonymousFilter);

            if (!allowAnonymous)
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<IParameter>();

                operation.Parameters.Add(new NonBodyParameter()
                {
                    Name = "Authorization",
                    In = "header",
                    Description = "JWT access token",
                    Required = true,
                    Type = "string"
                });
            }
        }
    }
}
