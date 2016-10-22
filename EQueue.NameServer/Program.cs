using Topshelf;

namespace EQueue.NameServer
{
	class Program
	{
		static void Main(string[] args)
		{
			HostFactory.Run(x =>
			{
				x.RunAsLocalService();
				x.StartAutomatically();
				x.SetDescription("EQueue NameServer Service");
				x.SetDisplayName("EQueueNameServer");
				x.SetServiceName("EQueueNameServer");
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
