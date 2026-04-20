namespace BrestCanser.Api.Errors;


public static class NotificationErrors
{
	public static readonly Error NotFound =
		new("Notification.NotFound", "No notifications found.", StatusCodes.Status404NotFound);

	public static readonly Error NotificationNotFound =
		new("Notification.NotificationNotFound", "Notification not found.", StatusCodes.Status404NotFound);
}