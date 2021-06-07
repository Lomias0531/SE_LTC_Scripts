using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Utils;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// This structure contains all information about damage being done.
	/// </summary>
	[ProtoContract]
	public struct MyDamageInformation
	{
		protected class VRage_Game_ModAPI_MyDamageInformation_003C_003EIsDeformation_003C_003EAccessor : IMemberAccessor<MyDamageInformation, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDamageInformation owner, in bool value)
			{
				owner.IsDeformation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDamageInformation owner, out bool value)
			{
				value = owner.IsDeformation;
			}
		}

		protected class VRage_Game_ModAPI_MyDamageInformation_003C_003EAmount_003C_003EAccessor : IMemberAccessor<MyDamageInformation, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDamageInformation owner, in float value)
			{
				owner.Amount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDamageInformation owner, out float value)
			{
				value = owner.Amount;
			}
		}

		protected class VRage_Game_ModAPI_MyDamageInformation_003C_003EType_003C_003EAccessor : IMemberAccessor<MyDamageInformation, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDamageInformation owner, in MyStringHash value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDamageInformation owner, out MyStringHash value)
			{
				value = owner.Type;
			}
		}

		protected class VRage_Game_ModAPI_MyDamageInformation_003C_003EAttackerId_003C_003EAccessor : IMemberAccessor<MyDamageInformation, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDamageInformation owner, in long value)
			{
				owner.AttackerId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDamageInformation owner, out long value)
			{
				value = owner.AttackerId;
			}
		}

		private class VRage_Game_ModAPI_MyDamageInformation_003C_003EActor : IActivator, IActivator<MyDamageInformation>
		{
			private sealed override object CreateInstance()
			{
				return default(MyDamageInformation);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDamageInformation CreateInstance()
			{
				return (MyDamageInformation)(object)default(MyDamageInformation);
			}

			MyDamageInformation IActivator<MyDamageInformation>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public bool IsDeformation;

		[ProtoMember(4)]
		public float Amount;

		[ProtoMember(7)]
		public MyStringHash Type;

		[ProtoMember(10)]
		public long AttackerId;

		public MyDamageInformation(bool isDeformation, float amount, MyStringHash type, long attackerId)
		{
			IsDeformation = isDeformation;
			Amount = amount;
			Type = type;
			AttackerId = attackerId;
		}
	}
}
