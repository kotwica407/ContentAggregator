﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace ContentAggregator.Web.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await _next(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            await using MemoryStream requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            _logger.LogInformation($"Http Request Information:{Environment.NewLine}" +
                $"Method: {context.Request.Method} " +
                $"Schema: {context.Request.Scheme} " +
                $"Host: {context.Request.Host} " +
                $"Path: {context.Request.Path} " +
                $"QueryString: {context.Request.QueryString} " +
                $"Request Body: {ReadStreamInChunks(requestStream)}");
            context.Request.Body.Position = 0;
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            char[] readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                    0,
                    readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }
    }
}