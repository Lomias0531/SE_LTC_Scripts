using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyParameterValue
	{
		protected class VRage_Game_MyParameterValue_003C_003EParameterName_003C_003EAccessor : IMemberAccessor<MyParameterValue, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyParameterValue owner, in string value)
			{
				owner.ParameterName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyParameterValue owner, out string value)
			{
				value = owner.ParameterName;
			}
		}

		protected class VRage_Game_MyParameterValue_003C_003EValue_003C_003EAccessor : IMemberAccessor<MyParameterValue, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyParameterValue owner, in string value)
			{
				owner.Value = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyParameterValue owner, out string value)
			{
				value = owner.Value;
			}
		}

		private class VRage_Game_MyParameterValue_003C_003EActor : IActivator, IActivator<MyParameterValue>
		{
			private sealed override object CreateInstance()
			{
				return new MyParameterValue();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyParameterValue CreateInstance()
			{
				return new MyParameterValue();
			}

			MyParameterValue IActivator<MyParameterValue>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string ParameterName;

		[ProtoMember(5)]
		public string Value;

		public MyParameterValue()
		{
			ParameterName = string.Empty;
			Value = string.Empty;
		}

		public MyParameterValue(string paramName)
		{
			ParameterName = paramName;
		}
	}
}
