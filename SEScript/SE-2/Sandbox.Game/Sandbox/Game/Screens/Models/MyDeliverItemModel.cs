using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;

namespace Sandbox.Game.Screens.Models
{
	public class MyDeliverItemModel : BindableBase
	{
		private string m_name;

		private BitmapImage m_icon;

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

		public MyPhysicalItemDefinition ItemDefinition
		{
			get;
			set;
		}

		public MyDeliverItemModel(MyPhysicalItemDefinition itemDefinition)
		{
			Name = itemDefinition.DisplayNameText;
			BitmapImage bitmapImage = new BitmapImage();
			string[] icons = itemDefinition.Icons;
			if (icons != null && icons.Length != 0)
			{
				bitmapImage.TextureAsset = itemDefinition.Icons[0];
			}
			Icon = bitmapImage;
			ItemDefinition = itemDefinition;
		}
	}
}
