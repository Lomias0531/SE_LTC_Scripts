using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyPlanetAnimalSpawnInfo
	{
		protected class VRage_Game_MyPlanetAnimalSpawnInfo_003C_003EAnimals_003C_003EAccessor : IMemberAccessor<MyPlanetAnimalSpawnInfo, MyPlanetAnimal[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetAnimalSpawnInfo owner, in MyPlanetAnimal[] value)
			{
				owner.Animals = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetAnimalSpawnInfo owner, out MyPlanetAnimal[] value)
			{
				value = owner.Animals;
			}
		}

		protected class VRage_Game_MyPlanetAnimalSpawnInfo_003C_003ESpawnDelayMin_003C_003EAccessor : IMemberAccessor<MyPlanetAnimalSpawnInfo, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetAnimalSpawnInfo owner, in int value)
			{
				owner.SpawnDelayMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetAnimalSpawnInfo owner, out int value)
			{
				value = owner.SpawnDelayMin;
			}
		}

		protected class VRage_Game_MyPlanetAnimalSpawnInfo_003C_003ESpawnDelayMax_003C_003EAccessor : IMemberAccessor<MyPlanetAnimalSpawnInfo, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetAnimalSpawnInfo owner, in int value)
			{
				owner.SpawnDelayMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetAnimalSpawnInfo owner, out int value)
			{
				value = owner.SpawnDelayMax;
			}
		}

		protected class VRage_Game_MyPlanetAnimalSpawnInfo_003C_003ESpawnDistMin_003C_003EAccessor : IMemberAccessor<MyPlanetAnimalSpawnInfo, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetAnimalSpawnInfo owner, in float value)
			{
				owner.SpawnDistMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetAnimalSpawnInfo owner, out float value)
			{
				value = owner.SpawnDistMin;
			}
		}

		protected class VRage_Game_MyPlanetAnimalSpawnInfo_003C_003ESpawnDistMax_003C_003EAccessor : IMemberAccessor<MyPlanetAnimalSpawnInfo, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetAnimalSpawnInfo owner, in float value)
			{
				owner.SpawnDistMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetAnimalSpawnInfo owner, out float value)
			{
				value = owner.SpawnDistMax;
			}
		}

		protected class VRage_Game_MyPlanetAnimalSpawnInfo_003C_003EKillDelay_003C_003EAccessor : IMemberAccessor<MyPlanetAnimalSpawnInfo, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetAnimalSpawnInfo owner, in int value)
			{
				owner.KillDelay = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetAnimalSpawnInfo owner, out int value)
			{
				value = owner.KillDelay;
			}
		}

		protected class VRage_Game_MyPlanetAnimalSpawnInfo_003C_003EWaveCountMin_003C_003EAccessor : IMemberAccessor<MyPlanetAnimalSpawnInfo, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetAnimalSpawnInfo owner, in int value)
			{
				owner.WaveCountMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetAnimalSpawnInfo owner, out int value)
			{
				value = owner.WaveCountMin;
			}
		}

		protected class VRage_Game_MyPlanetAnimalSpawnInfo_003C_003EWaveCountMax_003C_003EAccessor : IMemberAccessor<MyPlanetAnimalSpawnInfo, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetAnimalSpawnInfo owner, in int value)
			{
				owner.WaveCountMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetAnimalSpawnInfo owner, out int value)
			{
				value = owner.WaveCountMax;
			}
		}

		private class VRage_Game_MyPlanetAnimalSpawnInfo_003C_003EActor : IActivator, IActivator<MyPlanetAnimalSpawnInfo>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanetAnimalSpawnInfo();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetAnimalSpawnInfo CreateInstance()
			{
				return new MyPlanetAnimalSpawnInfo();
			}

			MyPlanetAnimalSpawnInfo IActivator<MyPlanetAnimalSpawnInfo>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlArrayItem("Animal")]
		public MyPlanetAnimal[] Animals;

		[ProtoMember(26)]
		public int SpawnDelayMin = 30000;

		[ProtoMember(27)]
		public int SpawnDelayMax = 60000;

		[ProtoMember(28)]
		public float SpawnDistMin = 10f;

		[ProtoMember(29)]
		public float SpawnDistMax = 140f;

		[ProtoMember(30)]
		public int KillDelay = 120000;

		[ProtoMember(31)]
		public int WaveCountMin = 1;

		[ProtoMember(32)]
		public int WaveCountMax = 5;
	}
}
