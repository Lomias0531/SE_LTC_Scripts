using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Voxels.Clipmap;
using VRageMath;
using VRageRender;
using VRageRender.Import;
using VRageRender.Messages;
using VRageRender.Voxels;

namespace Sandbox.Game.Components
{
	internal class MyRenderComponentPlanet : MyRenderComponentVoxelMap
	{
		private class Sandbox_Game_Components_MyRenderComponentPlanet_003C_003EActor : IActivator, IActivator<MyRenderComponentPlanet>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentPlanet();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentPlanet CreateInstance()
			{
				return new MyRenderComponentPlanet();
			}

			MyRenderComponentPlanet IActivator<MyRenderComponentPlanet>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyPlanet m_planet;

		private int m_shadowHelperRenderObjectIndex = -1;

		private int m_atmosphereRenderIndex = -1;

		private readonly List<int> m_cloudLayerRenderObjectIndexList = new List<int>();

		private int m_fogUpdateCounter;

		private static bool lastSentFogFlag = true;

		private bool m_oldNeedsDraw;

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_planet = (base.Entity as MyPlanet);
			m_oldNeedsDraw = NeedsDraw;
			NeedsDraw = true;
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			NeedsDraw = m_oldNeedsDraw;
			m_planet = null;
		}

		protected override IMyLodController CreateLodController()
		{
			MatrixD worldMatrix = MatrixD.CreateWorld(m_voxelMap.PositionLeftBottomCorner, m_voxelMap.Orientation.Forward, m_voxelMap.Orientation.Up);
			return new MyVoxelClipmap(m_voxelMap.Size, worldMatrix, Mesher, m_planet.AverageRadius, m_planet.PositionComp.GetPosition(), "Planet")
			{
				Cache = MyVoxelClipmapCache.Instance
			};
		}

		public override void AddRenderObjects()
		{
			base.AddRenderObjects();
			int num = base.RenderObjectIDs.Length;
			ResizeRenderObjectArray(16);
			int num2 = num;
			_ = m_planet.PositionLeftBottomCorner;
			Vector3 atmosphereWavelengths = default(Vector3);
			atmosphereWavelengths.X = 1f / (float)Math.Pow(m_planet.AtmosphereWavelengths.X, 4.0);
			atmosphereWavelengths.Y = 1f / (float)Math.Pow(m_planet.AtmosphereWavelengths.Y, 4.0);
			atmosphereWavelengths.Z = 1f / (float)Math.Pow(m_planet.AtmosphereWavelengths.Z, 4.0);
			_ = base.Entity;
			if (m_planet.HasAtmosphere)
			{
				MatrixD worldMatrix = MatrixD.Identity * m_planet.AtmosphereRadius;
				worldMatrix.M44 = 1.0;
				worldMatrix.Translation = base.Entity.PositionComp.GetPosition();
				m_atmosphereRenderIndex = num2;
				SetRenderObjectID(num2++, MyRenderProxy.CreateRenderEntityAtmosphere(base.Entity.GetFriendlyName() + " " + base.Entity.EntityId, "Models\\Environment\\Atmosphere_sphere.mwm", worldMatrix, MyMeshDrawTechnique.ATMOSPHERE, RenderFlags.Visible | RenderFlags.DrawOutsideViewDistance, GetRenderCullingOptions(), m_planet.AtmosphereRadius, m_planet.AverageRadius, atmosphereWavelengths, 0f, float.MaxValue, FadeIn));
				UpdateAtmosphereSettings(m_planet.AtmosphereSettings);
			}
			m_shadowHelperRenderObjectIndex = num2;
			MatrixD worldMatrix2 = MatrixD.CreateScale(m_planet.MinimumRadius);
			worldMatrix2.Translation = m_planet.WorldMatrix.Translation;
			SetRenderObjectID(num2++, MyRenderProxy.CreateRenderEntity("Shadow helper", "Models\\Environment\\Sky\\ShadowHelperSphere.mwm", worldMatrix2, MyMeshDrawTechnique.MESH, RenderFlags.CastShadows | RenderFlags.Visible | RenderFlags.DrawOutsideViewDistance | RenderFlags.NoBackFaceCulling | RenderFlags.SkipInMainView | RenderFlags.CastShadowsOnLow | RenderFlags.SkipInForward, CullingOptions.Default, Color.White, new Vector3(1f, 1f, 1f), 0f, float.MaxValue, 0, 1f, FadeIn));
			MyPlanetGeneratorDefinition generator = m_planet.Generator;
			if (MyFakes.ENABLE_PLANETARY_CLOUDS && generator != null && generator.CloudLayers != null)
			{
				foreach (MyCloudLayerSettings cloudLayer in generator.CloudLayers)
				{
					double num3 = (double)(m_planet.AverageRadius + m_planet.MaximumRadius) / 2.0;
					double altitude = num3 + ((double)m_planet.MaximumRadius - num3) * (double)cloudLayer.RelativeAltitude;
					Vector3D rotationAxis = Vector3D.Normalize((cloudLayer.RotationAxis == Vector3D.Zero) ? Vector3D.Up : cloudLayer.RotationAxis);
					int num4 = num2 + m_cloudLayerRenderObjectIndexList.Count;
					SetRenderObjectID(num4, MyRenderProxy.CreateRenderEntityCloudLayer((m_atmosphereRenderIndex != -1) ? m_renderObjectIDs[m_atmosphereRenderIndex] : uint.MaxValue, base.Entity.GetFriendlyName() + " " + base.Entity.EntityId, cloudLayer.Model, cloudLayer.Textures, base.Entity.PositionComp.GetPosition(), altitude, num3, cloudLayer.ScalingEnabled, cloudLayer.FadeOutRelativeAltitudeStart, cloudLayer.FadeOutRelativeAltitudeEnd, cloudLayer.ApplyFogRelativeDistance, m_planet.MaximumRadius, rotationAxis, cloudLayer.AngularVelocity, cloudLayer.InitialRotation, cloudLayer.Color.ToLinearRGB(), FadeIn));
					m_cloudLayerRenderObjectIndexList.Add(num4);
				}
				num2 += generator.CloudLayers.Count;
			}
		}

