using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components.Trading
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_SubmitOffer : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer_003C_003EInventoryItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SubmitOffer, List<MyObjectBuilder_InventoryItem>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SubmitOffer owner, in List<MyObjectBuilder_InventoryItem> value)
			{
				owner.InventoryItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SubmitOffer owner, out List<MyObjectBuilder_InventoryItem> value)
			{
				value = owner.InventoryItems;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer_003C_003ECurrencyAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SubmitOffer, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SubmitOffer owner, in long value)
			{
				owner.CurrencyAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SubmitOffer owner, out long value)
			{
				value = owner.CurrencyAmount;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer_003C_003EPCUAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SubmitOffer, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SubmitOffer owner, in int value)
			{
				owner.PCUAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SubmitOffer owner, out int value)
			{
				value = owner.PCUAmount;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SubmitOffer, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SubmitOffer owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SubmitOffer owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SubmitOffer, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SubmitOffer owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SubmitOffer owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SubmitOffer, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SubmitOffer owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SubmitOffer owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SubmitOffer, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SubmitOffer owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SubmitOffer owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer_003C_003EActor : IActivator, IActivator<MyObjectBuilder_SubmitOffer>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_SubmitOffer();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_SubmitOffer CreateInstance()
			{
				return new MyObjectBuilder_SubmitOffer();
			}

			MyObjectBuilder_SubmitOffer IActivator<MyObjectBuilder_SubmitOffer>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<MyObjectBuilder_InventoryItem> InventoryItems;

		public long CurrencyAmount;

		public int PCUAmount;
	}
}
