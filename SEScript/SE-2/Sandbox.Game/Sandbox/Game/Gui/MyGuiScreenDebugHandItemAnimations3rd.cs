using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Text;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "Hand item animations 3rd")]
	internal class MyGuiScreenDebugHandItemAnimations3rd : MyGuiScreenDebugHandItemBase
	{
		private Matrix m_storedItem;

		private Matrix m_storedWalkingItem;

		private bool m_canUpdateValues = true;

		private float m_itemRotationX;

		private float m_itemRotationY;

		private float m_itemRotationZ;

		private float m_itemPositionX;

		private float m_itemPositionY;

		private float m_itemPositionZ;

		private float m_itemWalkingRotationX;

		private float m_itemWalkingRotationY;

		private float m_itemWalkingRotationZ;

		private float m_itemWalkingPositionX;

		private float m_itemWalkingPositionY;

		private float m_itemWalkingPositionZ;

		private MyGuiControlSlider m_itemWalkingRotationXSlider;

		private MyGuiControlSlider m_itemWalkingRotationYSlider;

		private MyGuiControlSlider m_itemWalkingRotationZSlider;

		private MyGuiControlSlider m_itemWalkingPositionXSlider;

		private MyGuiControlSlider m_itemWalkingPositionYSlider;

		private MyGuiControlSlider m_itemWalkingPositionZSlider;

		private MyGuiControlSlider m_itemRotationXSlider;

		private MyGuiControlSlider m_itemRotationYSlider;

		private MyGuiControlSlider m_itemRotationZSlider;

		private MyGuiControlSlider m_itemPositionXSlider;

		private MyGuiControlSlider m_itemPositionYSlider;

		private MyGuiControlSlider m_itemPositionZSlider;

		private MyGuiControlSlider m_amplitudeMultiplierSlider;

		public MyGuiScreenDebugHandItemAnimations3rd()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Hand item animations 3rd", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			RecreateHandItemsCombo();
			m_sliderDebugScale = 0.6f;
			m_itemRotationXSlider = AddSlider("item rotation X", 0f, 0f, 360f);
			m_itemRotationXSlider.ValueChanged = ItemChanged;
			m_itemRotationYSlider = AddSlider("item rotation Y", 0f, 0f, 360f);
			m_itemRotationYSlider.ValueChanged = ItemChanged;
			m_itemRotationZSlider = AddSlider("item rotation Z", 0f, 0f, 360f);
			m_itemRotationZSlider.ValueChanged = ItemChanged;
			m_itemPositionXSlider = AddSlider("item position X", 0f, -1f, 1f);
			m_itemPositionXSlider.ValueChanged = ItemChanged;
			m_itemPositionYSlider = AddSlider("item position Y", 0f, -1f, 1f);
			m_itemPositionYSlider.ValueChanged = ItemChanged;
			m_itemPositionZSlider = AddSlider("item position Z", 0f, -1f, 1f);
			m_itemPositionZSlider.ValueChanged = ItemChanged;
			m_itemWalkingRotationXSlider = AddSlider("Walk item rotation X", 0f, 0f, 360f);
			m_itemWalkingRotationXSlider.ValueChanged = WalkingItemChanged;
			m_itemWalkingRotationYSlider = AddSlider("Walk item rotation Y", 0f, 0f, 360f);
			m_itemWalkingRotationYSlider.ValueChanged = WalkingItemChanged;
			m_itemWalkingRotationZSlider = AddSlider("Walk item rotation Z", 0f, 0f, 360f);
			m_itemWalkingRotationZSlider.ValueChanged = WalkingItemChanged;
			m_itemWalkingPositionXSlider = AddSlider("Walk item position X", 0f, -1f, 1f);
			m_itemWalkingPositionXSlider.ValueChanged = WalkingItemChanged;
			m_itemWalkingPositionYSlider = AddSlider("Walk item position Y", 0f, -1f, 1f);
			m_itemWalkingPositionYSlider.ValueChanged = WalkingItemChanged;
			m_itemWalkingPositionZSlider = AddSlider("Walk item position Z", 0f, -1f, 1f);
			m_itemWalkingPositionZSlider.ValueChanged = WalkingItemChanged;
			m_amplitudeMultiplierSlider = AddSlider("Amplitude multiplier", 0f, -1f, 3f);
			m_amplitudeMultiplierSlider.ValueChanged = WalkingItemChanged;
			AddButton(new StringBuilder("Walk!"), OnWalk);
			AddButton(new StringBuilder("Run!"), OnRun);
			RecreateSaveAndReloadButtons();
			SelectFirstHandItem();
			m_currentPosition.Y += 0.01f;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugHandItemsAnimations3rd";
		}

		protected override void handItemsCombo_ItemSelected()
		{
			base.handItemsCombo_ItemSelected();
			m_storedWalkingItem = CurrentSelectedItem.ItemWalkingLocation3rd;
			m_storedItem = CurrentSelectedItem.ItemLocation3rd;
			UpdateValues();
		}

		private void OnWalk(MyGuiControlButton button)
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			localCharacter.SwitchAnimation(MyCharacterMovementEnum.Walking);
			localCharacter.SetCurrentMovementState(MyCharacterMovementEnum.Walking);
		}

		private void OnRun(MyGuiControlButton button)
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			localCharacter.SwitchAnimation(MyCharacterMovementEnum.Sprinting);
			localCharacter.SetCurrentMovementState(MyCharacterMovementEnum.Sprinting);
		}

		private void UpdateValues()
		{
			m_itemWalkingRotationX = 0f;
			m_itemWalkingRotationY = 0f;
			m_itemWalkingRotationZ = 0f;
			m_itemWalkingPositionX = m_storedWalkingItem.Translation.X;
			m_itemWalkingPositionY = m_storedWalkingItem.Translation.Y;
			m_itemWalkingPositionZ = m_storedWalkingItem.Translation.Z;
			m_itemRotationX = 0f;
			m_itemRotationY = 0f;
			m_itemRotationZ = 0f;
			m_itemPositionX = m_storedItem.Translation.X;
			m_itemPositionY = m_storedItem.Translation.Y;
			m_itemPositionZ = m_storedItem.Translation.Z;
			m_canUpdateValues = false;
			m_itemWalkingRotationXSlider.Value = m_itemWalkingRotationX;
			m_itemWalkingRotationYSlider.Value = m_itemWalkingRotationY;
			m_itemWalkingRotationZSlider.Value = m_itemWalkingRotationZ;
			m_itemWalkingPositionXSlider.Value = m_itemWalkingPositionX;
			m_itemWalkingPositionYSlider.Value = m_itemWalkingPositionY;
			m_itemWalkingPositionZSlider.Value = m_itemWalkingPositionZ;
			m_itemRotationXSlider.Value = m_itemRotationX;
			m_itemRotationYSlider.Value = m_itemRotationY;
			m_itemRotationZSlider.Value = m_itemRotationZ;
			m_itemPositionXSlider.Value = m_itemPositionX;
			m_itemPositionYSlider.Value = m_itemPositionY;
			m_itemPositionZSlider.Value = m_itemPositionZ;
			m_amplitudeMultiplierSlider.Value = CurrentSelectedItem.AmplitudeMultiplier3rd;
			m_canUpdateValues = true;
		}

		private void WalkingItemChanged(MyGuiControlSlider slider)
		{
			if (m_canUpdateValues)
			{
				m_itemWalkingRotationX = m_itemWalkingRotationXSlider.Value;
				m_itemWalkingRotationY = m_itemWalkingRotationYSlider.Value;
				m_itemWalkingRotationZ = m_itemWalkingRotationZSlider.Value;
				m_itemWalkingPositionX = m_itemWalkingPositionXSlider.Value;
				m_itemWalkingPositionY = m_itemWalkingPositionYSlider.Value;
				m_itemWalkingPositionZ = m_itemWalkingPositionZSlider.Value;
				CurrentSelectedItem.ItemWalkingLocation3rd = m_storedWalkingItem * Matrix.CreateRotationX(MathHelper.ToRadians(m_itemWalkingRotationX)) * Matrix.CreateRotationY(MathHelper.ToRadians(m_itemWalkingRotationY)) * Matrix.CreateRotationZ(MathHelper.ToRadians(m_itemWalkingRotationZ));
				CurrentSelectedItem.ItemWalkingLocation3rd.Translation = new Vector3(m_itemWalkingPositionX, m_itemWalkingPositionY, m_itemWalkingPositionZ);
				CurrentSelectedItem.AmplitudeMultiplier3rd = m_amplitudeMultiplierSlider.Value;
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
				CurrentSelectedItem.ItemLocation3rd = m_storedItem * Matrix.CreateRotationX(MathHelper.ToRadians(m_itemRotationX)) * Matrix.CreateRotationY(MathHelper.ToRadians(m_itemRotationY)) * Matrix.CreateRotationZ(MathHelper.ToRadians(m_itemRotationZ));
				CurrentSelectedItem.ItemLocation3rd.Translation = new Vector3(m_itemPositionX, m_itemPositionY, m_itemPositionZ);
			}
		}
	}
}
