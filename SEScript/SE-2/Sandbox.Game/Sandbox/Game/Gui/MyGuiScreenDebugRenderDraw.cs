using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Render", "Draw")]
	internal class MyGuiScreenDebugRenderDraw : MyGuiScreenDebugBase
	{
		private List<MyGuiControlCheckbox> m_cbs = new List<MyGuiControlCheckbox>();

		public MyGuiScreenDebugRenderDraw()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Draw", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddCheckBox("Draw IDs", MyRenderProxy.Settings.DisplayIDs, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayIDs = x.IsChecked;
			});
			AddCheckBox("Draw AABBs", MyRenderProxy.Settings.DisplayAabbs, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayAabbs = x.IsChecked;
			});
			AddCheckBox("Draw Tree AABBs", MyRenderProxy.Settings.DisplayTreeAabbs, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayTreeAabbs = x.IsChecked;
			});
			m_currentPosition.Y += 0.01f;
			AddCheckBox("Draw Wireframe", MyRenderProxy.Settings.Wireframe, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.Wireframe = x.IsChecked;
			});
			AddCheckBox("Draw transparency heat map", MyRenderProxy.Settings.DisplayTransparencyHeatMap, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayTransparencyHeatMap = x.IsChecked;
			});
			AddCheckBox("Draw transparency heat map in grayscale", MyRenderProxy.Settings.DisplayTransparencyHeatMapInGrayscale, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayTransparencyHeatMapInGrayscale = x.IsChecked;
			});
			m_currentPosition.Y += 0.01f;
			AddLabel("Scene objects", Color.Yellow.ToVector4(), 1.2f);
			AddCheckBox("Draw non-merge-instanced", MyRenderProxy.Settings.DrawNonMergeInstanced, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawNonMergeInstanced = x.IsChecked;
			});
			AddCheckBox("Draw merge-instanced", MyRenderProxy.Settings.DrawMergeInstanced, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawMergeInstanced = x.IsChecked;
			});
			AddCheckBox("Draw groups", MyRenderProxy.Settings.DrawGroups, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawGroups = x.IsChecked;
			});
			m_currentPosition.Y += 0.01f;
			AddCheckBox("Draw standard meshes", MyRenderProxy.Settings.DrawMeshes, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawMeshes = x.IsChecked;
			});
			AddCheckBox("Draw standard instanced meshes", MyRenderProxy.Settings.DrawInstancedMeshes, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawInstancedMeshes = x.IsChecked;
			});
			AddCheckBox("Draw dynamic instances", MyRenderProxy.Settings.DrawDynamicInstances, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawDynamicInstances = x.IsChecked;
			});
			AddCheckBox("Draw glass", MyRenderProxy.Settings.DrawGlass, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawGlass = x.IsChecked;
			});
			AddCheckBox("Draw transparent models", MyRenderProxy.Settings.DrawTransparentModels, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawTransparentModels = x.IsChecked;
			});
			AddCheckBox("Draw transparent instanced models", MyRenderProxy.Settings.DrawTransparentModelsInstanced, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawTransparentModelsInstanced = x.IsChecked;
			});
			AddCheckBox("Draw alphamasked", MyRenderProxy.Settings.DrawAlphamasked, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawAlphamasked = x.IsChecked;
			});
			AddCheckBox("Draw billboards", MyRenderProxy.Settings.DrawBillboards, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawBillboards = x.IsChecked;
			});
			AddCheckBox("Draw impostors", MyRenderProxy.Settings.DrawImpostors, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawImpostors = x.IsChecked;
			});
			AddCheckBox("Draw voxels", MyRenderProxy.Settings.DrawVoxels, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawVoxels = x.IsChecked;
			});
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderProxy.SetSettingsDirty();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderDraw";
		}
	}
}
