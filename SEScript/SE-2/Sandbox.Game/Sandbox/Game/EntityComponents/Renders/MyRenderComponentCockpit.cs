using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using VRage.Game.Entity;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.EntityComponents.Renders
{
	public class MyRenderComponentCockpit : MyRenderComponentScreenAreas
	{
		private class Sandbox_Game_EntityComponents_Renders_MyRenderComponentCockpit_003C_003EActor
		{
		}

		protected MyCockpit m_cockpit;

		public uint ExteriorRenderId => m_renderObjectIDs[0];

		public uint InteriorRenderId => m_renderObjectIDs[1];

		public MyRenderComponentCockpit(MyEntity entity)
			: base(entity)
		{
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_cockpit = (base.Container.Entity as MyCockpit);
		}

		public override void AddRenderObjects()
		{
			if (m_model == null || m_cockpit == null || m_renderObjectIDs[0] != uint.MaxValue)
			{
				return;
			}
			if (!string.IsNullOrEmpty(m_cockpit.BlockDefinition.InteriorModel))
			{
				ResizeRenderObjectArray(2);
			}
			SetRenderObjectID(0, MyRenderProxy.CreateRenderEntity(base.Container.Entity.GetFriendlyName() + " " + base.Container.Entity.EntityId, m_model.AssetName, base.Container.Entity.PositionComp.WorldMatrix, MyMeshDrawTechnique.MESH, GetRenderFlags(), GetRenderCullingOptions(), m_diffuseColor, m_colorMaskHsv, Transparency, float.MaxValue, DepthBias, m_model.ScaleFactor, Transparency == 0f && FadeIn));
			if (m_textureChanges != null)
			{
				MyRenderProxy.ChangeMaterialTexture(m_renderObjectIDs[0], m_textureChanges);
			}
			if (!string.IsNullOrEmpty(m_cockpit.BlockDefinition.InteriorModel))
			{
				SetRenderObjectID(1, MyRenderProxy.CreateRenderEntity(base.Container.Entity.GetFriendlyName() + " " + base.Container.Entity.EntityId + "_interior", m_cockpit.BlockDefinition.InteriorModel, base.Container.Entity.PositionComp.WorldMatrix, MyMeshDrawTechnique.MESH, GetRenderFlags(), GetRenderCullingOptions(), m_diffuseColor, m_colorMaskHsv, Transparency, float.MaxValue, DepthBias, m_model.ScaleFactor, FadeIn));
				MyRenderProxy.UpdateRenderObjectVisibility(m_renderObjectIDs[1], visible: false, NearFlag);
				if (m_textureChanges != null)
				{
					MyRenderProxy.ChangeMaterialTexture(m_renderObjectIDs[1], m_textureChanges);
				}
			}
			m_cockpit.UpdateCockpitModel();
			UpdateGridParent();
			UpdateRenderAreas();
		}
	}
}
