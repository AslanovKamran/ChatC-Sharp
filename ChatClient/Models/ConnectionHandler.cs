using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatClient.Models
{
	public class ConnectionHandler : INotifyPropertyChanged
	{
		#region PropertyChanged Realization
		public event PropertyChangedEventHandler? PropertyChanged;
		private void InvokePropChanged([CallerMemberName] string? prop = default)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
		#endregion

		private bool connected = false;
		public bool Connected
		{
			get { return connected; }
			set { connected = value; InvokePropChanged(); }
		}

	}
}
