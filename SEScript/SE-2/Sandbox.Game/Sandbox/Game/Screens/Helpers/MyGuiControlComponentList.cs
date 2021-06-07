using Sandbox.Definitions;
using Sandbox.Graphics.GUI;
using System.Globalization;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	internal class MyGuiControlComponentList : MyGuiControlBase
	{
		private class ItemIconControl : MyGuiControlBase
		{
			private static readonly float SCALE = 0.85f;

			internal ItemIconControl(MyPhysicalItemDefinition def)
				: base(null, MyGuiConstants.TEXTURE_GRID_ITEM.SizeGui * SCALE, null, null, MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER, isActiveControl: false)
			{
				base.MinSize = (base.MaxSize = base.Size);
				MyGuiBorderThickness myGuiBorderThickness = new MyGuiBorderThickness(0.0025f, 0.001f);
				if (def == null)
				{
					Elements.Add(new MyGuiControlPanel(null, base.Size - myGuiBorderThickness.SizeChange, null, MyGuiConstants.TEXTURE_ICON_FAKE.Texture));
					return;
				}
				for (int i = 0; i < def.Icons.Length; i++)
				{
					Elements.Add(new MyGuiControlPanel(null, base.Size - myGuiBorderThickness.SizeChange, null, def.Icons[0]));
				}
				if (def.IconSymbol.HasValue)
				{
					Elements.Add(new MyGuiControlLabel(-0.5f * base.Size + myGuiBorderThickness.TopLeftOffset, null, MyTexts.GetString(def.IconSymbol.Value), null, SCALE * 0.75f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP));
				}
			}
		}

		internal class ComponentControl : MyGuiControlBase
		{
			public readonly MyDefinitionId Id;

			private ItemIconControl m_iconControl;

			private MyGuiControlLabel m_nameLabel;

			private MyGuiControlLabel m_valuesLabel;

			public string ValuesFont
			{
				set
				{
					m_valuesLabel.Font = value;
				}
			}

			internal ComponentControl(MyDefinitionId id)
				: base(null, new Vector2(0.2f, MyGuiConstants.TEXTURE_GRID_ITEM.SizeGui.Y * 0.75f), null, null, null, isActiveControl: false)
			{
				MyPhysicalItemDefinition myPhysicalItemDefinition = null;
				if (!id.TypeId.IsNull)
				{
					myPhysicalItemDefinition = (MyDefinitionManager.Static.GetDefinition(id) as MyPhysicalItemDefinition);
				}
				m_iconControl = new ItemIconControl(myPhysicalItemDefinition)
				{
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
				};
				m_nameLabel = new MyGuiControlLabel(null, null, (myPhysicalItemDefinition != null) ? myPhysicalItemDefinition.DisplayNameText : "N/A", null, 0.68f)
				{
					AutoEllipsis = true
				};
				m_valuesLabel = new MyGuiControlLabel(null, null, new StringBuilder("{0} / {1}").ToString(), null, 0.6f, "White", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
				SetValues(99.0, 99.0);
				Elements.Add(m_iconControl);
				Elements.Add(m_nameLabel);
				Elements.Add(m_valuesLabel);
				base.MinSize = new Vector2(m_iconControl.MinSize.X + m_nameLabel.Size.X + m_valuesLabel.Size.X, m_iconControl.MinSize.Y);
			}

			protected override void OnSizeChanged()
			{
				m_iconControl.Position = base.Size * new Vector2(-0.5f, 0f);
				m_nameLabel.Position = m_iconControl.Position + new Vector2(m_iconControl.Size.X + 0.01f, 0f);
				m_valuesLabel.Position = base.Size * new Vector2(0.5f, 0f);
				UpdateNameLabelSize();
				base.OnSizeChanged();
			}

			public void SetValues(double val1, double val2)
			{
				m_valuesLabel.UpdateFormatParams(val1.ToString("N", CultureInfo.InvariantCulture), val2.ToString("N", CultureInfo.InvariantCulture));
				UpdateNameLabelSize();
			}

			private void UpdateNameLabelSize()
			{
				m_nameLabel.Size = new Vector2(base.Size.X - (m_iconControl.Size.X + m_valuesLabel.Size.X), m_nameLabel.Size.Y);
			}
		}

		private float m_currentOffsetFromTop;

		private MyGuiBorderThickness m_padding;

		private MyGuiControlLabel m_valuesLabel;

		public StringBuilder ValuesText
		{
			get
			{
				return new StringBuilder(m_valuesLabel.Text);
			}
			set
			{
				m_valuesLabel.Text = value.ToString();
			}
		}

		public ComponentControl this[int i] => (ComponentControl)Elements[i + 1];

		public int Count => Elements.Count - 1;

		public MyGuiControlComponentList()
			: base(null, null, null, null, null, isActiveControl: false)
		{
			m_padding = new MyGuiBorderThickness(0.02f, 0.008f);
			m_valuesLabel = new MyGuiControlLabel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				TextScale = 0.6f
			};
			Elements.Add(m_valuesLabel);
			UpdatePositions();
		}

		public void Add(MyDefinitionId id, double val1, double val2, string font)
		{
			ComponentControl componentControl = new ComponentControl(id);
			componentControl.Size = new Vector2(base.Size.X - m_padding.HorizontalSum, componentControl.Size.Y);
			m_currentOffsetFromTop += componentControl.Size.Y;
			componentControl.Position = -0.5f * base.Size + new Vector2(m_padding.Left, m_currentOffsetFromTop);
			componentControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			componentControl.ValuesFont = font;
			componentControl.SetValues(val1, val2);
			Elements.Add(componentControl);
		}

		public new void Clear()
		{
			Elements.Clear();
			Elements.Add(m_valuesLabel);
			m_currentOffsetFromTop = m_valuesLabel.Size.Y + m_padding.Top;
		}

		protected override void OnSizeChanged()
		{
			UpdatePositions();
			base.OnSizeChanged();
		}

		private void UpdatePositions()
		{
			m_valuesLabel.Position = base.Size * new Vector2(0.5f, -0.5f) + m_padding.TopRightOffset;
			m_currentOffsetFromTop = m_valuesLabel.Size.Y + m_padding.Top;
			foreach (MyGuiControlBase element in Elements)
			{
				if (element != m_valuesLabel)
				{
					float y = element.Size.Y;
					m_currentOffsetFromTop += y;
					element.Position = -0.5f * base.Size + new Vector2(m_padding.Left, m_currentOffsetFromTop);
					element.Size = new Vector2(base.Size.X - m_padding.HorizontalSum, y);
				}
			}
		}
	}
}
