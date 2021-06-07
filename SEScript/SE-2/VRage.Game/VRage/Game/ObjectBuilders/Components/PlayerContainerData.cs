using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Components
{
	[ProtoContract]
	public class PlayerContainerData
	{
		protected class VRage_Game_ObjectBuilders_Components_PlayerContainerData_003C_003EPlayerId_003C_003EAccessor : IMemberAccessor<PlayerContainerData, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlayerContainerData owner, in long value)
			{
				owner.PlayerId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlayerContainerData owner, out long value)
			{
				value = owner.PlayerId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_PlayerContainerData_003C_003ETimer_003C_003EAccessor : IMemberAccessor<PlayerContainerData, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlayerContainerData owner, in int value)
			{
				owner.Timer = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlayerContainerData owner, out int value)
			{
				value = owner.Timer;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_PlayerContainerData_003C_003EActive_003C_003EAccessor : IMemberAccessor<PlayerContainerData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlayerContainerData owner, in bool value)
			{
				owner.Active = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlayerContainerData owner, out bool value)
			{
				value = owner.Active;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_PlayerContainerData_003C_003ECompetetive_003C_003EAccessor : IMemberAccessor<PlayerContainerData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlayerContainerData owner, in bool value)
			{
				owner.Competetive = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlayerContainerData owner, out bool value)
			{
				value = owner.Competetive;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_PlayerContainerData_003C_003EContainerId_003C_003EAccessor : IMemberAccessor<PlayerContainerData, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlayerContainerData owner, in long value)
			{
				owner.ContainerId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlayerContainerData owner, out long value)
			{
				value = owner.ContainerId;
			}
		}

		private class VRage_Game_ObjectBuilders_Components_PlayerContainerData_003C_003EActor : IActivator, IActivator<PlayerContainerData>
		{
			private sealed override object CreateInstance()
			{
				return new PlayerContainerData();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override PlayerContainerData CreateInstance()
			{
				return new PlayerContainerData();
			}

			PlayerContainerData IActivator<PlayerContainerData>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(13)]
		public long PlayerId;

		[ProtoMember(16)]
		public int Timer;

		[ProtoMember(19)]
		public bool Active;

		[ProtoMember(22)]
		public bool Competetive;

		[ProtoMember(25)]
		public long ContainerId;

		public PlayerContainerData()
		{
		}

		public PlayerContainerData(long player, int timer, bool active, bool competetive, long container)
		{
			PlayerId = player;
			Timer = timer;
			Active = active;
			Competetive = competetive;
			ContainerId = container;
		}
	}
}
