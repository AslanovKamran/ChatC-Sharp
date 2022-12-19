using ChatClient.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChatClient
{

	public partial class MainWindow : Window
	{
		public NetworkData Data { get; set; } = new();
		public User User { get; set; } = new();
		public Message Message { get; set; } = new();

		readonly TcpClient client = new();
		StreamWriter? writer;

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;


			#region Focus Events

			ipTextBox.GotFocus += ControlGotFocus;
			portTextBox.GotFocus += ControlGotFocus;
			userNameTextBox.GotFocus += ControlGotFocus;
			messageTextBox.GotFocus += ControlGotFocus;

			ipTextBox.LostFocus += ControlLostFocus;
			portTextBox.LostFocus += ControlLostFocus;
			userNameTextBox.LostFocus += ControlLostFocus;
			messageTextBox.LostFocus += ControlLostFocus;

			#endregion

		}

		#region Placeholders imitation
		private void ControlGotFocus(object sender, RoutedEventArgs e)
		{
			var control = sender as TextBox;
			switch (control?.Name)
			{
				case "ipTextBox": { if (ipTextBox.Text == "IP") ipTextBox.Text = ""; } break;
				case "portTextBox": { if (portTextBox.Text == "Port") portTextBox.Text = ""; } break;
				case "userNameTextBox": { if (userNameTextBox.Text == "User Name") userNameTextBox.Text = ""; } break;
				case "messageTextBox": { if (messageTextBox.Text == "Type your message here...") messageTextBox.Text = ""; } break;
				default: { MessageBox.Show("Something vent worng"); } break;
			}
		}
		private void ControlLostFocus(object sender, RoutedEventArgs e)
		{
			var control = sender as TextBox;
			switch (control?.Name)
			{
				case "ipTextBox": { if (String.IsNullOrWhiteSpace(ipTextBox.Text)) { ipTextBox.Text = "IP"; } } break;
				case "portTextBox": { if (String.IsNullOrWhiteSpace(portTextBox.Text)) { portTextBox.Text = "Port"; } } break;
				case "userNameTextBox": { if (String.IsNullOrWhiteSpace(userNameTextBox.Text)) { userNameTextBox.Text = "User Name"; } } break;
				case "messageTextBox": { if (String.IsNullOrWhiteSpace(messageTextBox.Text)) { messageTextBox.Text = "Type your message here..."; } } break;
				default: { MessageBox.Show("Something vent worng"); } break;
			}

			if (String.IsNullOrWhiteSpace(ipTextBox.Text)) { ipTextBox.Text = "IP"; }
		}
		#endregion

		private async void ConnectButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				IPEndPoint endPoint = new(IPAddress.Parse(Data.Ip), Data.Port);
				await client.ConnectAsync(endPoint);
				MessageBox.Show("Connected! You can send a message now.");
				writer = new(client.GetStream());
				writer.AutoFlush = true;

				var msg = new Message("Init", User.UserName);
				var json = JsonConvert.SerializeObject(msg);
				await writer.WriteLineAsync(json);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private async void SendButton_Click(object sender, RoutedEventArgs? e)
		{
			if (writer is not null)
			{
				if (!String.IsNullOrWhiteSpace(Message.Content) && Message.Content != "Type your message here...")
				{
					
					var json = JsonConvert.SerializeObject(Message);
					await writer.WriteLineAsync(json);
				}
				Message.Content = "Type your message here...";
			}
			else
			{
				MessageBox.Show("Connect First!");
			}
		}

		
	}
}
