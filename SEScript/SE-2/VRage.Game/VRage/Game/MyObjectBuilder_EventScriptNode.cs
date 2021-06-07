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
	public class MyObjectBuilder_EventScriptNode : MyObjectBuilder_ScriptNode
	{
		protected class VRage_Game_MyObjectBuilder_EventScriptNode_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EventScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EventScriptNode owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EventScriptNode owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EventScriptNode_003C_003ESequenceOutputID_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EventScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EventScriptNode owner, in int value)
			{
				owner.SequenceOutputID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EventScriptNode owner, out int value)
			{
				value = owner.SequenceOutputID;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EventScriptNode_003C_003EOutputIDs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EventScriptNode, List<IdentifierList>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EventScriptNode owner, in List<IdentifierList> value)
			{
				owner.OutputIDs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EventScriptNode owner, out List<IdentifierList> value)
			{
				value = owner.OutputIDs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EventScriptNode_003C_003EOutputNames_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EventScriptNode, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EventScriptNode owner, in List<string> value)
			{
				owner.OutputNames = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EventScriptNode owner, out List<string> value)
			{
				value = owner.OutputNames;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EventScriptNode_003C_003EOuputTypes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EventScriptNode, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EventScriptNode owner, in List<string> value)
			{
				owner.OuputTypes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EventScriptNode owner, out List<string> value)
			{
				value = owner.OuputTypes;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EventScriptNode_003C_003EID_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EID_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EventScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EventScriptNode owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EventScriptNode owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EventScriptNode_003C_003EPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EventScriptNode, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EventScriptNode owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_ScriptNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EventScriptNode owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_ScriptNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EventScriptNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EventScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EventScriptNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EventScriptNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EventScriptNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EventScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EventScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EventScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EventScriptNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EventScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EventScriptNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EventScriptNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EventScriptNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EventScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EventScriptNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EventScriptNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_EventScriptNode_003C_003EActor : IActivator, IActivator<MyObjectBuilder_EventScriptNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_EventScriptNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_EventScriptNode CreateInstance()
			{
				return new MyObjectBuilder_EventScriptNode();
			}

			MyObjectBuilder_EventScriptNode IActivator<MyObjectBuilder_EventScriptNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string Name;

		[ProtoMember(5)]
		public int SequenceOutputID = -1;

		[ProtoMember(10)]
		public List<IdentifierList> OutputIDs = new List<IdentifierList>();

		[ProtoMember(15)]
		public List<string> OutputNames = new List<string>();

		[ProtoMember(20)]
		public List<string> OuputTypes = new List<string>();
	}
}
