using ProtoBuf;
using System.Collections.Generic;
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
	public class MyObjectBuilder_VisualLevelScript : MyObjectBuilder_VisualScript
	{
		protected class VRage_Game_MyObjectBuilder_VisualLevelScript_003C_003EInterface_003C_003EAccessor : VRage_Game_MyObjectBuilder_VisualScript_003C_003EInterface_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VisualLevelScript owner, in string value)
			{
				Set(ref *(MyObjectBuilder_VisualScript*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VisualLevelScript owner, out string value)
			{
				Get(ref *(MyObjectBuilder_VisualScript*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualLevelScript_003C_003EDependencyFilePaths_003C_003EAccessor : VRage_Game_MyObjectBuilder_VisualScript_003C_003EDependencyFilePaths_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VisualLevelScript owner, in List<string> value)
			{
				Set(ref *(MyObjectBuilder_VisualScript*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VisualLevelScript owner, out List<string> value)
			{
				Get(ref *(MyObjectBuilder_VisualScript*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualLevelScript_003C_003ENodes_003C_003EAccessor : VRage_Game_MyObjectBuilder_VisualScript_003C_003ENodes_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, List<MyObjectBuilder_ScriptNode>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VisualLevelScript owner, in List<MyObjectBuilder_ScriptNode> value)
			{
				Set(ref *(MyObjectBuilder_VisualScript*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VisualLevelScript owner, out List<MyObjectBuilder_ScriptNode> value)
			{
				Get(ref *(MyObjectBuilder_VisualScript*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualLevelScript_003C_003EName_003C_003EAccessor : VRage_Game_MyObjectBuilder_VisualScript_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VisualLevelScript owner, in string value)
			{
				Set(ref *(MyObjectBuilder_VisualScript*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VisualLevelScript owner, out string value)
			{
				Get(ref *(MyObjectBuilder_VisualScript*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualLevelScript_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VisualLevelScript owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VisualLevelScript owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualLevelScript_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VisualLevelScript owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VisualLevelScript owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualLevelScript_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VisualLevelScript owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VisualLevelScript owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualLevelScript_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VisualLevelScript owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VisualLevelScript owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_VisualLevelScript_003C_003EActor : IActivator, IActivator<MyObjectBuilder_VisualLevelScript>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_VisualLevelScript();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_VisualLevelScript CreateInstance()
			{
				return new MyObjectBuilder_VisualLevelScript();
			}

			MyObjectBuilder_VisualLevelScript IActivator<MyObjectBuilder_VisualLevelScript>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
