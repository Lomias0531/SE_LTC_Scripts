using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Text;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "Hand item shoot")]
	internal class MyGuiScreenDebugHandItemShoot : MyGuiScreenDebugHandItemBase
	{
		private Matrix m_storedShootLocation;

		private Matrix m_storedShootLocation3rd;

		private bool m_canUpdateValues = true;

		private float m_itemRotationX;

		private float m_itemRotationY;

		private float m_itemRotationZ;

		private float m_itemPositionX;

		private float m_itemPositionY;

		private float m_itemPositionZ;

		private MyGuiControlSlider m_itemRotationXSlider;

		private MyGuiControlSlider m_itemRotationYSlider;

		private MyGuiControlSlider m_itemRotationZSlider;

		private MyGuiControlSlider m_itemPositionXSlider;

		private MyGuiControlSlider m_itemPositionYSlider;

		private MyGuiControlSlider m_itemPositionZSlider;

		private float m_itemRotationX3rd;

		private float m_itemRotationY3rd;

		private float m_itemRotationZ3rd;

		private float m_itemPositionX3rd;

		private float m_itemPositionY3rd;

		private float m_itemPositionZ3rd;

		private MyGuiControlSlider m_itemRotationX3rdSlider;

		private MyGuiControlSlider m_itemRotationY3rdSlider;

		private MyGuiControlSlider m_itemRotationZ3rdSlider;

		private MyGuiControlSlider m_itemPositionX3rdSlider;

		private MyGuiControlSlider m_itemPositionY3rdSlider;

		private MyGuiControlSlider m_itemPositionZ3rdSlider;

		private MyGuiControlSlider m_itemMuzzlePositionXSlider;

		private MyGuiControlSlider m_itemMuzzlePositionYSlider;

		private MyGuiControlSlider m_itemMuzzlePositionZSlider;

		private MyGuiControlSlider m_blendSlider;

		private MyGuiControlSlider m_shootScatterXSlider;

		private MyGuiControlSlider m_shootScatterYSlider;

		private MyGuiControlSlider m_shootScatterZSlider;

		private MyGuiControlSlider m_scatterSpeedSlider;

		public MyGuiScreenDebugHandItemShoot()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Hand item shoot", Color.Yellow.ToVector4());
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
			m_itemRotationX3rdSlider = AddSlider("item rotation X 3rd", 0f, 0f, 360f);
			m_itemRotationX3rdSlider.ValueChanged = ItemChanged;
			m_itemRotationY3rdSlider = AddSlider("item rotation Y 3rd", 0f, 0f, 360f);
			m_itemRotationY3rdSlider.ValueChanged = ItemChanged;
			m_itemRotationZ3rdSlider = AddSlider("item rotation Z 3rd", 0f, 0f, 360f);
			m_itemRotationZ3rdSlider.ValueChanged = ItemChanged;
			m_itemPositionX3rdSlider = AddSlider("item position X 3rd", 0f, -1f, 1f);
			m_itemPositionX3rdSlider.ValueChanged = ItemChanged;
			m_itemPositionY3rdSlider = AddSlider("item position Y 3rd", 0f, -1f, 1f);
			m_itemPositionY3rdSlider.ValueChanged = ItemChanged;
			m_itemPositionZ3rdSlider = AddSlider("item position Z 3rd", 0f, -1f, 1f);
			m_itemPositionZ3rdSlider.ValueChanged = ItemChanged;
			m_itemMuzzlePositionXSlider = AddSlider("item muzzle X", 0f, -1f, 1f);
			m_itemMuzzlePositionXSlider.ValueChanged = ItemChanged;
			m_itemMuzzlePositionYSlider = AddSlider("item muzzle Y", 0f, -1f, 1f);
			m_itemMuzzlePositionYSlider.ValueChanged = ItemChanged;
			m_itemMuzzlePositionZSlider = AddSlider("item muzzle Z", 0f, -1f, 1f);
			m_itemMuzzlePositionZSlider.ValueChanged = ItemChanged;
			m_blendSlider = AddSlider("Shoot blend", 0f, 0f, 3f);
			m_blendSlider.ValueChanged = ItemChanged;
			m_shootScatterXSlider = AddSlider("Scatter X", 0f, 0f, 1f);
			m_shootScatterXSlider.ValueChanged = ItemChanged;
			m_shootScatterYSlider = AddSlider("Scatter Y", 0f, 0f, 1f);
			m_shootScatterYSlider.ValueChanged = ItemChanged;
			m_shootScatterZSlider = AddSlider("Scatter Z", 0f, 0f, 1f);
			m_shootScatterZSlider.ValueChanged = ItemChanged;
			m_scatterSpeedSlider = AddSlider("Scatter speed", 0f, 0f, 1f);
			m_scatterSpeedSlider.ValueChanged = ItemChanged;
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
			m_storedShootLocation = CurrentSelectedItem.ItemShootLocation;
			m_storedShootLocation3rd = CurrentSelectedItem.ItemShootLocation3rd;
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
			m_itemRotationX = 0f;
			m_itemRotationY = 0f;
			m_itemRotationZ = 0f;
			m_itemPositionX = m_storedShootLocation.Translation.X;
			m_itemPositionY = m_storedShootLocation.Translation.Y;
			m_itemPositionZ = m_storedShootLocation.Translation.Z;
			m_itemRotationX3rd = 0f;
			m_itemRotationY3rd = 0f;
			m_itemRotationZ3rd = 0f;
			m_itemPositionX3rd = m_storedShootLocation3rd.Translation.X;
			m_itemPositionY3rd = m_storedShootLocation3rd.Translation.Y;
			m_itemPositionZ3rd = m_storedShootLocation3rd.Translation.Z;
			m_canUpdateValues = false;
			m_itemRotationXSlider.Value = m_itemRotationX;
			m_itemRotationYSlider.Value = m_itemRotationY;
			m_itemRotationZSlider.Value = m_itemRotationZ;
			m_itemPositionXSlider.Value = m_itemPositionX;
			m_itemPositionYSlider.Value = m_itemPositionY;
			m_itemPositionZSlider.Value = m_itemPositionZ;
			m_itemRotationX3rdSlider.Value = m_itemRotationX3rd;
			m_itemRotationY3rdSlider.Value = m_itemRotationY3rd;
			m_itemRotationZ3rdSlider.Value = m_itemRotationZ3rd;
			m_itemPositionX3rdSlider.Value = m_itemPositionX3rd;
			m_itemPositionY3rdSlider.Value = m_itemPositionY3rd;
			m_itemPositionZ3rdSlider.Value = m_itemPositionZ3rd;
			m_itemMuzzlePositionXSlider.Value = CurrentSelectedItem.MuzzlePosition.X;
			m_itemMuzzlePositionYSlider.Value = CurrentSelectedItem.MuzzlePosition.Y;
			m_itemMuzzlePositionZSlider.Value = CurrentSelectedItem.MuzzlePosition.Z;
			m_shootScatterXSlider.Value = CurrentSelectedItem.ShootScatter.X;
			m_shootScatterYSlider.Value = CurrentSelectedItem.ShootScatter.Y;
			m_shootScatterZSlider.Value = CurrentSelectedItem.ShootScatter.Z;
			m_scatterSpeedSlider.Value = CurrentSelectedItem.ScatterSpeed;
			m_blendSlider.Value = CurrentSelectedItem.ShootBlend;
			m_canUpdateValues = true;
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
				CurrentSelectedItem.ItemShootLocation = m_storedShootLocation * Matrix.CreateRotationX(MathHelper.ToRadians(m_itemRotationX)) * Matrix.CreateRotationY(MathHelper.ToRadians(m_itemRotationY)) * Matrix.CreateRotationZ(MathHelper.ToRadians(m_itemRotationZ));
				CurrentSelectedItem.ItemShootLocation.Translation = new Vector3(m_itemPositionX, m_itemPositionY, m_itemPositionZ);
				m_itemRotationX3rd = m_itemRotationX3rdSlider.Value;
				m_itemRotationY3rd = m_itemRotationY3rdSlider.Value;
				m_itemRotationZ3rd = m_itemRotationZ3rdSlider.Value;
				m_itemPositionX3rd = m_itemPositionX3rdSlider.Value;
				m_itemPositionY3rd = m_itemPositionY3rdSlider.Value;
				m_itemPositionZ3rd = m_itemPositionZ3rdSlider.Value;
				CurrentSelectedItem.ItemShootLocation3rd = m_storedShootLocation3rd * Matrix.CreateRotationX(MathHelper.ToRadians(m_itemRotationX3rd)) * Matrix.CreateRotationY(MathHelper.ToRadians(m_itemRotationY3rd)) * Matrix.CreateRotationZ(MathHelper.ToRadians(m_itemRotationZ3rd));
				CurrentSelectedItem.ItemShootLocation3rd.Translation = new Vector3(m_itemPositionX3rd, m_itemPositionY3rd, m_itemPositionZ3rd);
				CurrentSelectedItem.ShootBlend = m_blendSlider.Value;
				CurrentSelectedItem.MuzzlePosition.X = m_itemMuzzlePositionXSlider.Value;
				CurrentSelectedItem.MuzzlePosition.Y = m_itemMuzzlePositionYSlider.Value;
				CurrentSelectedItem.MuzzlePosition.Z = m_itemMuzzlePositionZSlider.Value;
				CurrentSelectedItem.ShootScatter.X = m_shootScatterXSlider.Value;
				CurrentSelectedItem.ShootScatter.Y = m_shootScatterYSlider.Value;
				CurrentSelectedItem.ShootScatter.Z = m_shootScatterZSlider.Value;
				CurrentSelectedItem.ScatterSpeed = m_scatterSpeedSlider.Value;
			}
		}
	}
}
