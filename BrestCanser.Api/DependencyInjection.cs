using BrestCanser.Api.Authentication;
using BrestCanser.Api.Settings;
using MailKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BrestCanser.Api;

public static class DependencyInjection
{
	public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration Configuration)
	{
		services.AddControllers();

		services.AddMapsterConfig()
				.AddFluentValidatonConfig()
				.AddAuthorConfig(Configuration);



		//add ConnectionString and register ApplicationDbContext
		var connectionString = Configuration.GetConnectionString("DefaultConnection") ??
			 throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

		services.AddDbContext<ApplicationDbContext>(options
			=> options.UseSqlServer(connectionString));


		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<IEmailSender, EmailService>();
		services.AddScoped<IChatService, GeminiService>();
		services.AddScoped<IHistoryService, HistoryService>();
		services.AddScoped<IImageService, ImageService>();



		services.AddExceptionHandler<GlobalExceptionHandler>();
		services.AddProblemDetails();

		services.Configure<MailSettings>(Configuration.GetSection(nameof(MailSettings)));

		services.Configure<CloudinarySettings>(Configuration.GetSection(nameof(CloudinarySettings)));


		return services;
	}

	private static IServiceCollection AddAuthorConfig(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddIdentity<ApplicationUser, IdentityRole>()
			 .AddEntityFrameworkStores<ApplicationDbContext>()
			 .AddDefaultTokenProviders();

		services.AddScoped<IJwtProvider, JwtProvider>();


		services.AddOptions<JwtOptions>()
			.BindConfiguration(JwtOptions.SectionName)
			.ValidateDataAnnotations()
			.ValidateOnStart();

		var JwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();


		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(o =>
		{
			o.SaveToken = true;
			o.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings?.key!)),
				ValidIssuer = JwtSettings?.Issuer,
				ValidAudience = JwtSettings?.Audience,
			};
		});

		services.Configure<IdentityOptions>(options =>
		{
			options.Password.RequiredLength = 8;
			//options.SignIn.RequireConfirmedEmail = true;
			options.User.RequireUniqueEmail = true;
		});

		return services;
	}
	private static IServiceCollection AddFluentValidatonConfig(this IServiceCollection services)
	{
		services.AddFluentValidationAutoValidation()
			.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		return services;
	}

	private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
	{
		var mapingconfig = TypeAdapterConfig.GlobalSettings;
		mapingconfig.Scan(Assembly.GetExecutingAssembly());
		services.AddSingleton<IMapper>(new Mapper(mapingconfig));

		return services;
	}
}