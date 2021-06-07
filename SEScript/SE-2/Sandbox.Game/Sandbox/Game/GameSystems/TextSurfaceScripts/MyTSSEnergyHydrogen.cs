using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Graphics;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
	[MyTextSurfaceScript("TSS_EnergyHydrogen", "DisplayName_TSS_EnergyHydrogen")]
	public class MyTSSEnergyHydrogen : MyTSSCommon
	{
		public static float ASPECT_RATIO = 3f;

		public static float DECORATION_RATIO = 0.25f;

		public static float TEXT_RATIO = 0.25f;

		public static string ENERGY_ICON = "IconEnergy";

		public static string HYDROGEN_ICON = "IconHydrogen";

		private Vector2 m_innerSize;

		private Vector2 m_decorationSize;

		private float m_firstLine;

		private float m_secondLine;

		private StringBuilder m_sb = new StringBuilder();

		private MyResourceDistributorComponent m_resourceDistributor;

		private MyCubeGrid m_grid;

		private float m_maxHydrogen;

		private List<Sandbox.Game.Entities.Interfaces.IMyGasTank> m_tankBlocks = new List<Sandbox.Game.Entities.Interfaces.IMyGasTank>();

		public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

		public MyTSSEnergyHydrogen(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
			: base(surface, block, size)
		{
			m_innerSize = new Vector2(ASPECT_RATIO, 1f);
			MyTextSurfaceScriptBase.FitRect(size, ref m_innerSize);
			m_decorationSize = new Vector2(0.012f * m_innerSize.X, DECORATION_RATIO * m_innerSize.Y);
			m_sb.Clear();
			m_sb.Append("Power Usage: 00.000");
			Vector2 vector = MyGuiManager.MeasureStringRaw(m_fontId, m_sb, 1f);
			float val = TEXT_RATIO * m_innerSize.Y / vector.Y;
			m_fontScale = Math.Min(m_innerSize.X * 0.72f / vector.X, val);
			m_firstLine = m_halfSize.Y - m_decorationSize.Y * 0.55f;
			m_secondLine = m_halfSize.Y + m_decorationSize.Y * 0.55f;
			if (m_block != null)
			{
				m_grid = (m_block.CubeGrid as MyCubeGrid);
				if (m_grid != null)
				{
					m_resourceDistributor = m_grid.GridSystems.ResourceDistributor;
					m_grid.GridSystems.ConveyorSystem.BlockAdded += ConveyorSystemOnBlockAdded;
					m_grid.GridSystems.ConveyorSystem.BlockRemoved += ConveyorSystemOnBlockRemoved;
					Recalculate();
				}
			}
		}

		public override void Run()
		{
			base.Run();
			using (MySpriteDrawFrame frame = m_surface.DrawFrame())
			{
				AddBackground(frame, new Color(m_foregroundColor, 0.66f));
				if (m_resourceDistributor == null && m_grid != null)
				{
					m_resourceDistributor = m_grid.GridSystems.ResourceDistributor;
				}
				if (m_resourceDistributor != null)
				{
					Color barBgColor = new Color(m_foregroundColor, 0.1f);
					float num = m_innerSize.X * 0.5f;
					float num2 = num * 0.06f;
					float num3 = m_resourceDistributor.MaxAvailableResourceByType(MyResourceDistributorComponent.ElectricityId);
					float num4 = MyMath.Clamp(m_resourceDistributor.TotalRequiredInputByType(MyResourceDistributorComponent.ElectricityId), 0f, num3);
					float num5 = (num3 > 0f) ? (num4 / num3) : 0f;
					m_sb.Clear();
					m_sb.Append("[");
					Vector2 vector = MyGuiManager.MeasureStringRaw(m_fontId, m_sb, 1f);
					float scale = m_decorationSize.Y / vector.Y;
					vector = MyGuiManager.MeasureStringRaw(m_fontId, m_sb, scale);
					m_sb.Clear();
					m_sb.Append($"{num5 * 100f:0}");
					Vector2 vector2 = MyGuiManager.MeasureStringRaw(m_fontId, m_sb, m_fontScale);
					MySprite mySprite = default(MySprite);
					mySprite.Position = new Vector2(m_halfSize.X + num * 0.6f - num2, m_firstLine - vector2.Y * 0.5f);
					mySprite.Size = new Vector2(m_innerSize.X, m_innerSize.Y);
					mySprite.Type = SpriteType.TEXT;
					mySprite.FontId = m_fontId;
					mySprite.Alignment = TextAlignment.LEFT;
					mySprite.Color = m_foregroundColor;
					mySprite.RotationOrScale = m_fontScale;
					mySprite.Data = m_sb.ToString();
					MySprite sprite = mySprite;
					frame.Add(sprite);
					mySprite = new MySprite(SpriteType.TEXTURE, ENERGY_ICON, null, null, m_foregroundColor);
					mySprite.Position = new Vector2(m_halfSize.X - num * 0.6f - num2, m_firstLine);
					mySprite.Size = new Vector2(vector.Y * 0.6f);
					MySprite sprite2 = mySprite;
					frame.Add(sprite2);
					AddProgressBar(frame, new Vector2(m_halfSize.X - num2, m_firstLine), new Vector2(num, vector.Y * 0.4f), num5, barBgColor, m_foregroundColor);
					float num6 = 0f;
					foreach (Sandbox.Game.Entities.Interfaces.IMyGasTank tankBlock in m_tankBlocks)
					{
						num6 += (float)(tankBlock.FilledRatio * (double)tankBlock.GasCapacity);
					}
					num5 = ((m_maxHydrogen > 0f) ? (num6 / m_maxHydrogen) : 0f);
					m_sb.Clear();
					m_sb.Append($"{num5 * 100f:0}");
					vector2 = MyGuiManager.MeasureStringRaw(m_fontId, m_sb, m_fontScale);
					mySprite = default(MySprite);
					mySprite.Position = new Vector2(m_halfSize.X + num * 0.6f - num2, m_secondLine - vector2.Y * 0.5f);
					mySprite.Size = new Vector2(m_innerSize.X, m_innerSize.Y);
					mySprite.Type = SpriteType.TEXT;
					mySprite.FontId = m_fontId;
					mySprite.Alignment = TextAlignment.LEFT;
					mySprite.Color = m_foregroundColor;
					mySprite.RotationOrScale = m_fontScale;
					mySprite.Data = m_sb.ToString();
					MySprite sprite3 = mySprite;
					frame.Add(sprite3);
					mySprite = new MySprite(SpriteType.TEXTURE, HYDROGEN_ICON, null, null, m_foregroundColor);
					mySprite.Position = new Vector2(m_halfSize.X - num * 0.6f - num2, m_secondLine);
					mySprite.Size = new Vector2(vector.Y * 0.6f);
					MySprite sprite4 = mySprite;
					frame.Add(sprite4);
					AddProgressBar(frame, new Vector2(m_halfSize.X - num2, m_secondLine), new Vector2(num, vector.Y * 0.4f), num5, barBgColor, m_foregroundColor);
					float scale2 = m_innerSize.Y / 256f * 0.9f;
					AddBrackets(frame, new Vector2(64f, 256f), scale2);
				}
			}
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		private void Recalculate()
		{
			m_maxHydrogen = 0f;
			if (m_grid != null)
			{
				foreach (IMyConveyorEndpointBlock conveyorEndpointBlock in m_grid.GridSystems.ConveyorSystem.ConveyorEndpointBlocks)
				{
					Sandbox.Game.Entities.Interfaces.IMyGasTank myGasTank = conveyorEndpointBlock as Sandbox.Game.Entities.Interfaces.IMyGasTank;
					if (myGasTank != null && myGasTank.IsResourceStorage(MyResourceDistributorComponent.HydrogenId))
					{
						m_maxHydrogen += myGasTank.GasCapacity;
						m_tankBlocks.Add(myGasTank);
					}
				}
			}
		}

		private void ConveyorSystemOnBlockRemoved(MyCubeBlock myCubeBlock)
		{
			Sandbox.Game.Entities.Interfaces.IMyGasTank myGasTank = myCubeBlock as Sandbox.Game.Entities.Interfaces.IMyGasTank;
			if (myGasTank != null && myGasTank.IsResourceStorage(MyResourceDistributorComponent.HydrogenId))
			{
				m_maxHydrogen -= myGasTank.GasCapacity;
				m_tankBlocks.Remove(myGasTank);
			}
		}

		private void ConveyorSystemOnBlockAdded(MyCubeBlock myCubeBlock)
		{
			Sandbox.Game.Entities.Interfaces.IMyGasTank myGasTank = myCubeBlock as Sandbox.Game.Entities.Interfaces.IMyGasTank;
			if (myGasTank != null && myGasTank.IsResourceStorage(MyResourceDistributorComponent.HydrogenId))
			{
				m_maxHydrogen += myGasTank.GasCapacity;
				m_tankBlocks.Add(myGasTank);
			}
		}
	}
}
