using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_InputScriptNode : MyObjectBuilder_EventScriptNode
	{
		protected class VRage_Game_MyObjectBuilder_InputScriptNode_003C_003EName_003C_003EAccessor : VRage_Game_MyObjectBuilder_EventScriptNode_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_InputScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_EventScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_InputScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_EventScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_InputScriptNode_003C_003ESequenceOutputID_003C_003EAccessor : VRage_Game_MyObjectBuilder_EventScriptNode_003C_003ESequenceOutputID_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_InputScriptNode owner, in int value)
			{
				Set(ref *(MyObjectBuilder_EventScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_InputScriptNode owner, out int value)
			{
				Get(ref *(MyObjectBuilder_EventScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_InputScriptNode_003C_003EOutputIDs_003C_003EAccessor : VRage_Game_MyObjectBuilder_EventScriptNode_003C_003EOutputIDs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, List<IdentifierList>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_InputScriptNode owner, in List<IdentifierList> value)
			{
				Set(ref *(MyObjectBuilder_EventScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_InputScriptNode owner, out List<IdentifierList> value)
			{
				Get(ref *(MyObjectBuilder_EventScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_InputScriptNode_003C_003EOutputNames_003C_003EAccessor : VRage_Game_MyObjectBuilder_EventScriptNode_003C_003EOutputNames_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_InputScriptNode owner, in List<string> value)
			{
				Set(ref *(MyObjectBuilder_EventScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_InputScriptNode owner, out List<string> value)
			{
				Get(ref *(MyObjectBuilder_EventScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_InputScriptNode_003C_003EOuputTypes_003C_003EAccessor : VRage_Game_MyObjectBuilder_EventScriptNode_003C_003EOuputTypes_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_InputScriptNode owner, in List<string> value)
			{
				Set(ref *(MyObjectBuilder_EventScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_InputScriptNode owner, out List<string> value)
			{
				Get(ref *(MyObjectBuilder_EventScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_InputScriptNode_003C_003EID_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EID_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_InputScriptNode owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_InputScriptNode owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_InputScriptNode_003C_003EPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_InputScriptNode owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_ScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_InputScriptNode owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_ScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_InputScriptNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_InputScriptNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_InputScriptNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_InputScriptNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_InputScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_InputScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_InputScriptNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_InputScriptNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_InputScriptNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_InputScriptNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_InputScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_InputScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_InputScriptNode_003C_003EActor : IActivator, IActivator<MyObjectBuilder_InputScriptNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_InputScriptNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_InputScriptNode CreateInstance()
			{
				return new MyObjectBuilder_InputScriptNode();
			}

			MyObjectBuilder_InputScriptNode IActivator<MyObjectBuilder_InputScriptNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
