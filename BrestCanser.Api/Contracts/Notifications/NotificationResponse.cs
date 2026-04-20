namespace BrestCanser.Api.Contracts.Notifications;

public record NotificationResponse(
	int Id,
	string Title,
	string Message,
	bool IsRead,
	string CreatedAt
);