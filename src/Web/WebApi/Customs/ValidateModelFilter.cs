using Application.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Customs;

public class ValidateModelFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return;

        var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
            .SelectMany(v => v.Errors)
            .Select(v => v.ErrorMessage)
            .ToList();

        var responseModel = new Response<List<string>>
        {
            Succeeded = false,
            Errors = errors,
            Message = string.Empty
        };

        context.Result = new BadRequestObjectResult(responseModel)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
}