using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_InventoryDefinition
	{
		protected class VRage_Game_MyObjectBuilder_InventoryDefinition_003C_003EInventoryVolume_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_InventoryDefinition owner, in float value)
			{
				owner.InventoryVolume = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_InventoryDefinition owner, out float value)
			{
				value = owner.InventoryVolume;
			}
		}

		protected class VRage_Game_MyObjectBuilder_InventoryDefinition_003C_003EInventoryMass_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_InventoryDefinition owner, in float value)
			{
				owner.InventoryMass = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_InventoryDefinition owner, out float value)
			{
				value = owner.InventoryMass;
			}
		}

		protected class VRage_Game_MyObjectBuilder_InventoryDefinition_003C_003EInventorySizeX_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_InventoryDefinition owner, in float value)
			{
				owner.InventorySizeX = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_InventoryDefinition owner, out float value)
			{
				value = owner.InventorySizeX;
			}
		}

		protected class VRage_Game_MyObjectBuilder_InventoryDefinition_003C_003EInventorySizeY_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_InventoryDefinition owner, in float value)
			{
				owner.InventorySizeY = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_InventoryDefinition owner, out float value)
			{
				value = owner.InventorySizeY;
			}
		}

		protected class VRage_Game_MyObjectBuilder_InventoryDefinition_003C_003EInventorySizeZ_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_InventoryDefinition owner, in float value)
			{
				owner.InventorySizeZ = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_InventoryDefinition owner, out float value)
			{
				value = owner.InventorySizeZ;
			}
		}

		protected class VRage_Game_MyObjectBuilder_InventoryDefinition_003C_003EMaxItemCount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_InventoryDefinition owner, in int value)
			{
				owner.MaxItemCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_InventoryDefinition owner, out int value)
			{
				value = owner.MaxItemCount;
			}
		}

		private class VRage_Game_MyObjectBuilder_InventoryDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_InventoryDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_InventoryDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_InventoryDefinition CreateInstance()
			{
				return new MyObjectBuilder_InventoryDefinition();
			}

			MyObjectBuilder_InventoryDefinition IActivator<MyObjectBuilder_InventoryDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public float InventoryVolume = float.MaxValue;

		[ProtoMember(4)]
		public float InventoryMass = float.MaxValue;

		[ProtoMember(7)]
		public float InventorySizeX = 1.2f;

		[ProtoMember(10)]
		public float InventorySizeY = 0.7f;

		[ProtoMember(13)]
		public float InventorySizeZ = 0.4f;

		[ProtoMember(16)]
		public int MaxItemCount = int.MaxValue;
	}
}
