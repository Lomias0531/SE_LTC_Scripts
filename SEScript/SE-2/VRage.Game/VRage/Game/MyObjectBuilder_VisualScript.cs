using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_VisualScript : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_VisualScript_003C_003EInterface_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScript, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScript owner, in string value)
			{
				owner.Interface = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScript owner, out string value)
			{
				value = owner.Interface;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScript_003C_003EDependencyFilePaths_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScript, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScript owner, in List<string> value)
			{
				owner.DependencyFilePaths = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScript owner, out List<string> value)
			{
				value = owner.DependencyFilePaths;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScript_003C_003ENodes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScript, List<MyObjectBuilder_ScriptNode>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScript owner, in List<MyObjectBuilder_ScriptNode> value)
			{
				owner.Nodes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScript owner, out List<MyObjectBuilder_ScriptNode> value)
			{
				value = owner.Nodes;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScript_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScript, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScript owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScript owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScript_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScript, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VisualScript owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VisualScript owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScript_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScript, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VisualScript owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VisualScript owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScript_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScript, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VisualScript owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VisualScript owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScript_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScript, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VisualScript owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VisualScript owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_VisualScript_003C_003EActor : IActivator, IActivator<MyObjectBuilder_VisualScript>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_VisualScript();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_VisualScript CreateInstance()
			{
				return new MyObjectBuilder_VisualScript();
			}

			MyObjectBuilder_VisualScript IActivator<MyObjectBuilder_VisualScript>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[Nullable]
		[ProtoMember(1)]
		public string Interface;

		[ProtoMember(4)]
		public List<string> DependencyFilePaths;

		[ProtoMember(7)]
		[DynamicObjectBuilder(false)]
		[XmlArrayItem("MyObjectBuilder_ScriptNode", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ScriptNode>))]
		public List<MyObjectBuilder_ScriptNode> Nodes;

		[ProtoMember(10)]
		public string Name;
	}
}
