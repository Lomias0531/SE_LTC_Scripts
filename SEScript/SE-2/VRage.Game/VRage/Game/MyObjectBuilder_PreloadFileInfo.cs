using ProtoBuf;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_PreloadFileInfo : MyObjectBuilder_Base
	{
		[Flags]
		public enum PreloadType
		{
			MainMenu = 0x1,
			SessionPreload = 0x2
		}

		protected class VRage_Game_MyObjectBuilder_PreloadFileInfo_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PreloadFileInfo, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PreloadFileInfo owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PreloadFileInfo owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PreloadFileInfo_003C_003ELoadOnDedicated_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PreloadFileInfo, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PreloadFileInfo owner, in bool value)
			{
				owner.LoadOnDedicated = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PreloadFileInfo owner, out bool value)
			{
				value = owner.LoadOnDedicated;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PreloadFileInfo_003C_003EType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PreloadFileInfo, PreloadType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PreloadFileInfo owner, in PreloadType value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PreloadFileInfo owner, out PreloadType value)
			{
				value = owner.Type;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PreloadFileInfo_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PreloadFileInfo, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PreloadFileInfo owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PreloadFileInfo owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PreloadFileInfo_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PreloadFileInfo, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PreloadFileInfo owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PreloadFileInfo owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PreloadFileInfo_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PreloadFileInfo, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PreloadFileInfo owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PreloadFileInfo owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PreloadFileInfo_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PreloadFileInfo, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PreloadFileInfo owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PreloadFileInfo owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_PreloadFileInfo_003C_003EActor : IActivator, IActivator<MyObjectBuilder_PreloadFileInfo>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_PreloadFileInfo();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_PreloadFileInfo CreateInstance()
			{
				return new MyObjectBuilder_PreloadFileInfo();
			}

			MyObjectBuilder_PreloadFileInfo IActivator<MyObjectBuilder_PreloadFileInfo>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(4)]
		public string Name;

		[ProtoMember(7)]
		[DefaultValue(false)]
		public bool LoadOnDedicated;

		[ProtoMember(10)]
		public PreloadType Type;
	}
}
