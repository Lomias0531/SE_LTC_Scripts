using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Toolbar : MyObjectBuilder_Base
	{
		[ProtoContract]
		public struct Slot
		{
			protected class VRage_Game_MyObjectBuilder_Toolbar_003C_003ESlot_003C_003EIndex_003C_003EAccessor : IMemberAccessor<Slot, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Slot owner, in int value)
				{
					owner.Index = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Slot owner, out int value)
				{
					value = owner.Index;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Toolbar_003C_003ESlot_003C_003EItem_003C_003EAccessor : IMemberAccessor<Slot, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Slot owner, in string value)
				{
					owner.Item = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Slot owner, out string value)
				{
					value = owner.Item;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Toolbar_003C_003ESlot_003C_003EData_003C_003EAccessor : IMemberAccessor<Slot, MyObjectBuilder_ToolbarItem>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Slot owner, in MyObjectBuilder_ToolbarItem value)
				{
					owner.Data = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Slot owner, out MyObjectBuilder_ToolbarItem value)
				{
					value = owner.Data;
				}
			}

			private class VRage_Game_MyObjectBuilder_Toolbar_003C_003ESlot_003C_003EActor : IActivator, IActivator<Slot>
			{
				private sealed override object CreateInstance()
				{
					return default(Slot);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override Slot CreateInstance()
				{
					return (Slot)(object)default(Slot);
				}

				Slot IActivator<Slot>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public int Index;

			[ProtoMember(4)]
			public string Item;

			[ProtoMember(7)]
			[DynamicObjectBuilder(false)]
			[XmlElement(Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ToolbarItem>))]
			public MyObjectBuilder_ToolbarItem Data;
		}

		protected class VRage_Game_MyObjectBuilder_Toolbar_003C_003EToolbarType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Toolbar, MyToolbarType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Toolbar owner, in MyToolbarType value)
			{
				owner.ToolbarType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Toolbar owner, out MyToolbarType value)
			{
				value = owner.ToolbarType;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Toolbar_003C_003ESelectedSlot_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Toolbar, int?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Toolbar owner, in int? value)
			{
				owner.SelectedSlot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Toolbar owner, out int? value)
			{
				value = owner.SelectedSlot;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Toolbar_003C_003ESlots_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Toolbar, List<Slot>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Toolbar owner, in List<Slot> value)
			{
				owner.Slots = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Toolbar owner, out List<Slot> value)
			{
				value = owner.Slots;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Toolbar_003C_003ESlotsGamepad_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Toolbar, List<Slot>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Toolbar owner, in List<Slot> value)
			{
				owner.SlotsGamepad = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Toolbar owner, out List<Slot> value)
			{
				value = owner.SlotsGamepad;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Toolbar_003C_003EColorMaskHSVList_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Toolbar, List<Vector3>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Toolbar owner, in List<Vector3> value)
			{
				owner.ColorMaskHSVList = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Toolbar owner, out List<Vector3> value)
			{
				value = owner.ColorMaskHSVList;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Toolbar_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Toolbar, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Toolbar owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Toolbar owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Toolbar_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Toolbar, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Toolbar owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Toolbar owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Toolbar_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Toolbar, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Toolbar owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Toolbar owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Toolbar_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Toolbar, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Toolbar owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Toolbar owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_Toolbar_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Toolbar>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Toolbar();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Toolbar CreateInstance()
			{
				return new MyObjectBuilder_Toolbar();
			}

			MyObjectBuilder_Toolbar IActivator<MyObjectBuilder_Toolbar>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(10)]
		public MyToolbarType ToolbarType;

		[ProtoMember(13)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public int? SelectedSlot;

		[ProtoMember(16)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public List<Slot> Slots;

		[ProtoMember(17)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public List<Slot> SlotsGamepad;

		[ProtoMember(19)]
		[DefaultValue(null)]
		[NoSerialize]
		public List<Vector3> ColorMaskHSVList;

		public bool ShouldSerializeColorMaskHSVList()
		{
			return false;
		}

		public MyObjectBuilder_Toolbar()
		{
			Slots = new List<Slot>();
		}

		public void Remap(IMyRemapHelper remapHelper)
		{
			if (Slots != null)
			{
				foreach (Slot slot in Slots)
				{
					slot.Data.Remap(remapHelper);
				}
			}
		}
	}
}
