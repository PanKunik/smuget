using Application.Abstractions.Time;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Time;
public static class Extensions
{
	public static IServiceCollection AddTime(this IServiceCollection services)
	{
		services.AddSingleton<IClock, Clock>();

		return services;
	}
}
