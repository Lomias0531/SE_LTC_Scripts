using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ShipSoundsDefinition), null)]
	public class MyShipSoundsDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyShipSoundsDefinition_003C_003EActor : IActivator, IActivator<MyShipSoundsDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyShipSoundsDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyShipSoundsDefinition CreateInstance()
			{
				return new MyShipSoundsDefinition();
			}

			MyShipSoundsDefinition IActivator<MyShipSoundsDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float MinWeight = 3000f;

		public bool AllowSmallGrid = true;

		public bool AllowLargeGrid = true;

		public Dictionary<ShipSystemSoundsEnum, MySoundPair> Sounds = new Dictionary<ShipSystemSoundsEnum, MySoundPair>();

		public List<MyTuple<float, float>> ThrusterVolumes = new List<MyTuple<float, float>>();

		public List<MyTuple<float, float>> EngineVolumes = new List<MyTuple<float, float>>();

		public List<MyTuple<float, float>> WheelsVolumes = new List<MyTuple<float, float>>();

		public float EnginePitchRangeInSemitones = 4f;

		public float EnginePitchRangeInSemitones_h = -2f;

		public float WheelsPitchRangeInSemitones = 4f;

		public float WheelsPitchRangeInSemitones_h = -2f;

		public float ThrusterPitchRangeInSemitones = 4f;

		public float ThrusterPitchRangeInSemitones_h = -2f;

		public float EngineTimeToTurnOn = 4f;

		public float EngineTimeToTurnOff = 3f;

		public float WheelsLowerThrusterVolumeBy = 0.33f;

		public float WheelsFullSpeed = 32f;

		public float WheelsSpeedCompensation = 3f;

		public float ThrusterCompositionMinVolume = 0.4f;

		public float ThrusterCompositionMinVolume_c = 0.6666666f;

		public float ThrusterCompositionChangeSpeed = 0.025f;

		public float SpeedUpSoundChangeVolumeTo = 1f;

		public float SpeedDownSoundChangeVolumeTo = 1f;

		public float SpeedUpDownChangeSpeed = 0.2f;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ShipSoundsDefinition myObjectBuilder_ShipSoundsDefinition = builder as MyObjectBuilder_ShipSoundsDefinition;
			MinWeight = myObjectBuilder_ShipSoundsDefinition.MinWeight;
			AllowSmallGrid = myObjectBuilder_ShipSoundsDefinition.AllowSmallGrid;
			AllowLargeGrid = myObjectBuilder_ShipSoundsDefinition.AllowLargeGrid;
			EnginePitchRangeInSemitones = myObjectBuilder_ShipSoundsDefinition.EnginePitchRangeInSemitones;
			EnginePitchRangeInSemitones_h = myObjectBuilder_ShipSoundsDefinition.EnginePitchRangeInSemitones * -0.5f;
			EngineTimeToTurnOn = myObjectBuilder_ShipSoundsDefinition.EngineTimeToTurnOn;
			EngineTimeToTurnOff = myObjectBuilder_ShipSoundsDefinition.EngineTimeToTurnOff;
			WheelsLowerThrusterVolumeBy = myObjectBuilder_ShipSoundsDefinition.WheelsLowerThrusterVolumeBy;
			WheelsFullSpeed = myObjectBuilder_ShipSoundsDefinition.WheelsFullSpeed;
			ThrusterCompositionMinVolume = myObjectBuilder_ShipSoundsDefinition.ThrusterCompositionMinVolume;
			ThrusterCompositionMinVolume_c = myObjectBuilder_ShipSoundsDefinition.ThrusterCompositionMinVolume / (1f - myObjectBuilder_ShipSoundsDefinition.ThrusterCompositionMinVolume);
			ThrusterCompositionChangeSpeed = myObjectBuilder_ShipSoundsDefinition.ThrusterCompositionChangeSpeed;
			SpeedDownSoundChangeVolumeTo = myObjectBuilder_ShipSoundsDefinition.SpeedDownSoundChangeVolumeTo;
			SpeedUpSoundChangeVolumeTo = myObjectBuilder_ShipSoundsDefinition.SpeedUpSoundChangeVolumeTo;
			SpeedUpDownChangeSpeed = myObjectBuilder_ShipSoundsDefinition.SpeedUpDownChangeSpeed * 0.0166666675f;
			foreach (ShipSound sound in myObjectBuilder_ShipSoundsDefinition.Sounds)
			{
				if (sound.SoundName.Length != 0)
				{
					MySoundPair mySoundPair = new MySoundPair(sound.SoundName);
					if (mySoundPair != MySoundPair.Empty)
					{
						Sounds.Add(sound.SoundType, mySoundPair);
					}
				}
			}
			List<MyTuple<float, float>> list = new List<MyTuple<float, float>>();
			foreach (ShipSoundVolumePair thrusterVolume in myObjectBuilder_ShipSoundsDefinition.ThrusterVolumes)
			{
				list.Add(new MyTuple<float, float>(Math.Max(0f, thrusterVolume.Speed), Math.Max(0f, thrusterVolume.Volume)));
			}
			ThrusterVolumes = list.OrderBy((MyTuple<float, float> o) => o.Item1).ToList();
			List<MyTuple<float, float>> list2 = new List<MyTuple<float, float>>();
			foreach (ShipSoundVolumePair engineVolume in myObjectBuilder_ShipSoundsDefinition.EngineVolumes)
			{
				list2.Add(new MyTuple<float, float>(Math.Max(0f, engineVolume.Speed), Math.Max(0f, engineVolume.Volume)));
			}
			EngineVolumes = list2.OrderBy((MyTuple<float, float> o) => o.Item1).ToList();
			List<MyTuple<float, float>> list3 = new List<MyTuple<float, float>>();
			foreach (ShipSoundVolumePair wheelsVolume in myObjectBuilder_ShipSoundsDefinition.WheelsVolumes)
			{
				list3.Add(new MyTuple<float, float>(Math.Max(0f, wheelsVolume.Speed), Math.Max(0f, wheelsVolume.Volume)));
			}
			WheelsVolumes = list3.OrderBy((MyTuple<float, float> o) => o.Item1).ToList();
		}
	}
}
