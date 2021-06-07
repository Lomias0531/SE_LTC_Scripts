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
	public class MyObjectBuilder_FunctionScriptNode : MyObjectBuilder_ScriptNode
	{
		protected class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003EVersion_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FunctionScriptNode owner, in int value)
			{
				owner.Version = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FunctionScriptNode owner, out int value)
			{
				value = owner.Version;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003EDeclaringType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FunctionScriptNode owner, in string value)
			{
				owner.DeclaringType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FunctionScriptNode owner, out string value)
			{
				value = owner.DeclaringType;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003EType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FunctionScriptNode owner, in string value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FunctionScriptNode owner, out string value)
			{
				value = owner.Type;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003EExtOfType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FunctionScriptNode owner, in string value)
			{
				owner.ExtOfType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FunctionScriptNode owner, out string value)
			{
				value = owner.ExtOfType;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003ESequenceInputID_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FunctionScriptNode owner, in int value)
			{
				owner.SequenceInputID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FunctionScriptNode owner, out int value)
			{
				value = owner.SequenceInputID;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003ESequenceOutputID_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FunctionScriptNode owner, in int value)
			{
				owner.SequenceOutputID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FunctionScriptNode owner, out int value)
			{
				value = owner.SequenceOutputID;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003EInstanceInputID_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, MyVariableIdentifier>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FunctionScriptNode owner, in MyVariableIdentifier value)
			{
				owner.InstanceInputID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FunctionScriptNode owner, out MyVariableIdentifier value)
			{
				value = owner.InstanceInputID;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003EInputParameterIDs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, List<MyVariableIdentifier>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FunctionScriptNode owner, in List<MyVariableIdentifier> value)
			{
				owner.InputParameterIDs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FunctionScriptNode owner, out List<MyVariableIdentifier> value)
			{
				value = owner.InputParameterIDs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003EOutputParametersIDs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, List<IdentifierList>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FunctionScriptNode owner, in List<IdentifierList> value)
			{
				owner.OutputParametersIDs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FunctionScriptNode owner, out List<IdentifierList> value)
			{
				value = owner.OutputParametersIDs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003EInputParameterValues_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, List<MyParameterValue>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FunctionScriptNode owner, in List<MyParameterValue> value)
			{
				owner.InputParameterValues = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FunctionScriptNode owner, out List<MyParameterValue> value)
			{
				value = owner.InputParameterValues;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003EID_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EID_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FunctionScriptNode owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FunctionScriptNode owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003EPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionScriptNode, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FunctionScriptNode owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_ScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FunctionScriptNode owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_ScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FunctionScriptNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FunctionScriptNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FunctionScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FunctionScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FunctionScriptNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FunctionScriptNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FunctionScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FunctionScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_FunctionScriptNode_003C_003EActor : IActivator, IActivator<MyObjectBuilder_FunctionScriptNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_FunctionScriptNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_FunctionScriptNode CreateInstance()
			{
				return new MyObjectBuilder_FunctionScriptNode();
			}

			MyObjectBuilder_FunctionScriptNode IActivator<MyObjectBuilder_FunctionScriptNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public int Version;

		[ProtoMember(5)]
		public string DeclaringType = string.Empty;

		[ProtoMember(10)]
		public string Type = string.Empty;

		[ProtoMember(15)]
		public string ExtOfType = string.Empty;

		[ProtoMember(20)]
		public int SequenceInputID = -1;

		[ProtoMember(25)]
		public int SequenceOutputID = -1;

		[ProtoMember(30)]
		public MyVariableIdentifier InstanceInputID = MyVariableIdentifier.Default;

		[ProtoMember(35)]
		public List<MyVariableIdentifier> InputParameterIDs = new List<MyVariableIdentifier>();

		[ProtoMember(40)]
		public List<IdentifierList> OutputParametersIDs = new List<IdentifierList>();

		[ProtoMember(45)]
		public List<MyParameterValue> InputParameterValues = new List<MyParameterValue>();
	}
}
