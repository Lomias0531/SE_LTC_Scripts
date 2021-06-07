using Sandbox.Game.World;
using VRage.Data.Audio;
using VRage.Library.Utils;

namespace Sandbox.Game.VoiceChat
{
	public interface IMyVoiceChatLogic
	{
		bool ShouldSendVoice(MyPlayer sender, MyPlayer receiver);

		bool ShouldPlayVoice(MyPlayer player, MyTimeSpan timestamp, out MySoundDimensions dimension, out float maxDistance);
	}
}
