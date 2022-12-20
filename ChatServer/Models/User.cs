

using System.Net.Sockets;

namespace ChatServer.Models
{
	public class User
	{
		public string Name { get; set; } 
		

		public StreamReader Reader { get; set; }
		public StreamWriter Writer{ get; set; }


		public User(string name, StreamReader reader, StreamWriter writer)
		{
			Name = name;
			Reader = reader;
			Writer = writer;

			Writer.AutoFlush = true;
		}

		
	}
}
