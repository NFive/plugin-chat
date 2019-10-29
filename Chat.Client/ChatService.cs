using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using JetBrains.Annotations;
using NFive.Chat.Client.Overlays;
using NFive.Chat.Shared;
using NFive.SDK.Client.Commands;
using NFive.SDK.Client.Communications;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Input;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Chat;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Events;
using NFive.SDK.Core.Models.Player;

namespace NFive.Chat.Client
{
	[PublicAPI]
	public class ChatService : Service
	{
		private Configuration config;
		private ChatOverlay overlay;

		public ChatService(ILogger logger, ITickManager ticks, ICommunicationManager comms, ICommandManager commands, IOverlayManager overlay, User user) : base(logger, ticks, comms, commands, overlay, user) { }

		public override Task Loaded()
		{
			// Disable stock GTA chat UI
			API.SetTextChatEnabled(false);

			return base.Loaded();
		}

		public override async Task Started()
		{
			// Request server configuration
			this.config = await this.Comms.Event(ChatEvents.Configuration).ToServer().Request<Configuration>();
			var hotkey = new Hotkey(this.config.Hotkey);

			// Create overlay
			this.overlay = new ChatOverlay(this.OverlayManager);

			this.overlay.MessageEntered += (s, a) =>
			{
				this.Comms.Event(CoreEvents.ChatSendMessage).ToServer().Emit(new ChatMessage
				{
					Content = a.Message
				});
			};

			// Listen for server messages
			this.Comms.Event(CoreEvents.ChatSendMessage).FromServer().On<ChatMessage>((e, message) =>
			{
				this.overlay.AddMessage(message);
			});

			// Attach a tick handler
			this.Ticks.On(() =>
			{
				if (!hotkey.IsJustPressed()) return;

				// Show chat overlay
				this.overlay.Open();
				API.SetNuiFocus(true, false);
			});
		}
	}
}
