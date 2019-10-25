using NFive.SDK.Client.Interface;

namespace NFive.Chat.Client.Overlays
{
	public class MessageEventArgs : OverlayEventArgs
	{
		public string Message { get; }

		public MessageEventArgs(Overlay overlay, string message) : base(overlay)
		{
			this.Message = message;
		}
	}
}
