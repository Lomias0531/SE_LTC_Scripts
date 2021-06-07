using ProtoBuf;
using System.ComponentModel;
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
	public class MyObjectBuilder_CuttingDefinition : MyObjectBuilder_DefinitionBase
	{
		[ProtoContract]
		public class MyCuttingPrefab
		{
			protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EMyCuttingPrefab_003C_003EPrefab_003C_003EAccessor : IMemberAccessor<MyCuttingPrefab, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyCuttingPrefab owner, in string value)
				{
					owner.Prefab = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyCuttingPrefab owner, out string value)
				{
					value = owner.Prefab;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EMyCuttingPrefab_003C_003ESpawnCount_003C_003EAccessor : IMemberAccessor<MyCuttingPrefab, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyCuttingPrefab owner, in int value)
				{
					owner.SpawnCount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyCuttingPrefab owner, out int value)
				{
					value = owner.SpawnCount;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EMyCuttingPrefab_003C_003EPhysicalItemId_003C_003EAccessor : IMemberAccessor<MyCuttingPrefab, SerializableDefinitionId?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyCuttingPrefab owner, in SerializableDefinitionId? value)
				{
					owner.PhysicalItemId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyCuttingPrefab owner, out SerializableDefinitionId? value)
				{
					value = owner.PhysicalItemId;
				}
			}

			private class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EMyCuttingPrefab_003C_003EActor : IActivator, IActivator<MyCuttingPrefab>
			{
				private sealed override object CreateInstance()
				{
					return new MyCuttingPrefab();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyCuttingPrefab CreateInstance()
				{
					return new MyCuttingPrefab();
				}

				MyCuttingPrefab IActivator<MyCuttingPrefab>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			[DefaultValue(null)]
			public string Prefab;

			[ProtoMember(4)]
			[DefaultValue(1)]
			public int SpawnCount = 1;

			[ProtoMember(7)]
			[DefaultValue(null)]
			public SerializableDefinitionId? PhysicalItemId;
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in SerializableDefinitionId value)
			{
				owner.EntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out SerializableDefinitionId value)
			{
				value = owner.EntityId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EScrapWoodBranchesId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in SerializableDefinitionId value)
			{
				owner.ScrapWoodBranchesId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out SerializableDefinitionId value)
			{
				value = owner.ScrapWoodBranchesId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EScrapWoodId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in SerializableDefinitionId value)
			{
				owner.ScrapWoodId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out SerializableDefinitionId value)
			{
				value = owner.ScrapWoodId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EScrapWoodAmountMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in int value)
			{
				owner.ScrapWoodAmountMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out int value)
			{
				value = owner.ScrapWoodAmountMin;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EScrapWoodAmountMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in int value)
			{
				owner.ScrapWoodAmountMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out int value)
			{
				value = owner.ScrapWoodAmountMax;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003ECraftingScrapWoodAmountMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in int value)
			{
				owner.CraftingScrapWoodAmountMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out int value)
			{
				value = owner.CraftingScrapWoodAmountMin;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003ECraftingScrapWoodAmountMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in int value)
			{
				owner.CraftingScrapWoodAmountMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out int value)
			{
				value = owner.CraftingScrapWoodAmountMax;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003ECuttingPrefabs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, MyCuttingPrefab[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in MyCuttingPrefab[] value)
			{
				owner.CuttingPrefabs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out MyCuttingPrefab[] value)
			{
				value = owner.CuttingPrefabs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EDestroySourceAfterCrafting_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in bool value)
			{
				owner.DestroySourceAfterCrafting = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out bool value)
			{
				value = owner.DestroySourceAfterCrafting;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003ECraftingTimeInSeconds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in float value)
			{
				owner.CraftingTimeInSeconds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out float value)
			{
				value = owner.CraftingTimeInSeconds;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CuttingDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CuttingDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_CuttingDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_CuttingDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_CuttingDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_CuttingDefinition CreateInstance()
			{
				return new MyObjectBuilder_CuttingDefinition();
			}

			MyObjectBuilder_CuttingDefinition IActivator<MyObjectBuilder_CuttingDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(10)]
		public SerializableDefinitionId EntityId;

		[ProtoMember(13)]
		public SerializableDefinitionId ScrapWoodBranchesId;

		[ProtoMember(16)]
		public SerializableDefinitionId ScrapWoodId;

		[ProtoMember(19)]
		public int ScrapWoodAmountMin = 5;

		[ProtoMember(22)]
		public int ScrapWoodAmountMax = 7;

		[ProtoMember(25)]
		public int CraftingScrapWoodAmountMin = 1;

		[ProtoMember(28)]
		public int CraftingScrapWoodAmountMax = 3;

		[XmlArrayItem("CuttingPrefab")]
		[ProtoMember(31)]
		[DefaultValue(null)]
		public MyCuttingPrefab[] CuttingPrefabs;

		[ProtoMember(34)]
		public bool DestroySourceAfterCrafting = true;

		[ProtoMember(37)]
		public float CraftingTimeInSeconds = 0.5f;
	}
}
