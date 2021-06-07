using Havok;
using Sandbox.Game.GameSystems;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Groups;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	public class MyGridPhysicalGroupData : IGroupData<MyCubeGrid>
	{
		public struct GroupSharedPxProperties
		{
			public readonly int GridCount;

			public readonly MyCubeGrid ReferenceGrid;

			public readonly HkMassProperties PxProperties;

			public Matrix InertiaTensor => PxProperties.InertiaTensor;

			public float Mass => PxProperties.Mass;

			public Vector3D CoMWorld
			{
				get
				{
					Vector3 position = PxProperties.CenterOfMass;
					MatrixD matrix = ReferenceGrid.WorldMatrix;
					Vector3D.Transform(ref position, ref matrix, out Vector3D result);
					return result;
				}
			}

			public GroupSharedPxProperties(MyCubeGrid referenceGrid, HkMassProperties sharedProperties, int gridCount)
			{
				GridCount = gridCount;
				ReferenceGrid = referenceGrid;
				PxProperties = sharedProperties;
			}

			public Matrix GetInertiaTensorLocalToGrid(MyCubeGrid localGrid)
			{
				MatrixD matrix = InertiaTensor;
				if (ReferenceGrid != localGrid)
				{
					MatrixD matrix2 = ReferenceGrid.WorldMatrix * localGrid.PositionComp.WorldMatrixNormalizedInv;
					MatrixD.Multiply(ref matrix, ref matrix2, out matrix);
				}
				return matrix;
			}
		}

		private volatile Ref<GroupSharedPxProperties> m_groupPropertiesCache;

		private MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group m_group;

		internal readonly MyGroupControlSystem ControlSystem = new MyGroupControlSystem();

		public static GroupSharedPxProperties GetGroupSharedProperties(MyCubeGrid localGrid, bool checkMultithreading = true)
		{
			MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(localGrid);
			if (group == null)
			{
				HkMassProperties valueOrDefault = GetGridMassProperties(localGrid).GetValueOrDefault();
				return new GroupSharedPxProperties(localGrid, valueOrDefault, 1);
			}
			return group.GroupData.GetSharedPxProperties(localGrid);
		}

		public static void InvalidateSharedMassPropertiesCache(MyCubeGrid groupRepresentative)
		{
			MyCubeGridGroups.Static.Physical.GetGroup(groupRepresentative)?.GroupData.InvalidateCoMCache();
		}

		private GroupSharedPxProperties GetSharedPxProperties(MyCubeGrid referenceGrid)
		{
			Ref<GroupSharedPxProperties> @ref = m_groupPropertiesCache;
			if (@ref == null)
			{
				HashSetReader<MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> nodes = m_group.Nodes;
				MatrixD worldMatrixNormalizedInv = referenceGrid.PositionComp.WorldMatrixNormalizedInv;
				int count = nodes.Count;
				HkMassElement[] array = ArrayPool<HkMassElement>.Shared.Rent(count);
				Span<HkMassElement> elements = new Span<HkMassElement>(array, 0, count);
				int length = 0;
				foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node item in nodes)
				{
					MyCubeGrid nodeData = item.NodeData;
					HkMassProperties? gridMassProperties = GetGridMassProperties(nodeData);
					if (gridMassProperties.HasValue)
					{
						MatrixD m = nodeData.PositionComp.WorldMatrix * worldMatrixNormalizedInv;
						elements[length++] = new HkMassElement
						{
							Tranform = m,
							Properties = gridMassProperties.Value
						};
					}
				}
				elements = elements.Slice(0, length);
				HkInertiaTensorComputer.CombineMassProperties(elements, out HkMassProperties massProperties);
				ArrayPool<HkMassElement>.Shared.Return(array);
				@ref = (m_groupPropertiesCache = Ref.Create(new GroupSharedPxProperties(referenceGrid, massProperties, nodes.Count)));
			}
			return @ref.Value;
		}

		private static void DrawDebugSphere(MyCubeGrid referenceGrid, Color color, Vector3 localPosition, double radius)
		{
			MyRenderProxy.DebugDrawSphere(Vector3D.Transform(localPosition, referenceGrid.PositionComp.WorldMatrix), (float)radius, color, 1f, depthRead: false);
		}

		private void InvalidateCoMCache()
		{
			m_groupPropertiesCache = null;
		}

		private static HkMassProperties? GetGridMassProperties(MyCubeGrid grid)
		{
			if (grid.Physics == null)
			{
				return null;
			}
			return grid.Physics.Shape.MassProperties;
		}

		public void OnNodeAdded(MyCubeGrid entity)
		{
			InvalidateCoMCache();
			entity.OnAddedToGroup(this);
		}

		public void OnNodeRemoved(MyCubeGrid entity)
		{
			InvalidateCoMCache();
			entity.OnRemovedFromGroup(this);
		}

		internal static bool IsMajorGroup(MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group a, MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group b)
		{
			float num = 0f;
			foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in a.Nodes)
			{
				if (node.NodeData.Physics != null)
				{
					num += node.NodeData.PositionComp.LocalVolume.Radius;
				}
			}
			foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node2 in b.Nodes)
			{
				if (node2.NodeData.Physics != null)
				{
					num -= node2.NodeData.PositionComp.LocalVolume.Radius;
				}
			}
			return num > 0f;
		}

		public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
			m_group = (group as MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group);
		}

		public void OnRelease()
		{
			m_group = null;
		}

		[DebuggerStepThrough]
		[Conditional("DEBUG")]
		private static void AssertThread()
		{
			_ = MySandboxGame.Static.UpdateThread;
			_ = Thread.CurrentThread;
		}
	}
}
