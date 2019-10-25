using System;
using NFive.SDK.Client.Interface;
using NFive.SDK.Core.Chat;

namespace NFive.Chat.Client.Overlays
{
	public class ChatOverlay : Overlay
	{
		public event EventHandler<MessageEventArgs> MessageEntered;

		public ChatOverlay(IOverlayManager manager) : base(manager)
		{
			On("message", new Action<string>(message => this.MessageEntered?.Invoke(this, new MessageEventArgs(this, message))));

			On("blur", Blur);
		}

		protected override dynamic Ready() => null;

		public void AddMessage(ChatMessage message)
		{
			Emit("add-message", message.Content);
		}

		public void Open()
		{
			Emit("open");
		}
	}
}
