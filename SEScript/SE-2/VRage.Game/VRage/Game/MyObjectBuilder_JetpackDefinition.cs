using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_JetpackDefinition
	{
		protected class VRage_Game_MyObjectBuilder_JetpackDefinition_003C_003EThrusts_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_JetpackDefinition, List<MyJetpackThrustDefinition>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_JetpackDefinition owner, in List<MyJetpackThrustDefinition> value)
			{
				owner.Thrusts = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_JetpackDefinition owner, out List<MyJetpackThrustDefinition> value)
			{
				value = owner.Thrusts;
			}
		}

		protected class VRage_Game_MyObjectBuilder_JetpackDefinition_003C_003EThrustProperties_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_JetpackDefinition, MyObjectBuilder_ThrustDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_JetpackDefinition owner, in MyObjectBuilder_ThrustDefinition value)
			{
				owner.ThrustProperties = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_JetpackDefinition owner, out MyObjectBuilder_ThrustDefinition value)
			{
				value = owner.ThrustProperties;
			}
		}

		private class VRage_Game_MyObjectBuilder_JetpackDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_JetpackDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_JetpackDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_JetpackDefinition CreateInstance()
			{
				return new MyObjectBuilder_JetpackDefinition();
			}

			MyObjectBuilder_JetpackDefinition IActivator<MyObjectBuilder_JetpackDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(4)]
		[XmlArrayItem("Thrust")]
		public List<MyJetpackThrustDefinition> Thrusts;

		[ProtoMember(5)]
		public MyObjectBuilder_ThrustDefinition ThrustProperties;
	}
}
