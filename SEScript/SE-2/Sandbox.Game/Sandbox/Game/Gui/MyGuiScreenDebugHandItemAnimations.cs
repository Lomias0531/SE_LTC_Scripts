using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Text;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "Hand item animations")]
	internal class MyGuiScreenDebugHandItemAnimations : MyGuiScreenDebugHandItemBase
	{
		private Matrix m_storedWalkingItem;

		private bool m_canUpdateValues = true;

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

		private MyGuiControlSlider m_blendTimeSlider;

		private MyGuiControlSlider m_xAmplitudeOffsetSlider;

		private MyGuiControlSlider m_yAmplitudeOffsetSlider;

		private MyGuiControlSlider m_zAmplitudeOffsetSlider;

		private MyGuiControlSlider m_xAmplitudeScaleSlider;

		private MyGuiControlSlider m_yAmplitudeScaleSlider;

		private MyGuiControlSlider m_zAmplitudeScaleSlider;

		private MyGuiControlSlider m_runMultiplierSlider;

		private MyGuiControlCheckbox m_simulateLeftHandCheckbox;

		private MyGuiControlCheckbox m_simulateRightHandCheckbox;

		public MyGuiScreenDebugHandItemAnimations()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Hand item animations", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			RecreateHandItemsCombo();
			m_sliderDebugScale = 0.6f;
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
			m_blendTimeSlider = AddSlider("Blend time", 0f, 0.001f, 1f);
			m_blendTimeSlider.ValueChanged = AmplitudeChanged;
			m_xAmplitudeOffsetSlider = AddSlider("X offset amplitude", 0f, -5f, 5f);
			m_xAmplitudeOffsetSlider.ValueChanged = AmplitudeChanged;
			m_yAmplitudeOffsetSlider = AddSlider("Y offset amplitude", 0f, -5f, 5f);
			m_yAmplitudeOffsetSlider.ValueChanged = AmplitudeChanged;
			m_zAmplitudeOffsetSlider = AddSlider("Z offset amplitude", 0f, -5f, 5f);
			m_zAmplitudeOffsetSlider.ValueChanged = AmplitudeChanged;
			m_xAmplitudeScaleSlider = AddSlider("X scale amplitude", 0f, -5f, 5f);
			m_xAmplitudeScaleSlider.ValueChanged = AmplitudeChanged;
			m_yAmplitudeScaleSlider = AddSlider("Y scale amplitude", 0f, -5f, 5f);
			m_yAmplitudeScaleSlider.ValueChanged = AmplitudeChanged;
			m_zAmplitudeScaleSlider = AddSlider("Z scale amplitude", 0f, -5f, 5f);
			m_zAmplitudeScaleSlider.ValueChanged = AmplitudeChanged;
			m_runMultiplierSlider = AddSlider("Run multiplier", 0f, -5f, 5f);
			m_runMultiplierSlider.ValueChanged = AmplitudeChanged;
			m_simulateLeftHandCheckbox = AddCheckBox("Simulate left hand", checkedState: false, SimulateHandChanged);
			m_simulateRightHandCheckbox = AddCheckBox("Simulate right hand", checkedState: false, SimulateHandChanged);
			AddButton(new StringBuilder("Walk!"), OnWalk);
			AddButton(new StringBuilder("Run!"), OnRun);
			RecreateSaveAndReloadButtons();
			SelectFirstHandItem();
			m_currentPosition.Y += 0.01f;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugHandItemsAnimations";
		}

		protected override void handItemsCombo_ItemSelected()
		{
			base.handItemsCombo_ItemSelected();
			m_storedWalkingItem = CurrentSelectedItem.ItemWalkingLocation;
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
			m_canUpdateValues = false;
			m_itemWalkingRotationXSlider.Value = m_itemWalkingRotationX;
			m_itemWalkingRotationYSlider.Value = m_itemWalkingRotationY;
			m_itemWalkingRotationZSlider.Value = m_itemWalkingRotationZ;
			m_itemWalkingPositionXSlider.Value = m_itemWalkingPositionX;
			m_itemWalkingPositionYSlider.Value = m_itemWalkingPositionY;
			m_itemWalkingPositionZSlider.Value = m_itemWalkingPositionZ;
			m_blendTimeSlider.Value = CurrentSelectedItem.BlendTime;
			m_xAmplitudeOffsetSlider.Value = CurrentSelectedItem.XAmplitudeOffset;
			m_yAmplitudeOffsetSlider.Value = CurrentSelectedItem.YAmplitudeOffset;
			m_zAmplitudeOffsetSlider.Value = CurrentSelectedItem.ZAmplitudeOffset;
			m_xAmplitudeScaleSlider.Value = CurrentSelectedItem.XAmplitudeScale;
			m_yAmplitudeScaleSlider.Value = CurrentSelectedItem.YAmplitudeScale;
			m_zAmplitudeScaleSlider.Value = CurrentSelectedItem.ZAmplitudeScale;
			m_runMultiplierSlider.Value = CurrentSelectedItem.RunMultiplier;
			m_simulateLeftHandCheckbox.IsChecked = CurrentSelectedItem.SimulateLeftHand;
			m_simulateRightHandCheckbox.IsChecked = CurrentSelectedItem.SimulateRightHand;
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
				CurrentSelectedItem.ItemWalkingLocation = m_storedWalkingItem * Matrix.CreateRotationX(MathHelper.ToRadians(m_itemWalkingRotationX)) * Matrix.CreateRotationY(MathHelper.ToRadians(m_itemWalkingRotationY)) * Matrix.CreateRotationZ(MathHelper.ToRadians(m_itemWalkingRotationZ));
				CurrentSelectedItem.ItemWalkingLocation.Translation = new Vector3(m_itemWalkingPositionX, m_itemWalkingPositionY, m_itemWalkingPositionZ);
			}
		}

		private void AmplitudeChanged(MyGuiControlSlider slider)
		{
			if (m_canUpdateValues)
			{
				CurrentSelectedItem.BlendTime = m_blendTimeSlider.Value;
				CurrentSelectedItem.XAmplitudeOffset = m_xAmplitudeOffsetSlider.Value;
				CurrentSelectedItem.YAmplitudeOffset = m_yAmplitudeOffsetSlider.Value;
				CurrentSelectedItem.ZAmplitudeOffset = m_zAmplitudeOffsetSlider.Value;
				CurrentSelectedItem.XAmplitudeScale = m_xAmplitudeScaleSlider.Value;
				CurrentSelectedItem.YAmplitudeScale = m_yAmplitudeScaleSlider.Value;
				CurrentSelectedItem.ZAmplitudeScale = m_zAmplitudeScaleSlider.Value;
				CurrentSelectedItem.RunMultiplier = m_runMultiplierSlider.Value;
			}
		}

		private void SimulateHandChanged(MyGuiControlCheckbox checkbox)
		{
			if (m_canUpdateValues)
			{
				CurrentSelectedItem.SimulateLeftHand = m_simulateLeftHandCheckbox.IsChecked;
				CurrentSelectedItem.SimulateRightHand = m_simulateRightHandCheckbox.IsChecked;
			}
		}
	}
}
