using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Render", "Lodding")]
	public class MyGuiScreenDebugRenderLodding : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugRenderLodding()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("Lodding", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			AddLabel("The new pipeline - lod shift", Color.White, 1f);
			AddSlider("GBuffer", MySector.Lodding.CurrentSettings.GBuffer.LodShift, 0f, 6f, delegate(MyGuiControlSlider x)
			{
				MySector.Lodding.CurrentSettings.GBuffer.LodShift = (int)Math.Round(x.Value);
				MySector.Lodding.CurrentSettings.GBuffer.LodShiftVisible = (int)Math.Round(x.Value);
			});
			if (MySector.Lodding.CurrentSettings.CascadeDepths.Length >= 3)
			{
				AddSlider("CSM_0 Visible in gbuffer", MySector.Lodding.CurrentSettings.CascadeDepths[0].LodShiftVisible, 0f, 6f, delegate(MyGuiControlSlider x)
				{
					MySector.Lodding.CurrentSettings.CascadeDepths[0].LodShiftVisible = (int)Math.Round(x.Value);
				});
				AddSlider("CSM_0", MySector.Lodding.CurrentSettings.CascadeDepths[0].LodShift, 0f, 6f, delegate(MyGuiControlSlider x)
				{
					MySector.Lodding.CurrentSettings.CascadeDepths[0].LodShift = (int)Math.Round(x.Value);
				});
				AddSlider("CSM_1 Visible in gbuffer", MySector.Lodding.CurrentSettings.CascadeDepths[1].LodShiftVisible, 0f, 6f, delegate(MyGuiControlSlider x)
				{
					MySector.Lodding.CurrentSettings.CascadeDepths[1].LodShiftVisible = (int)Math.Round(x.Value);
				});
				AddSlider("CSM_1", MySector.Lodding.CurrentSettings.CascadeDepths[1].LodShift, 0f, 6f, delegate(MyGuiControlSlider x)
				{
					MySector.Lodding.CurrentSettings.CascadeDepths[1].LodShift = (int)Math.Round(x.Value);
				});
				AddSlider("CSM_2 Visible in gbuffer", MySector.Lodding.CurrentSettings.CascadeDepths[2].LodShiftVisible, 0f, 6f, delegate(MyGuiControlSlider x)
				{
					MySector.Lodding.CurrentSettings.CascadeDepths[2].LodShiftVisible = (int)Math.Round(x.Value);
				});
				AddSlider("CSM_2", MySector.Lodding.CurrentSettings.CascadeDepths[2].LodShift, 0f, 6f, delegate(MyGuiControlSlider x)
				{
					MySector.Lodding.CurrentSettings.CascadeDepths[2].LodShift = (int)Math.Round(x.Value);
				});
			}
			AddSlider("Single depth visible in gbuffer", MySector.Lodding.CurrentSettings.SingleDepth.LodShiftVisible, 0f, 6f, delegate(MyGuiControlSlider x)
			{
				MySector.Lodding.CurrentSettings.SingleDepth.LodShiftVisible = (int)Math.Round(x.Value);
			});
			AddSlider("Single depth", MySector.Lodding.CurrentSettings.SingleDepth.LodShift, 0f, 6f, delegate(MyGuiControlSlider x)
			{
				MySector.Lodding.CurrentSettings.SingleDepth.LodShift = (int)Math.Round(x.Value);
			});
			AddLabel("The new pipeline - min lod", Color.White, 1f);
			AddSlider("GBuffer", MySector.Lodding.CurrentSettings.GBuffer.MinLod, 0f, 6f, delegate(MyGuiControlSlider x)
			{
				MySector.Lodding.CurrentSettings.GBuffer.MinLod = (int)Math.Round(x.Value);
			});
			if (MySector.Lodding.CurrentSettings.CascadeDepths.Length >= 3)
			{
				AddSlider("CSM_0", MySector.Lodding.CurrentSettings.CascadeDepths[0].MinLod, 0f, 6f, delegate(MyGuiControlSlider x)
				{
					MySector.Lodding.CurrentSettings.CascadeDepths[0].MinLod = (int)Math.Round(x.Value);
				});
				AddSlider("CSM_1", MySector.Lodding.CurrentSettings.CascadeDepths[1].MinLod, 0f, 6f, delegate(MyGuiControlSlider x)
				{
					MySector.Lodding.CurrentSettings.CascadeDepths[1].MinLod = (int)Math.Round(x.Value);
				});
				AddSlider("CSM_2", MySector.Lodding.CurrentSettings.CascadeDepths[2].MinLod, 0f, 6f, delegate(MyGuiControlSlider x)
				{
					MySector.Lodding.CurrentSettings.CascadeDepths[2].MinLod = (int)Math.Round(x.Value);
				});
			}
			AddSlider("Single depth", MySector.Lodding.CurrentSettings.SingleDepth.MinLod, 0f, 6f, delegate(MyGuiControlSlider x)
			{
				MySector.Lodding.CurrentSettings.SingleDepth.MinLod = (int)Math.Round(x.Value);
			});
			AddLabel("The new pipeline - global", Color.White, 1f);
			AddSlider("Object distance mult", MySector.Lodding.CurrentSettings.Global.ObjectDistanceMult, 0.01f, 8f, delegate(MyGuiControlSlider x)
			{
				MySector.Lodding.CurrentSettings.Global.ObjectDistanceMult = x.Value;
			});
			AddSlider("Object distance add", MySector.Lodding.CurrentSettings.Global.ObjectDistanceAdd, -100f, 100f, delegate(MyGuiControlSlider x)
			{
				MySector.Lodding.CurrentSettings.Global.ObjectDistanceAdd = x.Value;
			});
			AddSlider("Min transition in seconds", MySector.Lodding.CurrentSettings.Global.MinTransitionInSeconds, 0f, 2f, delegate(MyGuiControlSlider x)
			{
				MySector.Lodding.CurrentSettings.Global.MinTransitionInSeconds = x.Value;
			});
			AddSlider("Max transition in seconds", MySector.Lodding.CurrentSettings.Global.MaxTransitionInSeconds, 0f, 2f, delegate(MyGuiControlSlider x)
			{
				MySector.Lodding.CurrentSettings.Global.MaxTransitionInSeconds = x.Value;
			});
			AddSlider("Transition dead zone - const", MySector.Lodding.CurrentSettings.Global.TransitionDeadZoneConst, 0f, 2f, delegate(MyGuiControlSlider x)
			{
				MySector.Lodding.CurrentSettings.Global.TransitionDeadZoneConst = x.Value;
			});
			AddSlider("Transition dead zone - dist mult", MySector.Lodding.CurrentSettings.Global.TransitionDeadZoneDistanceMult, 0f, 2f, delegate(MyGuiControlSlider x)
			{
				MySector.Lodding.CurrentSettings.Global.TransitionDeadZoneDistanceMult = x.Value;
			});
			AddSlider("Lod histeresis ratio", MySector.Lodding.CurrentSettings.Global.HisteresisRatio, 0f, 1f, delegate(MyGuiControlSlider x)
			{
				MySector.Lodding.CurrentSettings.Global.HisteresisRatio = x.Value;
			});
			AddCheckBox("Update lods", () => MySector.Lodding.CurrentSettings.Global.IsUpdateEnabled, delegate(bool x)
			{
				MySector.Lodding.CurrentSettings.Global.IsUpdateEnabled = x;
			});
			AddCheckBox("Display lod", MyRenderProxy.Settings.DisplayGbufferLOD, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayGbufferLOD = x.IsChecked;
			});
			AddCheckBox("Enable lod selection", MySector.Lodding.CurrentSettings.Global.EnableLodSelection, delegate(MyGuiControlCheckbox x)
			{
				MySector.Lodding.CurrentSettings.Global.EnableLodSelection = x.IsChecked;
			});
			AddSlider("Lod selection", MySector.Lodding.CurrentSettings.Global.LodSelection, 0f, 5f, delegate(MyGuiControlSlider x)
			{
				MySector.Lodding.CurrentSettings.Global.LodSelection = (int)Math.Round(x.Value);
			});
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderProxy.SetSettingsDirty();
			MyRenderProxy.UpdateNewLoddingSettings(MySector.Lodding.CurrentSettings);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderLodding";
		}
	}
}
