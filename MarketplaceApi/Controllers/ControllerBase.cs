using System.Collections.Generic;
using System.Threading.Tasks;
using MarketplaceApi.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected IActionResult DoSwitch(KeyValuePair<StatusCodeEnum, string> result)
        {
            return result.Key switch
            {
                StatusCodeEnum.NotFound => NotFound(result.Value),
                StatusCodeEnum.Ok => Ok(result.Value),
                _ => NotFound($"Этот ответ не существует в {nameof(StatusCodeEnum)}")
            };
        }
        
        protected IActionResult DoSwitch<T>(KeyValuePair<StatusCodeEnum, EnumerableAndString<T>> result)
        {
            return result.Key switch
            {
                StatusCodeEnum.NotFound => NotFound(result.Value.TextResult),
                StatusCodeEnum.Ok => Ok(result.Value.QueryResult),
                _ => NotFound($"Этот ответ не существует в {nameof(StatusCodeEnum)}")
            };
        }
    }
}