using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatClient.Models
{
	public class User:INotifyPropertyChanged
	{
		#region PropertyChanged Realization
		public event PropertyChangedEventHandler? PropertyChanged;
		private void InvokePropChanged([CallerMemberName] string? prop = default)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
		#endregion

		private string userName = "Bob";
		public string UserName
		{
			get { return userName; }
			set { userName = value; InvokePropChanged(); }
		}

	}
}
