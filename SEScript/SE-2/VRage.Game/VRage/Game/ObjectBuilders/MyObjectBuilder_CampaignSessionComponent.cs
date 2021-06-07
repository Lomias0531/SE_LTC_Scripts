using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_CampaignSessionComponent : MyObjectBuilder_SessionComponent
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent_003C_003ECampaignName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in string value)
			{
				owner.CampaignName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out string value)
			{
				value = owner.CampaignName;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent_003C_003EActiveState_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in string value)
			{
				owner.ActiveState = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out string value)
			{
				value = owner.ActiveState;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent_003C_003EIsVanilla_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in bool value)
			{
				owner.IsVanilla = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out bool value)
			{
				value = owner.IsVanilla;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent_003C_003EMod_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, MyObjectBuilder_Checkpoint.ModItem>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in MyObjectBuilder_Checkpoint.ModItem value)
			{
				owner.Mod = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out MyObjectBuilder_Checkpoint.ModItem value)
			{
				value = owner.Mod;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent_003C_003ELocalModFolder_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in string value)
			{
				owner.LocalModFolder = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out string value)
			{
				value = owner.LocalModFolder;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent_003C_003ECurrentOutcome_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in string value)
			{
				owner.CurrentOutcome = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out string value)
			{
				value = owner.CurrentOutcome;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_SessionComponent*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_SessionComponent*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent_003C_003EActor : IActivator, IActivator<MyObjectBuilder_CampaignSessionComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_CampaignSessionComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_CampaignSessionComponent CreateInstance()
			{
				return new MyObjectBuilder_CampaignSessionComponent();
			}

			MyObjectBuilder_CampaignSessionComponent IActivator<MyObjectBuilder_CampaignSessionComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string CampaignName;

		public string ActiveState;

		public bool IsVanilla;

		public MyObjectBuilder_Checkpoint.ModItem Mod;

		public string LocalModFolder;

		public string CurrentOutcome;
	}
}
