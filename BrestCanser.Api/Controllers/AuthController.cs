namespace BrestCanser.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authorService;

	public AuthController(IAuthService authorService)
	{
		_authorService = authorService;
	}

	[HttpPost("")]
	[EnableRateLimiting(RateLimiters.AuthPolicy)]
	public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
	{
		var result = await _authorService.GetTokenAsync(request.Email, request.Password, cancellationToken);

		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}

	[HttpPost("refresh")]
	[EnableRateLimiting(RateLimiters.GeneralPolicy)]
	public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
	{
		var result = await _authorService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}

	[HttpPost("revoke-refresh-token")]
	[EnableRateLimiting(RateLimiters.GeneralPolicy)]
	public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
	{
		var result = await _authorService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

		return result.IsSuccess ? Ok() : result.ToProblem();
	}

	[HttpPost("register")]
	[EnableRateLimiting(RateLimiters.AuthPolicy)]
	public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
	{
		var authorResult = await _authorService.RegisterAsync(request, cancellationToken);

		return authorResult.IsSuccess ? Ok(authorResult.Value) : authorResult.ToProblem();
	}

	[HttpPost("forget-password")]
	[EnableRateLimiting(RateLimiters.SensitivePolicy)]
	public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
	{
		var result = await _authorService.SendResetPasswordCodeAsync(request.Email);

		return result.IsSuccess ? Ok() : result.ToProblem();
	}

	[HttpPost("verify-code")]
	[EnableRateLimiting(RateLimiters.SensitivePolicy)]
	public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCodeRequest request)
	{
		var result = await _authorService.VerifyResetCodeAsync(request.Email, request.Code);

		return result.IsSuccess ? Ok() : result.ToProblem();
	}

	[HttpPost("reset-password")]
	[EnableRateLimiting(RateLimiters.SensitivePolicy)]
	public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
	{
		var result = await _authorService.ResetPasswordAsync(request.Email, request.Code, request.NewPassword);

		return result.IsSuccess ? Ok() : result.ToProblem();
	}
}