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
	public class MyObjectBuilder_Configuration : MyObjectBuilder_Base
	{
		[ProtoContract]
		public struct CubeSizeSettings
		{
			protected class VRage_Game_MyObjectBuilder_Configuration_003C_003ECubeSizeSettings_003C_003ELarge_003C_003EAccessor : IMemberAccessor<CubeSizeSettings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeSizeSettings owner, in float value)
				{
					owner.Large = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeSizeSettings owner, out float value)
				{
					value = owner.Large;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Configuration_003C_003ECubeSizeSettings_003C_003ESmall_003C_003EAccessor : IMemberAccessor<CubeSizeSettings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeSizeSettings owner, in float value)
				{
					owner.Small = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeSizeSettings owner, out float value)
				{
					value = owner.Small;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Configuration_003C_003ECubeSizeSettings_003C_003ESmallOriginal_003C_003EAccessor : IMemberAccessor<CubeSizeSettings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeSizeSettings owner, in float value)
				{
					owner.SmallOriginal = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeSizeSettings owner, out float value)
				{
					value = owner.SmallOriginal;
				}
			}

			private class VRage_Game_MyObjectBuilder_Configuration_003C_003ECubeSizeSettings_003C_003EActor : IActivator, IActivator<CubeSizeSettings>
			{
				private sealed override object CreateInstance()
				{
					return default(CubeSizeSettings);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override CubeSizeSettings CreateInstance()
				{
					return (CubeSizeSettings)(object)default(CubeSizeSettings);
				}

				CubeSizeSettings IActivator<CubeSizeSettings>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			[XmlAttribute]
			public float Large;

			[ProtoMember(4)]
			[XmlAttribute]
			public float Small;

			[ProtoMember(7)]
			[XmlAttribute]
			public float SmallOriginal;
		}

		[ProtoContract]
		public struct BaseBlockSettings
		{
			protected class VRage_Game_MyObjectBuilder_Configuration_003C_003EBaseBlockSettings_003C_003ESmallStatic_003C_003EAccessor : IMemberAccessor<BaseBlockSettings, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BaseBlockSettings owner, in string value)
				{
					owner.SmallStatic = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BaseBlockSettings owner, out string value)
				{
					value = owner.SmallStatic;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Configuration_003C_003EBaseBlockSettings_003C_003ELargeStatic_003C_003EAccessor : IMemberAccessor<BaseBlockSettings, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BaseBlockSettings owner, in string value)
				{
					owner.LargeStatic = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BaseBlockSettings owner, out string value)
				{
					value = owner.LargeStatic;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Configuration_003C_003EBaseBlockSettings_003C_003ESmallDynamic_003C_003EAccessor : IMemberAccessor<BaseBlockSettings, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BaseBlockSettings owner, in string value)
				{
					owner.SmallDynamic = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BaseBlockSettings owner, out string value)
				{
					value = owner.SmallDynamic;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Configuration_003C_003EBaseBlockSettings_003C_003ELargeDynamic_003C_003EAccessor : IMemberAccessor<BaseBlockSettings, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BaseBlockSettings owner, in string value)
				{
					owner.LargeDynamic = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BaseBlockSettings owner, out string value)
				{
					value = owner.LargeDynamic;
				}
			}

			private class VRage_Game_MyObjectBuilder_Configuration_003C_003EBaseBlockSettings_003C_003EActor : IActivator, IActivator<BaseBlockSettings>
			{
				private sealed override object CreateInstance()
				{
					return default(BaseBlockSettings);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override BaseBlockSettings CreateInstance()
				{
					return (BaseBlockSettings)(object)default(BaseBlockSettings);
				}

				BaseBlockSettings IActivator<BaseBlockSettings>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(10)]
			[XmlAttribute]
			public string SmallStatic;

			[ProtoMember(13)]
			[XmlAttribute]
			public string LargeStatic;

			[ProtoMember(16)]
			[XmlAttribute]
			public string SmallDynamic;

			[ProtoMember(19)]
			[XmlAttribute]
			public string LargeDynamic;
		}

		[ProtoContract]
		public class LootBagDefinition
		{
			protected class VRage_Game_MyObjectBuilder_Configuration_003C_003ELootBagDefinition_003C_003EContainerDefinition_003C_003EAccessor : IMemberAccessor<LootBagDefinition, SerializableDefinitionId>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref LootBagDefinition owner, in SerializableDefinitionId value)
				{
					owner.ContainerDefinition = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref LootBagDefinition owner, out SerializableDefinitionId value)
				{
					value = owner.ContainerDefinition;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Configuration_003C_003ELootBagDefinition_003C_003ESearchRadius_003C_003EAccessor : IMemberAccessor<LootBagDefinition, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref LootBagDefinition owner, in float value)
				{
					owner.SearchRadius = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref LootBagDefinition owner, out float value)
				{
					value = owner.SearchRadius;
				}
			}

			private class VRage_Game_MyObjectBuilder_Configuration_003C_003ELootBagDefinition_003C_003EActor : IActivator, IActivator<LootBagDefinition>
			{
				private sealed override object CreateInstance()
				{
					return new LootBagDefinition();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override LootBagDefinition CreateInstance()
				{
					return new LootBagDefinition();
				}

				LootBagDefinition IActivator<LootBagDefinition>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(22)]
			public SerializableDefinitionId ContainerDefinition;

			[ProtoMember(25)]
			[XmlAttribute]
			public float SearchRadius = 3f;
		}

		protected class VRage_Game_MyObjectBuilder_Configuration_003C_003ECubeSizes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration, CubeSizeSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Configuration owner, in CubeSizeSettings value)
			{
				owner.CubeSizes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Configuration owner, out CubeSizeSettings value)
			{
				value = owner.CubeSizes;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Configuration_003C_003EBaseBlockPrefabs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration, BaseBlockSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Configuration owner, in BaseBlockSettings value)
			{
				owner.BaseBlockPrefabs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Configuration owner, out BaseBlockSettings value)
			{
				value = owner.BaseBlockPrefabs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Configuration_003C_003EBaseBlockPrefabsSurvival_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration, BaseBlockSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Configuration owner, in BaseBlockSettings value)
			{
				owner.BaseBlockPrefabsSurvival = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Configuration owner, out BaseBlockSettings value)
			{
				value = owner.BaseBlockPrefabsSurvival;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Configuration_003C_003ELootBag_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration, LootBagDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Configuration owner, in LootBagDefinition value)
			{
				owner.LootBag = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Configuration owner, out LootBagDefinition value)
			{
				value = owner.LootBag;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Configuration_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Configuration, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Configuration owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Configuration owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Configuration_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Configuration, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Configuration owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Configuration owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Configuration_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Configuration, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Configuration owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Configuration owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Configuration_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Configuration, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Configuration owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Configuration owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_Configuration_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Configuration>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Configuration();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Configuration CreateInstance()
			{
				return new MyObjectBuilder_Configuration();
			}

			MyObjectBuilder_Configuration IActivator<MyObjectBuilder_Configuration>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(28)]
		public CubeSizeSettings CubeSizes;

		[ProtoMember(31)]
		public BaseBlockSettings BaseBlockPrefabs;

		[ProtoMember(34)]
		public BaseBlockSettings BaseBlockPrefabsSurvival;

		[ProtoMember(37)]
		public LootBagDefinition LootBag;
	}
}
