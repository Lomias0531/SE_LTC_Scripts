using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game
{
	public class MyDecals : IMyDecalHandler
	{
		private const string DEFAULT = "Default";

		[ThreadStatic]
		private static MyCubeGrid.MyCubeGridHitInfo m_gridHitInfo;

		private static readonly MyDecals m_handler = new MyDecals();

		private MyDecals()
		{
		}

		public static void HandleAddDecal(IMyEntity entity, MyHitInfo hitInfo, MyStringHash material = default(MyStringHash), MyStringHash source = default(MyStringHash), object customdata = null, float damage = -1f)
		{
			IMyDecalProxy myDecalProxy = entity as IMyDecalProxy;
			if (myDecalProxy != null)
			{
				myDecalProxy.AddDecals(ref hitInfo, source, customdata, m_handler, material);
				return;
			}
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			MyCubeBlock myCubeBlock = entity as MyCubeBlock;
			MySlimBlock mySlimBlock = null;
			if (myCubeBlock != null)
			{
				myCubeGrid = myCubeBlock.CubeGrid;
				mySlimBlock = myCubeBlock.SlimBlock;
			}
			else if (myCubeGrid != null)
			{
				mySlimBlock = myCubeGrid.GetTargetedBlock(hitInfo.Position - 0.001f * hitInfo.Normal);
			}
			if (myCubeGrid != null)
			{
				if (mySlimBlock != null && !mySlimBlock.BlockDefinition.PlaceDecals)
				{
					return;
				}
				MyCubeGrid.MyCubeGridHitInfo myCubeGridHitInfo = customdata as MyCubeGrid.MyCubeGridHitInfo;
				if (myCubeGridHitInfo == null)
				{
					if (mySlimBlock == null)
					{
						return;
					}
					if (m_gridHitInfo == null)
					{
						m_gridHitInfo = new MyCubeGrid.MyCubeGridHitInfo();
					}
					m_gridHitInfo.Position = mySlimBlock.Position;
					customdata = m_gridHitInfo;
				}
				else
				{
					if (!myCubeGrid.TryGetCube(myCubeGridHitInfo.Position, out MyCube cube))
					{
						return;
					}
					mySlimBlock = cube.CubeBlock;
				}
				MyCompoundCubeBlock myCompoundCubeBlock = (mySlimBlock != null) ? (mySlimBlock.FatBlock as MyCompoundCubeBlock) : null;
				myDecalProxy = ((myCompoundCubeBlock != null) ? ((IMyDecalProxy)myCompoundCubeBlock) : ((IMyDecalProxy)mySlimBlock));
			}
			myDecalProxy?.AddDecals(ref hitInfo, source, customdata, m_handler, material);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void UpdateDecals(List<MyDecalPositionUpdate> decals)
		{
			MyRenderProxy.UpdateDecals(decals);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void RemoveDecal(uint decalId)
		{
			MyRenderProxy.RemoveDecal(decalId);
		}

		void IMyDecalHandler.AddDecal(ref MyDecalRenderInfo data, List<uint> ids)
		{
			if (data.RenderObjectIds == null)
			{
				return;
			}
			IReadOnlyList<MyDecalMaterial> decalMaterials;
			bool flag = MyDecalMaterials.TryGetDecalMaterial(data.Source.String, data.Material.String, out decalMaterials);
			if (!flag)
			{
				if (MyFakes.ENABLE_USE_DEFAULT_DAMAGE_DECAL)
				{
					flag = MyDecalMaterials.TryGetDecalMaterial("Default", "Default", out decalMaterials);
				}
				if (!flag)
				{
					return;
				}
			}
			MyDecalBindingInfo myDecalBindingInfo2;
			if (!data.Binding.HasValue)
			{
				MyDecalBindingInfo myDecalBindingInfo = default(MyDecalBindingInfo);
				myDecalBindingInfo.Position = data.Position;
				myDecalBindingInfo.Normal = data.Normal;
				myDecalBindingInfo.Transformation = Matrix.Identity;
				myDecalBindingInfo2 = myDecalBindingInfo;
			}
			else
			{
				myDecalBindingInfo2 = data.Binding.Value;
			}
			int num = (int)Math.Round(MyRandom.Instance.NextFloat() * (float)(decalMaterials.Count - 1));
			MyDecalMaterial myDecalMaterial = decalMaterials[num];
			float num2 = myDecalMaterial.Rotation;
			if (float.IsPositiveInfinity(myDecalMaterial.Rotation))
			{
				num2 = MyRandom.Instance.NextFloat() * (MathF.PI * 2f);
			}
			Vector3 vector = Vector3.CalculatePerpendicularVector(myDecalBindingInfo2.Normal);
			if (num2 != 0f)
			{
				Quaternion quaternion = Quaternion.CreateFromAxisAngle(myDecalBindingInfo2.Normal, num2);
				vector = new Vector3((new Quaternion(vector, 0f) * quaternion).ToVector4());
			}
			vector = Vector3.Normalize(vector);
			float num3 = myDecalMaterial.MinSize;
			if (myDecalMaterial.MaxSize > myDecalMaterial.MinSize)
			{
				num3 += MyRandom.Instance.NextFloat() * (myDecalMaterial.MaxSize - myDecalMaterial.MinSize);
			}
			float depth = myDecalMaterial.Depth;
			Vector3 scales = new Vector3(num3, num3, depth);
			MyDecalTopoData data2 = default(MyDecalTopoData);
			Matrix matrix;
			Vector3 v;
			if (data.Flags.HasFlag(MyDecalFlags.World))
			{
				matrix = Matrix.CreateWorld(Vector3.Zero, myDecalBindingInfo2.Normal, vector);
				v = data.Position;
			}
			else
			{
				matrix = Matrix.CreateWorld(myDecalBindingInfo2.Position - myDecalBindingInfo2.Normal * depth * 0.45f, myDecalBindingInfo2.Normal, vector);
				v = Vector3.Invalid;
			}
			data2.MatrixBinding = Matrix.CreateScale(scales) * matrix;
			data2.WorldPosition = v;
			data2.MatrixCurrent = myDecalBindingInfo2.Transformation * data2.MatrixBinding;
			data2.BoneIndices = data.BoneIndices;
			data2.BoneWeights = data.BoneWeights;
			MyDecalFlags myDecalFlags = myDecalMaterial.Transparent ? MyDecalFlags.Transparent : MyDecalFlags.None;
			string stringId = MyDecalMaterials.GetStringId(data.Source, data.Material);
			uint item = MyRenderProxy.CreateDecal((uint[])data.RenderObjectIds.Clone(), ref data2, data.Flags | myDecalFlags, stringId, myDecalMaterial.StringId, num);
			ids?.Add(item);
		}
	}
}
