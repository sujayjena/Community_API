using AVCommunity.Application.Constants;
using AVCommunity.Application.Helpers;
using AVCommunity.Application.Models;
using AVCommunity.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AVCommunity.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;
        private readonly AppSettings _appSettings;
        private ResponseModel _responseError;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
            _appSettings = appS ettings.Value;
            _responseError = new ResponseModel();
        }
    }
}
