using System.Configuration;
using System.Net;
using ECommon.Configurations;
using EQueue.Configurations;
using ECommonConfiguration = ECommon.Configurations.Configuration;
namespace EQueue.NameServer
{
	public class BootStrap
	{
		private readonly NameServerController _nameServerController;

		public BootStrap()
		{
			InitializeEQueue();
			var ipAddress = IPAddress.Parse(ConfigurationManager.AppSettings["ipAddress"]);
			var port = int.Parse(ConfigurationManager.AppSettings["port"]);
			var autoCreateTopic = bool.Parse(ConfigurationManager.AppSettings["autoCreateTopic"]);
			var brokerInactiveMaxMilliseconds = int.Parse(ConfigurationManager.AppSettings["brokerInactiveMaxMilliseconds"]);
			var setting = new NameServerSetting()
			{
				BindingAddress = new IPEndPoint(ipAddress, port),
				AutoCreateTopic = autoCreateTopic,
				BrokerInactiveMaxMilliseconds = brokerInactiveMaxMilliseconds
			};
			_nameServerController = new NameServerController(setting);
		}

		public void Start()
		{
			_nameServerController.Start();
		}

		public void Shutdown()
		{
			_nameServerController.Shutdown();
		}

		void InitializeEQueue()
		{
			var configuration = ECommonConfiguration
				.Create()
				.UseAutofac()
				.RegisterCommonComponents()
				.UseLog4Net()
				.UseJsonNet()
				.RegisterUnhandledExceptionHandler()
				.RegisterEQueueComponents();
		}
	}
}
