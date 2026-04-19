using BrestCanser.Api.Contracts.History;

namespace BrestCanser.Api.Mapping;

public class MappingConfigurations : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<RegisterRequest, ApplicationUser>()
			 .Map(dest => dest.UserName, src => src.Email);

		config.NewConfig<PredictionHistory, HistoryResponse>()
			 .Map(dest => dest.CreatedAt, src => DateOnly.FromDateTime(src.CreatedAt));
	}
}