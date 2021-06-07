using System;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyChatItem
	{
		public string Sender;

		public string Message;

		public Color SenderColor;

		public Color MessageColor;

		public string Font;

		public DateTime Timestamp;

		public MyChatItem(string sender, string message, string font, Color senderColor)
			: this(sender, message, font, senderColor, Color.White)
		{
		}

		public MyChatItem(string sender, string message, string font, Color senderColor, Color messageColor)
		{
			Sender = sender;
			Message = message;
			SenderColor = senderColor;
			MessageColor = messageColor;
			Font = font;
			Timestamp = DateTime.Now;
		}
	}
}
