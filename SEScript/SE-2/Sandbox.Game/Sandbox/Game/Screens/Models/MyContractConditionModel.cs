using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using VRage.Game.ObjectBuilders.Components.Contracts;

namespace Sandbox.Game.Screens.Models
{
	public class MyContractConditionModel : BindableBase
	{
		private string m_name;

		private BitmapImage m_icon;

		private long m_id;

		private long m_contractId;

		private long m_stationEndId;

		private long m_blockEndId;

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				SetProperty(ref m_name, value, "Name");
			}
		}

		public BitmapImage Icon
		{
			get
			{
				return m_icon;
			}
			set
			{
				SetProperty(ref m_icon, value, "Icon");
			}
		}

		public long Id
		{
			get
			{
				return m_id;
			}
			set
			{
				SetProperty(ref m_id, value, "Id");
			}
		}

		public long ContractId
		{
			get
			{
				return m_contractId;
			}
			set
			{
				SetProperty(ref m_contractId, value, "ContractId");
			}
		}

		public long StationEndId
		{
			get
			{
				return m_stationEndId;
			}
			set
			{
				SetProperty(ref m_stationEndId, value, "StationEndId");
			}
		}

		public long BlockEndId
		{
			get
			{
				return m_blockEndId;
			}
			set
			{
				SetProperty(ref m_blockEndId, value, "BlockEndId");
			}
		}

		protected virtual string BuildName(long id)
		{
			return $"DefaultCondition {id}";
		}

		protected virtual BitmapImage BuildImage()
		{
			return new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Icons\\WeaponDrill.dds"
			};
		}

		public virtual void Init(MyObjectBuilder_ContractCondition ob)
		{
			Name = BuildName(ob.Id);
			Icon = BuildImage();
			Id = ob.Id;
			ContractId = ob.ContractId;
			StationEndId = ob.StationEndId;
			BlockEndId = ob.BlockEndId;
		}
	}
}
