using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Definitions
{
	/// <summary>
	/// Module upgrade information
	/// </summary>
	[ProtoContract]
	public struct MyUpgradeModuleInfo
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyUpgradeModuleInfo_003C_003EUpgradeType_003C_003EAccessor : IMemberAccessor<MyUpgradeModuleInfo, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyUpgradeModuleInfo owner, in string value)
			{
				owner.UpgradeType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyUpgradeModuleInfo owner, out string value)
			{
				value = owner.UpgradeType;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyUpgradeModuleInfo_003C_003EModifier_003C_003EAccessor : IMemberAccessor<MyUpgradeModuleInfo, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyUpgradeModuleInfo owner, in float value)
			{
				owner.Modifier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyUpgradeModuleInfo owner, out float value)
			{
				value = owner.Modifier;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyUpgradeModuleInfo_003C_003EModifierType_003C_003EAccessor : IMemberAccessor<MyUpgradeModuleInfo, MyUpgradeModifierType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyUpgradeModuleInfo owner, in MyUpgradeModifierType value)
			{
				owner.ModifierType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyUpgradeModuleInfo owner, out MyUpgradeModifierType value)
			{
				value = owner.ModifierType;
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyUpgradeModuleInfo_003C_003EActor : IActivator, IActivator<MyUpgradeModuleInfo>
		{
			private sealed override object CreateInstance()
			{
				return default(MyUpgradeModuleInfo);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyUpgradeModuleInfo CreateInstance()
			{
				return (MyUpgradeModuleInfo)(object)default(MyUpgradeModuleInfo);
			}

			MyUpgradeModuleInfo IActivator<MyUpgradeModuleInfo>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		/// <summary>
		/// Name of upgrade
		/// </summary>
		[ProtoMember(1)]
		public string UpgradeType
		{
			get;
			set;
		}

		/// <summary>
		/// Modifier for upgrade (as decimal - 1 = 100%)
		/// </summary>
		[ProtoMember(4)]
		public float Modifier
		{
			get;
			set;
		}

		/// <summary>
		/// Type of modifier as <see cref="T:VRage.Game.ObjectBuilders.Definitions.MyUpgradeModifierType" />
		/// </summary>
		[ProtoMember(7)]
		public MyUpgradeModifierType ModifierType
		{
			get;
			set;
		}
	}
}
