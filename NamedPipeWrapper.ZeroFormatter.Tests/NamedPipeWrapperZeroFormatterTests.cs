using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NamedPipeWrapper.ZeroFormatter.Tests
{
	public class NamedPipeWrapperZeroFormatterTests
	{

		[Fact]
		public void Test1()
		{
			var message = new TestMessage
			{
				DecimalValue = 123456789,
				StringValue = "abcdefghijklmnopqrstuvwxyz" + "123456789" + "abcdefghijklmnopqrstuvwxyz".ToUpper(),
				IntValue = -123456789
			};

			const int timeout = 3 * 1000;// ms

			const string pipeName = "pipeTest1";

			NamedPipeWrapperZeroFormatter.Initialize();

			NamedPipeServer<TestMessage> server = null;

			NamedPipeClient<TestMessage> client = null;

			try
			{
				server = new NamedPipeServer<TestMessage>(pipeName);

				var server_ClientConnected = new ManualResetEvent(false);
				var server_ClientDisconnected = new ManualResetEvent(false);
				var server_ClientMessage = new ManualResetEvent(false);

				server.ClientMessage += (connnection, m) =>
				{
					server_ClientMessage.Set();
					Assert.Equal(message, m);
					connnection.PushMessage(message);
					connnection.Close();
				};
				server.ClientConnected += connnection => { server_ClientConnected.Set();};
				server.ClientDisconnected += connnection => { server_ClientDisconnected.Set(); };
				server.Error += e => Assert.False(true, "Server: " + e.Message);


				client = new NamedPipeClient<TestMessage>(pipeName);

				var client_ServerMessage = new ManualResetEvent(false);
				var client_Connected = new ManualResetEvent(false);
				var client_Disconnected = new ManualResetEvent(false);

				client.Connected += connnection => { client_Connected.Set(); };
				client.ServerMessage += (connnection, m) =>
				{
					client_ServerMessage.Set();

					Assert.Equal(message, m);
					connnection.Close();
				};
				client.Disconnected += connnection => { client_Disconnected.Set(); };
				client.Error += e => Assert.False(true, "Klient: "+ e.Message);

				server.Start();
				client.Start();

				Assert.True(client.WaitForConnection(timeout), "client.WaitForConnection Failed");
				Assert.True(client.PushMessage(message), "client.PushMessage Failed");


				Assert.True(client_Connected.WaitOne(timeout), "Timeout: client.Connected");
				Assert.True(server_ClientConnected.WaitOne(timeout), "Timeout: server.ClientConnected");

				Assert.True(server_ClientMessage.WaitOne(timeout), "Timeout: server.ClientMessage");
				Assert.True(client_ServerMessage.WaitOne(timeout), "Timeout: client.ServerMessage ");

				Assert.True(server_ClientDisconnected.WaitOne(timeout), "Timeout: server.ClientDisconnected");
				Assert.True(client_Disconnected.WaitOne(timeout), "Timeout: client.Disconnected");


			}
			finally 
			{
				client?.Stop();
				server?.Stop();
			}
		}

	}
}
