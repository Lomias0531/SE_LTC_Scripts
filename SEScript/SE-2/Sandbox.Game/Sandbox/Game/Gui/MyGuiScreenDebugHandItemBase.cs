using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal abstract class MyGuiScreenDebugHandItemBase : MyGuiScreenDebugBase
	{
		private List<MyHandItemDefinition> m_handItemDefinitions = new List<MyHandItemDefinition>();

		private MyGuiControlCombobox m_handItemsCombo;

		protected MyHandItemDefinition CurrentSelectedItem;

		private MyCharacter m_playerCharacter;

		public void OnWeaponChanged(object sender, EventArgs e)
		{
			SelectFirstHandItem();
		}

		protected override void OnShow()
		{
			m_playerCharacter = MySession.Static.LocalCharacter;
			if (m_playerCharacter != null)
			{
				m_playerCharacter.OnWeaponChanged += OnWeaponChanged;
			}
			base.OnShow();
		}

		protected override void OnClosed()
		{
			if (m_playerCharacter != null)
			{
				m_playerCharacter.OnWeaponChanged -= OnWeaponChanged;
			}
			base.OnClosed();
		}

		protected void RecreateHandItemsCombo()
		{
			m_handItemsCombo = AddCombo();
			m_handItemDefinitions.Clear();
			foreach (MyHandItemDefinition handItemDefinition in MyDefinitionManager.Static.GetHandItemDefinitions())
			{
				MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(handItemDefinition.PhysicalItemId);
				int count = m_handItemDefinitions.Count;
				m_handItemDefinitions.Add(handItemDefinition);
				m_handItemsCombo.AddItem(count, definition.DisplayNameText);
			}
			m_handItemsCombo.SortItemsByValueText();
			m_handItemsCombo.ItemSelected += handItemsCombo_ItemSelected;
		}

		protected void RecreateSaveAndReloadButtons()
		{
			AddButton(new StringBuilder("Save"), OnSave);
			AddButton(new StringBuilder("Reload"), OnReload);
			AddButton(new StringBuilder("Transform"), OnTransform);
			AddButton(new StringBuilder("Transform All"), OnTransformAll);
		}

		protected void SelectFirstHandItem()
		{
			IMyGunObject<MyDeviceBase> currentWeapon = MySession.Static.LocalCharacter.CurrentWeapon;
			if (currentWeapon == null)
			{
				if (m_handItemsCombo.GetItemsCount() > 0)
				{
					m_handItemsCombo.SelectItemByIndex(0);
				}
			}
			else if (m_handItemsCombo.GetItemsCount() > 0)
			{
				try
				{
					if (currentWeapon.DefinitionId.TypeId != typeof(MyObjectBuilder_PhysicalGunObject))
					{
						MyDefinitionId physicalItemId = MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId).Id;
						int num = m_handItemDefinitions.FindIndex((MyHandItemDefinition x) => x.PhysicalItemId == physicalItemId);
						m_handItemsCombo.SelectItemByKey(num);
					}
					else
					{
						MyDefinitionBase def = MyDefinitionManager.Static.GetDefinition(currentWeapon.DefinitionId);
						int num2 = m_handItemDefinitions.FindIndex((MyHandItemDefinition x) => x.DisplayNameText == def.DisplayNameText);
						m_handItemsCombo.SelectItemByKey(num2);
					}
				}
				catch (Exception)
				{
					m_handItemsCombo.SelectItemByIndex(0);
				}
			}
		}

		protected virtual void handItemsCombo_ItemSelected()
		{
			CurrentSelectedItem = m_handItemDefinitions[(int)m_handItemsCombo.GetSelectedKey()];
		}

		private void OnSave(MyGuiControlButton button)
		{
			MyDefinitionManager.Static.SaveHandItems();
		}

		private void OnReload(MyGuiControlButton button)
		{
			MyDefinitionManager.Static.ReloadHandItems();
		}

		private void OnTransformAll(MyGuiControlButton button)
		{
			foreach (MyHandItemDefinition handItemDefinition in MyDefinitionManager.Static.GetHandItemDefinitions())
			{
				TransformItem(handItemDefinition);
			}
		}

		private void OnTransform(MyGuiControlButton button)
		{
			TransformItem(CurrentSelectedItem);
		}

		private void TransformItem(MyHandItemDefinition item)
		{
			Reorientate(ref item.LeftHand);
			Reorientate(ref item.RightHand);
		}

		private void Reorientate(ref Matrix m)
		{
			Matrix matrix = new MatrixD(-1.0, 0.0, 0.0, 0.0, 0.0, -1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0);
			Vector3 translation = m.Translation;
			m = matrix * m;
			m.Translation = translation;
		}

		private void Reorientate(ref Vector3 v)
		{
			v.X = 0f - v.X;
			v.Y = 0f - v.Y;
		}

		private void SwapYZ(ref Matrix m)
		{
			Vector3 translation = m.Translation;
			float y = m.Translation.Y;
			translation.Y = m.Translation.Z;
			translation.Z = y;
			m.Translation = translation;
		}

		private void SwapYZ(ref Vector3 v)
		{
			float y = v.Y;
			v.Y = v.Z;
			v.Z = y;
		}
	}
}