		public override void Draw()
		{
			if (m_oldNeedsDraw)
			{
				base.Draw();
			}
			double num = Vector3D.Distance(MySector.MainCamera.Position, m_planet.WorldMatrix.Translation);
			MatrixD value = MatrixD.CreateScale((double)m_planet.MinimumRadius * Math.Min(num / (double)m_planet.MinimumRadius, 1.0) * 0.996999979019165);
			value.Translation = m_planet.PositionComp.WorldMatrix.Translation;
			MyRenderProxy.UpdateRenderObject(m_renderObjectIDs[m_shadowHelperRenderObjectIndex], value);
			DrawFog();
		}

		private void DrawFog()
		{
			if (!MyFakes.ENABLE_CLOUD_FOG || m_fogUpdateCounter-- > 0)
			{
				return;
			}
			m_fogUpdateCounter = (int)(100f * (0.8f + MyRandom.Instance.NextFloat() * 0.4f));
			Vector3D position = MySector.MainCamera.Position;
			Vector3D position2 = m_planet.PositionComp.GetPosition();
			double num = m_planet.AtmosphereRadius * 2f;
			if (!((position - position2).LengthSquared() > num * num))
			{
				m_fogUpdateCounter = (int)((float)m_fogUpdateCounter * 0.67f);
				bool flag = !IsPointInAirtightSpace(position);
				if (lastSentFogFlag != flag)
				{
					lastSentFogFlag = flag;
					MyRenderProxy.UpdateCloudLayerFogFlag(flag);
				}
			}
		}

		public void UpdateAtmosphereSettings(MyAtmosphereSettings settings)
		{
			MyRenderProxy.UpdateAtmosphereSettings(m_renderObjectIDs[m_atmosphereRenderIndex], settings);
		}

		private bool IsPointInAirtightSpace(Vector3D worldPosition)
		{
			if (!MySession.Static.Settings.EnableOxygen)
			{
				return true;
			}
			bool result = false;
			BoundingSphereD boundingSphere = new BoundingSphereD(worldPosition, 0.1);
			List<MyEntity> list = null;
			try
			{
				list = MyEntities.GetEntitiesInSphere(ref boundingSphere);
				foreach (MyEntity item in list)
				{
					MyCubeGrid myCubeGrid = item as MyCubeGrid;
					if (myCubeGrid != null && myCubeGrid.GridSystems.GasSystem != null)
					{
						MyOxygenBlock safeOxygenBlock = myCubeGrid.GridSystems.GasSystem.GetSafeOxygenBlock(worldPosition);
						if (safeOxygenBlock != null && safeOxygenBlock.Room != null && safeOxygenBlock.Room.IsAirtight)
						{
							return true;
						}
					}
				}
				return result;
			}
			finally
			{
				list?.Clear();
			}
		}
	}
}
