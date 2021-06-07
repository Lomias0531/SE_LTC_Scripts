using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyObjectSeedParams
	{
		protected class VRage_Game_MyObjectSeedParams_003C_003EIndex_003C_003EAccessor : IMemberAccessor<MyObjectSeedParams, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectSeedParams owner, in int value)
			{
				owner.Index = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectSeedParams owner, out int value)
			{
				value = owner.Index;
			}
		}

		protected class VRage_Game_MyObjectSeedParams_003C_003ESeed_003C_003EAccessor : IMemberAccessor<MyObjectSeedParams, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectSeedParams owner, in int value)
			{
				owner.Seed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectSeedParams owner, out int value)
			{
				value = owner.Seed;
			}
		}

		protected class VRage_Game_MyObjectSeedParams_003C_003EType_003C_003EAccessor : IMemberAccessor<MyObjectSeedParams, MyObjectSeedType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectSeedParams owner, in MyObjectSeedType value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectSeedParams owner, out MyObjectSeedType value)
			{
				value = owner.Type;
			}
		}

		protected class VRage_Game_MyObjectSeedParams_003C_003EGenerated_003C_003EAccessor : IMemberAccessor<MyObjectSeedParams, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectSeedParams owner, in bool value)
			{
				owner.Generated = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectSeedParams owner, out bool value)
			{
				value = owner.Generated;
			}
		}

		protected class VRage_Game_MyObjectSeedParams_003C_003Em_proxyId_003C_003EAccessor : IMemberAccessor<MyObjectSeedParams, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectSeedParams owner, in int value)
			{
				owner.m_proxyId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectSeedParams owner, out int value)
			{
				value = owner.m_proxyId;
			}
		}

		protected class VRage_Game_MyObjectSeedParams_003C_003EGeneratorSeed_003C_003EAccessor : IMemberAccessor<MyObjectSeedParams, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectSeedParams owner, in int value)
			{
				owner.GeneratorSeed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectSeedParams owner, out int value)
			{
				value = owner.GeneratorSeed;
			}
		}

		private class VRage_Game_MyObjectSeedParams_003C_003EActor : IActivator, IActivator<MyObjectSeedParams>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectSeedParams();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectSeedParams CreateInstance()
			{
				return new MyObjectSeedParams();
			}

			MyObjectSeedParams IActivator<MyObjectSeedParams>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public int Index;

		[ProtoMember(4)]
		public int Seed;

		[ProtoMember(7)]
		public MyObjectSeedType Type;

		[ProtoMember(10)]
		public bool Generated;

		[ProtoMember(13)]
		public int m_proxyId = -1;

		[ProtoMember(16, IsRequired = false)]
		public int GeneratorSeed;

		public override bool Equals(object obj)
		{
			MyObjectSeedParams myObjectSeedParams = obj as MyObjectSeedParams;
			if (Seed == myObjectSeedParams.Seed && Index == myObjectSeedParams.Index)
			{
				return GeneratorSeed == myObjectSeedParams.GeneratorSeed;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Seed;
		}
	}
}
