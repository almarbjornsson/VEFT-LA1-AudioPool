using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BasicTokenAuthorize : Attribute, IAuthorizationFilter
    {
        private const string ExpectedTokenValue = "RespectAndEnjoyThePeace";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("api-token", out var extractedToken))
            {
                // No api-token header was found, so return Unauthorized
                context.Result = new UnauthorizedObjectResult("API token was not provided.");
                return;
            }

            if (extractedToken != ExpectedTokenValue)
            {
                // Provided token doesn't match the expected value, so return Unauthorized
                context.Result = new UnauthorizedObjectResult("Invalid API token.");
            }
        }
    }
}