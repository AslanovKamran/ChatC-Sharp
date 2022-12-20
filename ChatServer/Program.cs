using ChatServer.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
	internal class Program
	{
		static List<User> Clients = new();

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
					var random = new Random().Next(1, 16);
					var userName = string.Empty;
					var connected = true;

					using (StreamReader reader = new(client.GetStream()))
					{
						while (connected)
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
												Clients.Add(new User
													(
														userName, 
														new StreamReader(client.GetStream()), 
														new StreamWriter(client.GetStream()))
													);
											}
											break;
										case "Message":
											{
												ShowMessage((ConsoleColor)random, $"{userName}: {message.Content}");
												await BroadCastMessage(userName +": " + message.Content);
											}
											break;

										case "Disconnect": 
											{
												var disconnected = Clients.FirstOrDefault((x) => x.Name == userName);
												if(disconnected is not null)
													Clients.Remove(disconnected);
												
												ShowMessage(ConsoleColor.Red, $"User {userName} has disconnected");
												connected = false;

											}
											break;
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

		private static async Task BroadCastMessage(string message)
		{
			foreach (var client in Clients)
			{
				await client.Writer.WriteLineAsync(message);
				
			}
		}
	}
}