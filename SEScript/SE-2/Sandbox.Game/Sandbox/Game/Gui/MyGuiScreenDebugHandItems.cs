using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "Hand items")]
	internal class MyGuiScreenDebugHandItems : MyGuiScreenDebugHandItemBase
	{
		private Matrix m_storedLeftHand;

		private Matrix m_storedRightHand;

		private Matrix m_storedItem;

		private bool m_canUpdateValues = true;

		private float m_leftHandRotationX;

		private float m_leftHandRotationY;

		private float m_leftHandRotationZ;

		private float m_leftHandPositionX;

		private float m_leftHandPositionY;

		private float m_leftHandPositionZ;

		private float m_rightHandRotationX;

		private float m_rightHandRotationY;

		private float m_rightHandRotationZ;

		private float m_rightHandPositionX;

		private float m_rightHandPositionY;

		private float m_rightHandPositionZ;

		private float m_itemRotationX;

		private float m_itemRotationY;

		private float m_itemRotationZ;

		private float m_itemPositionX;

		private float m_itemPositionY;

		private float m_itemPositionZ;

		private MyGuiControlSlider m_leftHandRotationXSlider;

		private MyGuiControlSlider m_leftHandRotationYSlider;

		private MyGuiControlSlider m_leftHandRotationZSlider;

		private MyGuiControlSlider m_leftHandPositionXSlider;

		private MyGuiControlSlider m_leftHandPositionYSlider;

		private MyGuiControlSlider m_leftHandPositionZSlider;

		private MyGuiControlSlider m_rightHandRotationXSlider;

		private MyGuiControlSlider m_rightHandRotationYSlider;

		private MyGuiControlSlider m_rightHandRotationZSlider;

		private MyGuiControlSlider m_rightHandPositionXSlider;

		private MyGuiControlSlider m_rightHandPositionYSlider;

		private MyGuiControlSlider m_rightHandPositionZSlider;

		private MyGuiControlSlider m_itemRotationXSlider;

		private MyGuiControlSlider m_itemRotationYSlider;

		private MyGuiControlSlider m_itemRotationZSlider;

		private MyGuiControlSlider m_itemPositionXSlider;

		private MyGuiControlSlider m_itemPositionYSlider;

		private MyGuiControlSlider m_itemPositionZSlider;

		public MyGuiScreenDebugHandItems()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Hand items properties", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			RecreateHandItemsCombo();
			m_sliderDebugScale = 0.6f;
			m_leftHandRotationXSlider = AddSlider("Left hand rotation X", 0f, 0f, 360f);
			m_leftHandRotationXSlider.ValueChanged = LeftHandChanged;
			m_leftHandRotationYSlider = AddSlider("Left hand rotation Y", 0f, 0f, 360f);
			m_leftHandRotationYSlider.ValueChanged = LeftHandChanged;
			m_leftHandRotationZSlider = AddSlider("Left hand rotation Z", 0f, 0f, 360f);
			m_leftHandRotationZSlider.ValueChanged = LeftHandChanged;
			m_leftHandPositionXSlider = AddSlider("Left hand position X", 0f, -1f, 1f);
			m_leftHandPositionXSlider.ValueChanged = LeftHandChanged;
			m_leftHandPositionYSlider = AddSlider("Left hand position Y", 0f, -1f, 1f);
			m_leftHandPositionYSlider.ValueChanged = LeftHandChanged;
			m_leftHandPositionZSlider = AddSlider("Left hand position Z", 0f, -1f, 1f);
			m_leftHandPositionZSlider.ValueChanged = LeftHandChanged;
			m_rightHandRotationXSlider = AddSlider("Right hand rotation X", 0f, 0f, 360f);
			m_rightHandRotationXSlider.ValueChanged = RightHandChanged;
			m_rightHandRotationYSlider = AddSlider("Right hand rotation Y", 0f, 0f, 360f);
			m_rightHandRotationYSlider.ValueChanged = RightHandChanged;
			m_rightHandRotationZSlider = AddSlider("Right hand rotation Z", 0f, 0f, 360f);
			m_rightHandRotationZSlider.ValueChanged = RightHandChanged;
			m_rightHandPositionXSlider = AddSlider("Right hand position X", 0f, -1f, 1f);
			m_rightHandPositionXSlider.ValueChanged = RightHandChanged;
			m_rightHandPositionYSlider = AddSlider("Right hand position Y", 0f, -1f, 1f);
			m_rightHandPositionYSlider.ValueChanged = RightHandChanged;
			m_rightHandPositionZSlider = AddSlider("Right hand position Z", 0f, -1f, 1f);
			m_rightHandPositionZSlider.ValueChanged = RightHandChanged;
			m_itemRotationXSlider = AddSlider("Item rotation X", 0f, 0f, 360f);
			m_itemRotationXSlider.ValueChanged = ItemChanged;
			m_itemRotationYSlider = AddSlider("Item rotation Y", 0f, 0f, 360f);
			m_itemRotationYSlider.ValueChanged = ItemChanged;
			m_itemRotationZSlider = AddSlider("Item rotation Z", 0f, 0f, 360f);
			m_itemRotationZSlider.ValueChanged = ItemChanged;
			m_itemPositionXSlider = AddSlider("Item position X", 0f, -1f, 1f);
			m_itemPositionXSlider.ValueChanged = ItemChanged;
			m_itemPositionYSlider = AddSlider("Item position Y", 0f, -1f, 1f);
			m_itemPositionYSlider.ValueChanged = ItemChanged;
			m_itemPositionZSlider = AddSlider("Item position Z", 0f, -1f, 1f);
			m_itemPositionZSlider.ValueChanged = ItemChanged;
			RecreateSaveAndReloadButtons();
			SelectFirstHandItem();
			m_currentPosition.Y += 0.01f;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugHandItems";
		}

		protected override void handItemsCombo_ItemSelected()
		{
			base.handItemsCombo_ItemSelected();
			m_storedLeftHand = CurrentSelectedItem.LeftHand;
			m_storedRightHand = CurrentSelectedItem.RightHand;
			m_storedItem = CurrentSelectedItem.ItemLocation;
			UpdateValues();
		}

		private void UpdateValues()
		{
			m_leftHandRotationX = 0f;
			m_leftHandRotationY = 0f;
			m_leftHandRotationZ = 0f;
			m_leftHandPositionX = m_storedLeftHand.Translation.X;
			m_leftHandPositionY = m_storedLeftHand.Translation.Y;
			m_leftHandPositionZ = m_storedLeftHand.Translation.Z;
			m_rightHandRotationX = 0f;
			m_rightHandRotationY = 0f;
			m_rightHandRotationZ = 0f;
			m_rightHandPositionX = m_storedRightHand.Translation.X;
			m_rightHandPositionY = m_storedRightHand.Translation.Y;
			m_rightHandPositionZ = m_storedRightHand.Translation.Z;
			m_itemRotationX = 0f;
			m_itemRotationY = 0f;
			m_itemRotationZ = 0f;
			m_itemPositionX = m_storedItem.Translation.X;
			m_itemPositionY = m_storedItem.Translation.Y;
			m_itemPositionZ = m_storedItem.Translation.Z;
			m_canUpdateValues = false;
			m_leftHandRotationXSlider.Value = m_leftHandRotationX;
			m_leftHandRotationYSlider.Value = m_leftHandRotationY;
			m_leftHandRotationZSlider.Value = m_leftHandRotationZ;
			m_leftHandPositionXSlider.Value = m_leftHandPositionX;
			m_leftHandPositionYSlider.Value = m_leftHandPositionY;
			m_leftHandPositionZSlider.Value = m_leftHandPositionZ;
			m_rightHandRotationXSlider.Value = m_rightHandRotationX;
			m_rightHandRotationYSlider.Value = m_rightHandRotationY;
			m_rightHandRotationZSlider.Value = m_rightHandRotationZ;
			m_rightHandPositionXSlider.Value = m_rightHandPositionX;
			m_rightHandPositionYSlider.Value = m_rightHandPositionY;
			m_rightHandPositionZSlider.Value = m_rightHandPositionZ;
			m_itemRotationXSlider.Value = m_itemRotationX;
			m_itemRotationYSlider.Value = m_itemRotationY;
			m_itemRotationZSlider.Value = m_itemRotationZ;
			m_itemPositionXSlider.Value = m_itemPositionX;
			m_itemPositionYSlider.Value = m_itemPositionY;
			m_itemPositionZSlider.Value = m_itemPositionZ;
			m_canUpdateValues = true;
		}

		private void LeftHandChanged(MyGuiControlSlider slider)
		{
			if (m_canUpdateValues)
			{
				m_leftHandRotationX = m_leftHandRotationXSlider.Value;
				m_leftHandRotationY = m_leftHandRotationYSlider.Value;
				m_leftHandRotationZ = m_leftHandRotationZSlider.Value;
				m_leftHandPositionX = m_leftHandPositionXSlider.Value;
				m_leftHandPositionY = m_leftHandPositionYSlider.Value;
				m_leftHandPositionZ = m_leftHandPositionZSlider.Value;
				CurrentSelectedItem.LeftHand = m_storedLeftHand * Matrix.CreateRotationX(MathHelper.ToRadians(m_leftHandRotationX)) * Matrix.CreateRotationY(MathHelper.ToRadians(m_leftHandRotationY)) * Matrix.CreateRotationZ(MathHelper.ToRadians(m_leftHandRotationZ));
				CurrentSelectedItem.LeftHand.Translation = new Vector3(m_leftHandPositionX, m_leftHandPositionY, m_leftHandPositionZ);
			}
		}

		private void RightHandChanged(MyGuiControlSlider slider)
		{
			if (m_canUpdateValues)
			{
				m_rightHandRotationX = m_rightHandRotationXSlider.Value;
				m_rightHandRotationY = m_rightHandRotationYSlider.Value;
				m_rightHandRotationZ = m_rightHandRotationZSlider.Value;
				m_rightHandPositionX = m_rightHandPositionXSlider.Value;
				m_rightHandPositionY = m_rightHandPositionYSlider.Value;
				m_rightHandPositionZ = m_rightHandPositionZSlider.Value;
				CurrentSelectedItem.RightHand = m_storedRightHand * Matrix.CreateRotationX(MathHelper.ToRadians(m_rightHandRotationX)) * Matrix.CreateRotationY(MathHelper.ToRadians(m_rightHandRotationY)) * Matrix.CreateRotationZ(MathHelper.ToRadians(m_rightHandRotationZ));
				CurrentSelectedItem.RightHand.Translation = new Vector3(m_rightHandPositionX, m_rightHandPositionY, m_rightHandPositionZ);
			}
		}

		private void ItemChanged(MyGuiControlSlider slider)
		{
			if (m_canUpdateValues)
			{
				m_itemRotationX = m_itemRotationXSlider.Value;
				m_itemRotationY = m_itemRotationYSlider.Value;
				m_itemRotationZ = m_itemRotationZSlider.Value;
				m_itemPositionX = m_itemPositionXSlider.Value;
				m_itemPositionY = m_itemPositionYSlider.Value;
				m_itemPositionZ = m_itemPositionZSlider.Value;
				CurrentSelectedItem.ItemLocation = m_storedItem * Matrix.CreateRotationX(MathHelper.ToRadians(m_itemRotationX)) * Matrix.CreateRotationY(MathHelper.ToRadians(m_itemRotationY)) * Matrix.CreateRotationZ(MathHelper.ToRadians(m_itemRotationZ));
				CurrentSelectedItem.ItemLocation.Translation = new Vector3(m_itemPositionX, m_itemPositionY, m_itemPositionZ);
			}
		}
	}
}
