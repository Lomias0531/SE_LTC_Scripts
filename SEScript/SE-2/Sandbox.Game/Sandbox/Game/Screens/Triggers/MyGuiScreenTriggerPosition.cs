using Sandbox.Game.Localization;
using Sandbox.Game.World.Triggers;
using Sandbox.Graphics.GUI;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Triggers
{
	public class MyGuiScreenTriggerPosition : MyGuiScreenTrigger
	{
		private MyGuiControlLabel m_labelInsX;

		protected MyGuiControlTextbox m_xCoord;

		private MyGuiControlLabel m_labelInsY;

		protected MyGuiControlTextbox m_yCoord;

		private MyGuiControlLabel m_labelInsZ;

		protected MyGuiControlTextbox m_zCoord;

		private MyGuiControlLabel m_labelRadius;

		protected MyGuiControlTextbox m_radius;

		protected MyGuiControlButton m_pasteButton;

		private const float WINSIZEX = 0.4f;

		private const float WINSIZEY = 0.37f;

		private const float spacingH = 0.01f;

		private string m_clipboardText;

		protected bool m_coordsChanged;

		protected Vector3D m_coords;

		private static readonly string m_ScanPattern = "GPS:([^:]{0,32}):([\\d\\.-]*):([\\d\\.-]*):([\\d\\.-]*):";

		public MyGuiScreenTriggerPosition(MyTrigger trg)
			: base(trg, new Vector2(0.5f, 0.420000017f))
		{
			float num = MyGuiScreenTrigger.MIDDLE_PART_ORIGIN.X - 0.2f;
			float num2 = -0.185f + MyGuiScreenTrigger.MIDDLE_PART_ORIGIN.Y;
			m_labelInsX = new MyGuiControlLabel(new Vector2(num, num2), new Vector2(0.01f, 0.035f), MyTexts.Get(MySpaceTexts.TerminalTab_GPS_X).ToString(), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			num += m_labelInsX.Size.X + 0.01f;
			m_xCoord = new MyGuiControlTextbox
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = new Vector2(num, num2),
				Size = new Vector2(0.110000014f - m_labelInsX.Size.X, 0.035f),
				Name = "textX"
			};
			m_xCoord.Enabled = false;
			num += m_xCoord.Size.X + 0.01f;
			m_labelInsY = new MyGuiControlLabel(new Vector2(num, num2), new Vector2(0.388f, 0.035f), MyTexts.Get(MySpaceTexts.TerminalTab_GPS_Y).ToString(), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			num += m_labelInsY.Size.X + 0.01f;
			m_yCoord = new MyGuiControlTextbox
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = new Vector2(num, num2),
				Size = new Vector2(0.110000014f - m_labelInsY.Size.X, 0.035f),
				Name = "textY"
			};
			m_yCoord.Enabled = false;
			num += m_yCoord.Size.X + 0.01f;
			m_labelInsZ = new MyGuiControlLabel(new Vector2(num, num2), new Vector2(0.01f, 0.035f), MyTexts.Get(MySpaceTexts.TerminalTab_GPS_Z).ToString(), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			num += m_labelInsZ.Size.X + 0.01f;
			m_zCoord = new MyGuiControlTextbox
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = new Vector2(num, num2),
				Size = new Vector2(0.110000014f - m_labelInsZ.Size.X, 0.035f),
				Name = "textZ"
			};
			m_zCoord.Enabled = false;
			num = MyGuiScreenTrigger.MIDDLE_PART_ORIGIN.X - 0.2f;
			num2 += m_zCoord.Size.Y + 0.01f;
			m_labelRadius = new MyGuiControlLabel(new Vector2(num, num2), new Vector2(0.01f, 0.035f), MyTexts.Get(MySpaceTexts.GuiTriggerPositionRadius).ToString(), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			num += m_labelRadius.Size.X + 0.01f;
			m_radius = new MyGuiControlTextbox
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = new Vector2(num, num2),
				Size = new Vector2(0.110000014f - m_labelInsZ.Size.X, 0.035f),
				Name = "radius"
			};
			m_radius.TextChanged += OnRadiusChanged;
			num += m_radius.Size.X + 0.01f + 0.05f;
			m_pasteButton = new MyGuiControlButton(text: MyTexts.Get(MySpaceTexts.GuiTriggerPasteGps), onButtonClick: OnPasteButtonClick, position: new Vector2(num, num2), visualStyle: MyGuiControlButtonStyleEnum.Small, size: null, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			Controls.Add(m_labelInsX);
			Controls.Add(m_xCoord);
			Controls.Add(m_labelInsY);
			Controls.Add(m_yCoord);
			Controls.Add(m_labelInsZ);
			Controls.Add(m_zCoord);
			Controls.Add(m_labelRadius);
			Controls.Add(m_radius);
			Controls.Add(m_pasteButton);
		}

		protected override void OnOkButtonClick(MyGuiControlButton sender)
		{
			StrToDouble(m_radius.Text);
			base.OnOkButtonClick(sender);
		}

		private void PasteFromClipboard()
		{
			m_clipboardText = MyVRage.Platform.Clipboard;
		}

		private void OnPasteButtonClick(MyGuiControlButton sender)
		{
			Thread thread = new Thread((ThreadStart)delegate
			{
				PasteFromClipboard();
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
			if (ScanText(m_clipboardText))
			{
				m_coordsChanged = true;
			}
		}

		private bool ScanText(string input)
		{
			foreach (Match item in Regex.Matches(input, m_ScanPattern))
			{
				_ = item.Groups[1].Value;
				double value;
				double value2;
				double value3;
				try
				{
					value = double.Parse(item.Groups[2].Value, CultureInfo.InvariantCulture);
					value = Math.Round(value, 2);
					value2 = double.Parse(item.Groups[3].Value, CultureInfo.InvariantCulture);
					value2 = Math.Round(value2, 2);
					value3 = double.Parse(item.Groups[4].Value, CultureInfo.InvariantCulture);
					value3 = Math.Round(value3, 2);
				}
				catch (SystemException)
				{
					continue;
				}
				m_xCoord.Text = value.ToString();
				m_coords.X = value;
				m_yCoord.Text = value2.ToString();
				m_coords.Y = value2;
				m_zCoord.Text = value3.ToString();
				m_coords.Z = value3;
				return true;
			}
			return false;
		}

		public void OnRadiusChanged(MyGuiControlTextbox sender)
		{
			if (StrToDouble(sender.Text).HasValue)
			{
				sender.ColorMask = Vector4.One;
				m_okButton.Enabled = true;
			}
			else
			{
				sender.ColorMask = Color.Red.ToVector4();
				m_okButton.Enabled = false;
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenTriggerPosition";
		}
	}
}
