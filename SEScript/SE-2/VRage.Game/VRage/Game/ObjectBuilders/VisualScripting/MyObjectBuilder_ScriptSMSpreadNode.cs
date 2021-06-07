using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.VisualScripting
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ScriptSMSpreadNode : MyObjectBuilder_ScriptSMNode
	{
		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMSpreadNode_003C_003EPosition_003C_003EAccessor : VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMSpreadNode, SerializableVector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMSpreadNode owner, in SerializableVector2 value)
			{
				Set(ref *(MyObjectBuilder_ScriptSMNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMSpreadNode owner, out SerializableVector2 value)
			{
				Get(ref *(MyObjectBuilder_ScriptSMNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMSpreadNode_003C_003EName_003C_003EAccessor : VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMSpreadNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMSpreadNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_ScriptSMNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMSpreadNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_ScriptSMNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMSpreadNode_003C_003EScriptFilePath_003C_003EAccessor : VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003EScriptFilePath_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMSpreadNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMSpreadNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_ScriptSMNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMSpreadNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_ScriptSMNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMSpreadNode_003C_003EScriptClassName_003C_003EAccessor : VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003EScriptClassName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMSpreadNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMSpreadNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_ScriptSMNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMSpreadNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_ScriptSMNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMSpreadNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMSpreadNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMSpreadNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMSpreadNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMSpreadNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMSpreadNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMSpreadNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMSpreadNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMSpreadNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMSpreadNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMSpreadNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMSpreadNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMSpreadNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMSpreadNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMSpreadNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMSpreadNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMSpreadNode_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ScriptSMSpreadNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ScriptSMSpreadNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ScriptSMSpreadNode CreateInstance()
			{
				return new MyObjectBuilder_ScriptSMSpreadNode();
			}

			MyObjectBuilder_ScriptSMSpreadNode IActivator<MyObjectBuilder_ScriptSMSpreadNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
