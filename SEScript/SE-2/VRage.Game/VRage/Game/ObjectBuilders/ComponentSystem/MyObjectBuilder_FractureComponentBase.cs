using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
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
	public class MyObjectBuilder_FractureComponentBase : MyObjectBuilder_ComponentBase
	{
		[ProtoContract]
		public struct FracturedShape
		{
			protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase_003C_003EFracturedShape_003C_003EName_003C_003EAccessor : IMemberAccessor<FracturedShape, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref FracturedShape owner, in string value)
				{
					owner.Name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref FracturedShape owner, out string value)
				{
					value = owner.Name;
				}
			}

			protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase_003C_003EFracturedShape_003C_003EFixed_003C_003EAccessor : IMemberAccessor<FracturedShape, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref FracturedShape owner, in bool value)
				{
					owner.Fixed = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref FracturedShape owner, out bool value)
				{
					value = owner.Fixed;
				}
			}

			private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase_003C_003EFracturedShape_003C_003EActor : IActivator, IActivator<FracturedShape>
			{
				private sealed override object CreateInstance()
				{
					return default(FracturedShape);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override FracturedShape CreateInstance()
				{
					return (FracturedShape)(object)default(FracturedShape);
				}

				FracturedShape IActivator<FracturedShape>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public string Name;

			[ProtoMember(4)]
			[DefaultValue(false)]
			public bool Fixed;
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase_003C_003EShapes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FractureComponentBase, List<FracturedShape>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FractureComponentBase owner, in List<FracturedShape> value)
			{
				owner.Shapes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FractureComponentBase owner, out List<FracturedShape> value)
			{
				value = owner.Shapes;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FractureComponentBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FractureComponentBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FractureComponentBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FractureComponentBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FractureComponentBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FractureComponentBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FractureComponentBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FractureComponentBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FractureComponentBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FractureComponentBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FractureComponentBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FractureComponentBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase_003C_003EActor : IActivator, IActivator<MyObjectBuilder_FractureComponentBase>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_FractureComponentBase();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_FractureComponentBase CreateInstance()
			{
				return new MyObjectBuilder_FractureComponentBase();
			}

			MyObjectBuilder_FractureComponentBase IActivator<MyObjectBuilder_FractureComponentBase>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(7)]
		public List<FracturedShape> Shapes = new List<FracturedShape>();
	}
}
