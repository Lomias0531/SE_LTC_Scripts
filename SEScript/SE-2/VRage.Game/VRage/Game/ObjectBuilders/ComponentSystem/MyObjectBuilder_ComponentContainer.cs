using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ComponentContainer : MyObjectBuilder_Base
	{
		[ProtoContract]
		public class ComponentData
		{
			protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer_003C_003EComponentData_003C_003ETypeId_003C_003EAccessor : IMemberAccessor<ComponentData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ComponentData owner, in string value)
				{
					owner.TypeId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ComponentData owner, out string value)
				{
					value = owner.TypeId;
				}
			}

			protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer_003C_003EComponentData_003C_003EComponent_003C_003EAccessor : IMemberAccessor<ComponentData, MyObjectBuilder_ComponentBase>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ComponentData owner, in MyObjectBuilder_ComponentBase value)
				{
					owner.Component = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ComponentData owner, out MyObjectBuilder_ComponentBase value)
				{
					value = owner.Component;
				}
			}

			private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer_003C_003EComponentData_003C_003EActor : IActivator, IActivator<ComponentData>
			{
				private sealed override object CreateInstance()
				{
					return new ComponentData();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override ComponentData CreateInstance()
				{
					return new ComponentData();
				}

				ComponentData IActivator<ComponentData>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public string TypeId;

			[ProtoMember(4)]
			[DynamicObjectBuilder(false)]
			[XmlElement(Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ComponentBase>))]
			public MyObjectBuilder_ComponentBase Component;
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer_003C_003EComponents_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ComponentContainer, List<ComponentData>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentContainer owner, in List<ComponentData> value)
			{
				owner.Components = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentContainer owner, out List<ComponentData> value)
			{
				value = owner.Components;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentContainer, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ComponentContainer owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ComponentContainer owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentContainer, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ComponentContainer owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ComponentContainer owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentContainer, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ComponentContainer owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ComponentContainer owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentContainer, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ComponentContainer owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ComponentContainer owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ComponentContainer>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ComponentContainer();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ComponentContainer CreateInstance()
			{
				return new MyObjectBuilder_ComponentContainer();
			}

			MyObjectBuilder_ComponentContainer IActivator<MyObjectBuilder_ComponentContainer>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(7)]
		public List<ComponentData> Components = new List<ComponentData>();
	}
}
