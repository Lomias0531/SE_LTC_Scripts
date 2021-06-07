using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game;
using Sandbox.Game.Localization;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ReactorDefinition), null)]
	public class MyReactorDefinition : MyFueledPowerProducerDefinition
	{
		public struct FuelInfo
		{
			public readonly float Ratio;

			public readonly MyDefinitionId FuelId;

			public readonly float ConsumptionPerSecond_Items;

			public readonly MyPhysicalItemDefinition FuelDefinition;

			public readonly MyObjectBuilder_PhysicalObject FuelItem;

			public FuelInfo(MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo fuelInfo, MyReactorDefinition blockDefinition)
			{
				FuelId = fuelInfo.Id;
				Ratio = fuelInfo.Ratio;
				FuelDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(fuelInfo.Id);
				FuelItem = (MyObjectBuilderSerializer.CreateNewObject(fuelInfo.Id) as MyObjectBuilder_PhysicalObject);
				float num = blockDefinition.MaxPowerOutput / blockDefinition.FuelProductionToCapacityMultiplier * Ratio;
				ConsumptionPerSecond_Items = num / FuelDefinition.Mass;
			}
		}

		private class Sandbox_Definitions_MyReactorDefinition_003C_003EActor : IActivator, IActivator<MyReactorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyReactorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyReactorDefinition CreateInstance()
			{
				return new MyReactorDefinition();
			}

			MyReactorDefinition IActivator<MyReactorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Vector3 InventorySize;

		public float InventoryMaxVolume;

		public MyInventoryConstraint InventoryConstraint;

		public FuelInfo[] FuelInfos;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ReactorDefinition myObjectBuilder_ReactorDefinition = builder as MyObjectBuilder_ReactorDefinition;
			InventorySize = myObjectBuilder_ReactorDefinition.InventorySize;
			InventoryMaxVolume = InventorySize.X * InventorySize.Y * InventorySize.Z;
			List<MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo> list = myObjectBuilder_ReactorDefinition.FuelInfos;
			if (myObjectBuilder_ReactorDefinition.FuelId.HasValue)
			{
				list = new List<MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo>(list)
				{
					new MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo
					{
						Ratio = 1f,
						Id = myObjectBuilder_ReactorDefinition.FuelId.Value
					}
				};
			}
			FuelInfos = new FuelInfo[list.Count];
			InventoryConstraint = new MyInventoryConstraint(string.Format(MyTexts.GetString(MySpaceTexts.ToolTipItemFilter_GenericProductionBlockInput), DisplayNameText));
			for (int i = 0; i < list.Count; i++)
			{
				MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo fuelInfo = list[i];
				InventoryConstraint.Add(fuelInfo.Id);
				FuelInfos[i] = new FuelInfo(fuelInfo, this);
			}
		}
	}
}
