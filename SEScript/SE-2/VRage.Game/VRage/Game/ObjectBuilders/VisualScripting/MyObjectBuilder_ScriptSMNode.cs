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
	public class MyObjectBuilder_ScriptSMNode : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptSMNode, SerializableVector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScriptSMNode owner, in SerializableVector2 value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScriptSMNode owner, out SerializableVector2 value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptSMNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScriptSMNode owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScriptSMNode owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003EScriptFilePath_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptSMNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScriptSMNode owner, in string value)
			{
				owner.ScriptFilePath = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScriptSMNode owner, out string value)
			{
				value = owner.ScriptFilePath;
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003EScriptClassName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptSMNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScriptSMNode owner, in string value)
			{
				owner.ScriptClassName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScriptSMNode owner, out string value)
			{
				value = owner.ScriptClassName;
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScriptSMNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScriptSMNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ScriptSMNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ScriptSMNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ScriptSMNode CreateInstance()
			{
				return new MyObjectBuilder_ScriptSMNode();
			}

			MyObjectBuilder_ScriptSMNode IActivator<MyObjectBuilder_ScriptSMNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public SerializableVector2 Position;

		[ProtoMember(4)]
		public string Name;

		[ProtoMember(7)]
		public string ScriptFilePath;

		[ProtoMember(10)]
		public string ScriptClassName;
	}
}
