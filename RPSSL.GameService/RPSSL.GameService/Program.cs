using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RPSSL.GameService.Application._Common;
using RPSSL.GameService.Common.Configurations.Settings;
using RPSSL.GameService.Configurations;
using RPSSL.GameService.Domain.Models;
using RPSSL.GameService.Endpoints.Base;
using RPSSL.GameService.Extensions;
using RPSSL.GameService.Infrastructure;
using RPSSL.GameService.Infrastructure.Persistence;
using RPSSL.GameService.Middleware;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.AddHttpContextAccessor();

#region Add extension services
builder.Services
	.AddServerServices(builder.Configuration)
	.AddApplicationServices()
	.AddInfrastructureServices(builder.Configuration);

#endregion

#region Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireUppercase = true;
	options.Password.RequiredLength = 8;

	options.User.RequireUniqueEmail = true;

	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.MaxFailedAccessAttempts = 5;

	options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager<SignInManager<User>>()
.AddUserManager<UserManager<User>>()
.AddRoles<IdentityRole>()
.AddRoleManager<RoleManager<IdentityRole>>()
.AddDefaultTokenProviders();
#endregion

#region Logging
builder.Host.UseSerilog((ctx, lc) => lc
	.ReadFrom.Configuration(builder.Configuration)
	.Destructure.ToMaximumDepth(2)
	.Destructure.ToMaximumStringLength(10000)
	.Destructure.ToMaximumCollectionCount(2));
#endregion

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
	options.SwaggerEndpoint(Constants.SwaggerEndpoint, Constants.SwaggerVersion);
	options.InjectStylesheet(Constants.SwaggerStyles);
});

app.UseRouting();

app.UseCors(Constants.CorsPolicy);

#region Custom middlewares
app.UseMiddleware<LogRequestsMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();
var jwtOptions = app.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();
if (jwtOptions is not null)
{
	var tokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = false,
		ValidIssuer = jwtOptions.Issuer,
		ValidateAudience = false,
		ValidAudience = jwtOptions.Audience,
		ValidateLifetime = false,
		ValidateIssuerSigningKey = false,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.SecretKey)),
		ClockSkew = TimeSpan.Zero,
	};
	app.UseMiddleware<JwtTokenValidationMiddleware>(tokenValidationParameters);
}
app.UseMiddleware<JwtValidationMiddleware>();
#endregion

app.UseAuthentication();
app.UseAuthorization();

app.UseHsts();
app.UseHttpsRedirection();

await app.ApplyInitialization();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints<Program>();

app.Run();
