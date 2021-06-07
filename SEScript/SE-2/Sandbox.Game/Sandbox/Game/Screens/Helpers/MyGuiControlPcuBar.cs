using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGuiControlPcuBar : MyGuiControlParent
	{
		private MyGuiControlLabel m_PCULabel;

		private MyGuiControlLabel m_PCUCost;

		private MyGuiControlLabel m_PCUCountLabel;

		private MyGuiControlImage m_PCUIcon;

		private MyGuiControlImage m_PCULineBG;

		private MyGuiControlImage m_PCULine;

		private int m_maxPCU;

		private int m_currentPCU;

		private int m_currentDisplayedPCU = -1;

		private int m_frameCounterPCU;

		private Vector2 m_barSize;

		public MyGuiControlPcuBar(Vector2? position = null, float? width = null)
			: base(position)
		{
			m_barSize = new Vector2(width ?? 0.32f, 0.007f);
			base.Size = new Vector2(m_barSize.X, 0.039f);
			Vector2 vector = -base.Size / 2f;
			m_PCULabel = new MyGuiControlLabel
			{
				Position = vector + new Vector2(0.03f, 0.002f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = "PCU:"
			};
			m_PCUCost = new MyGuiControlLabel
			{
				Position = vector + new Vector2(0.07f, 0.002f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			base.Controls.Add(m_PCUCost);
			m_PCUCountLabel = new MyGuiControlLabel
			{
				Position = vector + new Vector2(base.Size.X - 0.005f, 0.002f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP
			};
			m_PCUIcon = new MyGuiControlImage(vector, new Vector2(0.022f, 0.029f), null, null, new string[1]
			{
				"Textures\\GUI\\PCU.png"
			});
			m_PCUIcon.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			base.Controls.Add(m_PCUIcon);
			m_PCULineBG = new MyGuiControlImage(vector + new Vector2(0f, base.Size.Y), m_barSize, null, null, new string[1]
			{
				"Textures\\GUI\\Icons\\HUD 2017\\DrillBarBackground.png"
			});
			m_PCULineBG.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			base.Controls.Add(m_PCULineBG);
			m_PCULine = new MyGuiControlImage(vector + new Vector2(0f, base.Size.Y), m_barSize, null, null, new string[1]
			{
				"Textures\\GUI\\Icons\\HUD 2017\\DrillBarProgress.png"
			});
			m_PCULine.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			base.Controls.Add(m_PCULine);
			base.Controls.Add(m_PCULabel);
			base.Controls.Add(m_PCUCountLabel);
		}

		public void UpdatePCU(MyIdentity identity, bool performAnimation)
		{
			m_maxPCU = 0;
			m_currentPCU = 0;
			if (identity != null)
			{
				m_maxPCU = identity.GetMaxPCU();
				m_currentPCU = identity.BlockLimits.PCU;
			}
			if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE || MySession.Static.TotalPCU == 0)
			{
				m_currentDisplayedPCU = m_currentPCU;
				m_PCUCountLabel.TextEnum = MyCommonTexts.Unlimited;
			}
			else if (m_currentDisplayedPCU != m_currentPCU)
			{
				if (performAnimation)
				{
					int num = Math.Max(1, Math.Abs((m_currentPCU - m_currentDisplayedPCU) / 20));
					m_currentDisplayedPCU = ((m_currentPCU < m_currentDisplayedPCU) ? (m_currentDisplayedPCU - num) : (m_currentDisplayedPCU + num));
				}
				else
				{
					m_currentDisplayedPCU = m_currentPCU;
				}
				m_PCUCountLabel.Text = $"{m_currentPCU} / {m_maxPCU}";
			}
			m_PCULine.Size = new Vector2((m_maxPCU != 0) ? Math.Min(m_barSize.X / (float)m_maxPCU * (float)m_currentDisplayedPCU, m_barSize.X) : 0f, m_barSize.Y);
		}
	}
}
