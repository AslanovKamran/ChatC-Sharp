using ChatServer.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
	internal class Program
	{
		static Dictionary<string, TcpClient> Clients = new();


		static void Main(string[] args)
		{
			const string host = "127.0.0.1";
			const int port = 8080;

			IPEndPoint endPoint = new(IPAddress.Parse(host), port);
			TcpListener server = new(endPoint);
			server.Start();

			ShowMessage(ConsoleColor.Green, "Server Started");

			while (true)
			{
				TcpClient client = server.AcceptTcpClient();

				Task.Run(async () =>
				{
					var random = new Random().Next(1,16);
					var userName = string.Empty;

					using (StreamReader reader = new(client.GetStream()))
					{
						while (true)
						{
							try
							{
								var dataFromClient = await reader.ReadLineAsync();
								if (dataFromClient is not null)
								{
									var message = JsonConvert.DeserializeObject<Message>(dataFromClient);

									switch (message?.Type)
									{
										case "Init":
											{
												userName = message.Content;
												ShowMessage(ConsoleColor.DarkGreen, $"==Client {message.Content} accepted==");
												Clients.Add(message.Content, client);
											}
											break;
										case "Message": { ShowMessage((ConsoleColor)random, $"{userName}: {message.Content}"); } break;
										default: { } break;
									}
								}
								else
								{
									ShowMessage(ConsoleColor.Red, "Unexpected Error");
								}

							}
							catch (Exception ex)
							{
								ShowMessage(ConsoleColor.Red, ex.Message);
								break;
							}

						}
					}
				});
			}
		}

		private static void ShowMessage(ConsoleColor color, string message)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(message);
			Console.ResetColor();
		}
	}
}