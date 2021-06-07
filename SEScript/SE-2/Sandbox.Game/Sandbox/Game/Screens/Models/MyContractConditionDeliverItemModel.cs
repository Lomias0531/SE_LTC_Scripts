using EmptyKeys.UserInterface.Media.Imaging;
using Sandbox.Definitions;
using Sandbox.Game.Localization;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Screens.Models
{
	[MyContractConditionModelDescriptor(typeof(MyObjectBuilder_ContractConditionDeliverItems))]
	public class MyContractConditionDeliverItemModel : MyContractConditionModel
	{
		public MyDefinitionId m_itemType;

		public int m_itemAmount;

		public float m_itemVolume;

		public MyDefinitionId ItemType
		{
			get
			{
				return m_itemType;
			}
			set
			{
				SetProperty(ref m_itemType, value, "ItemType");
				RaisePropertyChanged("ItemType_Formated");
			}
		}

		public int ItemAmount
		{
			get
			{
				return m_itemAmount;
			}
			set
			{
				SetProperty(ref m_itemAmount, value, "ItemAmount");
			}
		}

		public float ItemVolume
		{
			get
			{
				return m_itemVolume;
			}
			set
			{
				SetProperty(ref m_itemVolume, value, "ItemVolume");
				RaisePropertyChanged("ItemVolume_Formated");
			}
		}

		public string ItemType_Formated
		{
			get
			{
				MyObjectBuilderType typeId = ItemType.TypeId;
				return typeId.ToString() + "/" + ItemType.SubtypeName;
			}
		}

		public string ItemVolume_Formated => string.Format(MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_VolumeValue), MyValueFormatter.GetFormatedFloat(1000f * ItemVolume, 2, ","));

		public override void Init(MyObjectBuilder_ContractCondition ob)
		{
			MyObjectBuilder_ContractConditionDeliverItems myObjectBuilder_ContractConditionDeliverItems = ob as MyObjectBuilder_ContractConditionDeliverItems;
			if (myObjectBuilder_ContractConditionDeliverItems != null)
			{
				ItemType = myObjectBuilder_ContractConditionDeliverItems.ItemType;
				ItemAmount = myObjectBuilder_ContractConditionDeliverItems.ItemAmount;
				ItemVolume = myObjectBuilder_ContractConditionDeliverItems.ItemVolume;
			}
			base.Init(ob);
		}

		protected override string BuildName(long id)
		{
			MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(ItemType);
			if (physicalItemDefinition == null)
			{
				return string.Empty;
			}
			return physicalItemDefinition.DisplayNameText;
		}

		protected override BitmapImage BuildImage()
		{
			MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(ItemType);
			if (physicalItemDefinition == null)
			{
				return null;
			}
			return new BitmapImage
			{
				TextureAsset = physicalItemDefinition.Icons[0]
			};
		}
	}
}
