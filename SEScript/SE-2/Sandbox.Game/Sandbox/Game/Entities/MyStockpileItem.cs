using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.Entities
{
	[ProtoContract]
	public struct MyStockpileItem
	{
		protected class Sandbox_Game_Entities_MyStockpileItem_003C_003EAmount_003C_003EAccessor : IMemberAccessor<MyStockpileItem, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStockpileItem owner, in int value)
			{
				owner.Amount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStockpileItem owner, out int value)
			{
				value = owner.Amount;
			}
		}

		protected class Sandbox_Game_Entities_MyStockpileItem_003C_003EContent_003C_003EAccessor : IMemberAccessor<MyStockpileItem, MyObjectBuilder_PhysicalObject>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStockpileItem owner, in MyObjectBuilder_PhysicalObject value)
			{
				owner.Content = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStockpileItem owner, out MyObjectBuilder_PhysicalObject value)
			{
				value = owner.Content;
			}
		}

		private class Sandbox_Game_Entities_MyStockpileItem_003C_003EActor : IActivator, IActivator<MyStockpileItem>
		{
			private sealed override object CreateInstance()
			{
				return default(MyStockpileItem);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyStockpileItem CreateInstance()
			{
				return (MyStockpileItem)(object)default(MyStockpileItem);
			}

			MyStockpileItem IActivator<MyStockpileItem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public int Amount;

		[ProtoMember(4)]
		[Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof(MyObjectBuilderDynamicSerializer))]
		public MyObjectBuilder_PhysicalObject Content;

		public override string ToString()
		{
			return $"{Amount}x {Content.SubtypeName}";
		}
	}
}
