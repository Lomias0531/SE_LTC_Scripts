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
	public class MyObjectBuilder_ScriptSMFinalNode : MyObjectBuilder_ScriptSMNode
	{
		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMFinalNode_003C_003EPosition_003C_003EAccessor : VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMFinalNode, SerializableVector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMFinalNode owner, in SerializableVector2 value)
			{
				Set(ref *(MyObjectBuilder_ScriptSMNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMFinalNode owner, out SerializableVector2 value)
			{
				Get(ref *(MyObjectBuilder_ScriptSMNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMFinalNode_003C_003EName_003C_003EAccessor : VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMFinalNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMFinalNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_ScriptSMNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMFinalNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_ScriptSMNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMFinalNode_003C_003EScriptFilePath_003C_003EAccessor : VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003EScriptFilePath_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMFinalNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMFinalNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_ScriptSMNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMFinalNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_ScriptSMNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMFinalNode_003C_003EScriptClassName_003C_003EAccessor : VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003EScriptClassName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMFinalNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMFinalNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_ScriptSMNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMFinalNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_ScriptSMNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMFinalNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMFinalNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMFinalNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMFinalNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMFinalNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMFinalNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMFinalNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMFinalNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMFinalNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMFinalNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMFinalNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMFinalNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMFinalNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMFinalNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMFinalNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMFinalNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMFinalNode_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ScriptSMFinalNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ScriptSMFinalNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ScriptSMFinalNode CreateInstance()
			{
				return new MyObjectBuilder_ScriptSMFinalNode();
			}

			MyObjectBuilder_ScriptSMFinalNode IActivator<MyObjectBuilder_ScriptSMFinalNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
