using VRage.Data.Audio;
using VRage.Game.Definitions;
using VRage.Network;

namespace VRage.Game
{
	[MyDefinitionType(typeof(MyObjectBuilder_AudioDefinition), null)]
	public class MyAudioDefinition : MyDefinitionBase
	{
		private class VRage_Game_MyAudioDefinition_003C_003EActor : IActivator, IActivator<MyAudioDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAudioDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAudioDefinition CreateInstance()
			{
				return new MyAudioDefinition();
			}

			MyAudioDefinition IActivator<MyAudioDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MySoundData SoundData;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_AudioDefinition myObjectBuilder_AudioDefinition = builder as MyObjectBuilder_AudioDefinition;
			SoundData = myObjectBuilder_AudioDefinition.SoundData;
			SoundData.SubtypeId = Id.SubtypeId;
			if (SoundData.Loopable)
			{
				bool flag = true;
				for (int i = 0; i < SoundData.Waves.Count; i++)
				{
					flag &= (SoundData.Waves[i].Loop != null);
				}
			}
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_AudioDefinition obj = (MyObjectBuilder_AudioDefinition)base.GetObjectBuilder();
			obj.SoundData = SoundData;
			return obj;
		}
	}
}
