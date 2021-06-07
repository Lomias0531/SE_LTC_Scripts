using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Voxels;
using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
	public static class IMyStorageExtensions
	{
		public static ContainmentType Intersect(this VRage.Game.Voxels.IMyStorage self, ref BoundingBox box, bool lazy = true)
		{
			BoundingBoxI box2 = new BoundingBoxI(box);
			return self.Intersect(ref box2, 0, lazy);
		}

		public static MyVoxelGeometry GetGeometry(this VRage.Game.Voxels.IMyStorage self)
		{
			return (self as MyStorageBase)?.Geometry;
		}

		public static void ClampVoxelCoord(this VRage.ModAPI.IMyStorage self, ref Vector3I voxelCoord, int distance = 1)
		{
			if (self != null)
			{
				Vector3I max = self.Size - distance;
				Vector3I.Clamp(ref voxelCoord, ref Vector3I.Zero, ref max, out voxelCoord);
			}
		}

		public static MyVoxelMaterialDefinition GetMaterialAt(this VRage.Game.Voxels.IMyStorage self, ref Vector3D localCoords)
		{
			Vector3I vector3I = Vector3D.Floor(localCoords / 1.0);
			MyStorageData myStorageData = new MyStorageData();
			myStorageData.Resize(Vector3I.One);
			self.ReadRange(myStorageData, MyStorageDataTypeFlags.Material, 0, vector3I, vector3I);
			return MyDefinitionManager.Static.GetVoxelMaterialDefinition(myStorageData.Material(0));
		}

		public static MyVoxelMaterialDefinition GetMaterialAt(this VRage.Game.Voxels.IMyStorage self, ref Vector3I voxelCoords)
		{
			MyStorageData myStorageData = new MyStorageData();
			myStorageData.Resize(Vector3I.One);
			self.ReadRange(myStorageData, MyStorageDataTypeFlags.ContentAndMaterial, 0, voxelCoords, voxelCoords);
			byte b = myStorageData.Material(0);
			if (b == byte.MaxValue)
			{
				return null;
			}
			return MyDefinitionManager.Static.GetVoxelMaterialDefinition(b);
		}

		public static void DebugDrawChunk(this VRage.Game.Voxels.IMyStorage self, Vector3I start, Vector3I end, Color? c = null)
		{
			if (!c.HasValue)
			{
				c = Color.Blue;
			}
			IEnumerable<MyVoxelBase> enumerable = MySession.Static.VoxelMaps.Instances.Where((MyVoxelBase x) => x.Storage == self);
			BoundingBoxD box = new BoundingBoxD(start, end + 1);
			box.Translate(-((Vector3D)self.Size * 0.5) - 0.5);
			foreach (MyVoxelBase item in enumerable)
			{
				MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(box, item.WorldMatrix), c.Value, 0.5f, depthRead: true, smooth: true);
			}
		}
	}
}
