using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using System.Collections.Generic;
using VRage.Game.Models;
using VRage.Generics;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Cube
{
	public class MyBlockBuilderRenderData
	{
		private struct MyRenderEntity
		{
			public uint RenderEntityId;

			public Matrix LocalMatrix;

			public Vector3 ColorMashHsv;

			public MyStringHash SkinId;

			public bool UpdateSkin;

			public void Update(MatrixD gridWorldMatrix, float transparency)
			{
				MatrixD value = LocalMatrix * gridWorldMatrix;
				MyRenderProxy.UpdateRenderObject(RenderEntityId, value);
				MyRenderProxy.UpdateRenderEntity(RenderEntityId, Vector3.One, ColorMashHsv, transparency);
				if (UpdateSkin)
				{
					MyDefinitionManager.MyAssetModifiers assetModifierDefinitionForRender = MyDefinitionManager.Static.GetAssetModifierDefinitionForRender(SkinId);
					if (assetModifierDefinitionForRender.SkinTextureChanges != null)
					{
						MyRenderProxy.ChangeMaterialTexture(RenderEntityId, assetModifierDefinitionForRender.SkinTextureChanges);
					}
					UpdateSkin = false;
				}
			}
		}

		[GenerateActivator]
		private class MyEntities
		{
			private class Sandbox_Game_Entities_Cube_MyBlockBuilderRenderData_003C_003EMyEntities_003C_003EActor : IActivator, IActivator<MyEntities>
			{
				private sealed override object CreateInstance()
				{
					return new MyEntities();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyEntities CreateInstance()
				{
					return new MyEntities();
				}

				MyEntities IActivator<MyEntities>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			private List<MyRenderEntity> RenderEntities = new List<MyRenderEntity>();

			private int NumUsedModels;

			public bool IsEmpty()
			{
				return NumUsedModels == 0;
			}

			public void ShrinkRenderEnties()
			{
				for (int i = NumUsedModels; i < RenderEntities.Count; i++)
				{
					MyRenderProxy.RemoveRenderObject(RenderEntities[i].RenderEntityId, MyRenderProxy.ObjectType.Entity);
				}
				RenderEntities.RemoveRange(NumUsedModels, RenderEntities.Count - NumUsedModels);
			}

			public void Clear()
			{
				NumUsedModels = 0;
				ShrinkRenderEnties();
			}

			public void PrepareCollecting()
			{
				NumUsedModels = 0;
			}

			public void Update(MatrixD gridWorldMatrix, float transparency)
			{
				foreach (MyRenderEntity renderEntity in RenderEntities)
				{
					renderEntity.Update(gridWorldMatrix, transparency);
				}
			}

			public void AddModel(int model, Matrix localMatrix, Vector3 colorMaskHsv, MyStringHash? skinId, float transparency)
			{
				RenderFlags renderFlags = (RenderFlags)0;
				renderFlags |= RenderFlags.Visible;
				if (skinId.HasValue)
				{
					renderFlags = ((!MyDefinitionManager.Static.GetAssetModifierDefinitionForRender(skinId.Value).MetalnessColorable) ? (renderFlags & ~RenderFlags.MetalnessColorable) : (renderFlags | RenderFlags.MetalnessColorable));
				}
				if (RenderEntities.Count < ++NumUsedModels)
				{
					AddRenderEntity(model, localMatrix, colorMaskHsv, skinId, transparency, renderFlags);
					return;
				}
				MyRenderEntity value = RenderEntities[NumUsedModels - 1];
				value.LocalMatrix = localMatrix;
				value.ColorMashHsv = colorMaskHsv;
				if (skinId.HasValue && skinId != value.SkinId)
				{
					RenderEntities.RemoveAt(NumUsedModels - 1);
					MyRenderProxy.RemoveRenderObject(value.RenderEntityId, MyRenderProxy.ObjectType.Entity);
					AddRenderEntity(model, localMatrix, colorMaskHsv, skinId, transparency, renderFlags);
				}
				else
				{
					value.UpdateSkin = false;
					RenderEntities[NumUsedModels - 1] = value;
				}
			}

			private void AddRenderEntity(int model, Matrix localMatrix, Vector3 colorMaskHsv, MyStringHash? skinId, float transparency, RenderFlags flags)
			{
				string byId = MyModel.GetById(model);
				uint renderEntityId = MyRenderProxy.CreateRenderEntity("Cube builder, part: " + model, byId, MatrixD.Identity, MyMeshDrawTechnique.MESH, flags, CullingOptions.Default, Vector3.One, colorMaskHsv, transparency, float.MaxValue, 0);
				MyRenderEntity myRenderEntity = default(MyRenderEntity);
				myRenderEntity.LocalMatrix = localMatrix;
				myRenderEntity.RenderEntityId = renderEntityId;
				myRenderEntity.ColorMashHsv = colorMaskHsv;
				myRenderEntity.SkinId = skinId.Value;
				myRenderEntity.UpdateSkin = true;
				MyRenderEntity item = myRenderEntity;
				RenderEntities.Add(item);
			}
		}

		private static MyObjectsPool<MyEntities> m_entitiesPool = new MyObjectsPool<MyEntities>(1);

		private Dictionary<int, MyEntities> m_allEntities = new Dictionary<int, MyEntities>();

		private List<int> m_tmpRemovedModels = new List<int>();

		private float Transparency = MyFakes.ENABLE_TRANSPARENT_CUBE_BUILDER ? 0.5f : 0f;

		public void UnloadRenderObjects()
		{
			foreach (KeyValuePair<int, MyEntities> allEntity in m_allEntities)
			{
				allEntity.Value.Clear();
			}
			m_allEntities.Clear();
		}

		public void BeginCollectingInstanceData()
		{
			foreach (KeyValuePair<int, MyEntities> allEntity in m_allEntities)
			{
				allEntity.Value.PrepareCollecting();
			}
		}

		public void AddInstance(int model, MatrixD matrix, ref MatrixD invGridWorldMatrix, Vector3 colorMaskHsv = default(Vector3), MyStringHash? skinId = null, Vector3UByte[] bones = null, float gridSize = 1f)
		{
			if (!m_allEntities.TryGetValue(model, out MyEntities value))
			{
				m_entitiesPool.AllocateOrCreate(out value);
				m_allEntities.Add(model, value);
			}
			value.AddModel(model, matrix * invGridWorldMatrix, colorMaskHsv, skinId, Transparency);
		}

		public void EndCollectingInstanceData(MatrixD gridWorldMatrix, bool useTransparency)
		{
			foreach (KeyValuePair<int, MyEntities> allEntity in m_allEntities)
			{
				allEntity.Value.ShrinkRenderEnties();
				if (allEntity.Value.IsEmpty())
				{
					m_tmpRemovedModels.Add(allEntity.Key);
				}
			}
			foreach (int tmpRemovedModel in m_tmpRemovedModels)
			{
				m_allEntities.Remove(tmpRemovedModel);
			}
			m_tmpRemovedModels.Clear();
			float transparency = useTransparency ? Transparency : 0f;
			foreach (KeyValuePair<int, MyEntities> allEntity2 in m_allEntities)
			{
				allEntity2.Value.Update(gridWorldMatrix, transparency);
			}
		}
	}
}
