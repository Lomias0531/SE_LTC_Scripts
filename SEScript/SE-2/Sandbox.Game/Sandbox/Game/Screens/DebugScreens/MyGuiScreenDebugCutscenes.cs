using Sandbox.Game.Gui;
using Sandbox.Game.GUI.DebugInputComponents;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Game", "Cutscenes")]
	internal class MyGuiScreenDebugCutscenes : MyGuiScreenDebugBase
	{
		private MyGuiControlCombobox m_comboCutscenes;

		private MyGuiControlCombobox m_comboNodes;

		private MyGuiControlCombobox m_comboWaypoints;

		private MyGuiControlButton m_playButton;

		private MyGuiControlSlider m_nodeTimeSlider;

		private MyGuiControlButton m_spawnButton;

		private MyGuiControlButton m_removeAllButton;

		private MyGuiControlButton m_addNodeButton;

		private MyGuiControlButton m_deleteNodeButton;

		private MyGuiControlButton m_addCutsceneButton;

		private MyGuiControlButton m_deleteCutsceneButton;

		private Cutscene m_selectedCutscene;

		private CutsceneSequenceNode m_selectedCutsceneNode;

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugCubeBlocks";
		}

		public MyGuiScreenDebugCutscenes()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Cutscenes", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_comboCutscenes = AddCombo();
			m_playButton = AddButton(new StringBuilder("Play"), onClick_PlayButton);
			m_addCutsceneButton = AddButton(new StringBuilder("Add cutscene"), onClick_AddCutsceneButton);
			m_deleteCutsceneButton = AddButton(new StringBuilder("Delete cutscene"), onClick_DeleteCutsceneButton);
			m_currentPosition.Y += 0.01f;
			AddLabel("Nodes", Color.Yellow.ToVector4(), 1f);
			m_comboNodes = AddCombo();
			m_comboNodes.ItemSelected += m_comboNodes_ItemSelected;
			m_addNodeButton = AddButton(new StringBuilder("Add node"), onClick_AddNodeButton);
			m_deleteNodeButton = AddButton(new StringBuilder("Delete node"), onClick_DeleteNodeButton);
			m_nodeTimeSlider = AddSlider("Node time", 0f, 0f, 100f, OnNodeTimeChanged);
			MySessionComponentCutscenes component = MySession.Static.GetComponent<MySessionComponentCutscenes>();
			m_comboCutscenes.ClearItems();
			foreach (string key in component.GetCutscenes().Keys)
			{
				m_comboCutscenes.AddItem(key.GetHashCode(), key);
			}
			m_comboCutscenes.SortItemsByValueText();
			m_comboCutscenes.ItemSelected += m_comboCutscenes_ItemSelected;
			AddLabel("Waypoints", Color.Yellow.ToVector4(), 1f);
			m_comboWaypoints = AddCombo();
			m_comboWaypoints.ItemSelected += m_comboWaypoints_ItemSelected;
			m_currentPosition.Y += 0.01f;
			m_spawnButton = AddButton(new StringBuilder("Spawn entity"), onSpawnButton);
			m_removeAllButton = AddButton(new StringBuilder("Remove all"), onRemoveAllButton);
			if (m_comboCutscenes.GetItemsCount() > 0)
			{
				m_comboCutscenes.SelectItemByIndex(0);
			}
		}

		private void m_comboCutscenes_ItemSelected()
		{
			m_selectedCutscene = MySession.Static.GetComponent<MySessionComponentCutscenes>().GetCutscene(m_comboCutscenes.GetSelectedValue().ToString());
			m_comboNodes.ClearItems();
			if (m_selectedCutscene.SequenceNodes != null)
			{
				int num = 0;
				foreach (CutsceneSequenceNode sequenceNode in m_selectedCutscene.SequenceNodes)
				{
					m_comboNodes.AddItem(num, sequenceNode.Time.ToString());
					num++;
				}
			}
			if (m_comboNodes.GetItemsCount() > 0)
			{
				m_comboNodes.SelectItemByIndex(0);
			}
		}

		private void m_comboNodes_ItemSelected()
		{
			m_selectedCutsceneNode = m_selectedCutscene.SequenceNodes[(int)m_comboNodes.GetSelectedKey()];
			m_nodeTimeSlider.Value = m_selectedCutsceneNode.Time;
			m_comboWaypoints.ClearItems();
			if (m_selectedCutsceneNode.Waypoints != null)
			{
				foreach (CutsceneSequenceNodeWaypoint waypoint in m_selectedCutsceneNode.Waypoints)
				{
					m_comboWaypoints.AddItem(waypoint.Name.GetHashCode(), waypoint.Name);
				}
				if (m_comboWaypoints.GetItemsCount() > 0)
				{
					m_comboWaypoints.SelectItemByIndex(0);
				}
			}
		}

		private void onClick_PlayButton(MyGuiControlButton sender)
		{
			if (m_comboCutscenes.GetItemsCount() > 0)
			{
				MySession.Static.GetComponent<MySessionComponentCutscenes>().PlayCutscene(m_comboCutscenes.GetSelectedValue().ToString());
			}
		}

		private void OnNodeTimeChanged(MyGuiControlSlider slider)
		{
			if (m_selectedCutsceneNode != null)
			{
				m_selectedCutsceneNode.Time = slider.Value;
			}
		}

		private void onSpawnButton(MyGuiControlButton sender)
		{
			MyVisualScriptingDebugInputComponent.SpawnEntity(onEntitySpawned);
		}

		private void onEntitySpawned(MyEntity entity)
		{
			if (m_selectedCutsceneNode != null)
			{
				m_selectedCutsceneNode.MoveTo = entity.Name;
				m_selectedCutsceneNode.RotateTowards = entity.Name;
			}
		}

		private void onClick_AddNodeButton(MyGuiControlButton sender)
		{
			List<CutsceneSequenceNode> list = new List<CutsceneSequenceNode>
			{
				new CutsceneSequenceNode()
			};
			if (m_selectedCutscene.SequenceNodes != null)
			{
				m_selectedCutscene.SequenceNodes = m_selectedCutscene.SequenceNodes.Union(list).ToList();
			}
			else
			{
				m_selectedCutscene.SequenceNodes = list;
			}
		}

		private void onClick_DeleteNodeButton(MyGuiControlButton sender)
		{
			if (m_selectedCutscene.SequenceNodes != null)
			{
				m_selectedCutscene.SequenceNodes = m_selectedCutscene.SequenceNodes.Where((CutsceneSequenceNode x) => x != m_selectedCutsceneNode).ToList();
			}
		}

		private void onRemoveAllButton(MyGuiControlButton sender)
		{
			MySession.Static.GetComponent<MySessionComponentCutscenes>().GetCutscenes().Clear();
		}

		private void m_comboWaypoints_ItemSelected()
		{
		}

		private void onClick_AddCutsceneButton(MyGuiControlButton sender)
		{
			MySessionComponentCutscenes component = MySession.Static.GetComponent<MySessionComponentCutscenes>();
			string text = "Cutscene" + component.GetCutscenes().Count;
			component.GetCutscenes().Add(text, new Cutscene());
			m_comboCutscenes.ClearItems();
			foreach (string key in component.GetCutscenes().Keys)
			{
				m_comboCutscenes.AddItem(key.GetHashCode(), key);
			}
			m_comboCutscenes.SelectItemByKey(text.GetHashCode());
		}

		private void onClick_DeleteCutsceneButton(MyGuiControlButton sender)
		{
			MySessionComponentCutscenes component = MySession.Static.GetComponent<MySessionComponentCutscenes>();
			if (m_selectedCutscene != null)
			{
				m_comboNodes.ClearItems();
				m_comboWaypoints.ClearItems();
				m_selectedCutsceneNode = null;
				component.GetCutscenes().Remove(m_selectedCutscene.Name);
				m_comboCutscenes.RemoveItem(m_selectedCutscene.Name.GetHashCode());
				if (component.GetCutscenes().Count == 0)
				{
					m_selectedCutscene = null;
				}
				else
				{
					m_comboCutscenes.SelectItemByIndex(component.GetCutscenes().Count - 1);
				}
			}
		}
	}
}
