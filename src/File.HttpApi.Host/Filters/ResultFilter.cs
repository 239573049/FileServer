using File.Application.Contract.Base;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace File.HttpApi.Host.Filters;

public class ResultFilter : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result != null)
        {
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult?.Value?.GetType().Name == nameof(HttpResultDto))
                {
                    var result = objectResult.Value as HttpResultDto;

                    context.Result = new ObjectResult(result);
                }
                else
                {
                    context.Result = new ObjectResult(new HttpResultDto(200, data: objectResult?.Value));
                }
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(new HttpResultDto(200));
            }
            else if (context.Result is HttpResultDto modelStateResult2)
            {
                context.Result = new ObjectResult(modelStateResult2);
            }
        }
        else
        {
            context.Result = new ObjectResult(new HttpResultDto(200));
        }
    
        base.OnActionExecuted(context);
    }
}