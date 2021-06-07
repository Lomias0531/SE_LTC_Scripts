using Sandbox.Engine.Utils;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Render", "Debug")]
	internal class MyGuiScreenDebugRenderDebug : MyGuiScreenDebugBase
	{
		public static readonly StringBuilder ClipboardText = new StringBuilder();

		private List<MyGuiControlCheckbox> m_cbs = new List<MyGuiControlCheckbox>();

		public MyGuiScreenDebugRenderDebug()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Debug", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddSlider("Worker thread count", 1f, 5f, () => MyRenderProxy.Settings.RenderThreadCount, delegate(float f)
			{
				MyRenderProxy.Settings.RenderThreadCount = (int)f;
			});
			AddCheckBox("Force IC", MyRenderProxy.Settings.ForceImmediateContext, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.ForceImmediateContext = x.IsChecked;
			});
			AddCheckBox("Render thread high priority", MyRenderProxy.Settings.RenderThreadHighPriority, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.RenderThreadHighPriority = x.IsChecked;
			});
			AddCheckBox("Force Slow CPU", MyRenderProxy.Settings.ForceSlowCPU, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.ForceSlowCPU = x.IsChecked;
			});
			m_currentPosition.Y += 0.01f;
			AddCheckBox("Total parrot view", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_MODEL_INFO));
			AddButton("Copy to clipboard", CopyClipboardTextToClipboard);
			m_currentPosition.Y += 0.01f;
			AddCheckBox("Debug missing file textures", MyRenderProxy.Settings.UseDebugMissingFileTextures, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.UseDebugMissingFileTextures = x.IsChecked;
			});
			AddButton("Print textures log", PrintUsedFileTexturesIntoLog);
			AddCheckBox("Skip global RO WM update", MyRenderProxy.Settings.SkipGlobalROWMUpdate, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.SkipGlobalROWMUpdate = x.IsChecked;
			});
			AddCheckBox("HQ Depth", MyRenderProxy.Settings.User.HqDepth, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.User.HqDepth = x.IsChecked;
				MyRenderProxy.SetSettingsDirty();
			});
		}

		private void PrintUsedFileTexturesIntoLog(MyGuiControlButton sender)
		{
			MyRenderProxy.PrintAllFileTexturesIntoLog();
		}

		private void CopyClipboardTextToClipboard(MyGuiControlButton sender)
		{
			string text = ClipboardText.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				MyVRage.Platform.Clipboard = text;
			}
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderProxy.SetSettingsDirty();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderDebug";
		}
	}
}
