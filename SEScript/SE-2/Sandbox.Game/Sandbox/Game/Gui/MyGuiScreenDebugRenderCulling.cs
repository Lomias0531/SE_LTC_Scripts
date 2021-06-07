using Sandbox.Graphics.GUI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Render", "Culling")]
	internal class MyGuiScreenDebugRenderCulling : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugRenderCulling()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Culling", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddSlider("CullGroupsThreshold", 0f, 1000f, () => MyRenderProxy.Settings.CullGroupsThreshold, delegate(float f)
			{
				MyRenderProxy.Settings.CullGroupsThreshold = (int)f;
			});
			AddSlider("CullTreeFallbackThreshold", 0f, 1f, () => MyRenderProxy.Settings.IncrementalCullingTreeFallbackThreshold, delegate(float x)
			{
				MyRenderProxy.Settings.IncrementalCullingTreeFallbackThreshold = x;
			});
			AddCheckBox("UseIncrementalCulling", () => MyRenderProxy.Settings.UseIncrementalCulling, delegate(bool x)
			{
				MyRenderProxy.Settings.UseIncrementalCulling = x;
			});
			AddSlider("IncrementalCullFrames", 1f, 100f, () => MyRenderProxy.Settings.IncrementalCullFrames, delegate(float x)
			{
				MyRenderProxy.Settings.IncrementalCullFrames = (int)x;
			});
			m_currentPosition.Y += 0.01f;
			AddLabel("Occlusion", Color.Yellow.ToVector4(), 1.2f);
			AddCheckBox("Skip occlusion queries", MyRenderProxy.Settings.IgnoreOcclusionQueries, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.IgnoreOcclusionQueries = x.IsChecked;
			});
			AddCheckBox("Disable occlusion queries", MyRenderProxy.Settings.DisableOcclusionQueries, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisableOcclusionQueries = x.IsChecked;
			});
			AddCheckBox("Draw occlusion queries debug", MyRenderProxy.Settings.DrawOcclusionQueriesDebug, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawOcclusionQueriesDebug = x.IsChecked;
			});
			AddCheckBox("Draw group occlusion queries debug", MyRenderProxy.Settings.DrawGroupOcclusionQueriesDebug, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawGroupOcclusionQueriesDebug = x.IsChecked;
			});
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderProxy.SetSettingsDirty();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderCulling";
		}
	}
}
