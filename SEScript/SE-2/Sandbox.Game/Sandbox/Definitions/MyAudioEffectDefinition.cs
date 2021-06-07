using System;
using System.Collections.Generic;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AudioEffectDefinition), null)]
	public class MyAudioEffectDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyAudioEffectDefinition_003C_003EActor : IActivator, IActivator<MyAudioEffectDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAudioEffectDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAudioEffectDefinition CreateInstance()
			{
				return new MyAudioEffectDefinition();
			}

			MyAudioEffectDefinition IActivator<MyAudioEffectDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyAudioEffect Effect = new MyAudioEffect();

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_AudioEffectDefinition myObjectBuilder_AudioEffectDefinition = builder as MyObjectBuilder_AudioEffectDefinition;
			Effect.EffectId = Id.SubtypeId;
			foreach (MyObjectBuilder_AudioEffectDefinition.SoundList sound in myObjectBuilder_AudioEffectDefinition.Sounds)
			{
				List<MyAudioEffect.SoundEffect> list = new List<MyAudioEffect.SoundEffect>();
				foreach (MyObjectBuilder_AudioEffectDefinition.SoundEffect soundEffect in sound.SoundEffects)
				{
					MyAudioEffect.SoundEffect item = default(MyAudioEffect.SoundEffect);
					if (!MyDefinitionManager.Static.TryGetDefinition(new MyDefinitionId(typeof(MyObjectBuilder_CurveDefinition), soundEffect.VolumeCurve), out MyCurveDefinition definition))
					{
						item.VolumeCurve = null;
					}
					else
					{
						item.VolumeCurve = definition.Curve;
					}
					item.Duration = soundEffect.Duration;
					item.Filter = soundEffect.Filter;
					item.Frequency = (float)(2.0 * Math.Sin(3.14 * (double)soundEffect.Frequency / 44100.0));
					item.OneOverQ = 1f / soundEffect.Q;
					item.StopAfter = soundEffect.StopAfter;
					list.Add(item);
				}
				Effect.SoundsEffects.Add(list);
			}
			if (myObjectBuilder_AudioEffectDefinition.OutputSound == 0)
			{
				Effect.ResultEmitterIdx = Effect.SoundsEffects.Count - 1;
			}
			else
			{
				Effect.ResultEmitterIdx = myObjectBuilder_AudioEffectDefinition.OutputSound - 1;
			}
		}
	}
}
