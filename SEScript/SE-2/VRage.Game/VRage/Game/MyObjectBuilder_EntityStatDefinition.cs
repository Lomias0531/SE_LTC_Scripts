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
	public class MyObjectBuilder_EntityStatDefinition : MyObjectBuilder_DefinitionBase
	{
		[ProtoContract]
		public class GuiDefinition
		{
			protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EGuiDefinition_003C_003EHeightMultiplier_003C_003EAccessor : IMemberAccessor<GuiDefinition, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref GuiDefinition owner, in float value)
				{
					owner.HeightMultiplier = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref GuiDefinition owner, out float value)
				{
					value = owner.HeightMultiplier;
				}
			}

			protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EGuiDefinition_003C_003EPriority_003C_003EAccessor : IMemberAccessor<GuiDefinition, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref GuiDefinition owner, in int value)
				{
					owner.Priority = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref GuiDefinition owner, out int value)
				{
					value = owner.Priority;
				}
			}

			protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EGuiDefinition_003C_003EColor_003C_003EAccessor : IMemberAccessor<GuiDefinition, SerializableVector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref GuiDefinition owner, in SerializableVector3I value)
				{
					owner.Color = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref GuiDefinition owner, out SerializableVector3I value)
				{
					value = owner.Color;
				}
			}

			protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EGuiDefinition_003C_003ECriticalRatio_003C_003EAccessor : IMemberAccessor<GuiDefinition, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref GuiDefinition owner, in float value)
				{
					owner.CriticalRatio = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref GuiDefinition owner, out float value)
				{
					value = owner.CriticalRatio;
				}
			}

			protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EGuiDefinition_003C_003EDisplayCriticalDivider_003C_003EAccessor : IMemberAccessor<GuiDefinition, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref GuiDefinition owner, in bool value)
				{
					owner.DisplayCriticalDivider = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref GuiDefinition owner, out bool value)
				{
					value = owner.DisplayCriticalDivider;
				}
			}

			protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EGuiDefinition_003C_003ECriticalColorFrom_003C_003EAccessor : IMemberAccessor<GuiDefinition, SerializableVector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref GuiDefinition owner, in SerializableVector3I value)
				{
					owner.CriticalColorFrom = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref GuiDefinition owner, out SerializableVector3I value)
				{
					value = owner.CriticalColorFrom;
				}
			}

			protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EGuiDefinition_003C_003ECriticalColorTo_003C_003EAccessor : IMemberAccessor<GuiDefinition, SerializableVector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref GuiDefinition owner, in SerializableVector3I value)
				{
					owner.CriticalColorTo = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref GuiDefinition owner, out SerializableVector3I value)
				{
					value = owner.CriticalColorTo;
				}
			}

			private class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EGuiDefinition_003C_003EActor : IActivator, IActivator<GuiDefinition>
			{
				private sealed override object CreateInstance()
				{
					return new GuiDefinition();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override GuiDefinition CreateInstance()
				{
					return new GuiDefinition();
				}

				GuiDefinition IActivator<GuiDefinition>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public float HeightMultiplier = 1f;

			[ProtoMember(4)]
			public int Priority = 1;

			[ProtoMember(7)]
			public SerializableVector3I Color = new SerializableVector3I(255, 255, 255);

			[ProtoMember(10)]
			public float CriticalRatio;

			[ProtoMember(13)]
			public bool DisplayCriticalDivider;

			[ProtoMember(16)]
			public SerializableVector3I CriticalColorFrom = new SerializableVector3I(155, 0, 0);

			[ProtoMember(19)]
			public SerializableVector3I CriticalColorTo = new SerializableVector3I(255, 0, 0);
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EMinValue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in float value)
			{
				owner.MinValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out float value)
			{
				value = owner.MinValue;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EMaxValue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in float value)
			{
				owner.MaxValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out float value)
			{
				value = owner.MaxValue;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EDefaultValue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in float value)
			{
				owner.DefaultValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out float value)
			{
				value = owner.DefaultValue;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EEnabledInCreative_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in bool value)
			{
				owner.EnabledInCreative = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out bool value)
			{
				value = owner.EnabledInCreative;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EGuiDef_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition, GuiDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in GuiDefinition value)
			{
				owner.GuiDef = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out GuiDefinition value)
			{
				value = owner.GuiDef;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_EntityStatDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_EntityStatDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_EntityStatDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_EntityStatDefinition CreateInstance()
			{
				return new MyObjectBuilder_EntityStatDefinition();
			}

			MyObjectBuilder_EntityStatDefinition IActivator<MyObjectBuilder_EntityStatDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(22)]
		public float MinValue;

		[ProtoMember(25)]
		public float MaxValue = 100f;

		[ProtoMember(28)]
		public float DefaultValue = float.NaN;

		[ProtoMember(31)]
		[XmlAttribute(AttributeName = "EnabledInCreative")]
		public bool EnabledInCreative = true;

		[ProtoMember(34)]
		public string Name = string.Empty;

		[ProtoMember(37)]
		public GuiDefinition GuiDef = new GuiDefinition();
	}
}
