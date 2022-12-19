

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatClient.Models
{
	public class Message:INotifyPropertyChanged
	{
		#region PropertyChanged Realization
		public event PropertyChangedEventHandler? PropertyChanged;
		private void InvokePropChanged([CallerMemberName] string? prop = default)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
		#endregion

		private string content="Type your message here...";
		public string Content
		{
			get { return content; }
			set { content = value; InvokePropChanged(); }
		}

		private string type = "Message";

		public string Type
		{
			get { return type; }
			set { type = value; InvokePropChanged(); }
		}

		public Message(){}

		public Message(string type, string content)
		{
			Type = type;
			Content = content;
		}
	}
}
