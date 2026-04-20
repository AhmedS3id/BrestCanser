using BrestCanser.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrestCanser.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationsController : ControllerBase
{
	private readonly INotificationService _notificationService;

	public NotificationsController(INotificationService notificationService)
	{
		_notificationService = notificationService;
	}

	[HttpGet("")]
	public async Task<IActionResult> GetNotifications()
	{
		var result = await _notificationService.GetNotificationsAsync(User.GetUserId()!);
		
		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}

	[HttpPut("{id}/mark-read")]
	public async Task<IActionResult> MarkAsRead(int id)
	{
		var result = await _notificationService.MarkAsReadAsync(id, User.GetUserId()!);
		
		return result.IsSuccess ? NoContent() : result.ToProblem();
	}

	[HttpPut("mark-all-read")]
	public async Task<IActionResult> MarkAllAsRead()
	{
		var result = await _notificationService.MarkAllAsReadAsync(User.GetUserId()!);

		return result.IsSuccess ? NoContent() : result.ToProblem();
	}
}