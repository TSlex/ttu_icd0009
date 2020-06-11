using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PublicApi.DTO.v1.Response;

namespace WebApp.Helpers
{
    /// <inheritdoc />
    public class ApiActionFilterAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var modelState = context.ModelState;

            if (modelState != null && modelState.IsValid == false)
            {
                context.Result = new BadRequestObjectResult(new ErrorResponseDTO(modelState.Values
                    .SelectMany(entry => entry.Errors)
                    .Select(error => error.ErrorMessage)
                    .ToArray()));
            }
        }
    }
}