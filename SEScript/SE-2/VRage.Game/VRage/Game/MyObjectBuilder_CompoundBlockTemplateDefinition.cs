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
	public class MyObjectBuilder_CompoundBlockTemplateDefinition : MyObjectBuilder_DefinitionBase
	{
		[ProtoContract]
		public class CompoundBlockRotationBinding
		{
			protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003ECompoundBlockRotationBinding_003C_003EBuildTypeReference_003C_003EAccessor : IMemberAccessor<CompoundBlockRotationBinding, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CompoundBlockRotationBinding owner, in string value)
				{
					owner.BuildTypeReference = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CompoundBlockRotationBinding owner, out string value)
				{
					value = owner.BuildTypeReference;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003ECompoundBlockRotationBinding_003C_003ERotations_003C_003EAccessor : IMemberAccessor<CompoundBlockRotationBinding, SerializableBlockOrientation[]>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CompoundBlockRotationBinding owner, in SerializableBlockOrientation[] value)
				{
					owner.Rotations = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CompoundBlockRotationBinding owner, out SerializableBlockOrientation[] value)
				{
					value = owner.Rotations;
				}
			}

			private class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003ECompoundBlockRotationBinding_003C_003EActor : IActivator, IActivator<CompoundBlockRotationBinding>
			{
				private sealed override object CreateInstance()
				{
					return new CompoundBlockRotationBinding();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override CompoundBlockRotationBinding CreateInstance()
				{
					return new CompoundBlockRotationBinding();
				}

				CompoundBlockRotationBinding IActivator<CompoundBlockRotationBinding>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(1)]
			public string BuildTypeReference;

			[XmlArrayItem("Rotation")]
			[ProtoMember(4)]
			public SerializableBlockOrientation[] Rotations;
		}

		[ProtoContract]
		public class CompoundBlockBinding
		{
			protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003ECompoundBlockBinding_003C_003EBuildType_003C_003EAccessor : IMemberAccessor<CompoundBlockBinding, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CompoundBlockBinding owner, in string value)
				{
					owner.BuildType = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CompoundBlockBinding owner, out string value)
				{
					value = owner.BuildType;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003ECompoundBlockBinding_003C_003EMultiple_003C_003EAccessor : IMemberAccessor<CompoundBlockBinding, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CompoundBlockBinding owner, in bool value)
				{
					owner.Multiple = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CompoundBlockBinding owner, out bool value)
				{
					value = owner.Multiple;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003ECompoundBlockBinding_003C_003ERotationBinds_003C_003EAccessor : IMemberAccessor<CompoundBlockBinding, CompoundBlockRotationBinding[]>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CompoundBlockBinding owner, in CompoundBlockRotationBinding[] value)
				{
					owner.RotationBinds = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CompoundBlockBinding owner, out CompoundBlockRotationBinding[] value)
				{
					value = owner.RotationBinds;
				}
			}

			private class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003ECompoundBlockBinding_003C_003EActor : IActivator, IActivator<CompoundBlockBinding>
			{
				private sealed override object CreateInstance()
				{
					return new CompoundBlockBinding();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override CompoundBlockBinding CreateInstance()
				{
					return new CompoundBlockBinding();
				}

				CompoundBlockBinding IActivator<CompoundBlockBinding>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(7)]
			public string BuildType;

			[XmlAttribute]
			[ProtoMember(10)]
			[DefaultValue(false)]
			public bool Multiple;

			[XmlArrayItem("RotationBind")]
			[ProtoMember(13)]
			public CompoundBlockRotationBinding[] RotationBinds;
		}

		protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003EBindings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, CompoundBlockBinding[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, in CompoundBlockBinding[] value)
			{
				owner.Bindings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, out CompoundBlockBinding[] value)
			{
				value = owner.Bindings;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundBlockTemplateDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_CompoundBlockTemplateDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_CompoundBlockTemplateDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_CompoundBlockTemplateDefinition CreateInstance()
			{
				return new MyObjectBuilder_CompoundBlockTemplateDefinition();
			}

			MyObjectBuilder_CompoundBlockTemplateDefinition IActivator<MyObjectBuilder_CompoundBlockTemplateDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlArrayItem("Binding")]
		[ProtoMember(16)]
		public CompoundBlockBinding[] Bindings;
	}
}
