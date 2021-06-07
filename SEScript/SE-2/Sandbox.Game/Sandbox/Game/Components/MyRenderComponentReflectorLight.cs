using Sandbox.Game.Entities;
using Sandbox.Game.Lights;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyRenderComponentReflectorLight : MyRenderComponentLight
	{
		private class Sandbox_Game_Components_MyRenderComponentReflectorLight_003C_003EActor
		{
		}

		private const float RADIUS_TO_CONE_MULTIPLIER = 0.25f;

		private const float SMALL_LENGTH_MULTIPLIER = 0.5f;

		private MyReflectorLight m_reflectorLight;

		private List<MyLight> m_lights;

		public MyRenderComponentReflectorLight(List<MyLight> lights)
		{
			m_lights = lights;
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_reflectorLight = (base.Container.Entity as MyReflectorLight);
		}

		public override void AddRenderObjects()
		{
			base.AddRenderObjects();
			BoundingBox localAABB = m_reflectorLight.PositionComp.LocalAABB;
			localAABB.Inflate(m_reflectorLight.IsLargeLight ? 3f : 1f);
			float num = m_reflectorLight.ReflectorRadiusBounds.Max * 0.25f;
			if (!m_reflectorLight.IsLargeLight)
			{
				num *= 0.5f;
			}
			localAABB = localAABB.Include(new Vector3(0f, 0f, 0f - num));
			MyRenderProxy.UpdateRenderObject(m_renderObjectIDs[0], null, localAABB);
		}

		public override void Draw()
		{
			base.Draw();
			if (m_reflectorLight.IsReflectorEnabled)
			{
				DrawReflectorCone();
			}
		}

		private void DrawReflectorCone()
		{
			if (!string.IsNullOrEmpty(m_reflectorLight.ReflectorConeMaterial))
			{
				foreach (MyLight light in m_lights)
				{
					Vector3 vector = Vector3.Normalize(MySector.MainCamera.Position - m_reflectorLight.PositionComp.GetPosition());
					Vector3.TransformNormal(light.ReflectorDirection, m_reflectorLight.PositionComp.WorldMatrix);
					float num = Math.Abs(Vector3.Dot(vector, light.ReflectorDirection));
					float scaleFactor = MathHelper.Saturate(1f - (float)Math.Pow(num, 30.0));
					uint parentID = light.ParentID;
					Vector3D position = light.Position;
					Vector3D v = light.ReflectorDirection;
					float num2 = Math.Max(15f, m_reflectorLight.ReflectorRadius * 0.25f);
					if (!m_reflectorLight.IsLargeLight)
					{
						num2 *= 0.5f;
					}
					float reflectorThickness = m_reflectorLight.BlockDefinition.ReflectorThickness;
					Color color = m_reflectorLight.Color;
					float n = m_reflectorLight.CurrentLightPower * m_reflectorLight.Intensity * 0.8f;
					MyTransparentGeometry.AddLocalLineBillboard(MyStringId.GetOrCompute(m_reflectorLight.ReflectorConeMaterial), color.ToVector4() * scaleFactor * MathHelper.Saturate(n), position, parentID, v, num2, reflectorThickness, MyBillboard.BlendTypeEnum.AdditiveBottom);
				}
			}
		}
	}
}
