using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.ObjectBuilders
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("Sandbox.Game.XmlSerializers")]
	public class MyObjectBuilder_ProceduralEnvironmentSector : MyObjectBuilder_EnvironmentSector
	{
		[ProtoContract]
		public struct Module
		{
			protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector_003C_003EModule_003C_003EModuleId_003C_003EAccessor : IMemberAccessor<Module, SerializableDefinitionId>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Module owner, in SerializableDefinitionId value)
				{
					owner.ModuleId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Module owner, out SerializableDefinitionId value)
				{
					value = owner.ModuleId;
				}
			}

			protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector_003C_003EModule_003C_003EBuilder_003C_003EAccessor : IMemberAccessor<Module, MyObjectBuilder_EnvironmentModuleBase>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Module owner, in MyObjectBuilder_EnvironmentModuleBase value)
				{
					owner.Builder = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Module owner, out MyObjectBuilder_EnvironmentModuleBase value)
				{
					value = owner.Builder;
				}
			}

			private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector_003C_003EModule_003C_003EActor : IActivator, IActivator<Module>
			{
				private sealed override object CreateInstance()
				{
					return default(Module);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override Module CreateInstance()
				{
					return (Module)(object)default(Module);
				}

				Module IActivator<Module>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public SerializableDefinitionId ModuleId;

			[ProtoMember(4)]
			[Serialize(MyObjectFlags.Dynamic, typeof(MyObjectBuilderDynamicSerializer))]
			[XmlElement(typeof(MyAbstractXmlSerializer<MyObjectBuilder_EnvironmentModuleBase>))]
			public MyObjectBuilder_EnvironmentModuleBase Builder;
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector_003C_003ESavedModules_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentSector, Module[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProceduralEnvironmentSector owner, in Module[] value)
			{
				owner.SavedModules = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProceduralEnvironmentSector owner, out Module[] value)
			{
				value = owner.SavedModules;
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector_003C_003ESectorId_003C_003EAccessor : Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_EnvironmentSector_003C_003ESectorId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentSector, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProceduralEnvironmentSector owner, in long value)
			{
				Set(ref *(MyObjectBuilder_EnvironmentSector*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProceduralEnvironmentSector owner, out long value)
			{
				Get(ref *(MyObjectBuilder_EnvironmentSector*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentSector, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProceduralEnvironmentSector owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProceduralEnvironmentSector owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentSector, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProceduralEnvironmentSector owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProceduralEnvironmentSector owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentSector, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProceduralEnvironmentSector owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProceduralEnvironmentSector owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentSector, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProceduralEnvironmentSector owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProceduralEnvironmentSector owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ProceduralEnvironmentSector>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ProceduralEnvironmentSector();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ProceduralEnvironmentSector CreateInstance()
			{
				return new MyObjectBuilder_ProceduralEnvironmentSector();
			}

			MyObjectBuilder_ProceduralEnvironmentSector IActivator<MyObjectBuilder_ProceduralEnvironmentSector>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(7)]
		public Module[] SavedModules;
	}
}
