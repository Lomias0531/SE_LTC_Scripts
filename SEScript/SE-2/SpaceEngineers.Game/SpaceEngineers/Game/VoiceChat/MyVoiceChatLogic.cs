using Sandbox;
using Sandbox.Game.GameSystems;
using Sandbox.Game.VoiceChat;
using Sandbox.Game.World;
using VRage.Data.Audio;
using VRage.Library.Utils;

namespace SpaceEngineers.Game.VoiceChat
{
	public class MyVoiceChatLogic : IMyVoiceChatLogic
	{
		private const float VOICE_DISTANCE = 40f;

		private const float VOICE_DISTANCE_SQ = 1600f;

		public bool ShouldSendVoice(MyPlayer sender, MyPlayer receiver)
		{
			return MyAntennaSystem.Static.CheckConnection(sender.Identity, receiver.Identity);
		}

		public bool ShouldPlayVoice(MyPlayer player, MyTimeSpan timestamp, out MySoundDimensions dimension, out float maxDistance)
		{
			MyTimeSpan totalTime = MySandboxGame.Static.TotalTime;
			double num = 500.0;
			if ((totalTime - timestamp).Milliseconds > num)
			{
				dimension = MySoundDimensions.D3;
				maxDistance = float.MaxValue;
				return true;
			}
			dimension = MySoundDimensions.D2;
			maxDistance = 0f;
			return false;
		}
	}
}
