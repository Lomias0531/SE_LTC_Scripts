using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRage;
using VRage.Game.Factions.Definitions;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyEditFactionIconViewModel : MyViewModelBase, IMyEditFactionIconViewModel
	{
		private ColorW m_factionColor;

		private float m_hue;

		private float m_saturation;

		private float m_value;

		private ColorW m_iconColor;

		private float m_hueIcon;

		private float m_saturationIcon;

		private float m_valueIcon;

		private ICommand m_okCommand;

		private ICommand m_cancelCommand;

		private ICommand m_selectIconCommand;

		private BitmapImage m_factionIconBitmap;

		private string m_iconTexturePath;

		private Vector3 m_runtimeBgColorHsv;

		private Vector3 m_runtimeIconColorHsv;

		private ObservableCollection<MyFactionIconModel> m_factionIcons = new ObservableCollection<MyFactionIconModel>();

		private MyFactionIconModel m_selectedIcon;

		public ColorW FactionColor
		{
			get
			{
				return m_factionColor;
			}
			set
			{
				SetProperty(ref m_factionColor, value, "FactionColor");
			}
		}

		public ColorW IconColorInternal
		{
			get
			{
				return m_iconColor;
			}
			set
			{
				SetProperty(ref m_iconColor, value, "IconColorInternal");
			}
		}

		public float Hue
		{
			get
			{
				return m_hue;
			}
			set
			{
				SetProperty(ref m_hue, value, "Hue");
				OnHueChanged(value);
			}
		}

		public float Saturation
		{
			get
			{
				return m_saturation;
			}
			set
			{
				SetProperty(ref m_saturation, value, "Saturation");
				OnSaturationChanged(value);
			}
		}

		public float ColorValue
		{
			get
			{
				return m_value;
			}
			set
			{
				SetProperty(ref m_value, value, "ColorValue");
				OnValueChanged(value);
			}
		}

		public float HueIcon
		{
			get
			{
				return m_hueIcon;
			}
			set
			{
				SetProperty(ref m_hueIcon, value, "HueIcon");
				OnHueIconChanged(value);
			}
		}

		public float SaturationIcon
		{
			get
			{
				return m_saturationIcon;
			}
			set
			{
				SetProperty(ref m_saturationIcon, value, "SaturationIcon");
				OnSaturationIconChanged(value);
			}
		}

		public float ColorValueIcon
		{
			get
			{
				return m_valueIcon;
			}
			set
			{
				SetProperty(ref m_valueIcon, value, "ColorValueIcon");
				OnValueIconChanged(value);
			}
		}

		public BitmapImage FactionIconBitmap
		{
			get
			{
				return m_factionIconBitmap;
			}
			set
			{
				SetProperty(ref m_factionIconBitmap, value, "FactionIconBitmap");
			}
		}

		public ICommand OkCommand
		{
			get
			{
				return m_okCommand;
			}
			set
			{
				SetProperty(ref m_okCommand, value, "OkCommand");
			}
		}

		public ICommand CancelCommand
		{
			get
			{
				return m_cancelCommand;
			}
			set
			{
				SetProperty(ref m_cancelCommand, value, "CancelCommand");
			}
		}

		public ICommand SelectIconCommand
		{
			get
			{
				return m_selectIconCommand;
			}
			set
			{
				SetProperty(ref m_selectIconCommand, value, "SelectIconCommand");
			}
		}

		public ObservableCollection<MyFactionIconModel> FactionIcons
		{
			get
			{
				return m_factionIcons;
			}
			set
			{
				SetProperty(ref m_factionIcons, value, "FactionIcons");
			}
		}

		public MyFactionIconModel SelectedIcon
		{
			get
			{
				return m_selectedIcon;
			}
			set
			{
				SetProperty(ref m_selectedIcon, value, "SelectedIcon");
				if (m_selectedIcon != null)
				{
					OnSelectIcon(m_selectedIcon);
				}
			}
		}

		public string ImageIconPath
		{
			get;
			private set;
		}

		public Color BackgroundColor
		{
			get;
			private set;
		}

		public Color IconColor
		{
			get;
			private set;
		}

		public SerializableDefinitionId FactionIconGroupId
		{
			get;
			private set;
		}

		public int FactionIconId
		{
			get;
			private set;
		}

		public event Action<MyEditFactionIconViewModel> OnFactionEditorOk;

		public MyEditFactionIconViewModel(SerializableDefinitionId iconGroupId, int iconId, string imageIconPath, Color bgColor, Color iconColor)
		{
			FactionIconGroupId = iconGroupId;
			FactionIconId = iconId;
			m_iconTexturePath = (ImageIconPath = imageIconPath);
			BackgroundColor = bgColor;
			IconColor = iconColor;
			m_runtimeBgColorHsv = bgColor.ColorToHSV();
			m_runtimeIconColorHsv = iconColor.ColorToHSV();
			RefreshIcon(bgColor, iconColor, imageIconPath);
			Hue = m_runtimeBgColorHsv.X;
			Saturation = m_runtimeBgColorHsv.Y;
			ColorValue = m_runtimeBgColorHsv.Z;
			HueIcon = m_runtimeIconColorHsv.X;
			SaturationIcon = m_runtimeIconColorHsv.Y;
			ColorValueIcon = m_runtimeIconColorHsv.Z;
			IEnumerable<MyFactionIconsDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MyFactionIconsDefinition>();
			List<MyFactionIconModel> list = new List<MyFactionIconModel>();
			foreach (MyFactionIconsDefinition item2 in allDefinitions)
			{
				string @string = MyTexts.GetString(MySpaceTexts.Economy_FactionIcon_Tooltip_Allowed);
				bool flag;
				if (item2.Id.SubtypeId.String == "Other")
				{
					flag = MySession.Static.GetComponent<MySessionComponentDLC>().HasDLC(MyDLCs.MyDLC.EconomyExpansion.Name, Sync.MyId);
					if (!flag)
					{
						@string = MyTexts.GetString(MySpaceTexts.Economy_FactionIcon_Tooltip_BuyEconomy);
					}
				}
				else
				{
					flag = true;
				}
				int num = 0;
				string[] icons = item2.Icons;
				foreach (string iconPath in icons)
				{
					MyFactionIconModel item = new MyFactionIconModel(item2.Id, num, iconPath, new ColorW(iconColor.R, iconColor.G, iconColor.B), flag, @string);
					list.Add(item);
					num++;
				}
			}
			FactionIcons = new ObservableCollection<MyFactionIconModel>(list);
			OkCommand = new RelayCommand(OnOK);
			CancelCommand = new RelayCommand(OnCancel);
			SelectIconCommand = new RelayCommand<MyFactionIconModel>(OnSelectIcon);
			ServiceManager.Instance.AddService((IMyEditFactionIconViewModel)this);
		}

		private void OnValueChanged(float obj)
		{
			m_runtimeBgColorHsv.Z = obj;
			RefreshIcon(m_runtimeBgColorHsv.HSVtoColor(), m_runtimeIconColorHsv.HSVtoColor(), m_iconTexturePath);
		}

		private void OnSaturationChanged(float obj)
		{
			m_runtimeBgColorHsv.Y = obj;
			RefreshIcon(m_runtimeBgColorHsv.HSVtoColor(), m_runtimeIconColorHsv.HSVtoColor(), m_iconTexturePath);
		}

		private void OnHueChanged(float obj)
		{
			_ = m_runtimeBgColorHsv;
			m_runtimeBgColorHsv.X = obj;
			RefreshIcon(m_runtimeBgColorHsv.HSVtoColor(), m_runtimeIconColorHsv.HSVtoColor(), m_iconTexturePath);
		}

		private void OnValueIconChanged(float obj)
		{
			m_runtimeIconColorHsv.Z = obj;
			RefreshIcon(m_runtimeBgColorHsv.HSVtoColor(), m_runtimeIconColorHsv.HSVtoColor(), m_iconTexturePath);
		}

		private void OnSaturationIconChanged(float obj)
		{
			m_runtimeIconColorHsv.Y = obj;
			RefreshIcon(m_runtimeBgColorHsv.HSVtoColor(), m_runtimeIconColorHsv.HSVtoColor(), m_iconTexturePath);
		}

		private void OnHueIconChanged(float obj)
		{
			_ = m_runtimeIconColorHsv;
			m_runtimeIconColorHsv.X = obj;
			RefreshIcon(m_runtimeBgColorHsv.HSVtoColor(), m_runtimeIconColorHsv.HSVtoColor(), m_iconTexturePath);
		}

		private void OnOK(object obj)
		{
			BackgroundColor = m_runtimeBgColorHsv.HSVtoColor();
			IconColor = m_runtimeIconColorHsv.HSVtoColor();
			ImageIconPath = m_iconTexturePath;
			MyScreenManager.GetScreenWithFocus().CloseScreen();
			this.OnFactionEditorOk(this);
		}

		private void OnCancel(object obj)
		{
			MyScreenManager.GetScreenWithFocus().CloseScreen();
		}

		private void OnSelectIcon(MyFactionIconModel iconItem)
		{
			if (iconItem.IsEnabled)
			{
				m_iconTexturePath = iconItem.Icon.TextureAsset;
				FactionIconGroupId = iconItem.GroupId;
				FactionIconId = iconItem.Id;
				RefreshIcon(m_runtimeBgColorHsv.HSVtoColor(), m_runtimeIconColorHsv.HSVtoColor(), m_iconTexturePath);
			}
		}

		private void RefreshIcon(Color color, Color iconColor, string imagePath)
		{
			FactionColor = new ColorW(color.R, color.G, color.B);
			ColorW iconColor2 = IconColorInternal = new ColorW(iconColor.R, iconColor.G, iconColor.B);
			foreach (MyFactionIconModel factionIcon in FactionIcons)
			{
				factionIcon.IconColor = iconColor2;
			}
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.TextureAsset = imagePath;
			FactionIconBitmap = bitmapImage;
		}

		public override void OnScreenClosing()
		{
			ServiceManager.Instance.RemoveService<IMyEditFactionIconViewModel>();
		}
	}
}
