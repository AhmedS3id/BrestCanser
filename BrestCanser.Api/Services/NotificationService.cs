//using BrestCanser.Api.Contracts.Notifications;
//using BrestCanser.Api.Hubs;
//using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.AspNetCore.SignalR;

//namespace BrestCanser.Api.Services;

//public class NotificationService : INotificationService
//{
//	//private readonly ApplicationDbContext _context;
//	//private readonly IEmailSender _emailSender;
//	//private readonly IHubContext<NotificationHub> _hubContext;

//	//public NotificationService(
//	//	ApplicationDbContext context,
//	//	IEmailSender emailSender,
//	//	IHubContext<NotificationHub> hubContext)
//	//{
//	//	_context = context;
//	//	_emailSender = emailSender;
//	//	_hubContext = hubContext;
//	//}

//	//public async Task SendPredictionNotificationAsync(
//	//	string userId,
//	//	PredictionHistory history,
//	//	CancellationToken cancellationToken = default)
//	//{
//	//	var user = await _context.Users.FindAsync([userId], cancellationToken);
//	//	if (user is null) return;

//	//	// 1. Save in-app notification
//	//	var notification = new Notification
//	//	{
//	//		UserId = userId,
//	//		Title = "Prediction Result Ready",
//	//		Message = $"Your scan result: {history.Diagnosis} ({history.Status}) with {history.Confidence:F1}% confidence."
//	//	};

//	//	_context.Notifications.Add(notification);
//	//	await _context.SaveChangesAsync(cancellationToken);

//	//	// 2. Push via SignalR (real-time)
//	//	var payload = notification.Adapt<NotificationResponse>();

//	//	await _hubContext.Clients
//	//		.Group(userId)
//	//		.SendAsync("ReceiveNotification", payload, cancellationToken);

//	//	// 3. Send email
//	//	var htmlBody = EmailTemplates.PredictionResult(
//	//		user.FirstName,
//	//		history.Diagnosis,
//	//		history.Status.ToString(),
//	//		history.Confidence
//	//	);

//	//	await _emailSender.SendEmailAsync(
//	//		user.Email!,
//	//		"Your Breast Cancer Scan Result",
//	//		htmlBody
//	//	);
//	//}

//	//public async Task<Result<IEnumerable<NotificationResponse>>> GetNotificationsAsync(string userId)
//	//{
//	//	var notifications = await _context.Notifications
//	//		.Where(x => x.UserId == userId)
//	//		.OrderByDescending(x => x.CreatedAt)
//	//		.ToListAsync();

//	//	if (!notifications.Any())
//	//		return Result.Failure<IEnumerable<NotificationResponse>>(NotificationErrors.NotFound);

//	//	return Result.Success(notifications.Adapt<IEnumerable<NotificationResponse>>());
//	//}

//	//public async Task<Result> MarkAsReadAsync(int notificationId, string userId)
//	//{
//	//	var notification = await _context.Notifications
//	//		.FirstOrDefaultAsync(x => x.Id == notificationId && x.UserId == userId);

//	//	if (notification is null)
//	//		return Result.Failure(NotificationErrors.NotificationNotFound);

//	//	notification.IsRead = true;
//	//	await _context.SaveChangesAsync();

//	//	return Result.Success();
//	//}

//	//public async Task<Result> MarkAllAsReadAsync(string userId)
//	//{
//	//	await _context.Notifications
//	//		.Where(x => x.UserId == userId && !x.IsRead)
//	//		.ExecuteUpdateAsync(x => x.SetProperty(n => n.IsRead, true));

//	//	return Result.Success();
//	//}
//}