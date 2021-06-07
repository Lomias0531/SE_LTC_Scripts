using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.Components;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_PhysicsBodyComponentDefinition : MyObjectBuilder_PhysicsComponentDefinitionBase
	{
		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003ECreateFromCollisionObject_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in bool value)
			{
				owner.CreateFromCollisionObject = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out bool value)
			{
				value = owner.CreateFromCollisionObject;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003EMassPropertiesComputation_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EMassPropertiesComputation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, MyMassPropertiesComputationType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in MyMassPropertiesComputationType value)
			{
				Set(ref *(MyObjectBuilder_PhysicsComponentDefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out MyMassPropertiesComputationType value)
			{
				Get(ref *(MyObjectBuilder_PhysicsComponentDefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003ERigidBodyFlags_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003ERigidBodyFlags_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, RigidBodyFlag>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in RigidBodyFlag value)
			{
				Set(ref *(MyObjectBuilder_PhysicsComponentDefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out RigidBodyFlag value)
			{
				Get(ref *(MyObjectBuilder_PhysicsComponentDefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003ECollisionLayer_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003ECollisionLayer_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicsComponentDefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicsComponentDefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003ELinearDamping_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003ELinearDamping_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in float? value)
			{
				Set(ref *(MyObjectBuilder_PhysicsComponentDefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out float? value)
			{
				Get(ref *(MyObjectBuilder_PhysicsComponentDefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003EAngularDamping_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EAngularDamping_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in float? value)
			{
				Set(ref *(MyObjectBuilder_PhysicsComponentDefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out float? value)
			{
				Get(ref *(MyObjectBuilder_PhysicsComponentDefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003EForceActivate_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EForceActivate_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicsComponentDefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicsComponentDefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003EUpdateFlags_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EUpdateFlags_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, MyUpdateFlags>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in MyUpdateFlags value)
			{
				Set(ref *(MyObjectBuilder_PhysicsComponentDefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out MyUpdateFlags value)
			{
				Get(ref *(MyObjectBuilder_PhysicsComponentDefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003ESerialize_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003ESerialize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicsComponentDefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicsComponentDefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003EComponentType_003C_003EAccessor : VRage_Game_MyObjectBuilder_ComponentDefinitionBase_003C_003EComponentType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_ComponentDefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_ComponentDefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsBodyComponentDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_PhysicsBodyComponentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_PhysicsBodyComponentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_PhysicsBodyComponentDefinition CreateInstance()
			{
				return new MyObjectBuilder_PhysicsBodyComponentDefinition();
			}

			MyObjectBuilder_PhysicsBodyComponentDefinition IActivator<MyObjectBuilder_PhysicsBodyComponentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public bool CreateFromCollisionObject;
	}
}
