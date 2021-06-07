using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using VRage;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
	[MyTextSurfaceScript("TSS_FactionIcon", "DisplayName_TSS_FactionIcon")]
	public class MyTSSFactionIcon : MyTSSCommon
	{
		private const string BG_TEXTURE_NAME = "SquareSimple";

		private MyFaction m_faction;

		private float m_iconSize;

		private string m_errorMsg;

		private bool m_updateNeeded = true;

		public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

		public MyTSSFactionIcon(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
			: base(surface, block, size)
		{
			MySession.Static.Factions.FactionEdited += OnFactionEdited;
			MySession.Static.Factions.FactionCreated += OnFactionEdited;
			UpdateFactionData(0L);
			m_iconSize = Math.Min(m_surface.SurfaceSize.X, m_surface.SurfaceSize.Y);
			m_errorMsg = MyTexts.GetString(MySpaceTexts.TSS_FactionIcon_Error);
			MyTerminalBlock myTerminalBlock;
			if ((myTerminalBlock = (block as MyTerminalBlock)) != null)
			{
				myTerminalBlock.OwnershipChanged += OnOwnershipChanged;
			}
		}

		private bool UpdateFactionData(long editedFactionId = 0L)
		{
			MyFaction myFaction = MySession.Static.Factions.TryGetFactionByTag(m_block.GetOwnerFactionTag());
			if (myFaction != m_faction || (myFaction != null && (myFaction.FactionId == editedFactionId || editedFactionId == 0L)))
			{
				m_faction = myFaction;
				return true;
			}
			return false;
		}

		public override void Run()
		{
			base.Run();
			if (m_updateNeeded)
			{
				using (MySpriteDrawFrame mySpriteDrawFrame = m_surface.DrawFrame())
				{
					Vector2 vector = m_halfSize - m_surface.SurfaceSize * 0.5f + new Vector2(m_surface.SurfaceSize.X * 0.5f, m_surface.SurfaceSize.Y * 0.5f);
					if (m_faction == null || !m_faction.FactionIcon.HasValue)
					{
						MySprite sprite = MySprite.CreateText(m_errorMsg, m_fontId, Color.White);
						sprite.Position = vector;
						mySpriteDrawFrame.Add(sprite);
						m_updateNeeded = false;
						return;
					}
					MySprite sprite2 = MySprite.CreateSprite("SquareSimple", vector, m_surface.SurfaceSize);
					sprite2.Color = MyColorPickerConstants.HSVOffsetToHSV(m_faction.CustomColor).HSVtoColor();
					mySpriteDrawFrame.Add(sprite2);
					MySprite sprite3 = MySprite.CreateSprite(m_faction.FactionIcon.Value.ToString(), vector, new Vector2(m_iconSize));
					sprite3.Color = MyColorPickerConstants.HSVOffsetToHSV(m_faction.IconColor).HSVtoColor();
					mySpriteDrawFrame.Add(sprite3);
				}
				m_updateNeeded = false;
			}
		}

		private void OnFactionEdited(long factionId)
		{
			if (m_faction == null)
			{
				if (UpdateFactionData(factionId))
				{
					m_updateNeeded = true;
				}
			}
			else if (m_faction != null && factionId == m_faction.FactionId)
			{
				m_updateNeeded = true;
			}
		}

		private void OnOwnershipChanged(MyTerminalBlock obj)
		{
			if (UpdateFactionData(0L))
			{
				m_updateNeeded = true;
			}
		}

		public override void Dispose()
		{
			MyTerminalBlock myTerminalBlock;
			if ((myTerminalBlock = (m_block as MyTerminalBlock)) != null)
			{
				myTerminalBlock.OwnershipChanged -= OnOwnershipChanged;
			}
			MySession.Static.Factions.FactionEdited -= OnFactionEdited;
			MySession.Static.Factions.FactionCreated -= OnFactionEdited;
			m_faction = null;
		}
	}
}
