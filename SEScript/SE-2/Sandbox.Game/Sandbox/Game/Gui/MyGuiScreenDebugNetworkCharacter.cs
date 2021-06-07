using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
using Sandbox.Graphics.GUI;
using System;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("VRage", "Network Character")]
	internal class MyGuiScreenDebugNetworkCharacter : MyGuiScreenDebugBase
	{
		private MyGuiControlSlider m_maxJetpackGridDistanceSlider;

		private MyGuiControlSlider m_maxDisconnectDistanceSlider;

		private MyGuiControlSlider m_minJetpackGridSpeedSlider;

		private MyGuiControlSlider m_minJetpackDisconnectGridSpeedSlider;

		private MyGuiControlSlider m_minJetpackInsideGridSpeedSlider;

		private MyGuiControlSlider m_minJetpackDisconnectInsideGridSpeedSlider;

		private MyGuiControlSlider m_maxJetpackGridAccelerationSlider;

		private MyGuiControlSlider m_maxJetpackDisconnectGridAccelerationSlider;

		public MyGuiScreenDebugNetworkCharacter()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption("Network Character", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddSlider("Animation fallback threshold [m]", MyCharacterPhysicsStateGroup.EXCESSIVE_CORRECTION_THRESHOLD, 0f, 100f, delegate(MyGuiControlSlider slider)
			{
				MyCharacterPhysicsStateGroup.EXCESSIVE_CORRECTION_THRESHOLD = slider.Value;
			});
			AddLabel("Support", Color.White, 1f);
			AddSlider("Change delay [ms]", (float)MyCharacterPhysicsStateGroup.ParentChangeTimeOut.Milliseconds, 0f, 5000f, delegate(MyGuiControlSlider slider)
			{
				MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action<double>>)((IMyEventOwner x) => MyMultiplayerBase.OnCharacterParentChangeTimeOut), (double)slider.Value, default(EndpointId), (Vector3D?)null);
			});
			AddLabel("Jetpack Connect", Color.White, 1f);
			m_maxJetpackGridDistanceSlider = AddSlider("Max distance [m]", MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDistance, 0f, 1000f, delegate(MyGuiControlSlider slider)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyMultiplayerBase.OnCharacterMaxJetpackGridDistance, slider.Value);
				MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDistance = slider.Value;
				m_maxDisconnectDistanceSlider.Value = Math.Max(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDisconnectDistance, slider.Value);
			});
			m_maxJetpackGridAccelerationSlider = AddSlider("Max acceleration [m/s^2]", MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentAcceleration, 0f, 1000f, delegate(MyGuiControlSlider slider)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyMultiplayerBase.OnCharacterMaxJetpackGridAcceleration, slider.Value);
				MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentAcceleration = slider.Value;
				m_maxJetpackDisconnectGridAccelerationSlider.Value = Math.Max(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxDisconnectParentAcceleration, slider.Value);
			});
			m_minJetpackGridSpeedSlider = AddSlider("Min speed [m/s]", MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinParentSpeed, 0f, 100f, delegate(MyGuiControlSlider slider)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyMultiplayerBase.OnCharacterMinJetpackGridSpeed, slider.Value);
				MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinParentSpeed = slider.Value;
				m_minJetpackDisconnectGridSpeedSlider.Value = Math.Min(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectParentSpeed, slider.Value);
			});
			m_minJetpackInsideGridSpeedSlider = AddSlider("Min inside speed [m/s]", MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinInsideParentSpeed, 0f, 100f, delegate(MyGuiControlSlider slider)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyMultiplayerBase.OnCharacterMinJetpackInsideGridSpeed, slider.Value);
				MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinInsideParentSpeed = slider.Value;
				m_minJetpackDisconnectInsideGridSpeedSlider.Value = Math.Min(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectInsideParentSpeed, slider.Value);
			});
			AddLabel("Jetpack Disconnect", Color.White, 1f);
			m_maxDisconnectDistanceSlider = AddSlider("Max distance [m]", MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDisconnectDistance, 0f, 1000f, delegate(MyGuiControlSlider slider)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyMultiplayerBase.OnCharacterMaxJetpackGridDisconnectDistance, slider.Value);
				MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDisconnectDistance = slider.Value;
				m_maxJetpackGridDistanceSlider.Value = Math.Min(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDistance, slider.Value);
			});
			m_maxJetpackDisconnectGridAccelerationSlider = AddSlider("Max acceleration [m/s^2]", MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxDisconnectParentAcceleration, 0f, 1000f, delegate(MyGuiControlSlider slider)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyMultiplayerBase.OnCharacterMaxJetpackDisconnectGridAcceleration, slider.Value);
				MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxDisconnectParentAcceleration = slider.Value;
				m_maxJetpackGridAccelerationSlider.Value = Math.Min(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentAcceleration, slider.Value);
			});
			m_minJetpackDisconnectGridSpeedSlider = AddSlider("Min speed [m/s]", MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectParentSpeed, 0f, 100f, delegate(MyGuiControlSlider slider)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyMultiplayerBase.OnCharacterMinJetpackDisconnectGridSpeed, slider.Value);
				MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectParentSpeed = slider.Value;
				m_minJetpackGridSpeedSlider.Value = Math.Max(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinParentSpeed, slider.Value);
			});
			m_minJetpackDisconnectInsideGridSpeedSlider = AddSlider("Min inside speed [m/s]", MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectInsideParentSpeed, 0f, 100f, delegate(MyGuiControlSlider slider)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyMultiplayerBase.OnCharacterMinJetpackDisconnectInsideGridSpeed, slider.Value);
				MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectInsideParentSpeed = slider.Value;
				m_minJetpackInsideGridSpeedSlider.Value = Math.Max(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinInsideParentSpeed, slider.Value);
			});
		}
	}
}
