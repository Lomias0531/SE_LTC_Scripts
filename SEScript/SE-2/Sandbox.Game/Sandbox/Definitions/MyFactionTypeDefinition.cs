using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_FactionTypeDefinition), null)]
	public class MyFactionTypeDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyFactionTypeDefinition_003C_003EActor : IActivator, IActivator<MyFactionTypeDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyFactionTypeDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFactionTypeDefinition CreateInstance()
			{
				return new MyFactionTypeDefinition();
			}

			MyFactionTypeDefinition IActivator<MyFactionTypeDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public SerializableDefinitionId[] OffersList
		{
			get;
			set;
		}

		public SerializableDefinitionId[] OrdersList
		{
			get;
			set;
		}

		public float OfferPriceUpMultiplierMax
		{
			get;
			set;
		}

		public float OfferPriceUpMultiplierMin
		{
			get;
			set;
		}

		public float OfferPriceDownMultiplierMax
		{
			get;
			set;
		}

		public float OfferPriceDownMultiplierMin
		{
			get;
			set;
		}

		public float OfferPriceUpDownPoint
		{
			get;
			set;
		}

		public float OfferPriceBellowMinimumMultiplier
		{
			get;
			set;
		}

		public float OfferPriceStartingMultiplier
		{
			get;
			set;
		}

		public byte OfferMaxUpdateCount
		{
			get;
			set;
		}

		public float OrderPriceStartingMultiplier
		{
			get;
			set;
		}

		public float OrderPriceUpMultiplierMax
		{
			get;
			set;
		}

		public float OrderPriceUpMultiplierMin
		{
			get;
			set;
		}

		public float OrderPriceDownMultiplierMax
		{
			get;
			set;
		}

		public float OrderPriceDownMultiplierMin
		{
			get;
			set;
		}

		public float OrderPriceOverMinimumMultiplier
		{
			get;
			set;
		}

		public float OrderPriceUpDownPoint
		{
			get;
			set;
		}

		public byte OrderMaxUpdateCount
		{
			get;
			set;
		}

		public bool CanSellOxygen
		{
			get;
			set;
		}

		public bool CanSellHydrogen
		{
			get;
			set;
		}

		public int MinimumOfferGasAmount
		{
			get;
			set;
		}

		public int MaximumOfferGasAmount
		{
			get;
			set;
		}

		public float BaseCostProductionSpeedMultiplier
		{
			get;
			set;
		}

		public int MinimumOxygenPrice
		{
			get;
			set;
		}

		public int MinimumHydrogenPrice
		{
			get;
			set;
		}

		public string[] GridsForSale
		{
			get;
			set;
		}

		public int MaxContractCount
		{
			get;
			set;
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_FactionTypeDefinition myObjectBuilder_FactionTypeDefinition = (MyObjectBuilder_FactionTypeDefinition)builder;
			if (myObjectBuilder_FactionTypeDefinition.OffersList != null)
			{
				OffersList = new SerializableDefinitionId[myObjectBuilder_FactionTypeDefinition.OffersList.Length];
				for (int i = 0; i < myObjectBuilder_FactionTypeDefinition.OffersList.Length; i++)
				{
					OffersList[i] = myObjectBuilder_FactionTypeDefinition.OffersList[i];
				}
			}
			if (myObjectBuilder_FactionTypeDefinition.OrdersList != null)
			{
				OrdersList = new SerializableDefinitionId[myObjectBuilder_FactionTypeDefinition.OrdersList.Length];
				for (int j = 0; j < myObjectBuilder_FactionTypeDefinition.OrdersList.Length; j++)
				{
					OrdersList[j] = myObjectBuilder_FactionTypeDefinition.OrdersList[j];
				}
			}
			if (myObjectBuilder_FactionTypeDefinition.GridsForSale != null)
			{
				GridsForSale = new string[myObjectBuilder_FactionTypeDefinition.GridsForSale.Length];
				for (int k = 0; k < myObjectBuilder_FactionTypeDefinition.GridsForSale.Length; k++)
				{
					GridsForSale[k] = myObjectBuilder_FactionTypeDefinition.GridsForSale[k];
				}
			}
			MaxContractCount = myObjectBuilder_FactionTypeDefinition.MaxContractCount;
			OfferPriceUpMultiplierMax = myObjectBuilder_FactionTypeDefinition.OfferPriceUpMultiplierMax;
			OfferPriceUpMultiplierMin = myObjectBuilder_FactionTypeDefinition.OfferPriceUpMultiplierMin;
			OfferPriceDownMultiplierMax = myObjectBuilder_FactionTypeDefinition.OfferPriceDownMultiplierMax;
			OfferPriceDownMultiplierMin = myObjectBuilder_FactionTypeDefinition.OfferPriceDownMultiplierMin;
			OfferPriceUpDownPoint = myObjectBuilder_FactionTypeDefinition.OfferPriceUpDownPoint;
			OfferPriceBellowMinimumMultiplier = myObjectBuilder_FactionTypeDefinition.OfferPriceBellowMinimumMultiplier;
			OfferPriceStartingMultiplier = myObjectBuilder_FactionTypeDefinition.OfferPriceStartingMultiplier;
			OfferMaxUpdateCount = myObjectBuilder_FactionTypeDefinition.OfferMaxUpdateCount;
			OrderPriceStartingMultiplier = myObjectBuilder_FactionTypeDefinition.OrderPriceStartingMultiplier;
			OrderPriceUpMultiplierMax = myObjectBuilder_FactionTypeDefinition.OrderPriceUpMultiplierMax;
			OrderPriceUpMultiplierMin = myObjectBuilder_FactionTypeDefinition.OrderPriceUpMultiplierMin;
			OrderPriceDownMultiplierMax = myObjectBuilder_FactionTypeDefinition.OrderPriceDownMultiplierMax;
			OrderPriceDownMultiplierMin = myObjectBuilder_FactionTypeDefinition.OrderPriceDownMultiplierMin;
			OrderPriceOverMinimumMultiplier = myObjectBuilder_FactionTypeDefinition.OrderPriceOverMinimumMultiplier;
			OrderPriceUpDownPoint = myObjectBuilder_FactionTypeDefinition.OrderPriceUpDownPoint;
			OrderMaxUpdateCount = myObjectBuilder_FactionTypeDefinition.OrderMaxUpdateCount;
			CanSellOxygen = myObjectBuilder_FactionTypeDefinition.CanSellOxygen;
			MinimumOxygenPrice = myObjectBuilder_FactionTypeDefinition.MinimumOxygenPrice;
			CanSellHydrogen = myObjectBuilder_FactionTypeDefinition.CanSellHydrogen;
			MinimumHydrogenPrice = myObjectBuilder_FactionTypeDefinition.MinimumHydrogenPrice;
			MinimumOfferGasAmount = myObjectBuilder_FactionTypeDefinition.MinimumOfferGasAmount;
			MaximumOfferGasAmount = myObjectBuilder_FactionTypeDefinition.MaximumOfferGasAmount;
			BaseCostProductionSpeedMultiplier = myObjectBuilder_FactionTypeDefinition.BaseCostProductionSpeedMultiplier;
		}
	}
}
