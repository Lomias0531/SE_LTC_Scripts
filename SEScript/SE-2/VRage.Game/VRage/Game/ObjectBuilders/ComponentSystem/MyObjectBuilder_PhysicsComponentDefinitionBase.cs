using ProtoBuf;
using System;
using System.ComponentModel;
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
	public class MyObjectBuilder_PhysicsComponentDefinitionBase : MyObjectBuilder_ComponentDefinitionBase
	{
		public enum MyMassPropertiesComputationType
		{
			None,
			Box,
			Sphere,
			Capsule,
			Cylinder
		}

		[Flags]
		public enum MyUpdateFlags
		{
			Gravity = 0x1
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EMassPropertiesComputation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, MyMassPropertiesComputationType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in MyMassPropertiesComputationType value)
			{
				owner.MassPropertiesComputation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out MyMassPropertiesComputationType value)
			{
				value = owner.MassPropertiesComputation;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003ERigidBodyFlags_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, RigidBodyFlag>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in RigidBodyFlag value)
			{
				owner.RigidBodyFlags = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out RigidBodyFlag value)
			{
				value = owner.RigidBodyFlags;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003ECollisionLayer_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in string value)
			{
				owner.CollisionLayer = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out string value)
			{
				value = owner.CollisionLayer;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003ELinearDamping_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in float? value)
			{
				owner.LinearDamping = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out float? value)
			{
				value = owner.LinearDamping;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EAngularDamping_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in float? value)
			{
				owner.AngularDamping = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out float? value)
			{
				value = owner.AngularDamping;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EForceActivate_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in bool value)
			{
				owner.ForceActivate = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out bool value)
			{
				value = owner.ForceActivate;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EUpdateFlags_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, MyUpdateFlags>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in MyUpdateFlags value)
			{
				owner.UpdateFlags = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out MyUpdateFlags value)
			{
				value = owner.UpdateFlags;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003ESerialize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in bool value)
			{
				owner.Serialize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out bool value)
			{
				value = owner.Serialize;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EComponentType_003C_003EAccessor : VRage_Game_MyObjectBuilder_ComponentDefinitionBase_003C_003EComponentType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_ComponentDefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_ComponentDefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicsComponentDefinitionBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase_003C_003EActor : IActivator, IActivator<MyObjectBuilder_PhysicsComponentDefinitionBase>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_PhysicsComponentDefinitionBase();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_PhysicsComponentDefinitionBase CreateInstance()
			{
				return new MyObjectBuilder_PhysicsComponentDefinitionBase();
			}

			MyObjectBuilder_PhysicsComponentDefinitionBase IActivator<MyObjectBuilder_PhysicsComponentDefinitionBase>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[DefaultValue(MyMassPropertiesComputationType.None)]
		public MyMassPropertiesComputationType MassPropertiesComputation;

		[ProtoMember(4)]
		[DefaultValue(RigidBodyFlag.RBF_DEFAULT)]
		public RigidBodyFlag RigidBodyFlags;

		[ProtoMember(7)]
		[DefaultValue(null)]
		public string CollisionLayer;

		[ProtoMember(10)]
		[DefaultValue(null)]
		public float? LinearDamping;

		[ProtoMember(13)]
		[DefaultValue(null)]
		public float? AngularDamping;

		[ProtoMember(16)]
		public bool ForceActivate;

		[ProtoMember(19)]
		public MyUpdateFlags UpdateFlags;

		[ProtoMember(22)]
		public bool Serialize;
	}
}
