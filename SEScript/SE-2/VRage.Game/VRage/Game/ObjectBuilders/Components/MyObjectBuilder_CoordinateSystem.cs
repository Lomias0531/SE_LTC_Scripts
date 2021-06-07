using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_CoordinateSystem : MyObjectBuilder_SessionComponent
	{
		[ProtoContract]
		public struct CoordSysInfo
		{
			protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem_003C_003ECoordSysInfo_003C_003EId_003C_003EAccessor : IMemberAccessor<CoordSysInfo, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CoordSysInfo owner, in long value)
				{
					owner.Id = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CoordSysInfo owner, out long value)
				{
					value = owner.Id;
				}
			}

			protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem_003C_003ECoordSysInfo_003C_003EEntityCount_003C_003EAccessor : IMemberAccessor<CoordSysInfo, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CoordSysInfo owner, in long value)
				{
					owner.EntityCount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CoordSysInfo owner, out long value)
				{
					value = owner.EntityCount;
				}
			}

			protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem_003C_003ECoordSysInfo_003C_003ERotation_003C_003EAccessor : IMemberAccessor<CoordSysInfo, SerializableQuaternion>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CoordSysInfo owner, in SerializableQuaternion value)
				{
					owner.Rotation = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CoordSysInfo owner, out SerializableQuaternion value)
				{
					value = owner.Rotation;
				}
			}

			protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem_003C_003ECoordSysInfo_003C_003EPosition_003C_003EAccessor : IMemberAccessor<CoordSysInfo, SerializableVector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CoordSysInfo owner, in SerializableVector3D value)
				{
					owner.Position = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CoordSysInfo owner, out SerializableVector3D value)
				{
					value = owner.Position;
				}
			}

			private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem_003C_003ECoordSysInfo_003C_003EActor : IActivator, IActivator<CoordSysInfo>
			{
				private sealed override object CreateInstance()
				{
					return default(CoordSysInfo);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override CoordSysInfo CreateInstance()
				{
					return (CoordSysInfo)(object)default(CoordSysInfo);
				}

				CoordSysInfo IActivator<CoordSysInfo>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public long Id;

			[ProtoMember(4)]
			public long EntityCount;

			[ProtoMember(7)]
			public SerializableQuaternion Rotation;

			[ProtoMember(10)]
			public SerializableVector3D Position;
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem_003C_003ELastCoordSysId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CoordinateSystem, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CoordinateSystem owner, in long value)
			{
				owner.LastCoordSysId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CoordinateSystem owner, out long value)
			{
				value = owner.LastCoordSysId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem_003C_003ECoordSystems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CoordinateSystem, List<CoordSysInfo>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CoordinateSystem owner, in List<CoordSysInfo> value)
			{
				owner.CoordSystems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CoordinateSystem owner, out List<CoordSysInfo> value)
			{
				value = owner.CoordSystems;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CoordinateSystem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CoordinateSystem owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CoordinateSystem owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CoordinateSystem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CoordinateSystem owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CoordinateSystem owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CoordinateSystem, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CoordinateSystem owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_SessionComponent*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CoordinateSystem owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_SessionComponent*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CoordinateSystem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CoordinateSystem owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CoordinateSystem owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CoordinateSystem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CoordinateSystem owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CoordinateSystem owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem_003C_003EActor : IActivator, IActivator<MyObjectBuilder_CoordinateSystem>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_CoordinateSystem();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_CoordinateSystem CreateInstance()
			{
				return new MyObjectBuilder_CoordinateSystem();
			}

			MyObjectBuilder_CoordinateSystem IActivator<MyObjectBuilder_CoordinateSystem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(13)]
		public long LastCoordSysId = 1L;

		[ProtoMember(16)]
		public List<CoordSysInfo> CoordSystems = new List<CoordSysInfo>();
	}
}
