using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System.Collections.ObjectModel;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.Models
{
	public abstract class MyContractModel : BindableBase
	{
		private string m_nameWithId;

		private string m_name;

		private BitmapImage m_icon;

		private BitmapImage m_header;

		private bool m_isFactionIconPrepared;

		private bool m_shouldFactionIconBeVisible;

		private BitmapImage m_factionIcon;

		private ColorW m_factionIconBackgroundColor;

		private ColorW m_factionIconColor;

		private string m_factionIconTooltip;

		private long m_id;

		private long m_rewardMoney;

		private int m_rewardReputation;

		private long m_startingDeposit;

		private int m_failReputationPrice;

		private double m_timeRemaining;

		private ObservableCollection<MyContractConditionModel> m_conditions;

		private BitmapImage m_currencyIcon;

		private MyContractStateEnum m_state;

		protected MyContractTypeDefinition ContractTypeDefinition;

		public MyDefinitionId? DefinitionId
		{
			get
			{
				if (ContractTypeDefinition == null)
				{
					return null;
				}
				return ContractTypeDefinition.Id;
			}
		}

		public string NameWithId
		{
			get
			{
				return m_nameWithId;
			}
			set
			{
				SetProperty(ref m_nameWithId, value, "NameWithId");
			}
		}

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

		public bool IsFactionIconPrepared
		{
			get
			{
				return m_isFactionIconPrepared;
			}
			set
			{
				SetProperty(ref m_isFactionIconPrepared, value, "IsFactionIconPrepared");
				RaisePropertyChanged("IsFactionIconVisible");
			}
		}

		public bool ShouldFactionIconBeVisible
		{
			get
			{
				return m_shouldFactionIconBeVisible;
			}
			set
			{
				SetProperty(ref m_shouldFactionIconBeVisible, value, "ShouldFactionIconBeVisible");
				RaisePropertyChanged("IsFactionIconVisible");
			}
		}

		public bool IsFactionIconVisible
		{
			get
			{
				if (IsFactionIconPrepared)
				{
					return ShouldFactionIconBeVisible;
				}
				return false;
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

		public BitmapImage FactionIcon
		{
			get
			{
				return m_factionIcon;
			}
			set
			{
				SetProperty(ref m_factionIcon, value, "FactionIcon");
			}
		}

		public ColorW FactionIconBackgroundColor
		{
			get
			{
				return m_factionIconBackgroundColor;
			}
			set
			{
				SetProperty(ref m_factionIconBackgroundColor, value, "FactionIconBackgroundColor");
			}
		}

		public ColorW FactionIconColor
		{
			get
			{
				return m_factionIconColor;
			}
			set
			{
				SetProperty(ref m_factionIconColor, value, "FactionIconColor");
			}
		}

		public string FactionIconTooltip
		{
			get
			{
				return m_factionIconTooltip;
			}
			set
			{
				SetProperty(ref m_factionIconTooltip, value, "FactionIconTooltip");
			}
		}

		public BitmapImage Header
		{
			get
			{
				return m_header;
			}
			set
			{
				SetProperty(ref m_header, value, "Header");
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

		public long RewardMoney
		{
			get
			{
				return m_rewardMoney;
			}
			set
			{
				SetProperty(ref m_rewardMoney, value, "RewardMoney");
			}
		}

		public string RewardMoney_Formatted => MyBankingSystem.GetFormatedValue(RewardMoney);

		public int RewardReputation
		{
			get
			{
				return m_rewardReputation;
			}
			set
			{
				SetProperty(ref m_rewardReputation, value, "RewardReputation");
			}
		}

		public string RewardReputation_Formatted => RewardReputation.ToString();

		public long StartingDeposit
		{
			get
			{
				return m_startingDeposit;
			}
			set
			{
				SetProperty(ref m_startingDeposit, value, "StartingDeposit");
			}
		}

		public int FailReputationPrice
		{
			get
			{
				return m_failReputationPrice;
			}
			set
			{
				SetProperty(ref m_failReputationPrice, value, "FailReputationPrice");
			}
		}

		public double RemainingTime
		{
			get
			{
				return m_timeRemaining;
			}
			set
			{
				SetProperty(ref m_timeRemaining, value, "RemainingTime");
				RaisePropertyChanged("TimeLimit_Formated");
			}
		}

		public bool CanBeFinishedInTerminal => Conditions.Count > 0;

		public ObservableCollection<MyContractConditionModel> Conditions
		{
			get
			{
				return m_conditions;
			}
			set
			{
				SetProperty(ref m_conditions, value, "Conditions");
				RaisePropertyChanged("CanBeFinishedInTerminal");
			}
		}

		public string TimeLimit_Formated
		{
			get
			{
				if (RemainingTime <= 0.0)
				{
					return MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_No);
				}
				MyTimeSpan myTimeSpan = MyTimeSpan.FromSeconds(RemainingTime);
				int num = (int)myTimeSpan.Minutes;
				int num2 = num / 60;
				int num3 = num2 / 24;
				num -= num2 * 60;
				num2 -= num3 * 24;
				if (num3 > 0)
				{
					return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_Days), num3, num2, num);
				}
				if (num2 > 0)
				{
					return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_Hours), num2, num);
				}
				if (num > 0)
				{
					return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_Mins), num);
				}
				return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_Secs), myTimeSpan.Seconds);
			}
		}

		public string TimeRemaining_Formated
		{
			get
			{
				if (m_state != MyContractStateEnum.Active)
				{
					return string.Empty;
				}
				if (RemainingTime <= 0.0)
				{
					return MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_No);
				}
				MyTimeSpan myTimeSpan = MyTimeSpan.FromSeconds(RemainingTime);
				int num = (int)myTimeSpan.Minutes;
				int num2 = num / 60;
				int num3 = num2 / 24;
				num -= num2 * 60;
				num2 -= num3 * 24;
				if (num3 > 0)
				{
					return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_Days), num3, num2, num);
				}
				if (num2 > 0)
				{
					return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_Hours), num2, num);
				}
				if (num > 0)
				{
					return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_Mins), num);
				}
				return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_Secs), myTimeSpan.Seconds);
			}
		}

		public string TimeLeft
		{
			get
			{
				if (m_state == MyContractStateEnum.Active)
				{
					return TimeRemaining_Formated;
				}
				return TimeLimit_Formated;
			}
		}

		public string InitialDeposit_Formated
		{
			get
			{
				if (StartingDeposit <= 0)
				{
					return MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_Deposit_None);
				}
				return MyBankingSystem.GetFormatedValue(StartingDeposit);
			}
		}

		public string FailReputationPenalty_Formated
		{
			get
			{
				if (FailReputationPrice <= 0)
				{
					return MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_RepPenalty_None);
				}
				return FailReputationPrice.ToString();
			}
		}

		public abstract string Description
		{
			get;
		}

		public BitmapImage CurrencyIcon
		{
			get
			{
				return m_currencyIcon;
			}
			set
			{
				SetProperty(ref m_currencyIcon, value, "CurrencyIcon");
			}
		}

		public MyContractModel()
		{
		}

		protected virtual string BuildNameWithId(long id)
		{
			return $"DefaultContract {id}";
		}

		protected virtual string BuildName()
		{
			return $"DefaultContract";
		}

		protected virtual BitmapImage CreateIcon()
		{
			BitmapImage bitmapImage = new BitmapImage();
			MyContractTypeDefinition contractTypeDefinition = ContractTypeDefinition;
			if (contractTypeDefinition != null && contractTypeDefinition.Icons?.Length > 0)
			{
				bitmapImage.TextureAsset = ContractTypeDefinition.Icons[0];
			}
			return bitmapImage;
		}

		protected virtual BitmapImage CreateHeader()
		{
			BitmapImage bitmapImage = new BitmapImage();
			MyContractTypeDefinition contractTypeDefinition = ContractTypeDefinition;
			if (contractTypeDefinition != null && contractTypeDefinition.Icons?.Length > 1)
			{
				bitmapImage.TextureAsset = ContractTypeDefinition.Icons[1];
			}
			return bitmapImage;
		}

		public static ColorW ConvertVector3ToColorW(Vector3 color)
		{
			return new ColorW(color.X, color.Y, color.Z);
		}

		protected virtual void PrepareFactionIcon(long factionId, bool showFactionIcons)
		{
			ShouldFactionIconBeVisible = showFactionIcons;
			if (!ShouldFactionIconBeVisible)
			{
				return;
			}
			if (factionId == 0L)
			{
				IsFactionIconPrepared = false;
				return;
			}
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(factionId);
			if (myFaction == null)
			{
				IsFactionIconPrepared = false;
				return;
			}
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.TextureAsset = (myFaction.FactionIcon.HasValue ? myFaction.FactionIcon.Value.ToString() : "");
			FactionIcon = bitmapImage;
			FactionIconColor = ConvertVector3ToColorW(MyColorPickerConstants.HSVOffsetToHSV(myFaction.IconColor).HsvToRgb());
			FactionIconBackgroundColor = ConvertVector3ToColorW(MyColorPickerConstants.HSVOffsetToHSV(myFaction.CustomColor).HsvToRgb());
			FactionIconTooltip = $"[{myFaction.Tag}] {myFaction.Name}";
			IsFactionIconPrepared = true;
		}

		public virtual void Init(MyObjectBuilder_Contract ob, bool showFactionIcons = true)
		{
			BitmapImage bitmapImage = new BitmapImage();
			string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
			bitmapImage.TextureAsset = ((icons != null && icons.Length != 0) ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty);
			CurrencyIcon = bitmapImage;
			NameWithId = BuildNameWithId(ob.Id);
			Name = BuildName();
			Icon = CreateIcon();
			Header = CreateHeader();
			Id = ob.Id;
			RewardMoney = ob.RewardMoney;
			RewardReputation = ob.RewardReputation;
			StartingDeposit = ob.StartingDeposit;
			FailReputationPrice = ob.FailReputationPrice;
			RemainingTime = (ob.RemainingTimeInS.HasValue ? ob.RemainingTimeInS.Value : 0.0);
			m_state = ob.State;
			PrepareFactionIcon(ob.StartFaction, showFactionIcons);
			ObservableCollection<MyContractConditionModel> observableCollection = new ObservableCollection<MyContractConditionModel>();
			if (ob.ContractCondition != null)
			{
				MyContractConditionModel item = MyContractConditionModelFactory.CreateInstance(ob.ContractCondition);
				observableCollection.Add(item);
			}
			Conditions = observableCollection;
		}
	}
}
