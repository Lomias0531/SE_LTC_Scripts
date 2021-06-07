using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Triggers;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Library;
using VRage.Utils;

namespace Sandbox.Game.World.Triggers
{
	[TriggerType(typeof(MyObjectBuilder_TriggerBlockDestroyed))]
	internal class MyTriggerBlockDestroyed : MyTrigger, ICloneable
	{
		public enum BlockState
		{
			Ok,
			Destroyed,
			MessageShown
		}

		private Dictionary<MyTerminalBlock, BlockState> m_blocks = new Dictionary<MyTerminalBlock, BlockState>();

		public string SingleMessage;

		private static List<MyTerminalBlock> m_blocksHelper = new List<MyTerminalBlock>();

		private StringBuilder m_progress = new StringBuilder();

		public Dictionary<MyTerminalBlock, BlockState> Blocks
		{
			get
			{
				return m_blocks;
			}
			private set
			{
				m_blocks = value;
			}
		}

		public MyTriggerBlockDestroyed()
		{
		}

		public MyTriggerBlockDestroyed(MyTriggerBlockDestroyed trg)
			: base(trg)
		{
			SingleMessage = trg.SingleMessage;
			m_blocks.Clear();
			foreach (KeyValuePair<MyTerminalBlock, BlockState> block in trg.m_blocks)
			{
				m_blocks.Add(block.Key, block.Value);
			}
		}

		public override object Clone()
		{
			return new MyTriggerBlockDestroyed(this);
		}

		public override void DisplayHints(MyPlayer player, MyEntity me)
		{
			foreach (KeyValuePair<MyTerminalBlock, BlockState> block in m_blocks)
			{
				if (block.Value != BlockState.MessageShown && block.Key.SlimBlock.IsDestroyed)
				{
					m_blocksHelper.Add(block.Key);
				}
			}
			foreach (MyTerminalBlock item in m_blocksHelper)
			{
				if (SingleMessage != null)
				{
					MyAPIGateway.Utilities.ShowNotification(string.Format(SingleMessage, item.CustomName), 20000, "Blue");
				}
				m_blocks[item] = BlockState.MessageShown;
			}
			m_blocksHelper.Clear();
			base.DisplayHints(player, me);
		}

		public override bool Update(MyPlayer player, MyEntity me)
		{
			bool flag = false;
			foreach (KeyValuePair<MyTerminalBlock, BlockState> block in m_blocks)
			{
				if (block.Value != BlockState.MessageShown)
				{
					if (block.Key.SlimBlock.IsDestroyed)
					{
						m_blocksHelper.Add(block.Key);
					}
					else
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				m_IsTrue = true;
			}
			if (m_blocksHelper.Count > 0)
			{
				foreach (MyTerminalBlock item in m_blocksHelper)
				{
					m_blocks[item] = BlockState.Destroyed;
				}
				m_blocksHelper.Clear();
			}
			return m_IsTrue;
		}

		public override StringBuilder GetProgress()
		{
			m_progress.Clear().Append((object)MyTexts.Get(MySpaceTexts.ScenarioProgressDestroyBlocks));
			foreach (KeyValuePair<MyTerminalBlock, BlockState> block in m_blocks)
			{
				if (block.Value == BlockState.Ok)
				{
					m_progress.Append(MyEnvironment.NewLine).Append("   ").Append((object)block.Key.CustomName);
				}
			}
			return m_progress;
		}

		public override void Init(MyObjectBuilder_Trigger builder)
		{
			base.Init(builder);
			MyObjectBuilder_TriggerBlockDestroyed myObjectBuilder_TriggerBlockDestroyed = (MyObjectBuilder_TriggerBlockDestroyed)builder;
			foreach (long blockId in myObjectBuilder_TriggerBlockDestroyed.BlockIds)
			{
				if (MyEntities.TryGetEntityById(blockId, out MyTerminalBlock entity))
				{
					m_blocks.Add(entity, BlockState.Ok);
				}
			}
			SingleMessage = myObjectBuilder_TriggerBlockDestroyed.SingleMessage;
		}

		public override MyObjectBuilder_Trigger GetObjectBuilder()
		{
			MyObjectBuilder_TriggerBlockDestroyed myObjectBuilder_TriggerBlockDestroyed = (MyObjectBuilder_TriggerBlockDestroyed)base.GetObjectBuilder();
			myObjectBuilder_TriggerBlockDestroyed.BlockIds = new List<long>();
			foreach (KeyValuePair<MyTerminalBlock, BlockState> block in m_blocks)
			{
				if (!block.Key.SlimBlock.IsDestroyed)
				{
					myObjectBuilder_TriggerBlockDestroyed.BlockIds.Add(block.Key.EntityId);
				}
			}
			myObjectBuilder_TriggerBlockDestroyed.SingleMessage = SingleMessage;
			return myObjectBuilder_TriggerBlockDestroyed;
		}

		public override void DisplayGUI()
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenTriggerBlockDestroyed(this));
		}

		public new static MyStringId GetCaption()
		{
			return MySpaceTexts.GuiTriggerCaptionBlockDestroyed;
		}
	}
}
