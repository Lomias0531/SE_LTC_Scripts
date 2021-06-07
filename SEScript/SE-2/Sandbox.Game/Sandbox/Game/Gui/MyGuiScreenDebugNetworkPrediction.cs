using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Replication.History;
using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("VRage", "Network Prediction")]
	internal class MyGuiScreenDebugNetworkPrediction : MyGuiScreenDebugBase
	{
		private MyGuiControlCombobox m_animationComboA;

		private MyGuiControlCombobox m_animationComboB;

		private MyGuiControlSlider m_blendSlider;

		private MyGuiControlCombobox m_animationCombo;

		private MyGuiControlCheckbox m_loopCheckbox;

		private const float m_forcedPriority = 1f;

		public MyGuiScreenDebugNetworkPrediction()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("Network Prediction", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			if (MyMultiplayer.Static != null)
			{
				AddCheckBox("Apply Corrections", MyPredictedSnapshotSync.POSITION_CORRECTION && MyPredictedSnapshotSync.SMOOTH_POSITION_CORRECTION && MyPredictedSnapshotSync.LINEAR_VELOCITY_CORRECTION && MyPredictedSnapshotSync.SMOOTH_LINEAR_VELOCITY_CORRECTION && MyPredictedSnapshotSync.ROTATION_CORRECTION && MyPredictedSnapshotSync.SMOOTH_ROTATION_CORRECTION && MyPredictedSnapshotSync.ANGULAR_VELOCITY_CORRECTION && MyPredictedSnapshotSync.SMOOTH_ANGULAR_VELOCITY_CORRECTION, delegate(MyGuiControlCheckbox x)
				{
					MyPredictedSnapshotSync.POSITION_CORRECTION = x.IsChecked;
					MyPredictedSnapshotSync.SMOOTH_POSITION_CORRECTION = x.IsChecked;
					MyPredictedSnapshotSync.LINEAR_VELOCITY_CORRECTION = x.IsChecked;
					MyPredictedSnapshotSync.SMOOTH_LINEAR_VELOCITY_CORRECTION = x.IsChecked;
					MyPredictedSnapshotSync.ROTATION_CORRECTION = x.IsChecked;
					MyPredictedSnapshotSync.SMOOTH_ROTATION_CORRECTION = x.IsChecked;
					MyPredictedSnapshotSync.ANGULAR_VELOCITY_CORRECTION = x.IsChecked;
					MyPredictedSnapshotSync.SMOOTH_ANGULAR_VELOCITY_CORRECTION = x.IsChecked;
				});
				AddCheckBox("Apply Trend Reset", MyPredictedSnapshotSync.ApplyTrend, delegate(MyGuiControlCheckbox x)
				{
					MyPredictedSnapshotSync.ApplyTrend = x.IsChecked;
				});
				AddCheckBox("Force animated", MyPredictedSnapshotSync.ForceAnimated, delegate(MyGuiControlCheckbox x)
				{
					MyPredictedSnapshotSync.ForceAnimated = x.IsChecked;
				});
				AddSlider("Velocity change to reset", MyPredictedSnapshotSync.MIN_VELOCITY_CHANGE_TO_RESET, 0f, 30f, delegate(MyGuiControlSlider slider)
				{
					MyPredictedSnapshotSync.MIN_VELOCITY_CHANGE_TO_RESET = slider.Value;
				});
				AddSlider("Delta factor", MyPredictedSnapshotSync.DELTA_FACTOR, 0f, 1f, delegate(MyGuiControlSlider slider)
				{
					MyPredictedSnapshotSync.DELTA_FACTOR = slider.Value;
				});
				AddSlider("Smooth iterations", MyPredictedSnapshotSync.SMOOTH_ITERATIONS, 0f, 1000f, delegate(MyGuiControlSlider slider)
				{
					MyPredictedSnapshotSync.SMOOTH_ITERATIONS = (int)slider.Value;
				});
				AddCheckBox("Apply Reset", MySnapshot.ApplyReset, delegate(MyGuiControlCheckbox x)
				{
					MySnapshot.ApplyReset = x.IsChecked;
				});
				AddSlider("Smooth distance", MyPredictedSnapshotSync.SMOOTH_DISTANCE, 0f, 1000f, delegate(MyGuiControlSlider slider)
				{
					MyPredictedSnapshotSync.SMOOTH_DISTANCE = (int)slider.Value;
				});
				AddCheckBox("Propagate To Connections", MySnapshotCache.PROPAGATE_TO_CONNECTIONS, delegate(MyGuiControlCheckbox x)
				{
					MySnapshotCache.PROPAGATE_TO_CONNECTIONS = x.IsChecked;
				});
				m_currentPosition.Y += 0.01f;
				AddCheckBox("Position corrections", MyPredictedSnapshotSync.POSITION_CORRECTION, delegate(MyGuiControlCheckbox x)
				{
					MyPredictedSnapshotSync.POSITION_CORRECTION = x.IsChecked;
				});
				AddCheckBox("Smooth position corrections", MyPredictedSnapshotSync.SMOOTH_POSITION_CORRECTION, delegate(MyGuiControlCheckbox x)
				{
					MyPredictedSnapshotSync.SMOOTH_POSITION_CORRECTION = x.IsChecked;
				});
				AddSlider("Minimum pos delta", MyPredictedSnapshotSync.MIN_POSITION_DELTA, 0f, 0.5f, delegate(MyGuiControlSlider slider)
				{
					MyPredictedSnapshotSync.MIN_POSITION_DELTA = slider.Value;
				});
				AddSlider("Maximum pos delta", MyPredictedSnapshotSync.MAX_POSITION_DELTA, 0f, 5f, delegate(MyGuiControlSlider slider)
				{
					MyPredictedSnapshotSync.MAX_POSITION_DELTA = slider.Value;
				});
				m_currentPosition.Y += 0.01f;
				AddCheckBox("Linear velocity corrections", MyPredictedSnapshotSync.LINEAR_VELOCITY_CORRECTION, delegate(MyGuiControlCheckbox x)
				{
					MyPredictedSnapshotSync.LINEAR_VELOCITY_CORRECTION = x.IsChecked;
				});
				AddCheckBox("Smooth linear velocity corrections", MyPredictedSnapshotSync.SMOOTH_LINEAR_VELOCITY_CORRECTION, delegate(MyGuiControlCheckbox x)
				{
					MyPredictedSnapshotSync.SMOOTH_LINEAR_VELOCITY_CORRECTION = x.IsChecked;
				});
				AddSlider("Minimum linVel delta", MyPredictedSnapshotSync.MIN_LINEAR_VELOCITY_DELTA, 0f, 0.5f, delegate(MyGuiControlSlider slider)
				{
					MyPredictedSnapshotSync.MIN_LINEAR_VELOCITY_DELTA = slider.Value;
				});
				AddSlider("Maximum linVel delta", MyPredictedSnapshotSync.MAX_LINEAR_VELOCITY_DELTA, 0f, 5f, delegate(MyGuiControlSlider slider)
				{
					MyPredictedSnapshotSync.MAX_LINEAR_VELOCITY_DELTA = slider.Value;
				});
				m_currentPosition.Y += 0.01f;
				AddCheckBox("Rotation corrections", MyPredictedSnapshotSync.ROTATION_CORRECTION, delegate(MyGuiControlCheckbox x)
				{
					MyPredictedSnapshotSync.ROTATION_CORRECTION = x.IsChecked;
				});
				AddCheckBox("Smooth rotation corrections", MyPredictedSnapshotSync.SMOOTH_ROTATION_CORRECTION, delegate(MyGuiControlCheckbox x)
				{
					MyPredictedSnapshotSync.SMOOTH_ROTATION_CORRECTION = x.IsChecked;
				});
				AddSlider("Minimum angle delta", MathHelper.ToDegrees(MyPredictedSnapshotSync.MIN_ROTATION_ANGLE), 0f, 90f, delegate(MyGuiControlSlider slider)
				{
					MyPredictedSnapshotSync.MIN_ROTATION_ANGLE = MathHelper.ToRadians(slider.Value);
				});
				AddSlider("Maximum angle delta", MathHelper.ToDegrees(MyPredictedSnapshotSync.MAX_ROTATION_ANGLE), 0f, 90f, delegate(MyGuiControlSlider slider)
				{
					MyPredictedSnapshotSync.MAX_ROTATION_ANGLE = MathHelper.ToRadians(slider.Value);
				});
				m_currentPosition.Y += 0.01f;
				AddCheckBox("Angular velocity corrections", MyPredictedSnapshotSync.ANGULAR_VELOCITY_CORRECTION, delegate(MyGuiControlCheckbox x)
				{
					MyPredictedSnapshotSync.ANGULAR_VELOCITY_CORRECTION = x.IsChecked;
				});
				AddCheckBox("Smooth angular velocity corrections", MyPredictedSnapshotSync.SMOOTH_ANGULAR_VELOCITY_CORRECTION, delegate(MyGuiControlCheckbox x)
				{
					MyPredictedSnapshotSync.SMOOTH_ANGULAR_VELOCITY_CORRECTION = x.IsChecked;
				});
				AddSlider("Minimum angle delta", MyPredictedSnapshotSync.MIN_ANGULAR_VELOCITY_DELTA, 0f, 1f, delegate(MyGuiControlSlider slider)
				{
					MyPredictedSnapshotSync.MIN_ANGULAR_VELOCITY_DELTA = slider.Value;
				});
				AddSlider("Maximum angle delta", MyPredictedSnapshotSync.MAX_ANGULAR_VELOCITY_DELTA, 0f, 1f, delegate(MyGuiControlSlider slider)
				{
					MyPredictedSnapshotSync.MAX_ANGULAR_VELOCITY_DELTA = slider.Value;
				});
				AddSlider("Impulse scale", MyGridPhysics.PREDICTION_IMPULSE_SCALE, 0f, 0.2f, delegate(MyGuiControlSlider slider)
				{
					MyGridPhysics.PREDICTION_IMPULSE_SCALE = slider.Value;
				});
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugNetworkPrediction";
		}
	}
}
