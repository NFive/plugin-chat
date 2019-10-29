using JetBrains.Annotations;
using NFive.Chat.Shared;
using NFive.SDK.Core.Chat;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Events;
using NFive.SDK.Server.Communications;
using NFive.SDK.Server.Controllers;

namespace NFive.Chat.Server
{
	[PublicAPI]
	public class ChatController : ConfigurableController<Configuration>
	{
		public ChatController(ILogger logger, Configuration configuration, ICommunicationManager comms) : base(logger, configuration)
		{
			// Send configuration when requested
			comms.Event(ChatEvents.Configuration).FromClients().OnRequest(e => e.Reply(this.Configuration));

			comms.Event(CoreEvents.ChatSendMessage).FromClients().On<ChatMessage>((e, message) =>
			{
				comms.Event(CoreEvents.ChatSendMessage).ToClients().Emit(message);
			});
		}
	}
}
