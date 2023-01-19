﻿using DatingApp.Extensions;
using DatingApp.Interface;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatingApp.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId();

            var uow = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

            var user = await uow.UserRepository.GetUserByIdAsync(int.Parse(userId));
            user.LastActive = DateTime.UtcNow;

            await uow.Complete();
        }
    }
}
