using System;
using System.Net;
using System.Threading.Tasks;
using ContentAggregator.Common;
using ContentAggregator.Models.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ContentAggregator.Web.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            ILogger logger = loggerFactory.CreateLogger("ExceptionMiddlewareHandlerLogger");
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    context.Response.ContentType = Consts.ContentTypes.Json;
                    Exception exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
                    if (exception != null)
                    {
                        logger.LogError(exception, $"An error occured during request: {context.Request.Path}");
                        await context.HandleException(exception);
                    }
                });
            });
        }

        private static async Task HandleException(this HttpContext context, Exception exception)
        {
            if (exception is HttpErrorException httpErrorException)
                await context.HandleException((int)httpErrorException.HttpStatusCode, 
                    exception.Message, 
                    Consts.ContentTypes.Text);
            else
                await context.HandleException((int) HttpStatusCode.InternalServerError,
                    "An error occurred",
                    Consts.ContentTypes.Text);
        }

        private static async Task HandleException(
            this HttpContext context,
            int statusCode,
            string message,
            string contentType)
        {
            context.Response.ContentType = contentType;
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(message);
        }
    }
}