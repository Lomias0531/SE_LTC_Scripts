using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_TreeCuttingDefinition : MyObjectBuilder_CuttingDefinition
	{
		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003EEntityId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EEntityId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003EScrapWoodBranchesId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EScrapWoodBranchesId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003EScrapWoodId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EScrapWoodId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003EScrapWoodAmountMin_003C_003EAccessor : VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EScrapWoodAmountMin_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003EScrapWoodAmountMax_003C_003EAccessor : VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EScrapWoodAmountMax_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003ECraftingScrapWoodAmountMin_003C_003EAccessor : VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003ECraftingScrapWoodAmountMin_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003ECraftingScrapWoodAmountMax_003C_003EAccessor : VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003ECraftingScrapWoodAmountMax_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003ECuttingPrefabs_003C_003EAccessor : VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003ECuttingPrefabs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, MyCuttingPrefab[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in MyCuttingPrefab[] value)
			{
				Set(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out MyCuttingPrefab[] value)
			{
				Get(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003EDestroySourceAfterCrafting_003C_003EAccessor : VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EDestroySourceAfterCrafting_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003ECraftingTimeInSeconds_003C_003EAccessor : VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003ECraftingTimeInSeconds_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CuttingDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_TreeCuttingDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_TreeCuttingDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_TreeCuttingDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_TreeCuttingDefinition CreateInstance()
			{
				return new MyObjectBuilder_TreeCuttingDefinition();
			}

			MyObjectBuilder_TreeCuttingDefinition IActivator<MyObjectBuilder_TreeCuttingDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
