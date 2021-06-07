using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using VRage;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("VRage", "Wheels")]
	public class MyGuiScreenDebugWheels : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugWheels()
		{
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugWheels";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			base.BackgroundColor = new Vector4(1f, 1f, 1f, 0.5f);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.13f);
			AddCaption("Wheels", Color.Yellow.ToVector4());
			AddShareFocusHint();
			AddCaption("DebugDraw");
			AddCheckBox("Physics", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_WHEEL_PHYSICS));
			AddCheckBox("Systems", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_WHEEL_SYSTEMS));
			AddCheckBox("Draw voxel contact materials", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTACT_MATERIAL));
			AddSubcaption("Response modifier");
			AddSlider("Max accel", 0f, 0.1f, () => MyPhysicsConfig.WheelSoftnessVelocity, delegate(float v)
			{
				MyPhysicsConfig.WheelSoftnessVelocity = v;
			});
			AddSlider("Softness ratio", 0f, 1f, () => MyPhysicsConfig.WheelSoftnessRatio, delegate(float v)
			{
				MyPhysicsConfig.WheelSoftnessRatio = v;
			});
			AddSubcaption("Steering model");
			AddSlider("Slip countdown", 0f, 100f, () => MyPhysicsConfig.WheelSlipCountdown, delegate(float x)
			{
				MyPhysicsConfig.WheelSlipCountdown = (int)x;
			});
			AddSlider("Impulse Blending", 0f, 1f, () => MyPhysicsConfig.WheelImpulseBlending, delegate(float x)
			{
				MyPhysicsConfig.WheelImpulseBlending = x;
			});
			AddSlider("Impulse Blending", 0f, 1f, () => MyPhysicsConfig.WheelImpulseBlending, delegate(float x)
			{
				MyPhysicsConfig.WheelImpulseBlending = x;
			});
			AddSlider("Slip CutAway Ratio", 0f, 1f, () => MyPhysicsConfig.WheelSlipCutAwayRatio, delegate(float x)
			{
				MyPhysicsConfig.WheelSlipCutAwayRatio = x;
			});
			AddSlider("Surface Material steering ratio", 0f, 1f, () => MyPhysicsConfig.WheelSurfaceMaterialSteerRatio, delegate(float x)
			{
				MyPhysicsConfig.WheelSurfaceMaterialSteerRatio = x;
			});
			AddCheckBox("Override axle friction", null, MemberHelper.GetMember(() => MyPhysicsConfig.OverrideWheelAxleFriction));
			AddSlider("Axle friction", 0f, 10000f, () => MyPhysicsConfig.WheelAxleFriction, delegate(float x)
			{
				MyPhysicsConfig.WheelAxleFriction = x;
			});
			AddSlider("Artificial breaking", 0f, 10f, () => MyPhysicsConfig.ArtificialBrakingMultiplier, delegate(float x)
			{
				MyPhysicsConfig.ArtificialBrakingMultiplier = x;
			});
			AddSlider("Artificial breaking CoM stabilization", 0f, 1f, () => MyPhysicsConfig.ArtificialBrakingCoMStabilization, delegate(float x)
			{
				MyPhysicsConfig.ArtificialBrakingCoMStabilization = x;
			});
		}
	}
}
