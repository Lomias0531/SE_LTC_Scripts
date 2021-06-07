using ProtoBuf;
using System.Collections.Generic;
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
	public class MyObjectBuilder_ControllerSchemaDefinition : MyObjectBuilder_DefinitionBase
	{
		[ProtoContract]
		public class ControlDef
		{
			protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EControlDef_003C_003EType_003C_003EAccessor : IMemberAccessor<ControlDef, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ControlDef owner, in string value)
				{
					owner.Type = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ControlDef owner, out string value)
				{
					value = owner.Type;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EControlDef_003C_003EControl_003C_003EAccessor : IMemberAccessor<ControlDef, MyControllerSchemaEnum>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ControlDef owner, in MyControllerSchemaEnum value)
				{
					owner.Control = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ControlDef owner, out MyControllerSchemaEnum value)
				{
					value = owner.Control;
				}
			}

			private class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EControlDef_003C_003EActor : IActivator, IActivator<ControlDef>
			{
				private sealed override object CreateInstance()
				{
					return new ControlDef();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override ControlDef CreateInstance()
				{
					return new ControlDef();
				}

				ControlDef IActivator<ControlDef>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(1)]
			public string Type;

			[XmlAttribute]
			[ProtoMember(4)]
			public MyControllerSchemaEnum Control;
		}

		[ProtoContract]
		public class ControlGroup
		{
			protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EControlGroup_003C_003EType_003C_003EAccessor : IMemberAccessor<ControlGroup, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ControlGroup owner, in string value)
				{
					owner.Type = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ControlGroup owner, out string value)
				{
					value = owner.Type;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EControlGroup_003C_003EName_003C_003EAccessor : IMemberAccessor<ControlGroup, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ControlGroup owner, in string value)
				{
					owner.Name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ControlGroup owner, out string value)
				{
					value = owner.Name;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EControlGroup_003C_003EControlDefs_003C_003EAccessor : IMemberAccessor<ControlGroup, List<ControlDef>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ControlGroup owner, in List<ControlDef> value)
				{
					owner.ControlDefs = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ControlGroup owner, out List<ControlDef> value)
				{
					value = owner.ControlDefs;
				}
			}

			private class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EControlGroup_003C_003EActor : IActivator, IActivator<ControlGroup>
			{
				private sealed override object CreateInstance()
				{
					return new ControlGroup();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override ControlGroup CreateInstance()
				{
					return new ControlGroup();
				}

				ControlGroup IActivator<ControlGroup>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(7)]
			public string Type;

			[ProtoMember(10)]
			public string Name;

			[ProtoMember(13)]
			public List<ControlDef> ControlDefs;
		}

		[ProtoContract]
		public class CompatibleDevice
		{
			protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003ECompatibleDevice_003C_003EDeviceId_003C_003EAccessor : IMemberAccessor<CompatibleDevice, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CompatibleDevice owner, in string value)
				{
					owner.DeviceId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CompatibleDevice owner, out string value)
				{
					value = owner.DeviceId;
				}
			}

			private class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003ECompatibleDevice_003C_003EActor : IActivator, IActivator<CompatibleDevice>
			{
				private sealed override object CreateInstance()
				{
					return new CompatibleDevice();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override CompatibleDevice CreateInstance()
				{
					return new CompatibleDevice();
				}

				CompatibleDevice IActivator<CompatibleDevice>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(16)]
			public string DeviceId;
		}

		[ProtoContract]
		public class Schema
		{
			protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003ESchema_003C_003ESchemaName_003C_003EAccessor : IMemberAccessor<Schema, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Schema owner, in string value)
				{
					owner.SchemaName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Schema owner, out string value)
				{
					value = owner.SchemaName;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003ESchema_003C_003EControlGroups_003C_003EAccessor : IMemberAccessor<Schema, List<ControlGroup>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Schema owner, in List<ControlGroup> value)
				{
					owner.ControlGroups = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Schema owner, out List<ControlGroup> value)
				{
					value = owner.ControlGroups;
				}
			}

			private class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003ESchema_003C_003EActor : IActivator, IActivator<Schema>
			{
				private sealed override object CreateInstance()
				{
					return new Schema();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override Schema CreateInstance()
				{
					return new Schema();
				}

				Schema IActivator<Schema>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(19)]
			public string SchemaName;

			[ProtoMember(22)]
			public List<ControlGroup> ControlGroups;
		}

		protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003ECompatibleDeviceIds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ControllerSchemaDefinition owner, in List<string> value)
			{
				owner.CompatibleDeviceIds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ControllerSchemaDefinition owner, out List<string> value)
			{
				value = owner.CompatibleDeviceIds;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003ESchemas_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, List<Schema>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ControllerSchemaDefinition owner, in List<Schema> value)
			{
				owner.Schemas = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ControllerSchemaDefinition owner, out List<Schema> value)
			{
				value = owner.Schemas;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ControllerSchemaDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ControllerSchemaDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ControllerSchemaDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ControllerSchemaDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ControllerSchemaDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ControllerSchemaDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ControllerSchemaDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ControllerSchemaDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ControllerSchemaDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ControllerSchemaDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ControllerSchemaDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ControllerSchemaDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ControllerSchemaDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ControllerSchemaDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ControllerSchemaDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ControllerSchemaDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ControllerSchemaDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ControllerSchemaDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ControllerSchemaDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ControllerSchemaDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ControllerSchemaDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ControllerSchemaDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ControllerSchemaDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ControllerSchemaDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ControllerSchemaDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ControllerSchemaDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ControllerSchemaDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ControllerSchemaDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ControllerSchemaDefinition CreateInstance()
			{
				return new MyObjectBuilder_ControllerSchemaDefinition();
			}

			MyObjectBuilder_ControllerSchemaDefinition IActivator<MyObjectBuilder_ControllerSchemaDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlArrayItem("DeviceId")]
		[ProtoMember(25)]
		public List<string> CompatibleDeviceIds;

		[ProtoMember(28)]
		public List<Schema> Schemas;
	}
}
