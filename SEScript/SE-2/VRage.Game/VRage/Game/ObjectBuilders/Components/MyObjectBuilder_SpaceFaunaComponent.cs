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
	public class MyObjectBuilder_SpaceFaunaComponent : MyObjectBuilder_SessionComponent
	{
		[ProtoContract]
		public class SpawnInfo
		{
			protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003ESpawnInfo_003C_003EX_003C_003EAccessor : IMemberAccessor<SpawnInfo, double>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnInfo owner, in double value)
				{
					owner.X = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnInfo owner, out double value)
				{
					value = owner.X;
				}
			}

			protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003ESpawnInfo_003C_003EY_003C_003EAccessor : IMemberAccessor<SpawnInfo, double>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnInfo owner, in double value)
				{
					owner.Y = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnInfo owner, out double value)
				{
					value = owner.Y;
				}
			}

			protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003ESpawnInfo_003C_003EZ_003C_003EAccessor : IMemberAccessor<SpawnInfo, double>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnInfo owner, in double value)
				{
					owner.Z = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnInfo owner, out double value)
				{
					value = owner.Z;
				}
			}

			protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003ESpawnInfo_003C_003ESpawnTime_003C_003EAccessor : IMemberAccessor<SpawnInfo, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnInfo owner, in int value)
				{
					owner.SpawnTime = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnInfo owner, out int value)
				{
					value = owner.SpawnTime;
				}
			}

			protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003ESpawnInfo_003C_003EAbandonTime_003C_003EAccessor : IMemberAccessor<SpawnInfo, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnInfo owner, in int value)
				{
					owner.AbandonTime = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnInfo owner, out int value)
				{
					value = owner.AbandonTime;
				}
			}

			private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003ESpawnInfo_003C_003EActor : IActivator, IActivator<SpawnInfo>
			{
				private sealed override object CreateInstance()
				{
					return new SpawnInfo();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override SpawnInfo CreateInstance()
				{
					return new SpawnInfo();
				}

				SpawnInfo IActivator<SpawnInfo>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			[XmlAttribute]
			public double X;

			[ProtoMember(4)]
			[XmlAttribute]
			public double Y;

			[ProtoMember(7)]
			[XmlAttribute]
			public double Z;

			[ProtoMember(10)]
			[XmlAttribute("S")]
			public int SpawnTime;

			[ProtoMember(13)]
			[XmlAttribute("A")]
			public int AbandonTime;
		}

		[ProtoContract]
		public class TimeoutInfo
		{
			protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003ETimeoutInfo_003C_003EX_003C_003EAccessor : IMemberAccessor<TimeoutInfo, double>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref TimeoutInfo owner, in double value)
				{
					owner.X = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref TimeoutInfo owner, out double value)
				{
					value = owner.X;
				}
			}

			protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003ETimeoutInfo_003C_003EY_003C_003EAccessor : IMemberAccessor<TimeoutInfo, double>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref TimeoutInfo owner, in double value)
				{
					owner.Y = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref TimeoutInfo owner, out double value)
				{
					value = owner.Y;
				}
			}

			protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003ETimeoutInfo_003C_003EZ_003C_003EAccessor : IMemberAccessor<TimeoutInfo, double>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref TimeoutInfo owner, in double value)
				{
					owner.Z = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref TimeoutInfo owner, out double value)
				{
					value = owner.Z;
				}
			}

			protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003ETimeoutInfo_003C_003ETimeout_003C_003EAccessor : IMemberAccessor<TimeoutInfo, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref TimeoutInfo owner, in int value)
				{
					owner.Timeout = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref TimeoutInfo owner, out int value)
				{
					value = owner.Timeout;
				}
			}

			private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003ETimeoutInfo_003C_003EActor : IActivator, IActivator<TimeoutInfo>
			{
				private sealed override object CreateInstance()
				{
					return new TimeoutInfo();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override TimeoutInfo CreateInstance()
				{
					return new TimeoutInfo();
				}

				TimeoutInfo IActivator<TimeoutInfo>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(16)]
			[XmlAttribute]
			public double X;

			[ProtoMember(19)]
			[XmlAttribute]
			public double Y;

			[ProtoMember(22)]
			[XmlAttribute]
			public double Z;

			[ProtoMember(25)]
			[XmlAttribute("T")]
			public int Timeout;
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003ESpawnInfos_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent, List<SpawnInfo>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SpaceFaunaComponent owner, in List<SpawnInfo> value)
			{
				owner.SpawnInfos = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SpaceFaunaComponent owner, out List<SpawnInfo> value)
			{
				value = owner.SpawnInfos;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003ETimeoutInfos_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent, List<TimeoutInfo>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SpaceFaunaComponent owner, in List<TimeoutInfo> value)
			{
				owner.TimeoutInfos = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SpaceFaunaComponent owner, out List<TimeoutInfo> value)
			{
				value = owner.TimeoutInfos;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpaceFaunaComponent owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpaceFaunaComponent owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpaceFaunaComponent owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpaceFaunaComponent owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpaceFaunaComponent owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_SessionComponent*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpaceFaunaComponent owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_SessionComponent*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpaceFaunaComponent owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpaceFaunaComponent owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpaceFaunaComponent owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpaceFaunaComponent owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent_003C_003EActor : IActivator, IActivator<MyObjectBuilder_SpaceFaunaComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_SpaceFaunaComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_SpaceFaunaComponent CreateInstance()
			{
				return new MyObjectBuilder_SpaceFaunaComponent();
			}

			MyObjectBuilder_SpaceFaunaComponent IActivator<MyObjectBuilder_SpaceFaunaComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlArrayItem("Info")]
		[ProtoMember(28)]
		public List<SpawnInfo> SpawnInfos = new List<SpawnInfo>();

		[XmlArrayItem("Info")]
		[ProtoMember(31)]
		public List<TimeoutInfo> TimeoutInfos = new List<TimeoutInfo>();
	}
}
