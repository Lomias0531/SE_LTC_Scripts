using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.ObjectBuilders
{
	[ProtoContract]
	public struct MyRuntimeObjectBuilderId
	{
		protected class VRage_ObjectBuilders_MyRuntimeObjectBuilderId_003C_003EValue_003C_003EAccessor : IMemberAccessor<MyRuntimeObjectBuilderId, ushort>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyRuntimeObjectBuilderId owner, in ushort value)
			{
				owner.Value = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyRuntimeObjectBuilderId owner, out ushort value)
			{
				value = owner.Value;
			}
		}

		private class VRage_ObjectBuilders_MyRuntimeObjectBuilderId_003C_003EActor : IActivator, IActivator<MyRuntimeObjectBuilderId>
		{
			private sealed override object CreateInstance()
			{
				return default(MyRuntimeObjectBuilderId);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRuntimeObjectBuilderId CreateInstance()
			{
				return (MyRuntimeObjectBuilderId)(object)default(MyRuntimeObjectBuilderId);
			}

			MyRuntimeObjectBuilderId IActivator<MyRuntimeObjectBuilderId>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public static readonly MyRuntimeObjectBuilderIdComparer Comparer = new MyRuntimeObjectBuilderIdComparer();

		[ProtoMember(1)]
		public readonly ushort Value;

		public bool IsValid => Value != 0;

		public MyRuntimeObjectBuilderId(ushort value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return $"{Value}: {(MyObjectBuilderType)this}";
		}
	}
}
