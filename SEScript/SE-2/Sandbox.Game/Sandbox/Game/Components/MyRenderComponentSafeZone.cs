using System.Collections.Generic;
using VRage.Game.Models;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;
using VRageRender.Messages;

namespace Sandbox.Game.Components
{
	internal class MyRenderComponentSafeZone : MyRenderComponent
	{
		private class Sandbox_Game_Components_MyRenderComponentSafeZone_003C_003EActor : IActivator, IActivator<MyRenderComponentSafeZone>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentSafeZone();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentSafeZone CreateInstance()
			{
				return new MyRenderComponentSafeZone();
			}

			MyRenderComponentSafeZone IActivator<MyRenderComponentSafeZone>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MatrixD m_scaledRenderMatrix;

		private Vector3 m_hsvColor = new Color(0.1f, 0.63f, 0.95f).ColorToHSV();

		private uint m_tempRenderObjPtr = uint.MaxValue;

		private void UpdateRenderObjectMatrices(Matrix matrix)
		{
			for (int i = 0; i < m_renderObjectIDs.Length; i++)
			{
				if (m_renderObjectIDs[i] != uint.MaxValue)
				{
					MyRenderProxy.UpdateRenderObject(base.RenderObjectIDs[i], matrix, null, LastMomentUpdateIndex);
				}
			}
		}

		public void ChangeScale(Vector3 scale)
		{
			scale /= base.Model.BoundingBoxSizeHalf;
			Matrix m = base.Container.Entity.WorldMatrix;
			MyUtils.Normalize(ref m, out m);
			m_scaledRenderMatrix = Matrix.CreateScale(scale) * m;
		}

		public void SwitchModel(string modelName)
		{
			MyModel modelOnlyData = MyModels.GetModelOnlyData(modelName);
			if (modelOnlyData != null && ModelStorage != modelOnlyData)
			{
				if (base.RenderObjectIDs[0] != uint.MaxValue)
				{
					RemoveRenderObjects();
				}
				ModelStorage = modelOnlyData;
				if (base.RenderObjectIDs[0] == uint.MaxValue)
				{
					AddRenderObjects();
				}
			}
		}

		public void ChangeColor(Color newColor)
		{
			m_hsvColor = newColor.ColorToHSV();
		}

		public override void InvalidateRenderObjects()
		{
			if ((!base.Container.Entity.Visible && !base.Container.Entity.CastShadows) || !base.Container.Entity.InScene || !base.Container.Entity.InvalidateOnMove)
			{
				return;
			}
			for (int i = 0; i < m_renderObjectIDs.Length; i++)
			{
				if (base.RenderObjectIDs[i] != uint.MaxValue)
				{
					MyRenderProxy.UpdateRenderObject(base.RenderObjectIDs[i], m_scaledRenderMatrix, null, LastMomentUpdateIndex);
					MyRenderProxy.UpdateRenderEntity(base.RenderObjectIDs[i], null, m_hsvColor);
				}
			}
		}

		public void AddTransitionObject(Dictionary<string, MyTextureChange> texture)
		{
			if (m_tempRenderObjPtr == uint.MaxValue)
			{
				MatrixD scaledRenderMatrix = m_scaledRenderMatrix;
				m_tempRenderObjPtr = MyRenderProxy.CreateRenderEntity(base.Container.Entity.GetFriendlyName() + " " + base.Container.Entity.EntityId, m_model.AssetName, scaledRenderMatrix, MyMeshDrawTechnique.MESH, GetRenderFlags(), GetRenderCullingOptions(), m_diffuseColor, m_colorMaskHsv, Transparency, float.MaxValue, DepthBias, m_model.ScaleFactor);
			}
			MyRenderProxy.ChangeMaterialTexture(m_tempRenderObjPtr, texture);
		}

		public void RemoveTransitionObject()
		{
			MyRenderProxy.RemoveRenderObject(m_tempRenderObjPtr, MyRenderProxy.ObjectType.Invalid, FadeOut);
			m_tempRenderObjPtr = uint.MaxValue;
		}

		public void UpdateTransitionObjColor(Color color)
		{
			if ((base.Container.Entity.Visible || base.Container.Entity.CastShadows) && base.Container.Entity.InScene && base.Container.Entity.InvalidateOnMove)
			{
				MyRenderProxy.UpdateRenderEntity(m_tempRenderObjPtr, null, color.ColorToHSV());
			}
		}

		public override void AddRenderObjects()
		{
			base.AddRenderObjects();
			m_scaledRenderMatrix = base.Container.Entity.WorldMatrix;
		}

		public override void RemoveRenderObjects()
		{
			base.RemoveRenderObjects();
			if (m_tempRenderObjPtr != uint.MaxValue)
			{
				RemoveTransitionObject();
			}
		}
	}
}
