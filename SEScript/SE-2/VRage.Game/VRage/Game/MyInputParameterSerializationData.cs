using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyInputParameterSerializationData
	{
		protected class VRage_Game_MyInputParameterSerializationData_003C_003EType_003C_003EAccessor : IMemberAccessor<MyInputParameterSerializationData, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputParameterSerializationData owner, in string value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputParameterSerializationData owner, out string value)
			{
				value = owner.Type;
			}
		}

		protected class VRage_Game_MyInputParameterSerializationData_003C_003EName_003C_003EAccessor : IMemberAccessor<MyInputParameterSerializationData, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputParameterSerializationData owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputParameterSerializationData owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyInputParameterSerializationData_003C_003EInput_003C_003EAccessor : IMemberAccessor<MyInputParameterSerializationData, MyVariableIdentifier>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputParameterSerializationData owner, in MyVariableIdentifier value)
			{
				owner.Input = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputParameterSerializationData owner, out MyVariableIdentifier value)
			{
				value = owner.Input;
			}
		}

		private class VRage_Game_MyInputParameterSerializationData_003C_003EActor : IActivator, IActivator<MyInputParameterSerializationData>
		{
			private sealed override object CreateInstance()
			{
				return new MyInputParameterSerializationData();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyInputParameterSerializationData CreateInstance()
			{
				return new MyInputParameterSerializationData();
			}

			MyInputParameterSerializationData IActivator<MyInputParameterSerializationData>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string Type;

		[ProtoMember(5)]
		public string Name;

		[ProtoMember(10)]
		public MyVariableIdentifier Input;

		public MyInputParameterSerializationData()
		{
			Input = MyVariableIdentifier.Default;
		}
	}
}
