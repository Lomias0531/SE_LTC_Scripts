using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Library;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_PhysicalItemDefinition), null)]
	public class MyPhysicalItemDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyPhysicalItemDefinition_003C_003EActor : IActivator, IActivator<MyPhysicalItemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPhysicalItemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPhysicalItemDefinition CreateInstance()
			{
				return new MyPhysicalItemDefinition();
			}

			MyPhysicalItemDefinition IActivator<MyPhysicalItemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Vector3 Size;

		public float Mass;

		public string Model;

		public string[] Models;

		public MyStringId? IconSymbol;

		public float Volume;

		public float ModelVolume;

		public MyStringHash PhysicalMaterial;

		public MyStringHash VoxelMaterial;

		public bool CanSpawnFromScreen;

		public bool RotateOnSpawnX;

		public bool RotateOnSpawnY;

		public bool RotateOnSpawnZ;

		public int Health;

		public MyDefinitionId? DestroyedPieceId;

		public int DestroyedPieces;

		public StringBuilder ExtraInventoryTooltipLine;

		public MyFixedPoint MaxStackAmount;

		public int MinimalPricePerUnit;

		public int MinimumOfferAmount;

		public int MaximumOfferAmount;

		public int MinimumOrderAmount;

		public int MaximumOrderAmount;

		public int MinimumAcquisitionAmount;

		public int MaximumAcquisitionAmount;

		public bool CanPlayerOrder;

		public bool HasIntegralAmounts
		{
			get
			{
				if (Id.TypeId != typeof(MyObjectBuilder_Ingot))
				{
					return Id.TypeId != typeof(MyObjectBuilder_Ore);
				}
				return false;
			}
		}

		public bool IsOre => Id.TypeId == typeof(MyObjectBuilder_Ore);

		public bool IsIngot => Id.TypeId == typeof(MyObjectBuilder_Ingot);

		public bool HasModelVariants
		{
			get
			{
				if (Models != null)
				{
					return Models.Length != 0;
				}
				return false;
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_PhysicalItemDefinition myObjectBuilder_PhysicalItemDefinition = builder as MyObjectBuilder_PhysicalItemDefinition;
			Size = myObjectBuilder_PhysicalItemDefinition.Size;
			Mass = myObjectBuilder_PhysicalItemDefinition.Mass;
			Model = myObjectBuilder_PhysicalItemDefinition.Model;
			Models = myObjectBuilder_PhysicalItemDefinition.Models;
			Volume = (myObjectBuilder_PhysicalItemDefinition.Volume.HasValue ? (myObjectBuilder_PhysicalItemDefinition.Volume.Value / 1000f) : myObjectBuilder_PhysicalItemDefinition.Size.Volume);
			ModelVolume = (myObjectBuilder_PhysicalItemDefinition.ModelVolume.HasValue ? (myObjectBuilder_PhysicalItemDefinition.ModelVolume.Value / 1000f) : Volume);
			if (string.IsNullOrEmpty(myObjectBuilder_PhysicalItemDefinition.IconSymbol))
			{
				IconSymbol = null;
			}
			else
			{
				IconSymbol = MyStringId.GetOrCompute(myObjectBuilder_PhysicalItemDefinition.IconSymbol);
			}
			PhysicalMaterial = MyStringHash.GetOrCompute(myObjectBuilder_PhysicalItemDefinition.PhysicalMaterial);
			VoxelMaterial = MyStringHash.GetOrCompute(myObjectBuilder_PhysicalItemDefinition.VoxelMaterial);
			CanSpawnFromScreen = myObjectBuilder_PhysicalItemDefinition.CanSpawnFromScreen;
			RotateOnSpawnX = myObjectBuilder_PhysicalItemDefinition.RotateOnSpawnX;
			RotateOnSpawnY = myObjectBuilder_PhysicalItemDefinition.RotateOnSpawnY;
			RotateOnSpawnZ = myObjectBuilder_PhysicalItemDefinition.RotateOnSpawnZ;
			Health = myObjectBuilder_PhysicalItemDefinition.Health;
			if (myObjectBuilder_PhysicalItemDefinition.DestroyedPieceId.HasValue)
			{
				DestroyedPieceId = myObjectBuilder_PhysicalItemDefinition.DestroyedPieceId.Value;
			}
			DestroyedPieces = myObjectBuilder_PhysicalItemDefinition.DestroyedPieces;
			if (myObjectBuilder_PhysicalItemDefinition.ExtraInventoryTooltipLine != null)
			{
				ExtraInventoryTooltipLine = new StringBuilder().Append(MyEnvironment.NewLine).Append(myObjectBuilder_PhysicalItemDefinition.ExtraInventoryTooltipLine);
			}
			else
			{
				ExtraInventoryTooltipLine = new StringBuilder();
			}
			MaxStackAmount = myObjectBuilder_PhysicalItemDefinition.MaxStackAmount;
			MinimalPricePerUnit = myObjectBuilder_PhysicalItemDefinition.MinimalPricePerUnit;
			MinimumOfferAmount = myObjectBuilder_PhysicalItemDefinition.MinimumOfferAmount;
			MaximumOfferAmount = myObjectBuilder_PhysicalItemDefinition.MaximumOfferAmount;
			MinimumOrderAmount = myObjectBuilder_PhysicalItemDefinition.MinimumOrderAmount;
			MaximumOrderAmount = myObjectBuilder_PhysicalItemDefinition.MaximumOrderAmount;
			MinimumAcquisitionAmount = myObjectBuilder_PhysicalItemDefinition.MinimumAcquisitionAmount;
			MaximumAcquisitionAmount = myObjectBuilder_PhysicalItemDefinition.MaximumAcquisitionAmount;
			CanPlayerOrder = myObjectBuilder_PhysicalItemDefinition.CanPlayerOrder;
		}

		internal virtual string GetTooltipDisplayName(MyObjectBuilder_PhysicalObject content)
		{
			return DisplayNameText;
		}
	}
}
