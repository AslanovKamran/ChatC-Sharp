using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatClient.Models
{
	public class NetworkData : INotifyPropertyChanged
	{
		#region PropertyChanged Realization
		public event PropertyChangedEventHandler? PropertyChanged;
		private void InvokePropChanged([CallerMemberName] string? prop = default)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
		#endregion

		private int port = 8080;
		private string ip = "127.0.0.1";

		public int Port
		{
			get { return port; }
			set { port = value; InvokePropChanged(); }
		}
		public string Ip
		{
			get { return ip; }
			set { ip = value; InvokePropChanged(); }
		}

	}
}
