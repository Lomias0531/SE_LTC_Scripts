using Sandbox.Engine.Voxels;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.Entities.Components;
using VRage.Factory;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Voxels.Clipmap;
using VRageMath;
using VRageRender;
using VRageRender.Voxels;

namespace Sandbox.Game.EntityComponents.Renders
{
	[MyDependency(typeof(MyVoxelMesherComponent), Critical = true)]
	public class MyRenderComponentVoxelMap : MyRenderComponent
	{
		public static class VoxelLoadingWaitStep
		{
			public static readonly HashSet<IMyLodController> Clipmaps = new HashSet<IMyLodController>();

			public static int Total;

			public static bool Complete => Clipmaps.Count == 0;

			public static float Progress => (float)Clipmaps.Count / (float)Total;

			public static void AddClipmap(IMyLodController controller)
			{
				lock (Clipmaps)
				{
					if (Clipmaps.Add(controller))
					{
						Total++;
						controller.Loaded += RemoveClipmap;
					}
				}
			}

			public static void RemoveClipmap(IMyLodController clipmap)
			{
				lock (Clipmaps)
				{
					if (Clipmaps.Remove(clipmap))
					{
						clipmap.Loaded -= RemoveClipmap;
					}
				}
				if (Complete)
				{
					MyRenderProxy.SendClipmapsReady();
				}
			}
		}

		private class Sandbox_Game_EntityComponents_Renders_MyRenderComponentVoxelMap_003C_003EActor : IActivator, IActivator<MyRenderComponentVoxelMap>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentVoxelMap();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentVoxelMap CreateInstance()
			{
				return new MyRenderComponentVoxelMap();
			}

			MyRenderComponentVoxelMap IActivator<MyRenderComponentVoxelMap>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public const string DefaultSettingsGroup = "Default";

		public const string PlanetSettingsGroup = "Planet";

		protected MyVoxelBase m_voxelMap;

		protected MyVoxelMesherComponent Mesher;

		public IMyLodController Clipmap
		{
			get;
			protected set;
		}

		public uint ClipmapId => m_renderObjectIDs[0];

		public static event Action TerrainQualityChange;

		static MyRenderComponentVoxelMap()
		{
			SetLodQuality((!MySandboxGame.Config.VoxelQuality.HasValue) ? MyRenderQualityEnum.NORMAL : MySandboxGame.Config.VoxelQuality.Value);
		}

		protected virtual IMyLodController CreateLodController()
		{
			MatrixD worldMatrix = MatrixD.CreateWorld(m_voxelMap.PositionLeftBottomCorner, m_voxelMap.Orientation.Forward, m_voxelMap.Orientation.Up);
			return new MyVoxelClipmap(m_voxelMap.Size, worldMatrix, Mesher, null, Vector3D.Zero, "Default");
		}

		public override void OnAddedToScene()
		{
			m_voxelMap = (base.Container.Entity as MyVoxelBase);
			Mesher = new MyVoxelMesherComponent();
			Mesher.SetContainer(base.Entity.Components);
			Mesher.OnAddedToScene();
			base.OnAddedToScene();
		}

		public void ResetLoading()
		{
			VoxelLoadingWaitStep.AddClipmap(Clipmap);
		}

		public override void AddRenderObjects()
		{
			if (Mesher != null)
			{
				MatrixD worldMatrix = MatrixD.CreateWorld(m_voxelMap.PositionLeftBottomCorner, m_voxelMap.Orientation.Forward, m_voxelMap.Orientation.Up);
				Clipmap = CreateLodController();
				VoxelLoadingWaitStep.AddClipmap(Clipmap);
				SetRenderObjectID(0, MyRenderProxy.RenderVoxelCreate(m_voxelMap.StorageName, worldMatrix, Clipmap, GetRenderFlags(), Transparency));
			}
		}

		public override void InvalidateRenderObjects()
		{
			if (base.Visible && m_renderObjectIDs[0] != uint.MaxValue)
			{
				MatrixD value = MatrixD.CreateWorld(m_voxelMap.PositionLeftBottomCorner, m_voxelMap.Orientation.Forward, m_voxelMap.Orientation.Up);
				MyRenderProxy.UpdateRenderObject(m_renderObjectIDs[0], value);
			}
		}

		public void UpdateCells()
		{
			if (m_renderObjectIDs[0] != uint.MaxValue)
			{
				MatrixD value = MatrixD.CreateWorld(m_voxelMap.PositionLeftBottomCorner, m_voxelMap.Orientation.Forward, m_voxelMap.Orientation.Up);
				MyRenderProxy.UpdateRenderObject(m_renderObjectIDs[0], value);
			}
		}

		public void InvalidateRange(Vector3I minVoxelChanged, Vector3I maxVoxelChanged)
		{
			minVoxelChanged -= 1;
			maxVoxelChanged += 1;
			m_voxelMap.Storage.ClampVoxelCoord(ref minVoxelChanged);
			m_voxelMap.Storage.ClampVoxelCoord(ref maxVoxelChanged);
			minVoxelChanged -= m_voxelMap.StorageMin;
			maxVoxelChanged -= m_voxelMap.StorageMin;
			if (Clipmap != null)
			{
				Clipmap.InvalidateRange(minVoxelChanged, maxVoxelChanged);
			}
		}

		public static void RefreshClipmapSettings()
		{
			if (MyEntities.IsLoaded)
			{
				foreach (MyEntity entity in MyEntities.GetEntities())
				{
					if (!entity.MarkedForClose)
					{
						MyRenderComponentVoxelMap myRenderComponentVoxelMap = entity.Render as MyRenderComponentVoxelMap;
						if (myRenderComponentVoxelMap != null)
						{
							MyVoxelClipmap myVoxelClipmap = myRenderComponentVoxelMap.Clipmap as MyVoxelClipmap;
							if (myVoxelClipmap != null)
							{
								MyVoxelClipmapSettings settings = MyVoxelClipmapSettings.GetSettings(myVoxelClipmap.SettingsGroup);
								myVoxelClipmap.UpdateSettings(settings);
							}
						}
					}
				}
			}
		}

		public static void SetLodQuality(MyRenderQualityEnum quality)
		{
			MyVoxelClipmapSettings.SetSettingsForGroup("Default", MyVoxelClipmapSettingsPresets.NormalSettings[(int)quality]);
			MyVoxelClipmapSettings.SetSettingsForGroup("Planet", MyVoxelClipmapSettingsPresets.PlanetSettings[(int)quality]);
			RefreshClipmapSettings();
			if (MyRenderComponentVoxelMap.TerrainQualityChange != null)
			{
				MyRenderComponentVoxelMap.TerrainQualityChange();
			}
		}
	}
}
