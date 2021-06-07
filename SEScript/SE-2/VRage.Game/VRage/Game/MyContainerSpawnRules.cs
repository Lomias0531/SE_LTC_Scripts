using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyContainerSpawnRules
	{
		protected class VRage_Game_MyContainerSpawnRules_003C_003ECanSpawnInSpace_003C_003EAccessor : IMemberAccessor<MyContainerSpawnRules, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyContainerSpawnRules owner, in bool value)
			{
				owner.CanSpawnInSpace = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyContainerSpawnRules owner, out bool value)
			{
				value = owner.CanSpawnInSpace;
			}
		}

		protected class VRage_Game_MyContainerSpawnRules_003C_003ECanSpawnInAtmosphere_003C_003EAccessor : IMemberAccessor<MyContainerSpawnRules, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyContainerSpawnRules owner, in bool value)
			{
				owner.CanSpawnInAtmosphere = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyContainerSpawnRules owner, out bool value)
			{
				value = owner.CanSpawnInAtmosphere;
			}
		}

		protected class VRage_Game_MyContainerSpawnRules_003C_003ECanSpawnOnMoon_003C_003EAccessor : IMemberAccessor<MyContainerSpawnRules, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyContainerSpawnRules owner, in bool value)
			{
				owner.CanSpawnOnMoon = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyContainerSpawnRules owner, out bool value)
			{
				value = owner.CanSpawnOnMoon;
			}
		}

		protected class VRage_Game_MyContainerSpawnRules_003C_003ECanBePersonal_003C_003EAccessor : IMemberAccessor<MyContainerSpawnRules, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyContainerSpawnRules owner, in bool value)
			{
				owner.CanBePersonal = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyContainerSpawnRules owner, out bool value)
			{
				value = owner.CanBePersonal;
			}
		}

		protected class VRage_Game_MyContainerSpawnRules_003C_003ECanBeCompetetive_003C_003EAccessor : IMemberAccessor<MyContainerSpawnRules, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyContainerSpawnRules owner, in bool value)
			{
				owner.CanBeCompetetive = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyContainerSpawnRules owner, out bool value)
			{
				value = owner.CanBeCompetetive;
			}
		}

		private class VRage_Game_MyContainerSpawnRules_003C_003EActor : IActivator, IActivator<MyContainerSpawnRules>
		{
			private sealed override object CreateInstance()
			{
				return new MyContainerSpawnRules();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContainerSpawnRules CreateInstance()
			{
				return new MyContainerSpawnRules();
			}

			MyContainerSpawnRules IActivator<MyContainerSpawnRules>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1, IsRequired = false)]
		public bool CanSpawnInSpace = true;

		[ProtoMember(4, IsRequired = false)]
		public bool CanSpawnInAtmosphere = true;

		[ProtoMember(7, IsRequired = false)]
		public bool CanSpawnOnMoon = true;

		[ProtoMember(10, IsRequired = false)]
		public bool CanBePersonal = true;

		[ProtoMember(13, IsRequired = false)]
		public bool CanBeCompetetive = true;
	}
}
