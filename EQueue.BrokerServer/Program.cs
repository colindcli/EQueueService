using Topshelf;

namespace EQueue.BrokerServer
{
	class Program
	{
		static void Main(string[] args)
		{
			HostFactory.Run(x =>
			{
				x.RunAsLocalService();
				x.StartAutomatically();
				x.SetDescription("EQueue Broker Service");
				x.SetDisplayName("EQueueBrokerServer");
				x.SetServiceName("EQueueBrokerServer");
				x.Service<BootStrap>(s =>
				{
					s.ConstructUsing(b => new BootStrap());
					s.WhenStarted(tc => tc.Start());
					s.WhenStopped(tc => tc.Shutdown());
				});

			});
		}
	}
}
