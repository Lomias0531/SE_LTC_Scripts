using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	[MyEnvironmentItems(typeof(MyObjectBuilder_EnvironmentItemDefinition))]
	public class MyObjectBuilder_EnvironmentItems : MyObjectBuilder_EntityBase
	{
		[ProtoContract]
		public struct MyOBEnvironmentItemData
		{
			protected class VRage_Game_MyObjectBuilder_EnvironmentItems_003C_003EMyOBEnvironmentItemData_003C_003EPositionAndOrientation_003C_003EAccessor : IMemberAccessor<MyOBEnvironmentItemData, MyPositionAndOrientation>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyOBEnvironmentItemData owner, in MyPositionAndOrientation value)
				{
					owner.PositionAndOrientation = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyOBEnvironmentItemData owner, out MyPositionAndOrientation value)
				{
					value = owner.PositionAndOrientation;
				}
			}

			protected class VRage_Game_MyObjectBuilder_EnvironmentItems_003C_003EMyOBEnvironmentItemData_003C_003ESubtypeName_003C_003EAccessor : IMemberAccessor<MyOBEnvironmentItemData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyOBEnvironmentItemData owner, in string value)
				{
					owner.SubtypeName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyOBEnvironmentItemData owner, out string value)
				{
					value = owner.SubtypeName;
				}
			}

			private class VRage_Game_MyObjectBuilder_EnvironmentItems_003C_003EMyOBEnvironmentItemData_003C_003EActor : IActivator, IActivator<MyOBEnvironmentItemData>
			{
				private sealed override object CreateInstance()
				{
					return default(MyOBEnvironmentItemData);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyOBEnvironmentItemData CreateInstance()
				{
					return (MyOBEnvironmentItemData)(object)default(MyOBEnvironmentItemData);
				}

				MyOBEnvironmentItemData IActivator<MyOBEnvironmentItemData>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public MyPositionAndOrientation PositionAndOrientation;

			[ProtoMember(4)]
			public string SubtypeName;
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentItems_003C_003EItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentItems, MyOBEnvironmentItemData[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentItems owner, in MyOBEnvironmentItemData[] value)
			{
				owner.Items = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentItems owner, out MyOBEnvironmentItemData[] value)
			{
				value = owner.Items;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentItems_003C_003ECellsOffset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentItems, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentItems owner, in Vector3D value)
			{
				owner.CellsOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentItems owner, out Vector3D value)
			{
				value = owner.CellsOffset;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentItems_003C_003EEntityId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentItems owner, in long value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentItems owner, out long value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentItems_003C_003EPersistentFlags_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPersistentFlags_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, MyPersistentEntityFlags2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentItems owner, in MyPersistentEntityFlags2 value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentItems owner, out MyPersistentEntityFlags2 value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentItems_003C_003EName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentItems owner, in string value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentItems owner, out string value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentItems_003C_003EPositionAndOrientation_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPositionAndOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, MyPositionAndOrientation?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentItems owner, in MyPositionAndOrientation? value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentItems owner, out MyPositionAndOrientation? value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentItems_003C_003EComponentContainer_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EComponentContainer_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, MyObjectBuilder_ComponentContainer>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentItems owner, in MyObjectBuilder_ComponentContainer value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentItems owner, out MyObjectBuilder_ComponentContainer value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentItems_003C_003EEntityDefinitionId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityDefinitionId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentItems owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentItems owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentItems_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentItems owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentItems owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentItems_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentItems owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentItems owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentItems_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentItems owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentItems owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentItems_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentItems owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentItems owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_EnvironmentItems_003C_003EActor : IActivator, IActivator<MyObjectBuilder_EnvironmentItems>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_EnvironmentItems();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_EnvironmentItems CreateInstance()
			{
				return new MyObjectBuilder_EnvironmentItems();
			}

			MyObjectBuilder_EnvironmentItems IActivator<MyObjectBuilder_EnvironmentItems>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlArrayItem("Item")]
		[ProtoMember(7)]
		public MyOBEnvironmentItemData[] Items;

		[ProtoMember(10)]
		public Vector3D CellsOffset;
	}
}
