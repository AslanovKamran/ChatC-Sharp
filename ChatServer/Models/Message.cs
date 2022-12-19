

namespace ChatServer.Models
{
	internal class Message
	{
		public string Content { get; set; } = string.Empty;
		public string Type { get; set; } = string.Empty;

		public override string ToString()
		{
			return $"{Type} - {Content}";
		}
	}
}
