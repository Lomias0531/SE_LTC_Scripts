using Sandbox.Definitions;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Game", "Voxel materials")]
	public class MyGuiScreenDebugVoxelMaterials : MyGuiScreenDebugBase
	{
		private MyGuiControlCombobox m_materialsCombo;

		private MyDx11VoxelMaterialDefinition m_selectedVoxelMaterial;

		private bool m_canUpdate;

		private MyGuiControlSlider m_sliderInitialScale;

		private MyGuiControlSlider m_sliderScaleMultiplier;

		private MyGuiControlSlider m_sliderInitialDistance;

		private MyGuiControlSlider m_sliderDistanceMultiplier;

		private MyGuiControlSlider m_sliderTilingScale;

		private MyGuiControlSlider m_sliderFar1Scale;

		private MyGuiControlSlider m_sliderFar1Distance;

		private MyGuiControlSlider m_sliderFar2Scale;

		private MyGuiControlSlider m_sliderFar2Distance;

		private MyGuiControlSlider m_sliderFar3Distance;

		private MyGuiControlSlider m_sliderFar3Scale;

		private MyGuiControlColor m_colorFar3;

		private MyGuiControlSlider m_sliderExtScale;

		private MyGuiControlSlider m_sliderFriction;

		private MyGuiControlSlider m_sliderRestitution;

		public MyGuiScreenDebugVoxelMaterials()
		{
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugVoxelMaterials";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			base.BackgroundColor = new Vector4(1f, 1f, 1f, 0.5f);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.13f);
			AddCaption("Voxel materials", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_materialsCombo = AddCombo();
			foreach (MyVoxelMaterialDefinition item in (from x in MyDefinitionManager.Static.GetVoxelMaterialDefinitions()
				orderby x.Id.SubtypeName
				select x).ToList())
			{
				m_materialsCombo.AddItem(item.Index, new StringBuilder(item.Id.SubtypeName));
			}
			m_materialsCombo.ItemSelected += materialsCombo_OnSelect;
			m_currentPosition.Y += 0.01f;
			m_sliderInitialScale = AddSlider("Initial scale", 0f, 1f, 20f);
			MyGuiControlSlider sliderInitialScale = m_sliderInitialScale;
			sliderInitialScale.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderInitialScale.ValueChanged, new Action<MyGuiControlSlider>(ValueChanged));
			m_sliderScaleMultiplier = AddSlider("Scale multiplier", 0f, 1f, 30f);
			MyGuiControlSlider sliderScaleMultiplier = m_sliderScaleMultiplier;
			sliderScaleMultiplier.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderScaleMultiplier.ValueChanged, new Action<MyGuiControlSlider>(ValueChanged));
			m_sliderInitialDistance = AddSlider("Initial distance", 0f, 1f, 30f);
			MyGuiControlSlider sliderInitialDistance = m_sliderInitialDistance;
			sliderInitialDistance.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderInitialDistance.ValueChanged, new Action<MyGuiControlSlider>(ValueChanged));
			m_sliderDistanceMultiplier = AddSlider("Distance multiplier", 0f, 1f, 30f);
			MyGuiControlSlider sliderDistanceMultiplier = m_sliderDistanceMultiplier;
			sliderDistanceMultiplier.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderDistanceMultiplier.ValueChanged, new Action<MyGuiControlSlider>(ValueChanged));
			m_currentPosition.Y += 0.01f;
			m_sliderTilingScale = AddSlider("Tiling scale", 0f, 1f, 1024f);
			MyGuiControlSlider sliderTilingScale = m_sliderTilingScale;
			sliderTilingScale.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderTilingScale.ValueChanged, new Action<MyGuiControlSlider>(ValueChanged));
			m_currentPosition.Y += 0.01f;
			m_sliderFar1Distance = AddSlider("Far1 distance", 0f, 0f, 500f);
			MyGuiControlSlider sliderFar1Distance = m_sliderFar1Distance;
			sliderFar1Distance.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderFar1Distance.ValueChanged, new Action<MyGuiControlSlider>(ValueChanged));
			m_sliderFar1Scale = AddSlider("Far1 scale", 0f, 1f, 1000f);
			MyGuiControlSlider sliderFar1Scale = m_sliderFar1Scale;
			sliderFar1Scale.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderFar1Scale.ValueChanged, new Action<MyGuiControlSlider>(ValueChanged));
			m_sliderFar2Distance = AddSlider("Far2 distance", 0f, 0f, 1500f);
			MyGuiControlSlider sliderFar2Distance = m_sliderFar2Distance;
			sliderFar2Distance.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderFar2Distance.ValueChanged, new Action<MyGuiControlSlider>(ValueChanged));
			m_sliderFar2Scale = AddSlider("Far2 scale", 0f, 1f, 2000f);
			MyGuiControlSlider sliderFar2Scale = m_sliderFar2Scale;
			sliderFar2Scale.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderFar2Scale.ValueChanged, new Action<MyGuiControlSlider>(ValueChanged));
			m_sliderFar3Distance = AddSlider("Far3 distance", 0f, 0f, 40000f);
			MyGuiControlSlider sliderFar3Distance = m_sliderFar3Distance;
			sliderFar3Distance.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderFar3Distance.ValueChanged, new Action<MyGuiControlSlider>(ValueChanged));
			m_sliderFar3Scale = AddSlider("Far3 scale", 0f, 1f, 50000f);
			MyGuiControlSlider sliderFar3Scale = m_sliderFar3Scale;
			sliderFar3Scale.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderFar3Scale.ValueChanged, new Action<MyGuiControlSlider>(ValueChanged));
			m_currentPosition.Y += 0.01f;
			m_sliderExtScale = AddSlider("Detail scale (/1000)", 0f, 0f, 10f);
			MyGuiControlSlider sliderExtScale = m_sliderExtScale;
			sliderExtScale.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderExtScale.ValueChanged, new Action<MyGuiControlSlider>(ValueChanged));
			m_currentPosition.Y += 0.01f;
			m_sliderFriction = AddSlider("Friction", 0f, 0f, 2f, null, null, null);
			MyGuiControlSlider sliderFriction = m_sliderFriction;
			sliderFriction.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderFriction.ValueChanged, new Action<MyGuiControlSlider>(ValueChanged));
			m_sliderRestitution = AddSlider("Restitution", 0f, 0f, 2f, null, null, null);
			MyGuiControlSlider sliderRestitution = m_sliderRestitution;
			sliderRestitution.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderRestitution.ValueChanged, new Action<MyGuiControlSlider>(ValueChanged));
			m_materialsCombo.SelectItemByIndex(0);
			m_colorFar3 = AddColor("Far3 color", m_selectedVoxelMaterial.RenderParams, MemberHelper.GetMember(() => m_selectedVoxelMaterial.RenderParams.Far3Color));
			m_colorFar3.SetColor(m_selectedVoxelMaterial.RenderParams.Far3Color);
			m_colorFar3.OnChange += ValueChanged;
			m_currentPosition.Y += 0.01f;
			AddButton(new StringBuilder("Reload definition"), OnReloadDefinition);
		}

		private void materialsCombo_OnSelect()
		{
			m_selectedVoxelMaterial = (MyDx11VoxelMaterialDefinition)MyDefinitionManager.Static.GetVoxelMaterialDefinition((byte)m_materialsCombo.GetSelectedKey());
			UpdateValues();
		}

		private void UpdateValues()
		{
			m_canUpdate = false;
			m_sliderInitialScale.Value = m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.InitialScale;
			m_sliderScaleMultiplier.Value = m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.ScaleMultiplier;
			m_sliderInitialDistance.Value = m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.InitialDistance;
			m_sliderDistanceMultiplier.Value = m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.DistanceMultiplier;
			m_sliderTilingScale.Value = m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.TilingScale;
			m_sliderFar1Scale.Value = m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far1Scale;
			m_sliderFar1Distance.Value = m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far1Distance;
			m_sliderFar2Scale.Value = m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far2Scale;
			m_sliderFar2Distance.Value = m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far2Distance;
			m_sliderFar3Scale.Value = m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far3Scale;
			m_sliderFar3Distance.Value = m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far3Distance;
			if (m_colorFar3 != null)
			{
				m_colorFar3.SetColor(m_selectedVoxelMaterial.RenderParams.Far3Color);
			}
			m_sliderExtScale.Value = 1000f * m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.ExtensionDetailScale;
			m_sliderFriction.Value = m_selectedVoxelMaterial.Friction;
			m_sliderRestitution.Value = m_selectedVoxelMaterial.Restitution;
			m_canUpdate = true;
		}

		private new void ValueChanged(MyGuiControlBase sender)
		{
			if (m_canUpdate)
			{
				m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.InitialScale = m_sliderInitialScale.Value;
				m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.ScaleMultiplier = m_sliderScaleMultiplier.Value;
				m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.InitialDistance = m_sliderInitialDistance.Value;
				m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.DistanceMultiplier = m_sliderDistanceMultiplier.Value;
				m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.TilingScale = m_sliderTilingScale.Value;
				m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far1Scale = m_sliderFar1Scale.Value;
				m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far1Distance = m_sliderFar1Distance.Value;
				m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far2Scale = m_sliderFar2Scale.Value;
				m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far2Distance = m_sliderFar2Distance.Value;
				m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far3Scale = m_sliderFar3Scale.Value;
				m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far3Distance = m_sliderFar3Distance.Value;
				m_selectedVoxelMaterial.RenderParams.Far3Color = m_colorFar3.GetColor();
				m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.ExtensionDetailScale = m_sliderExtScale.Value / 1000f;
				m_selectedVoxelMaterial.Friction = m_sliderFriction.Value;
				m_selectedVoxelMaterial.Restitution = m_sliderRestitution.Value;
				m_selectedVoxelMaterial.UpdateVoxelMaterial();
			}
		}

		private void OnReloadDefinition(MyGuiControlButton button)
		{
			MyDefinitionManager.Static.ReloadVoxelMaterials();
			materialsCombo_OnSelect();
			m_selectedVoxelMaterial.UpdateVoxelMaterial();
		}
	}
}
