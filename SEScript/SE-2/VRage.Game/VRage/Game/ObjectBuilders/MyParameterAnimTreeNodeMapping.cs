using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game.ObjectBuilders
{
	/// <summary>
	/// Helper struct: parameter mapping.
	/// </summary>
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public struct MyParameterAnimTreeNodeMapping
	{
		protected class VRage_Game_ObjectBuilders_MyParameterAnimTreeNodeMapping_003C_003EParam_003C_003EAccessor : IMemberAccessor<MyParameterAnimTreeNodeMapping, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyParameterAnimTreeNodeMapping owner, in float value)
			{
				owner.Param = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyParameterAnimTreeNodeMapping owner, out float value)
			{
				value = owner.Param;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyParameterAnimTreeNodeMapping_003C_003ENode_003C_003EAccessor : IMemberAccessor<MyParameterAnimTreeNodeMapping, MyObjectBuilder_AnimationTreeNode>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyParameterAnimTreeNodeMapping owner, in MyObjectBuilder_AnimationTreeNode value)
			{
				owner.Node = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyParameterAnimTreeNodeMapping owner, out MyObjectBuilder_AnimationTreeNode value)
			{
				value = owner.Node;
			}
		}

		private class VRage_Game_ObjectBuilders_MyParameterAnimTreeNodeMapping_003C_003EActor : IActivator, IActivator<MyParameterAnimTreeNodeMapping>
		{
			private sealed override object CreateInstance()
			{
				return default(MyParameterAnimTreeNodeMapping);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyParameterAnimTreeNodeMapping CreateInstance()
			{
				return (MyParameterAnimTreeNodeMapping)(object)default(MyParameterAnimTreeNodeMapping);
			}

			MyParameterAnimTreeNodeMapping IActivator<MyParameterAnimTreeNodeMapping>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(31)]
		public float Param;

		[ProtoMember(34)]
		[XmlElement(typeof(MyAbstractXmlSerializer<MyObjectBuilder_AnimationTreeNode>))]
		public MyObjectBuilder_AnimationTreeNode Node;
	}
}
