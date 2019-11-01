using System;
using System.Linq;
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

			// Listen to new client messages
			comms.Event(ChatEvents.MessageEntered).FromClients().On<string>((e, message) =>
			{
				// Check if message is a command
				if (message.Trim().StartsWith("/"))
				{
					this.Logger.Debug(message.Trim().Substring(1));

					// Dispatch command
					comms.Event(CoreEvents.CommandDispatch).ToClients().Emit(message.Trim().Substring(1).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList());

					this.Logger.Debug(message.Trim().Substring(1));
				}
				else
				{
					// Un-prefixed message, send to everyone
					comms.Event(CoreEvents.ChatMessage).ToClients().Emit(new ChatMessage
					{
						Sender = e.User,
						Style = "secondary",
						Template = "default",
						Values = new[]
						{
							e.User.Name,
							message
						}
					});
				}
			});
		}
	}
}
