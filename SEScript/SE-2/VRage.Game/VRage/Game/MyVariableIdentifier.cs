using ProtoBuf;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage.Game.VisualScripting;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game
{
	[ProtoContract]
	public struct MyVariableIdentifier
	{
		protected class VRage_Game_MyVariableIdentifier_003C_003ENodeID_003C_003EAccessor : IMemberAccessor<MyVariableIdentifier, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyVariableIdentifier owner, in int value)
			{
				owner.NodeID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyVariableIdentifier owner, out int value)
			{
				value = owner.NodeID;
			}
		}

		protected class VRage_Game_MyVariableIdentifier_003C_003EVariableName_003C_003EAccessor : IMemberAccessor<MyVariableIdentifier, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyVariableIdentifier owner, in string value)
			{
				owner.VariableName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyVariableIdentifier owner, out string value)
			{
				value = owner.VariableName;
			}
		}

		protected class VRage_Game_MyVariableIdentifier_003C_003EOriginName_003C_003EAccessor : IMemberAccessor<MyVariableIdentifier, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyVariableIdentifier owner, in string value)
			{
				owner.OriginName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyVariableIdentifier owner, out string value)
			{
				value = owner.OriginName;
			}
		}

		protected class VRage_Game_MyVariableIdentifier_003C_003EOriginType_003C_003EAccessor : IMemberAccessor<MyVariableIdentifier, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyVariableIdentifier owner, in string value)
			{
				owner.OriginType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyVariableIdentifier owner, out string value)
			{
				value = owner.OriginType;
			}
		}

		private class VRage_Game_MyVariableIdentifier_003C_003EActor : IActivator, IActivator<MyVariableIdentifier>
		{
			private sealed override object CreateInstance()
			{
				return default(MyVariableIdentifier);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyVariableIdentifier CreateInstance()
			{
				return (MyVariableIdentifier)(object)default(MyVariableIdentifier);
			}

			MyVariableIdentifier IActivator<MyVariableIdentifier>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public int NodeID;

		[ProtoMember(5)]
		public string VariableName;

		[ProtoMember(10)]
		public string OriginName;

		[ProtoMember(15)]
		public string OriginType;

		[NoSerialize]
		public static MyVariableIdentifier Default = new MyVariableIdentifier
		{
			NodeID = -1,
			VariableName = ""
		};

		public MyVariableIdentifier(int nodeId, string variableName)
		{
			NodeID = nodeId;
			VariableName = variableName;
			OriginName = string.Empty;
			OriginType = string.Empty;
		}

		public MyVariableIdentifier(int nodeId, string variableName, ParameterInfo parameter)
			: this(nodeId, variableName)
		{
			OriginName = parameter.Name;
			OriginType = parameter.ParameterType.Signature();
		}

		public MyVariableIdentifier(ParameterInfo parameter)
			: this(-1, string.Empty, parameter)
		{
		}

		public override bool Equals(object obj)
		{
			if (!(obj is MyVariableIdentifier))
			{
				return false;
			}
			MyVariableIdentifier myVariableIdentifier = (MyVariableIdentifier)obj;
			if (NodeID == myVariableIdentifier.NodeID)
			{
				return VariableName == myVariableIdentifier.VariableName;
			}
			return false;
		}
	}
}
