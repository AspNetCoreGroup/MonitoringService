using System;
namespace MonitoringCommonLibrary.Interfaces.BackgroundServices
{
	public interface IDataEventsService
	{
        Task WorkAsync(CancellationToken stoppingToken);
    }
}

