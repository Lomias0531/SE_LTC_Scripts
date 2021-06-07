using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Screens.Models
{
	public class MyFactionIconModel : BindableBase
	{
		private float CONST_OPACITY_ENABLED = 1f;

		private float CONST_OPACITY_DISABLED = 0.8f;

		private BitmapImage m_icon;

		private ColorW m_iconColor;

		private bool m_isEnabled = true;

		private string m_tooltipText;

		public SerializableDefinitionId GroupId
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
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

		public string TooltipText
		{
			get
			{
				return m_tooltipText;
			}
			set
			{
				SetProperty(ref m_tooltipText, value, "TooltipText");
			}
		}

		public bool IsEnabled
		{
			get
			{
				return m_isEnabled;
			}
			set
			{
				SetProperty(ref m_isEnabled, value, "IsEnabled");
				RaisePropertyChanged("Opacity");
			}
		}

		public ColorW IconColor
		{
			get
			{
				return m_iconColor;
			}
			set
			{
				SetProperty(ref m_iconColor, value, "IconColor");
			}
		}

		public float Opacity
		{
			get
			{
				if (!IsEnabled)
				{
					return CONST_OPACITY_DISABLED;
				}
				return CONST_OPACITY_ENABLED;
			}
		}

		public MyFactionIconModel(SerializableDefinitionId groupId, int id, string iconPath, ColorW iconColor, bool isEnabled = true, string tooltipText = "")
		{
			GroupId = groupId;
			Id = id;
			IconColor = iconColor;
			Icon = new BitmapImage
			{
				TextureAsset = iconPath
			};
			IsEnabled = isEnabled;
			TooltipText = tooltipText;
		}
	}
}
