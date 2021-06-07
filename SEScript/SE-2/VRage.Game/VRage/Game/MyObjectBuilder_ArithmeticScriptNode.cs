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
	public class MyObjectBuilder_ArithmeticScriptNode : MyObjectBuilder_ScriptNode
	{
		protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode_003C_003EOutputNodeIDs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, List<MyVariableIdentifier>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in List<MyVariableIdentifier> value)
			{
				owner.OutputNodeIDs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out List<MyVariableIdentifier> value)
			{
				value = owner.OutputNodeIDs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode_003C_003EOperation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in string value)
			{
				owner.Operation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out string value)
			{
				value = owner.Operation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode_003C_003EType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in string value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out string value)
			{
				value = owner.Type;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode_003C_003EInputAID_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, MyVariableIdentifier>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in MyVariableIdentifier value)
			{
				owner.InputAID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out MyVariableIdentifier value)
			{
				value = owner.InputAID;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode_003C_003EInputBID_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, MyVariableIdentifier>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in MyVariableIdentifier value)
			{
				owner.InputBID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out MyVariableIdentifier value)
			{
				value = owner.InputBID;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode_003C_003EValueA_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in string value)
			{
				owner.ValueA = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out string value)
			{
				value = owner.ValueA;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode_003C_003EValueB_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in string value)
			{
				owner.ValueB = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out string value)
			{
				value = owner.ValueB;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode_003C_003EID_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EID_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode_003C_003EPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_ScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_ScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ArithmeticScriptNode_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ArithmeticScriptNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ArithmeticScriptNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ArithmeticScriptNode CreateInstance()
			{
				return new MyObjectBuilder_ArithmeticScriptNode();
			}

			MyObjectBuilder_ArithmeticScriptNode IActivator<MyObjectBuilder_ArithmeticScriptNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public List<MyVariableIdentifier> OutputNodeIDs = new List<MyVariableIdentifier>();

		[ProtoMember(5)]
		public string Operation;

		[ProtoMember(10)]
		public string Type;

		[ProtoMember(15)]
		public MyVariableIdentifier InputAID = MyVariableIdentifier.Default;

		[ProtoMember(20)]
		public MyVariableIdentifier InputBID = MyVariableIdentifier.Default;

		[ProtoMember(25)]
		public string ValueA = string.Empty;

		[ProtoMember(30)]
		public string ValueB = string.Empty;
	}
}
