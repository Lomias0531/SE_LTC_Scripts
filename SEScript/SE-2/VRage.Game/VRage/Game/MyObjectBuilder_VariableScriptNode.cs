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
	public class MyObjectBuilder_VariableScriptNode : MyObjectBuilder_ScriptNode
	{
		protected class VRage_Game_MyObjectBuilder_VariableScriptNode_003C_003EVariableName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VariableScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableScriptNode owner, in string value)
			{
				owner.VariableName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableScriptNode owner, out string value)
			{
				value = owner.VariableName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableScriptNode_003C_003EVariableType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VariableScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableScriptNode owner, in string value)
			{
				owner.VariableType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableScriptNode owner, out string value)
			{
				value = owner.VariableType;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableScriptNode_003C_003EVariableValue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VariableScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableScriptNode owner, in string value)
			{
				owner.VariableValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableScriptNode owner, out string value)
			{
				value = owner.VariableValue;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableScriptNode_003C_003EOutputNodeIds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VariableScriptNode, List<MyVariableIdentifier>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableScriptNode owner, in List<MyVariableIdentifier> value)
			{
				owner.OutputNodeIds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableScriptNode owner, out List<MyVariableIdentifier> value)
			{
				value = owner.OutputNodeIds;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableScriptNode_003C_003EVector_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VariableScriptNode, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableScriptNode owner, in Vector3D value)
			{
				owner.Vector = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableScriptNode owner, out Vector3D value)
			{
				value = owner.Vector;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableScriptNode_003C_003EOutputNodeIdsX_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VariableScriptNode, List<MyVariableIdentifier>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableScriptNode owner, in List<MyVariableIdentifier> value)
			{
				owner.OutputNodeIdsX = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableScriptNode owner, out List<MyVariableIdentifier> value)
			{
				value = owner.OutputNodeIdsX;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableScriptNode_003C_003EOutputNodeIdsY_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VariableScriptNode, List<MyVariableIdentifier>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableScriptNode owner, in List<MyVariableIdentifier> value)
			{
				owner.OutputNodeIdsY = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableScriptNode owner, out List<MyVariableIdentifier> value)
			{
				value = owner.OutputNodeIdsY;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableScriptNode_003C_003EOutputNodeIdsZ_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VariableScriptNode, List<MyVariableIdentifier>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableScriptNode owner, in List<MyVariableIdentifier> value)
			{
				owner.OutputNodeIdsZ = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableScriptNode owner, out List<MyVariableIdentifier> value)
			{
				value = owner.OutputNodeIdsZ;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableScriptNode_003C_003EID_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EID_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VariableScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VariableScriptNode owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VariableScriptNode owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableScriptNode_003C_003EPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VariableScriptNode, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VariableScriptNode owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_ScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VariableScriptNode owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_ScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableScriptNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VariableScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VariableScriptNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VariableScriptNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableScriptNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VariableScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VariableScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VariableScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableScriptNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VariableScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VariableScriptNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VariableScriptNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableScriptNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VariableScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VariableScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VariableScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_VariableScriptNode_003C_003EActor : IActivator, IActivator<MyObjectBuilder_VariableScriptNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_VariableScriptNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_VariableScriptNode CreateInstance()
			{
				return new MyObjectBuilder_VariableScriptNode();
			}

			MyObjectBuilder_VariableScriptNode IActivator<MyObjectBuilder_VariableScriptNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string VariableName = "Default";

		[ProtoMember(5)]
		public string VariableType = string.Empty;

		[ProtoMember(10)]
		public string VariableValue = string.Empty;

		[ProtoMember(15)]
		public List<MyVariableIdentifier> OutputNodeIds = new List<MyVariableIdentifier>();

		[ProtoMember(20)]
		public Vector3D Vector;

		[ProtoMember(25)]
		public List<MyVariableIdentifier> OutputNodeIdsX = new List<MyVariableIdentifier>();

		[ProtoMember(30)]
		public List<MyVariableIdentifier> OutputNodeIdsY = new List<MyVariableIdentifier>();

		[ProtoMember(35)]
		public List<MyVariableIdentifier> OutputNodeIdsZ = new List<MyVariableIdentifier>();
	}
}
