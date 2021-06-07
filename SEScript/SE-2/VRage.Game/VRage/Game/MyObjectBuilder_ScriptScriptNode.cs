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
	public class MyObjectBuilder_ScriptScriptNode : MyObjectBuilder_ScriptNode
	{
		protected class VRage_Game_MyObjectBuilder_ScriptScriptNode_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScriptScriptNode owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScriptScriptNode owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScriptScriptNode_003C_003EPath_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScriptScriptNode owner, in string value)
			{
				owner.Path = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScriptScriptNode owner, out string value)
			{
				value = owner.Path;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScriptScriptNode_003C_003ESequenceOutput_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScriptScriptNode owner, in int value)
			{
				owner.SequenceOutput = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScriptScriptNode owner, out int value)
			{
				value = owner.SequenceOutput;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScriptScriptNode_003C_003ESequenceInput_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScriptScriptNode owner, in int value)
			{
				owner.SequenceInput = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScriptScriptNode owner, out int value)
			{
				value = owner.SequenceInput;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScriptScriptNode_003C_003EInputs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptScriptNode, List<MyInputParameterSerializationData>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScriptScriptNode owner, in List<MyInputParameterSerializationData> value)
			{
				owner.Inputs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScriptScriptNode owner, out List<MyInputParameterSerializationData> value)
			{
				value = owner.Inputs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScriptScriptNode_003C_003EOutputs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptScriptNode, List<MyOutputParameterSerializationData>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScriptScriptNode owner, in List<MyOutputParameterSerializationData> value)
			{
				owner.Outputs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScriptScriptNode owner, out List<MyOutputParameterSerializationData> value)
			{
				value = owner.Outputs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScriptScriptNode_003C_003EID_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EID_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptScriptNode owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptScriptNode owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScriptScriptNode_003C_003EPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptScriptNode, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptScriptNode owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_ScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptScriptNode owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_ScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScriptScriptNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptScriptNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptScriptNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScriptScriptNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScriptScriptNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptScriptNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptScriptNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScriptScriptNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ScriptScriptNode_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ScriptScriptNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ScriptScriptNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ScriptScriptNode CreateInstance()
			{
				return new MyObjectBuilder_ScriptScriptNode();
			}

			MyObjectBuilder_ScriptScriptNode IActivator<MyObjectBuilder_ScriptScriptNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string Name = string.Empty;

		[ProtoMember(5)]
		public string Path;

		[ProtoMember(10)]
		public int SequenceOutput = -1;

		[ProtoMember(15)]
		public int SequenceInput = -1;

		[ProtoMember(20)]
		public List<MyInputParameterSerializationData> Inputs = new List<MyInputParameterSerializationData>();

		[ProtoMember(25)]
		public List<MyOutputParameterSerializationData> Outputs = new List<MyOutputParameterSerializationData>();
	}
}
