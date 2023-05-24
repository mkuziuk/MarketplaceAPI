using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected IActionResult DoSwitch(KeyValuePair<StatusCodeEnum, string> result)
        {
            return result.Key switch
            {
                StatusCodeEnum.BadRequest => BadRequest(result.Value),
                StatusCodeEnum.Ok => Ok(result.Value),
                _ => BadRequest($"Этот ответ не существует в {nameof(StatusCodeEnum)}")
            };
        }
        
        protected IActionResult DoSwitch<T>(KeyValuePair<StatusCodeEnum, QueryableAndString<T>> result)
        {
            return result.Key switch
            {
                StatusCodeEnum.BadRequest => BadRequest(result.Value.TextResult),
                StatusCodeEnum.Ok => Ok(result.Value.QueryResult),
                _ => BadRequest($"Этот ответ не существует в {nameof(StatusCodeEnum)}")
            };
        }
    }
}