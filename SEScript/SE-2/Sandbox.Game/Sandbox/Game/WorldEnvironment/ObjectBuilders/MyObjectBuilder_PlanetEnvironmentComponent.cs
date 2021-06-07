using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.ObjectBuilders
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("Sandbox.Game.XmlSerializers")]
	public class MyObjectBuilder_PlanetEnvironmentComponent : MyObjectBuilder_ComponentBase
	{
		[ProtoContract]
		public struct ObstructingBox
		{
			protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent_003C_003EObstructingBox_003C_003ESectorId_003C_003EAccessor : IMemberAccessor<ObstructingBox, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ObstructingBox owner, in long value)
				{
					owner.SectorId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ObstructingBox owner, out long value)
				{
					value = owner.SectorId;
				}
			}

			protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent_003C_003EObstructingBox_003C_003EObstructingBoxes_003C_003EAccessor : IMemberAccessor<ObstructingBox, List<SerializableOrientedBoundingBoxD>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ObstructingBox owner, in List<SerializableOrientedBoundingBoxD> value)
				{
					owner.ObstructingBoxes = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ObstructingBox owner, out List<SerializableOrientedBoundingBoxD> value)
				{
					value = owner.ObstructingBoxes;
				}
			}

			private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent_003C_003EObstructingBox_003C_003EActor : IActivator, IActivator<ObstructingBox>
			{
				private sealed override object CreateInstance()
				{
					return default(ObstructingBox);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override ObstructingBox CreateInstance()
				{
					return (ObstructingBox)(object)default(ObstructingBox);
				}

				ObstructingBox IActivator<ObstructingBox>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(4)]
			public long SectorId;

			[ProtoMember(7)]
			public List<SerializableOrientedBoundingBoxD> ObstructingBoxes;
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent_003C_003EDataProviders_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetEnvironmentComponent, MyObjectBuilder_EnvironmentDataProvider[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetEnvironmentComponent owner, in MyObjectBuilder_EnvironmentDataProvider[] value)
			{
				owner.DataProviders = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetEnvironmentComponent owner, out MyObjectBuilder_EnvironmentDataProvider[] value)
			{
				value = owner.DataProviders;
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent_003C_003ESectorObstructions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetEnvironmentComponent, List<ObstructingBox>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetEnvironmentComponent owner, in List<ObstructingBox> value)
			{
				owner.SectorObstructions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetEnvironmentComponent owner, out List<ObstructingBox> value)
			{
				value = owner.SectorObstructions;
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetEnvironmentComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetEnvironmentComponent owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetEnvironmentComponent owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetEnvironmentComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetEnvironmentComponent owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetEnvironmentComponent owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetEnvironmentComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetEnvironmentComponent owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetEnvironmentComponent owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetEnvironmentComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetEnvironmentComponent owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetEnvironmentComponent owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent_003C_003EActor : IActivator, IActivator<MyObjectBuilder_PlanetEnvironmentComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_PlanetEnvironmentComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_PlanetEnvironmentComponent CreateInstance()
			{
				return new MyObjectBuilder_PlanetEnvironmentComponent();
			}

			MyObjectBuilder_PlanetEnvironmentComponent IActivator<MyObjectBuilder_PlanetEnvironmentComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[XmlElement("Provider", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_EnvironmentDataProvider>))]
		[DynamicNullableObjectBuilderItem(false)]
		public MyObjectBuilder_EnvironmentDataProvider[] DataProviders;

		[ProtoMember(10)]
		[XmlArrayItem("Sector")]
		[Nullable]
		public List<ObstructingBox> SectorObstructions;

		public MyObjectBuilder_PlanetEnvironmentComponent()
		{
			DataProviders = new MyObjectBuilder_EnvironmentDataProvider[0];
			SectorObstructions = new List<ObstructingBox>();
		}
	}
}
