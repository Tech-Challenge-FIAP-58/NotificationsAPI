using FCG.Notifications.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FCG.Notifications.Configuration
{
	public static class DependencyInjectionConfig
	{
		public static void RegisterServices(this HostApplicationBuilder builder)
		{
			builder.Services.AddDbContext<NotificationContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
		}
	}
}
