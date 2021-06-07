using ProtoBuf;
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
	public class MyObjectBuilder_ConstantScriptNode : MyObjectBuilder_ScriptNode
	{
		protected class VRage_Game_MyObjectBuilder_ConstantScriptNode_003C_003EValue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ConstantScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ConstantScriptNode owner, in string value)
			{
				owner.Value = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ConstantScriptNode owner, out string value)
			{
				value = owner.Value;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConstantScriptNode_003C_003EType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ConstantScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ConstantScriptNode owner, in string value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ConstantScriptNode owner, out string value)
			{
				value = owner.Type;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConstantScriptNode_003C_003EOutputIds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ConstantScriptNode, IdentifierList>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ConstantScriptNode owner, in IdentifierList value)
			{
				owner.OutputIds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ConstantScriptNode owner, out IdentifierList value)
			{
				value = owner.OutputIds;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConstantScriptNode_003C_003EVector_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ConstantScriptNode, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ConstantScriptNode owner, in Vector3D value)
			{
				owner.Vector = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ConstantScriptNode owner, out Vector3D value)
			{
				value = owner.Vector;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConstantScriptNode_003C_003EID_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EID_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConstantScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConstantScriptNode owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConstantScriptNode owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConstantScriptNode_003C_003EPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConstantScriptNode, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConstantScriptNode owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_ScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConstantScriptNode owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_ScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConstantScriptNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConstantScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConstantScriptNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConstantScriptNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConstantScriptNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConstantScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConstantScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConstantScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConstantScriptNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConstantScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConstantScriptNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConstantScriptNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConstantScriptNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConstantScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConstantScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConstantScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ConstantScriptNode_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ConstantScriptNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ConstantScriptNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ConstantScriptNode CreateInstance()
			{
				return new MyObjectBuilder_ConstantScriptNode();
			}

			MyObjectBuilder_ConstantScriptNode IActivator<MyObjectBuilder_ConstantScriptNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string Value = string.Empty;

		[ProtoMember(5)]
		public string Type = string.Empty;

		[ProtoMember(10)]
		public IdentifierList OutputIds;

		[ProtoMember(15)]
		public Vector3D Vector;
	}
}
