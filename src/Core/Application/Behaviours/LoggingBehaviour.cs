using Application.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IAccountService _accountService;

        public LoggingBehaviour(ILogger<TRequest> logger, IAuthenticatedUserService authenticatedUserService, IAccountService accountService)
        {
            _logger = logger;
            _authenticatedUserService = authenticatedUserService;
            _accountService = accountService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _authenticatedUserService.UserId ?? string.Empty;
            var userName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                userName = await _accountService.GetUserName(userId);
            }

            _logger.LogInformation("Request: {Name} {@UserId} {@UserName} {@Request}", requestName, userId, userName, request);
        }
    }
}
