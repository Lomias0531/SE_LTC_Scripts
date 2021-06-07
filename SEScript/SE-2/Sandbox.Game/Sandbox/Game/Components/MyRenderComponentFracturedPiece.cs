using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Network;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Components
{
	public class MyRenderComponentFracturedPiece : MyRenderComponent
	{
		private struct ModelInfo
		{
			public string Name;

			public MatrixD LocalTransform;
		}

		private class Sandbox_Game_Components_MyRenderComponentFracturedPiece_003C_003EActor : IActivator, IActivator<MyRenderComponentFracturedPiece>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentFracturedPiece();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentFracturedPiece CreateInstance()
			{
				return new MyRenderComponentFracturedPiece();
			}

			MyRenderComponentFracturedPiece IActivator<MyRenderComponentFracturedPiece>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const string EMPTY_MODEL = "Models\\Debug\\Error.mwm";

		private readonly List<ModelInfo> Models = new List<ModelInfo>();

		public void AddPiece(string modelName, MatrixD localTransform)
		{
			if (string.IsNullOrEmpty(modelName))
			{
				modelName = "Models\\Debug\\Error.mwm";
			}
			Models.Add(new ModelInfo
			{
				Name = modelName,
				LocalTransform = localTransform
			});
		}

		public void RemovePiece(string modelName)
		{
			if (string.IsNullOrEmpty(modelName))
			{
				modelName = "Models\\Debug\\Error.mwm";
			}
			Models.RemoveAll((ModelInfo m) => m.Name == modelName);
		}

		public override void InvalidateRenderObjects()
		{
			MatrixD worldMatrix = base.Container.Entity.PositionComp.WorldMatrix;
			if ((base.Container.Entity.Visible || base.Container.Entity.CastShadows) && base.Container.Entity.InScene && base.Container.Entity.InvalidateOnMove && m_renderObjectIDs.Length != 0)
			{
				MyRenderProxy.UpdateRenderObject(m_renderObjectIDs[0], worldMatrix);
			}
		}

		public override void AddRenderObjects()
		{
			if (Models.Count == 0)
			{
				return;
			}
			MyCubeBlock myCubeBlock = base.Container.Entity as MyCubeBlock;
			if (myCubeBlock != null)
			{
				this.CalculateBlockDepthBias(myCubeBlock);
			}
			m_renderObjectIDs = new uint[Models.Count + 1];
			m_parentIDs = new uint[Models.Count + 1];
			m_parentIDs[0] = (m_renderObjectIDs[0] = uint.MaxValue);
			SetRenderObjectID(0, MyRenderProxy.CreateManualCullObject(base.Container.Entity.Name ?? "Fracture", base.Container.Entity.PositionComp.WorldMatrix));
			for (int i = 0; i < Models.Count; i++)
			{
				m_parentIDs[i + 1] = (m_renderObjectIDs[i + 1] = uint.MaxValue);
				SetRenderObjectID(i + 1, MyRenderProxy.CreateRenderEntity("Fractured piece " + i + " " + base.Container.Entity.EntityId, Models[i].Name, Models[i].LocalTransform, MyMeshDrawTechnique.MESH, GetRenderFlags(), GetRenderCullingOptions(), m_diffuseColor, m_colorMaskHsv, 0f, float.MaxValue, DepthBias, 1f, FadeIn));
				if (m_textureChanges != null)
				{
					MyRenderProxy.ChangeMaterialTexture(m_renderObjectIDs[i + 1], m_textureChanges);
				}
				SetParent(i + 1, m_renderObjectIDs[0], Models[i].LocalTransform);
			}
		}

		public void ClearModels()
		{
			Models.Clear();
		}
	}
}
