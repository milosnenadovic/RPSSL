//using MediatR;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

//using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RPSSL.GameService.Common.Configurations;
using RPSSL.GameService.Common.Configurations.Settings;
using RPSSL.GameService.Common.Exceptions;
using RPSSL.GameService.Configurations.Identity;



//using RPSSL.GameService.Configurations.Identity;
using RPSSL.GameService.Configurations.Mapping;
using RPSSL.GameService.Handlers;
using RPSSL.GameService.Infrastructure.Settings;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace RPSSL.GameService.Configurations;

public static class ConfigureServices
{
	public static IServiceCollection AddServerServices(this IServiceCollection services, ConfigurationManager configuration)
	{
		#region Options
		services.AddOptions<JwtSettings>().Bind(configuration.GetSection(JwtSettings.SectionName));
		services.AddOptions<DatabaseSettings>().Bind(configuration.GetSection(DatabaseSettings.SectionName));
		services.AddOptions<ServicesSettings>().Bind(configuration.GetSection(ServicesSettings.SectionName));
		#endregion

		#region Behaviours pipeline
		services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
		#endregion

		services.AddHttpContextAccessor();
		services.AddEndpointsApiExplorer();
		services.AddMvc();
		//services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

		services.RegisterMapsterConfiguration();

		#region Authentication
		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			var jwtOptions = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();

			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidIssuer = jwtOptions?.Issuer,
				ValidateAudience = true,
				ValidAudience = jwtOptions?.Audience,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions?.SecretKey ?? string.Empty)),
			};
			options.Events = new JwtBearerEvents
			{
				OnMessageReceived = context =>
				{
					if (context.Request.Cookies.ContainsKey(HttpContextItemKeys.AuthTokenCookie))
						context.Token = context.Request.Cookies[HttpContextItemKeys.AuthTokenCookie];
					return Task.CompletedTask;
				},
				OnAuthenticationFailed = context =>
				{
					if (context.Exception is SecurityTokenExpiredException)
						throw new SecurityTokenExpiredException(Common.Constants.Errors.Error.Authorization.TokenExpired);
					else throw new CantResolveUserException(Common.Constants.Errors.Error.Authorization.CantResolveUser);
				},
				OnForbidden = context => throw new CantResolveUserException(Common.Constants.Errors.Error.Authorization.Role),
				OnChallenge = context =>
				{
					throw new CantResolveUserException(Common.Constants.Errors.Error.Authorization.MissingToken);
				}
			};
			options.SaveToken = true;
		});
		#endregion

		#region Authorization policies
		services.AddAuthorization(options =>
		{
			options.AddPolicy(IdentityData.PolicyAdmin,
				policy => policy.RequireRole(IdentityData.RoleAdmin));
			options.AddPolicy(IdentityData.PolicyUser,
				policy => policy.RequireRole(IdentityData.RoleUser));

			options.AddPolicy(IdentityData.PolicyAdminUser,
				policy => policy.RequireRole(IdentityData.RoleAdmin, IdentityData.RoleUser));
		});
		#endregion

		#region CORS
		services.AddCors(options =>
		{
			options.AddPolicy(name: Constants.CorsPolicy, policy =>
			{
				policy.WithOrigins("https://localhost:7247", "http://localhost:5247")
					.AllowAnyHeader()
					.AllowAnyMethod()
					.SetIsOriginAllowed(options => true)
					.AllowCredentials();
			});
			options.AddDefaultPolicy(policy =>
			{
				policy.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod();
			});
		});
		#endregion

		#region Swagger
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc(Constants.SwaggerVersion, new OpenApiInfo
			{
				Version = Constants.SwaggerVersion,
				Title = Constants.SwaggerTitle,
				Description = Constants.SwaggerDescription,
				TermsOfService = new Uri(Constants.SwaggerTerms),
				Contact = new OpenApiContact
				{
					Name = Constants.OpenApiContactName,
					Url = new Uri(Constants.OpenApiContactUri)
				},
				License = new OpenApiLicense
				{
					Name = Constants.OpenApiLicenseName,
					Url = new Uri(Constants.OpenApiLicenseUri)
				}
			});
			options.AddSecurityDefinition(HttpContextItemKeys.OAuth2, new OpenApiSecurityScheme
			{
				Description = HttpContextItemKeys.Authorization + " must appear in header",
				Name = HttpContextItemKeys.Authorization,
				Type = SecuritySchemeType.ApiKey,
				In = ParameterLocation.Header
			});
			options.AddSecurityRequirement(new OpenApiSecurityRequirement()
			{
				{
					new OpenApiSecurityScheme()
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id =  HttpContextItemKeys.Authorization
						},
						In = ParameterLocation.Header
					},
					Array.Empty<string>()
				}
			});
			options.OperationFilter<SecurityRequirementsOperationFilter>();
		});
		#endregion

		services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();

		return services;
	}
}
