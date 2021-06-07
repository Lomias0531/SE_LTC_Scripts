using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.VisualScripting
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ForLoopScriptNode : MyObjectBuilder_ScriptNode
	{
		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003ESequenceInputs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, List<int>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in List<int> value)
			{
				owner.SequenceInputs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out List<int> value)
			{
				value = owner.SequenceInputs;
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003ESequenceBody_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in int value)
			{
				owner.SequenceBody = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out int value)
			{
				value = owner.SequenceBody;
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003ESequenceOutput_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in int value)
			{
				owner.SequenceOutput = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out int value)
			{
				value = owner.SequenceOutput;
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003EFirstIndexValueInput_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, MyVariableIdentifier>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in MyVariableIdentifier value)
			{
				owner.FirstIndexValueInput = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out MyVariableIdentifier value)
			{
				value = owner.FirstIndexValueInput;
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003ELastIndexValueInput_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, MyVariableIdentifier>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in MyVariableIdentifier value)
			{
				owner.LastIndexValueInput = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out MyVariableIdentifier value)
			{
				value = owner.LastIndexValueInput;
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003EIncrementValueInput_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, MyVariableIdentifier>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in MyVariableIdentifier value)
			{
				owner.IncrementValueInput = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out MyVariableIdentifier value)
			{
				value = owner.IncrementValueInput;
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003ECounterValueOutputs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, List<MyVariableIdentifier>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in List<MyVariableIdentifier> value)
			{
				owner.CounterValueOutputs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out List<MyVariableIdentifier> value)
			{
				value = owner.CounterValueOutputs;
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003EFirstIndexValue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in string value)
			{
				owner.FirstIndexValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out string value)
			{
				value = owner.FirstIndexValue;
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003ELastIndexValue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in string value)
			{
				owner.LastIndexValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out string value)
			{
				value = owner.LastIndexValue;
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003EIncrementValue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in string value)
			{
				owner.IncrementValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out string value)
			{
				value = owner.IncrementValue;
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003EID_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EID_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003EPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_ScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_ScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ForLoopScriptNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ForLoopScriptNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ForLoopScriptNode CreateInstance()
			{
				return new MyObjectBuilder_ForLoopScriptNode();
			}

			MyObjectBuilder_ForLoopScriptNode IActivator<MyObjectBuilder_ForLoopScriptNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public List<int> SequenceInputs = new List<int>();

		[ProtoMember(5)]
		public int SequenceBody = -1;

		[ProtoMember(10)]
		public int SequenceOutput = -1;

		[ProtoMember(15)]
		public MyVariableIdentifier FirstIndexValueInput = MyVariableIdentifier.Default;

		[ProtoMember(20)]
		public MyVariableIdentifier LastIndexValueInput = MyVariableIdentifier.Default;

		[ProtoMember(25)]
		public MyVariableIdentifier IncrementValueInput = MyVariableIdentifier.Default;

		[ProtoMember(30)]
		public List<MyVariableIdentifier> CounterValueOutputs = new List<MyVariableIdentifier>();

		[ProtoMember(35)]
		public string FirstIndexValue = "0";

		[ProtoMember(40)]
		public string LastIndexValue = "0";

		[ProtoMember(45)]
		public string IncrementValue = "1";
	}
}
