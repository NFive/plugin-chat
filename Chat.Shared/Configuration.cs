using NFive.SDK.Core.Controllers;
using NFive.SDK.Core.Input;

namespace NFive.Chat.Shared
{
	public class Configuration : ControllerConfiguration
	{
		public InputControl Hotkey { get; set; } = InputControl.MpTextChatAll; // Default to T

		public int HistoryLimit { get; set; } = 50;

		public string DefaultTemplate { get; set; } = "<h1><i class=\"fas fa-fw fa-star\"></i> {0}</h1>{1}";
	}
}
