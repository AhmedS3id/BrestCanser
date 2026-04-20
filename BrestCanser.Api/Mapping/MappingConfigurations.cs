using BrestCanser.Api.Contracts.History;
using BrestCanser.Api.Contracts.Notifications;

namespace BrestCanser.Api.Mapping;

public class MappingConfigurations : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<RegisterRequest, ApplicationUser>()
			 .Map(dest => dest.UserName, src => src.Email);

		config.NewConfig<PredictionHistory, HistoryResponse>()
			 .Map(dest => dest.CreatedAt, src => DateOnly.FromDateTime(src.CreatedAt));

		config.NewConfig<Notification, NotificationResponse>()
			.Map(dest => dest.CreatedAt, src => src.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
	}
}