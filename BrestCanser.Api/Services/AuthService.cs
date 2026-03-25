using BrestCanser.Api.Authentication;
using System.Security.Cryptography;

namespace BrestCanser.Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly int _refreshTokenExpiryDays = 14;

    public AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<AuthorResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        // check user?
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return Result.Failure<AuthorResponse>(UserErrors.InvalidCredentials);



        //check password
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

        if (!isPasswordValid)
            return Result.Failure<AuthorResponse>(UserErrors.InvalidCredentials);



        //generate jwt token
        var (token, expiresIn) = _jwtProvider.GenerateToken(user);

        //generate jwt refresh token
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);


        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);


        //return response
        var response = new AuthorResponse(user.Id, user.Email, user.PhoneNumber, user.FirstName, user.LastName, user.Gender, token, expiresIn, refreshToken, refreshTokenExpiration);

        return Result.Success(response);
    }

    public async Task<Result<AuthorResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure<AuthorResponse>(UserErrors.InvalidJwtToken);




        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure<AuthorResponse>(UserErrors.InvalidJwtToken);


        var userRefreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken && t.IsActive);

        if (userRefreshToken is null)
            return Result.Failure<AuthorResponse>(UserErrors.InvalidRefreshToken);


        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);


        var response = new AuthorResponse(user.Id, user.Email, user.PhoneNumber, user.FirstName, user.LastName, user.Gender, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);

        return Result.Success(response);
    }

    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken && t.IsActive);

        if (userRefreshToken is null)
            return Result.Failure(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }

    public async Task<Result<AuthorResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var emailIsExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

        if (emailIsExists)
            return Result.Failure<AuthorResponse>(UserErrors.DuplicatedEmail);

        var user = request.Adapt<ApplicationUser>();

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            var (token, expiresIn) = _jwtProvider.GenerateToken(user);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration
            });

            await _userManager.UpdateAsync(user);

            var response = new AuthorResponse(user.Id, user.Email, user.PhoneNumber, user.FirstName, user.LastName, user.Gender, token, expiresIn, refreshToken, refreshTokenExpiration);

            return Result.Success(response);
        }

        var error = result.Errors.First();

        return Result.Failure<AuthorResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    private static string GenerateRefreshToken()
    {
        var refreshToken = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(refreshToken);
    }
}