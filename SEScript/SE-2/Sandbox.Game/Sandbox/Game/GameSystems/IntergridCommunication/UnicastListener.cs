using Sandbox.ModAPI.Ingame;

namespace Sandbox.Game.GameSystems.IntergridCommunication
{
	internal class UnicastListener : MyMessageListener, IMyUnicastListener, IMyMessageProvider
	{
		public UnicastListener(MyIntergridCommunicationContext context)
			: base(context)
		{
		}
	}
}
