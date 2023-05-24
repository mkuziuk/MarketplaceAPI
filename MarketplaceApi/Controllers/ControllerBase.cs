using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Controllers
{
    public abstract class ControllerBase : Controller
    {
        public IActionResult DoSwitch(KeyValuePair<StatusCodeEnum, string> result)
        {
            return result.Key switch
            {
                StatusCodeEnum.BadRequest => BadRequest(result.Value),
                StatusCodeEnum.Ok => Ok(result.Value),
                _ => BadRequest($"Этот ответ не существует в {nameof(StatusCodeEnum)}")
            };
        }

        public IActionResult DoSwitch(KeyValuePair<StatusCodeEnum, IQueryable<Order>> result, string badRequestText = null)
        {
            return result.Key switch
            {
                StatusCodeEnum.BadRequest => BadRequest(badRequestText),
                StatusCodeEnum.Ok => Ok(result.Value),
                _ => BadRequest($"Этот ответ не существует в {nameof(StatusCodeEnum)}")
            };
        }
    }
}