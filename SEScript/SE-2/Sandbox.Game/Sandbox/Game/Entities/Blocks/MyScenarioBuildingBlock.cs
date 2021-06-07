using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Network;

namespace Sandbox.Game.Entities.Blocks
{
	public class MyScenarioBuildingBlock : MyTerminalBlock
	{
		private class Sandbox_Game_Entities_Blocks_MyScenarioBuildingBlock_003C_003EActor : IActivator, IActivator<MyScenarioBuildingBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyScenarioBuildingBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyScenarioBuildingBlock CreateInstance()
			{
				return new MyScenarioBuildingBlock();
			}

			MyScenarioBuildingBlock IActivator<MyScenarioBuildingBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public static List<MyTerminalBlock> Clipboard = new List<MyTerminalBlock>();

		private static TimeSpan m_lastAccess;

		private static void AddToClipboard(MyTerminalBlock block)
		{
			if (MySession.Static.ElapsedGameTime != m_lastAccess)
			{
				Clipboard.Clear();
				m_lastAccess = MySession.Static.ElapsedGameTime;
			}
			Clipboard.Add(block);
		}

		public MyScenarioBuildingBlock()
		{
			CreateTerminalControls();
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyTerminalBlock>())
			{
				base.CreateTerminalControls();
				MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MyTerminalBlock>());
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyTerminalBlock>("CopyBlockID", MySpaceTexts.GuiScenarioEdit_CopyIds, MySpaceTexts.GuiScenarioEdit_CopyIdsTooltip, delegate(MyTerminalBlock self)
				{
					AddToClipboard(self);
				})
				{
					Enabled = ((MyTerminalBlock x) => true),
					Visible = ((MyTerminalBlock x) => MySession.Static.Settings.ScenarioEditMode),
					SupportsMultipleBlocks = true
				});
			}
		}
	}
}
