using System.Collections.Generic;
using System.Configuration;
using System.Net;
using ECommon.Configurations;
using ECommon.Extensions;
using EQueue.Broker;
using EQueue.Configurations;
using EQueue.Utils;
using ECommonConfiguration = ECommon.Configurations.Configuration;
namespace EQueue.BrokerServer
{
	public class Bootstrap
	{
		private BrokerController _brokerController;

		public void Initialize()
		{
			InitializeEQueue();
			//NameServer 地址和端口
			var nameServerAddress = IPAddress.Parse(ConfigurationManager.AppSettings["nameServerAddress"]);
			var nameServerPort = int.Parse(ConfigurationManager.AppSettings["nameServerPort"]);
			//本机IP
			var ipAddress = IPAddress.Parse(ConfigurationManager.AppSettings["ipAddress"]);
			var setting = new BrokerSetting(
				bool.Parse(ConfigurationManager.AppSettings["isMemoryMode"]),
				ConfigurationManager.AppSettings["fileStoreRootPath"],
				chunkCacheMaxPercent: 95,
				chunkFlushInterval: int.Parse(ConfigurationManager.AppSettings["flushInterval"]),
				messageChunkDataSize: int.Parse(ConfigurationManager.AppSettings["chunkSize"])*1024*1024,
				chunkWriteBuffer: int.Parse(ConfigurationManager.AppSettings["chunkWriteBuffer"])*1024,
				enableCache: bool.Parse(ConfigurationManager.AppSettings["enableCache"]),
				chunkCacheMinPercent: int.Parse(ConfigurationManager.AppSettings["chunkCacheMinPercent"]),
				syncFlush: bool.Parse(ConfigurationManager.AppSettings["syncFlush"]),
				messageChunkLocalCacheSize: 30*10000,
				queueChunkLocalCacheSize: 10000)
			{
				NotifyWhenMessageArrived = bool.Parse(ConfigurationManager.AppSettings["notifyWhenMessageArrived"]),
				MessageWriteQueueThreshold = int.Parse(ConfigurationManager.AppSettings["messageWriteQueueThreshold"])
			};
			setting.NameServerList = new List<IPEndPoint> {new IPEndPoint(nameServerAddress, nameServerPort)};
			setting.BrokerInfo.BrokerName = ConfigurationManager.AppSettings["brokerName"];
			setting.BrokerInfo.GroupName = ConfigurationManager.AppSettings["groupName"];
			setting.BrokerInfo.ProducerAddress =
				new IPEndPoint(ipAddress, int.Parse(ConfigurationManager.AppSettings["producerPort"])).ToAddress();
			setting.BrokerInfo.ConsumerAddress =
				new IPEndPoint(ipAddress, int.Parse(ConfigurationManager.AppSettings["consumerPort"])).ToAddress();
			setting.BrokerInfo.AdminAddress =
				new IPEndPoint(ipAddress, int.Parse(ConfigurationManager.AppSettings["adminPort"])).ToAddress();
			_brokerController = BrokerController.Create(setting);
		}


		public void Start()
		{
			_brokerController.Start();
		}

		public void Shutdown()
		{
			_brokerController.Shutdown();
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
				.RegisterEQueueComponents()
				.UseDeleteMessageByCountStrategy(10);
		}
	}
}
