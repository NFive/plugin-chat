using NFive.SDK.Core.Controllers;
using NFive.SDK.Core.Input;

namespace NFive.Chat.Shared
{
	public class Configuration : ControllerConfiguration
	{
		public InputControl Hotkey { get; set; } = InputControl.MpTextChatAll; // Default to T
	}
}
