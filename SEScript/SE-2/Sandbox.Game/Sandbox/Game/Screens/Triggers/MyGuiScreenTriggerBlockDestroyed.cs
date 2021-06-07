using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using Sandbox.Game.World.Triggers;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Triggers
{
	public class MyGuiScreenTriggerBlockDestroyed : MyGuiScreenTrigger
	{
		private MyGuiControlTable m_selectedBlocks;

		private MyGuiControlButton m_buttonPaste;

		private MyGuiControlButton m_buttonDelete;

		private MyGuiControlTextbox m_textboxSingleMessage;

		private MyGuiControlLabel m_labelSingleMessage;

		private MyTriggerBlockDestroyed trigger;

		private static StringBuilder m_tempSb = new StringBuilder();

		public MyGuiScreenTriggerBlockDestroyed(MyTrigger trig)
			: base(trig, new Vector2(0.5f, 0.8f))
		{
			trigger = (MyTriggerBlockDestroyed)trig;
			AddCaption(MySpaceTexts.GuiTriggerCaptionBlockDestroyed);
			MyLayoutTable myLayoutTable = new MyLayoutTable(this);
			myLayoutTable.SetColumnWidthsNormalized(10f, 30f, 3f, 30f, 10f);
			myLayoutTable.SetRowHeightsNormalized(20f, 35f, 6f, 4f, 4f, 5f, 33f);
			m_selectedBlocks = new MyGuiControlTable();
			m_selectedBlocks.VisibleRowsCount = 8;
			m_selectedBlocks.ColumnsCount = 1;
			m_selectedBlocks.SetCustomColumnWidths(new float[1]
			{
				1f
			});
			m_selectedBlocks.SetColumnName(0, MyTexts.Get(MySpaceTexts.GuiTriggerBlockDestroyed_ColumnName));
			myLayoutTable.AddWithSize(m_selectedBlocks, MyAlignH.Left, MyAlignV.Top, 1, 1, 1, 3);
			m_buttonPaste = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.GuiTriggerPasteBlocks), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnPasteButtonClick);
			m_buttonPaste.SetToolTip(MySpaceTexts.GuiTriggerPasteBlocksTooltip);
			myLayoutTable.AddWithSize(m_buttonPaste, MyAlignH.Left, MyAlignV.Top, 2, 1);
			m_buttonDelete = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.GuiTriggerDeleteBlocks), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnDeleteButtonClick);
			myLayoutTable.AddWithSize(m_buttonDelete, MyAlignH.Left, MyAlignV.Top, 2, 3);
			m_labelSingleMessage = new MyGuiControlLabel(null, null, MyTexts.Get(MySpaceTexts.GuiTriggerBlockDestroyedSingleMessage).ToString());
			myLayoutTable.AddWithSize(m_labelSingleMessage, MyAlignH.Left, MyAlignV.Top, 3, 1);
			m_textboxSingleMessage = new MyGuiControlTextbox(null, trigger.SingleMessage, 85);
			myLayoutTable.AddWithSize(m_textboxSingleMessage, MyAlignH.Left, MyAlignV.Top, 4, 1, 1, 3);
			foreach (KeyValuePair<MyTerminalBlock, MyTriggerBlockDestroyed.BlockState> block in trigger.Blocks)
			{
				AddRow(block.Key);
			}
			m_tempSb.Clear().Append(trigger.SingleMessage);
			m_textboxSingleMessage.SetText(m_tempSb);
		}

		public override bool Update(bool hasFocus)
		{
			if (m_selectedBlocks.SelectedRowIndex.HasValue && m_selectedBlocks.SelectedRowIndex < m_selectedBlocks.RowsCount)
			{
				m_buttonDelete.Enabled = true;
			}
			else
			{
				m_buttonDelete.Enabled = false;
			}
			return base.Update(hasFocus);
		}

		private void AddRow(MyTerminalBlock block)
		{
			MyGuiControlTable.Row row = new MyGuiControlTable.Row(block);
			MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(block.CustomName);
			row.AddCell(cell);
			m_selectedBlocks.Add(row);
		}

		private void OnPasteButtonClick(MyGuiControlButton sender)
		{
			foreach (MyTerminalBlock item in MyScenarioBuildingBlock.Clipboard)
			{
				int i;
				for (i = 0; i < m_selectedBlocks.RowsCount && m_selectedBlocks.GetRow(i).UserData != item; i++)
				{
				}
				if (i == m_selectedBlocks.RowsCount)
				{
					AddRow(item);
				}
			}
		}

		protected override void OnOkButtonClick(MyGuiControlButton sender)
		{
			trigger.Blocks.Clear();
			for (int i = 0; i < m_selectedBlocks.RowsCount; i++)
			{
				trigger.Blocks.Add((MyTerminalBlock)m_selectedBlocks.GetRow(i).UserData, MyTriggerBlockDestroyed.BlockState.Ok);
			}
			trigger.SingleMessage = m_textboxSingleMessage.Text;
			base.OnOkButtonClick(sender);
		}

		private void OnDeleteButtonClick(MyGuiControlButton sender)
		{
			m_selectedBlocks.RemoveSelectedRow();
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Delete))
			{
				m_selectedBlocks.RemoveSelectedRow();
			}
			base.HandleInput(receivedFocusInThisUpdate);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenTriggerBlockDestroyed";
		}
	}
}
