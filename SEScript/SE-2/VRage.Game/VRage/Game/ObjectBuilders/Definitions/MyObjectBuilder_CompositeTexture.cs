using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_CompositeTexture : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ELeftTop_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value)
			{
				owner.LeftTop = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value)
			{
				value = owner.LeftTop;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ELeftCenter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value)
			{
				owner.LeftCenter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value)
			{
				value = owner.LeftCenter;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ELeftBottom_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value)
			{
				owner.LeftBottom = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value)
			{
				value = owner.LeftBottom;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ECenterTop_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value)
			{
				owner.CenterTop = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value)
			{
				value = owner.CenterTop;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ECenter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value)
			{
				owner.Center = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value)
			{
				value = owner.Center;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ECenterBottom_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value)
			{
				owner.CenterBottom = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value)
			{
				value = owner.CenterBottom;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ERightTop_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value)
			{
				owner.RightTop = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value)
			{
				value = owner.RightTop;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ERightCenter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value)
			{
				owner.RightCenter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value)
			{
				value = owner.RightCenter;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ERightBottom_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value)
			{
				owner.RightBottom = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value)
			{
				value = owner.RightBottom;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompositeTexture, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompositeTexture owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompositeTexture owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompositeTexture, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompositeTexture owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompositeTexture owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003EActor : IActivator, IActivator<MyObjectBuilder_CompositeTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_CompositeTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_CompositeTexture CreateInstance()
			{
				return new MyObjectBuilder_CompositeTexture();
			}

			MyObjectBuilder_CompositeTexture IActivator<MyObjectBuilder_CompositeTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash LeftTop = MyStringHash.NullOrEmpty;

		public MyStringHash LeftCenter = MyStringHash.NullOrEmpty;

		public MyStringHash LeftBottom = MyStringHash.NullOrEmpty;

		public MyStringHash CenterTop = MyStringHash.NullOrEmpty;

		public MyStringHash Center = MyStringHash.NullOrEmpty;

		public MyStringHash CenterBottom = MyStringHash.NullOrEmpty;

		public MyStringHash RightTop = MyStringHash.NullOrEmpty;

		public MyStringHash RightCenter = MyStringHash.NullOrEmpty;

		public MyStringHash RightBottom = MyStringHash.NullOrEmpty;

		public virtual bool IsValid()
		{
			if (!(LeftTop != MyStringHash.NullOrEmpty) && !(LeftTop != MyStringHash.NullOrEmpty) && !(LeftCenter != MyStringHash.NullOrEmpty) && !(LeftBottom != MyStringHash.NullOrEmpty) && !(CenterTop != MyStringHash.NullOrEmpty) && !(Center != MyStringHash.NullOrEmpty) && !(CenterBottom != MyStringHash.NullOrEmpty) && !(RightTop != MyStringHash.NullOrEmpty) && !(RightCenter != MyStringHash.NullOrEmpty))
			{
				return RightBottom != MyStringHash.NullOrEmpty;
			}
			return true;
		}
	}
}
