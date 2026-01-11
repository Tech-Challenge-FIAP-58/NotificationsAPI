using FCG.Core.Messages;
using FCG.Notifications.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FCG.Notifications.Data
{
	public class NotificationContext : DbContext
	{
		public NotificationContext(DbContextOptions<NotificationContext> options)
			: base(options)
		{
			ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			ChangeTracker.AutoDetectChangesEnabled = false;
		}

		public DbSet<Notification> Notifications { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Ignore<ValidationResult>();
			modelBuilder.Ignore<Event>();

			foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
				e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
				property.SetColumnType("varchar(100)");

			foreach (var relationship in modelBuilder.Model.GetEntityTypes()
				.SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationContext).Assembly);
		}
	}
}
