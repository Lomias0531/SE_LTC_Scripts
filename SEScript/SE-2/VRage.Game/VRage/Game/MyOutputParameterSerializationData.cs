using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyOutputParameterSerializationData
	{
		protected class VRage_Game_MyOutputParameterSerializationData_003C_003EType_003C_003EAccessor : IMemberAccessor<MyOutputParameterSerializationData, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyOutputParameterSerializationData owner, in string value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyOutputParameterSerializationData owner, out string value)
			{
				value = owner.Type;
			}
		}

		protected class VRage_Game_MyOutputParameterSerializationData_003C_003EName_003C_003EAccessor : IMemberAccessor<MyOutputParameterSerializationData, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyOutputParameterSerializationData owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyOutputParameterSerializationData owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyOutputParameterSerializationData_003C_003EOutputs_003C_003EAccessor : IMemberAccessor<MyOutputParameterSerializationData, IdentifierList>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyOutputParameterSerializationData owner, in IdentifierList value)
			{
				owner.Outputs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyOutputParameterSerializationData owner, out IdentifierList value)
			{
				value = owner.Outputs;
			}
		}

		private class VRage_Game_MyOutputParameterSerializationData_003C_003EActor : IActivator, IActivator<MyOutputParameterSerializationData>
		{
			private sealed override object CreateInstance()
			{
				return new MyOutputParameterSerializationData();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyOutputParameterSerializationData CreateInstance()
			{
				return new MyOutputParameterSerializationData();
			}

			MyOutputParameterSerializationData IActivator<MyOutputParameterSerializationData>.CreateInstance()
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
		public IdentifierList Outputs;

		public MyOutputParameterSerializationData()
		{
			Outputs.Ids = new List<MyVariableIdentifier>();
		}
	}
}
