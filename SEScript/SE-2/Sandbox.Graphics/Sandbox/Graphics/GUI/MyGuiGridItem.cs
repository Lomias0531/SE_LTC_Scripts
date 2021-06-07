using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiGridItem
	{
		public readonly Dictionary<MyGuiDrawAlignEnum, StringBuilder> TextsByAlign;

		public readonly Dictionary<MyGuiDrawAlignEnum, ColoredIcon> IconsByAlign;

		public string[] Icons;

		public string SubIcon;

		public string SubIcon2;

		public Vector2? SubIconOffset;

		public MyToolTips ToolTip;

		public object UserData;

		public bool Enabled;

		public float OverlayPercent;

		public Vector4 IconColorMask;

		public Vector4 MainIconColorMask = Vector4.One;

		public Vector4 OverlayColorMask;

		public Vector4? BackgroundColor;

		public long blinkCount;

		public const int MILISSECONDS_TO_BLINK = 400;

		public float IconScale = 1f;

		public MyDefinitionBase ItemDefinition
		{
			get;
			set;
		}

		public Vector2 Position
		{
			get;
			set;
		}

		public MyGuiGridItem(string icon = null, string subicon = null, string toolTip = null, object userData = null, bool enabled = true, float iconScale = 1f)
			: this(new string[1]
			{
				icon
			}, subicon, (toolTip != null) ? new MyToolTips(toolTip) : null, userData, enabled, iconScale)
		{
		}

		public MyGuiGridItem(string[] icons = null, string subicon = null, string toolTip = null, object userData = null, bool enabled = true, float iconScale = 1f)
			: this(icons, subicon, (toolTip != null) ? new MyToolTips(toolTip) : null, userData, enabled, iconScale)
		{
		}

		public MyGuiGridItem(string icon = null, string subicon = null, MyToolTips toolTips = null, object userData = null, bool enabled = true, float iconScale = 1f)
		{
			TextsByAlign = new Dictionary<MyGuiDrawAlignEnum, StringBuilder>();
			IconsByAlign = new Dictionary<MyGuiDrawAlignEnum, ColoredIcon>();
			Icons = new string[1]
			{
				icon
			};
			SubIcon = subicon;
			ToolTip = toolTips;
			UserData = userData;
			Enabled = enabled;
			IconColorMask = Vector4.One;
			OverlayColorMask = Vector4.One;
			BackgroundColor = null;
			blinkCount = 0L;
			IconScale = iconScale;
		}

		public MyGuiGridItem(string[] icons = null, string subicon = null, MyToolTips toolTips = null, object userData = null, bool enabled = true, float iconScale = 1f)
		{
			TextsByAlign = new Dictionary<MyGuiDrawAlignEnum, StringBuilder>();
			IconsByAlign = new Dictionary<MyGuiDrawAlignEnum, ColoredIcon>();
			Icons = icons;
			SubIcon = subicon;
			ToolTip = toolTips;
			UserData = userData;
			Enabled = enabled;
			IconColorMask = Vector4.One;
			OverlayColorMask = Vector4.One;
			BackgroundColor = null;
			blinkCount = 0L;
			IconScale = iconScale;
		}

		public void AddText(StringBuilder text, MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
		{
			if (!TextsByAlign.ContainsKey(textAlign))
			{
				TextsByAlign[textAlign] = new StringBuilder();
			}
			if (TextsByAlign[textAlign].CompareTo(text) != 0)
			{
				TextsByAlign[textAlign].Clear().AppendStringBuilder(text);
			}
		}

		public void AddText(string text, MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
		{
			if (!TextsByAlign.ContainsKey(textAlign))
			{
				TextsByAlign[textAlign] = new StringBuilder();
			}
			if (TextsByAlign[textAlign].CompareTo(text) != 0)
			{
				TextsByAlign[textAlign].Clear().Append(text);
			}
		}

		public void AddIcon(ColoredIcon icon, MyGuiDrawAlignEnum iconAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
		{
			if (!IconsByAlign.ContainsKey(iconAlign))
			{
				IconsByAlign.Add(iconAlign, icon);
			}
			else
			{
				IconsByAlign[iconAlign] = icon;
			}
		}

		public void ClearText(MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
		{
			TextsByAlign.Remove(textAlign);
		}

		public void ClearAllText()
		{
			TextsByAlign.Clear();
		}

		public float blinkingTransparency()
		{
			if (MyGuiManager.TotalTimeInMilliseconds - blinkCount > 400)
			{
				return 1f;
			}
			long num = MyGuiManager.TotalTimeInMilliseconds - blinkCount;
			return (3f + (float)Math.Cos((double)(num * 4) * Math.PI / 400.0)) / 4f;
		}

		public void startBlinking()
		{
			blinkCount = MyGuiManager.TotalTimeInMilliseconds;
		}
	}
}
