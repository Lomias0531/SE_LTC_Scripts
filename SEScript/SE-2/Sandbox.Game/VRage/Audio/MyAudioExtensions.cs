using Sandbox.Definitions;
using System.Linq;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Utils;

namespace VRage.Audio
{
	public static class MyAudioExtensions
	{
		public static readonly MySoundErrorDelegate OnSoundError = delegate(MySoundData cue, string message)
		{
			MyAudioDefinition soundDefinition = MyDefinitionManager.Static.GetSoundDefinition(cue.SubtypeId);
			MyDefinitionErrors.Add((soundDefinition != null) ? soundDefinition.Context : MyModContext.UnknownContext, message, TErrorSeverity.Error);
		};

		public static MyCueId GetCueId(this IMyAudio self, string cueName)
		{
			if (self == null || !MyStringHash.TryGet(cueName, out MyStringHash id))
			{
				id = MyStringHash.NullOrEmpty;
			}
			return new MyCueId(id);
		}

		internal static ListReader<MySoundData> GetSoundDataFromDefinitions()
		{
			return (from x in MyDefinitionManager.Static.GetSoundDefinitions()
				where x.Enabled
				select x.SoundData).ToList();
		}

		internal static ListReader<MyAudioEffect> GetEffectData()
		{
			return (from x in MyDefinitionManager.Static.GetAudioEffectDefinitions()
				select x.Effect).ToList();
		}
	}
}
