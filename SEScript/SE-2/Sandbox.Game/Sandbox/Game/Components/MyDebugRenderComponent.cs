using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Components
{
	public class MyDebugRenderComponent : MyDebugRenderComponentBase
	{
		protected MyEntity Entity;

		public MyDebugRenderComponent(IMyEntity entity)
		{
			Entity = (MyEntity)entity;
		}

		public override void DebugDrawInvalidTriangles()
		{
			if (Entity == null)
			{
				return;
			}
			foreach (MyHierarchyComponentBase child in Entity.Hierarchy.Children)
			{
				child.Container.Entity.DebugDrawInvalidTriangles();
			}
			if (Entity.Render.GetModel() == null)
			{
				return;
			}
			int trianglesCount = Entity.Render.GetModel().GetTrianglesCount();
			for (int i = 0; i < trianglesCount; i++)
			{
				MyTriangleVertexIndices triangle = Entity.Render.GetModel().GetTriangle(i);
				if (MyUtils.IsWrongTriangle(Entity.Render.GetModel().GetVertex(triangle.I0), Entity.Render.GetModel().GetVertex(triangle.I1), Entity.Render.GetModel().GetVertex(triangle.I2)))
				{
					Vector3 vector = Vector3.Transform(Entity.Render.GetModel().GetVertex(triangle.I0), Entity.PositionComp.WorldMatrix);
					Vector3 vector2 = Vector3.Transform(Entity.Render.GetModel().GetVertex(triangle.I1), Entity.PositionComp.WorldMatrix);
					Vector3 vector3 = Vector3.Transform(Entity.Render.GetModel().GetVertex(triangle.I2), Entity.PositionComp.WorldMatrix);
					MyRenderProxy.DebugDrawLine3D(vector, vector2, Color.Purple, Color.Purple, depthRead: false);
					MyRenderProxy.DebugDrawLine3D(vector2, vector3, Color.Purple, Color.Purple, depthRead: false);
					MyRenderProxy.DebugDrawLine3D(vector3, vector, Color.Purple, Color.Purple, depthRead: false);
					Vector3 vector4 = (vector + vector2 + vector3) / 3f;
					MyRenderProxy.DebugDrawLine3D(vector4, vector4 + Vector3.UnitX, Color.Yellow, Color.Yellow, depthRead: false);
					MyRenderProxy.DebugDrawLine3D(vector4, vector4 + Vector3.UnitY, Color.Yellow, Color.Yellow, depthRead: false);
					MyRenderProxy.DebugDrawLine3D(vector4, vector4 + Vector3.UnitZ, Color.Yellow, Color.Yellow, depthRead: false);
				}
			}
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_MODEL_DUMMIES)
			{
				DebugDrawDummies(Entity.Render.GetModel());
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_ENTITY_IDS && (Entity.Parent == null || !MyDebugDrawSettings.DEBUG_DRAW_ENTITY_IDS_ONLY_ROOT))
			{
				MyRenderProxy.DebugDrawText3D(Entity.PositionComp.WorldMatrix.Translation, Entity.EntityId.ToString("X16"), Color.White, 0.6f, depthRead: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS && Entity.Physics != null)
			{
				Entity.Physics.DebugDraw();
			}
		}

		protected void DebugDrawDummies(MyModel model)
		{
			if (model != null)
			{
				float num = 0f;
				Vector3D value = Vector3D.Zero;
				if (MySector.MainCamera != null)
				{
					num = MyDebugDrawSettings.DEBUG_DRAW_MODEL_DUMMIES_DISTANCE * MyDebugDrawSettings.DEBUG_DRAW_MODEL_DUMMIES_DISTANCE;
					value = MySector.MainCamera.WorldMatrix.Translation;
				}
				foreach (KeyValuePair<string, MyModelDummy> dummy in model.Dummies)
				{
					MatrixD matrix = (MatrixD)dummy.Value.Matrix * Entity.PositionComp.WorldMatrix;
					if (num == 0f || !(Vector3D.DistanceSquared(value, matrix.Translation) > (double)num))
					{
						MyRenderProxy.DebugDrawText3D(matrix.Translation, dummy.Key, Color.White, 0.7f, depthRead: false);
						MyRenderProxy.DebugDrawAxis(MatrixD.Normalize(matrix), 0.1f, depthRead: false);
						MyRenderProxy.DebugDrawOBB(matrix, Vector3.One, 0.1f, depthRead: false, smooth: false);
					}
				}
			}
		}
	}
}
