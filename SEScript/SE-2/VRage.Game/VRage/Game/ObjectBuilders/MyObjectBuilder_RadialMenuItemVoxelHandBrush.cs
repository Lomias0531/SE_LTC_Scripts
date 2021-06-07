using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_RadialMenuItemVoxelHandBrush : MyObjectBuilder_RadialMenuItem
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush_003C_003EBrushSubtypeName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHandBrush, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner, in string value)
			{
				owner.BrushSubtypeName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner, out string value)
			{
				value = owner.BrushSubtypeName;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush_003C_003EIcon_003C_003EAccessor : VRage_Game_MyObjectBuilder_RadialMenuItem_003C_003EIcon_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHandBrush, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner, in string value)
			{
				Set(ref *(MyObjectBuilder_RadialMenuItem*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner, out string value)
			{
				Get(ref *(MyObjectBuilder_RadialMenuItem*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush_003C_003ELabel_003C_003EAccessor : VRage_Game_MyObjectBuilder_RadialMenuItem_003C_003ELabel_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHandBrush, MyStringId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner, in MyStringId value)
			{
				Set(ref *(MyObjectBuilder_RadialMenuItem*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner, out MyStringId value)
			{
				Get(ref *(MyObjectBuilder_RadialMenuItem*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHandBrush, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHandBrush, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHandBrush, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHandBrush, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush_003C_003EActor : IActivator, IActivator<MyObjectBuilder_RadialMenuItemVoxelHandBrush>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_RadialMenuItemVoxelHandBrush();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_RadialMenuItemVoxelHandBrush CreateInstance()
			{
				return new MyObjectBuilder_RadialMenuItemVoxelHandBrush();
			}

			MyObjectBuilder_RadialMenuItemVoxelHandBrush IActivator<MyObjectBuilder_RadialMenuItemVoxelHandBrush>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string BrushSubtypeName;
	}
}
