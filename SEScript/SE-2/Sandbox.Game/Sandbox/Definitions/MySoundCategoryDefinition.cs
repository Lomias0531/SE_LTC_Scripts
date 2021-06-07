using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_SoundCategoryDefinition), null)]
	public class MySoundCategoryDefinition : MyDefinitionBase
	{
		public class SoundDescription
		{
			public string SoundId;

			public string SoundName;

			public MyStringId? SoundNameEnum;

			public string SoundText
			{
				get
				{
					if (!SoundNameEnum.HasValue)
					{
						return SoundName;
					}
					return MyTexts.GetString(SoundNameEnum.Value);
				}
			}

			public SoundDescription(string soundId, string soundName, MyStringId? soundNameEnum)
			{
				SoundId = soundId;
				SoundName = soundName;
				SoundNameEnum = soundNameEnum;
			}
		}

		private class Sandbox_Definitions_MySoundCategoryDefinition_003C_003EActor : IActivator, IActivator<MySoundCategoryDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MySoundCategoryDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySoundCategoryDefinition CreateInstance()
			{
				return new MySoundCategoryDefinition();
			}

			MySoundCategoryDefinition IActivator<MySoundCategoryDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<SoundDescription> Sounds;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_SoundCategoryDefinition myObjectBuilder_SoundCategoryDefinition = builder as MyObjectBuilder_SoundCategoryDefinition;
			Sounds = new List<SoundDescription>();
			if (myObjectBuilder_SoundCategoryDefinition.Sounds == null)
			{
				return;
			}
			MyObjectBuilder_SoundCategoryDefinition.SoundDesc[] sounds = myObjectBuilder_SoundCategoryDefinition.Sounds;
			for (int i = 0; i < sounds.Length; i++)
			{
				MyObjectBuilder_SoundCategoryDefinition.SoundDesc soundDesc = sounds[i];
				MyStringId orCompute = MyStringId.GetOrCompute(soundDesc.SoundName);
				if (MyTexts.Exists(orCompute))
				{
					Sounds.Add(new SoundDescription(soundDesc.Id, soundDesc.SoundName, orCompute));
				}
				else
				{
					Sounds.Add(new SoundDescription(soundDesc.Id, soundDesc.SoundName, null));
				}
			}
		}
	}
}
