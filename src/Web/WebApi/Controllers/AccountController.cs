using Application.DTOs.Account;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Wrappers;
using Application.Commons.Extensions;

namespace WebApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IViewRenderService _razorService;

        public AccountController(IAccountService accountService, IAuthenticatedUserService authenticatedUserService, IViewRenderService razorService)
        {
            _accountService = accountService;
            _authenticatedUserService = authenticatedUserService;
            _razorService = razorService;
        }

        [HttpGet("info")]
        public async Task<IActionResult> ContextAsync(string token)
        {
            var result = new Response<UserContextDto>(_authenticatedUserService.CurrentUser);
            return Ok(await Task.FromResult(result));
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request, GenerateIpAddress()));
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest tokenRequest)
        {
            var result = await _accountService.VerifyAndGenerateToken(tokenRequest);

            return Ok(result);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] RevokeTokenRequest revokeTokenRequest)
        {
            var result = await _accountService.RevokeToken(revokeTokenRequest.RefreshToken);

            return Ok(result);

        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.RegisterAsync(request, origin));
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            return Ok(await _accountService.ConfirmEmailAsync(userId, code));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            await _accountService.ForgotPassword(model, Request.Headers["origin"]);
            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            return Ok(await _accountService.ResetPassword(model));
        }

        [HttpPost("export")]
        public async Task<IActionResult> Export()
        {
            var html = await _razorService.RenderToStringAsync("~/Views/Account/_ExportData.cshtml", _authenticatedUserService.CurrentUser);
            var bytes = html.PrintPdf();

            return File(bytes, "application/pdf", _authenticatedUserService.CurrentUser.Id + ".pdf");
        }

        private string GenerateIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}