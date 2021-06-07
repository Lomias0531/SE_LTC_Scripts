using System;
using System.Diagnostics;
using VRage.Collections;
using VRage.Game.Voxels;
using VRage.Network;
using VRage.Utils;
using VRage.Voxels.Mesh;
using VRage.Voxels.Sewing;
using VRageMath;
using VRageRender;
using VRageRender.Voxels;

namespace VRage.Voxels.Clipmap
{
	public sealed class MyClipmapSewJob : MyPrecalcJob
	{
		private class VRage_Voxels_Clipmap_MyClipmapSewJob_003C_003EActor : IActivator, IActivator<MyClipmapSewJob>
		{
			private sealed override object CreateInstance()
			{
				return new MyClipmapSewJob();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyClipmapSewJob CreateInstance()
			{
				return new MyClipmapSewJob();
			}

			MyClipmapSewJob IActivator<MyClipmapSewJob>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public static bool DebugDrawDependencies = false;

		public static bool DebugDrawGeneration = false;

		public static VrTailor.GeneratedVertexProtocol GeneratedVertexProtocol = VrTailor.GeneratedVertexProtocol.Dynamic;

		private static readonly MyConcurrentPool<MyClipmapSewJob> m_instancePool = new MyConcurrentPool<MyClipmapSewJob>(16);

		private MyVoxelRenderCellData m_cellData;

		private bool m_forceStitchCommit;

		private volatile bool m_isCancelled;

		/// <summary>
		/// Target clipmap.
		/// </summary>
		public MyVoxelClipmap Clipmap
		{
			get;
			private set;
		}

		/// <summary>
		/// Clipmap cell.
		/// </summary>
		public MyCellCoord Cell
		{
			get;
			private set;
		}

		/// <summary>
		/// Set of neghbouring meshes.
		/// </summary>
		internal MyVoxelClipmap.StitchOperation Operation
		{
			get;
			private set;
		}

		/// <summary>
		/// Work tracker.
		/// </summary>
		public MyWorkTracker<MyCellCoord, MyClipmapSewJob> WorkTracker
		{
			get;
			private set;
		}

		public override bool IsCanceled => m_isCancelled;

		public override int Priority
		{
			get
			{
				if (!m_isCancelled)
				{
					return Cell.Lod;
				}
				return int.MaxValue;
			}
		}

		public MyClipmapSewJob()
			: base(enableCompletionCallback: true)
		{
		}

		internal static bool Start(MyWorkTracker<MyCellCoord, MyClipmapSewJob> tracker, MyVoxelClipmap clipmap, MyVoxelClipmap.StitchOperation operation)
		{
			if (tracker == null)
			{
				throw new ArgumentNullException("tracker");
			}
			if (clipmap == null)
			{
				throw new ArgumentNullException("clipmap");
			}
			if (operation == null)
			{
				throw new ArgumentNullException("operation");
			}
			if (tracker.Exists(operation.Cell))
			{
				MyLog.Default.Error("A Stitch job for cell {0} is already scheduled.", operation.Cell);
				return false;
			}
			MyClipmapSewJob myClipmapSewJob = m_instancePool.Get();
			myClipmapSewJob.m_isCancelled = false;
			myClipmapSewJob.Clipmap = clipmap;
			myClipmapSewJob.WorkTracker = tracker;
			myClipmapSewJob.Operation = operation;
			myClipmapSewJob.Cell = operation.Cell;
			return myClipmapSewJob.Enqueue();
		}

		private bool Enqueue()
		{
			m_forceStitchCommit = true;
			WorkTracker.Add(Cell, this);
			if (!MyPrecalcComponent.EnqueueBack(this))
			{
				WorkTracker.Complete(Cell);
				return false;
			}
			return true;
		}

		public override void DoWork()
		{
			try
			{
				if (m_isCancelled || !IsValid)
				{
					Clipmap.CommitStitchOperation(Operation);
					WorkTracker.Complete(Cell);
				}
				else
				{
					MyIsoMeshTaylor.NativeInstance.SetDebug(DebugDrawGeneration || GeneratedVertexProtocol != VrTailor.GeneratedVertexProtocol.Dynamic);
					MyIsoMeshTaylor.NativeInstance.SetGenerate(GeneratedVertexProtocol);
					if (Clipmap.InstanceStitchMode != MyVoxelClipmap.StitchMode.Stitch)
					{
						goto IL_010f;
					}
					if (Operation.Guides[0] != null)
					{
						if (Operation.Guides[0].Sewn)
						{
							Operation.Guides[0].Reset();
						}
						MyIsoMeshTaylor.NativeInstance.Sew(Operation.Guides, Operation.Operation);
						if (DebugDrawDependencies)
						{
							DebugDrawStitch(Operation.Guides);
						}
						MyVoxelClipmap.CompoundStitchOperation compound = Operation.GetCompound();
						if (compound != null && compound.Children.Count > 0)
						{
							SewChildren(compound);
						}
						goto IL_010f;
					}
				}
				goto end_IL_0000;
				IL_010f:
				if (Operation.Guides[0].Mesh != null)
				{
					MyRenderDataBuilder.Instance.Build(Operation.Guides[0].Mesh, out m_cellData, Clipmap.VoxelRenderDataProcessorProvider);
				}
				Clipmap.UpdateCellRender(Cell, Operation, ref m_cellData);
				WorkTracker.Complete(Cell);
				m_forceStitchCommit = false;
				end_IL_0000:;
			}
			finally
			{
			}
		}

		/// <summary>
		/// Sew together child meshes.
		/// </summary>
		/// <param name="compound"></param>
		private void SewChildren(MyVoxelClipmap.CompoundStitchOperation compound)
		{
			foreach (MyVoxelClipmap.StitchOperation child in compound.Children)
			{
				for (int i = 0; i < 8; i++)
				{
					MyCellCoord myCellCoord = MyVoxelClipmap.MakeFulfilled(child.Dependencies[i]);
					if (child.Guides[i] == null)
					{
						_ = myCellCoord.Lod;
						_ = compound.Cell.Lod;
					}
				}
				MyIsoMeshTaylor.NativeInstance.Sew(child.Guides, child.Operation, child.Range.Value.Min, child.Range.Value.Max);
				if (DebugDrawDependencies)
				{
					DebugDrawStitch(child.Guides);
				}
			}
		}

		private void DebugDrawStitch(VrSewGuide[] meshes)
		{
			Vector3D center = GetCenter(meshes[0]);
			for (int i = 0; i < 8; i++)
			{
				if (meshes[i] != null)
				{
					MyRenderProxy.DebugDrawArrow3D(center, GetCenter(meshes[i]), Color.Red, Color.Green, depthRead: true, 0.1, null, 0.5f, persistent: true);
				}
			}
		}

		[Conditional("DEBUG")]
		private unsafe void DebugDrawGenerated(VrSewGuide[] meshes)
		{
			if (!DebugDrawGeneration)
			{
				return;
			}
			MyIsoMeshTaylor.NativeInstance.DebugReadStudied(out VrTailor.VertexRef* studiedVertices, out int count);
			MyIsoMeshTaylor.NativeInstance.DebugReadGenerated(out ushort* generatedVertices, out int count2);
			MyIsoMeshTaylor.NativeInstance.DebugReadRemapped(out VrTailor.RemappedVertex* remappedVertices, out int count3);
			for (int i = 0; i < count; i++)
			{
				VrSewGuide vrSewGuide = meshes[studiedVertices[i].Mesh];
				VrVoxelVertex vrVoxelVertex = vrSewGuide.Mesh.Vertices[(int)studiedVertices[i].Index];
				MyRenderProxy.DebugDrawText3D(Position(vrSewGuide, studiedVertices[i].Index), vrVoxelVertex.Cell.ToString(), Color.Gray, 0.7f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, -1, persistent: true);
			}
			VrSewGuide vrSewGuide2 = meshes[0];
			for (int j = 0; j < count2; j++)
			{
				Vector3D vector3D = Position(vrSewGuide2, generatedVertices[j]);
				Vector3I cell = vrSewGuide2.Mesh.Vertices[(int)generatedVertices[j]].Cell;
				MyRenderProxy.DebugDrawText3D(vector3D, cell.ToString(), Color.Red, 0.7f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, -1, persistent: true);
				MyRenderProxy.DebugDrawText3D(vector3D, ((int*)(&j))->ToString(), Color.Red, 0.7f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP, -1, persistent: true);
				MyRenderProxy.DebugDrawSphere(vector3D, 0.25f, Color.Red, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
			}
			for (int k = 0; k < count3; k++)
			{
				Vector3I cell2 = remappedVertices[k].Cell;
				Vector3D vector3D2 = Position(vrSewGuide2, cell2) + 0.5 * (double)vrSewGuide2.Scale;
				Vector3D vector3D3 = Position(vrSewGuide2, remappedVertices[k].Index);
				byte generationCorner = remappedVertices[k].GenerationCorner;
				for (int l = 0; l < 3; l++)
				{
					int num = (generationCorner >> l) & 1;
					Vector3I pos = cell2;
					pos[l] += ((num != 1) ? 1 : (-1));
					MyRenderProxy.DebugDrawText3D(Position(vrSewGuide2, pos) + 0.5 * (double)vrSewGuide2.Scale, pos.ToString(), Color.Orange, 0.7f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, -1, persistent: true);
				}
				MyRenderProxy.DebugDrawSphere(vector3D2, 0.25f, Color.Blue, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
				MyRenderProxy.DebugDrawSphere(vector3D3, 0.25f, Color.Green, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
				MyRenderProxy.DebugDrawArrow3D(vector3D2, vector3D3, Color.Blue, Color.Green, depthRead: true, 0.1, null, 0.5f, persistent: true);
				MyRenderProxy.DebugDrawText3D(vector3D2, cell2.ToString(), Color.Blue, 0.7f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, -1, persistent: true);
				MyRenderProxy.DebugDrawText3D(vector3D2, ((int*)(&k))->ToString(), Color.Blue, 0.7f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP, -1, persistent: true);
				MyRenderProxy.DebugDrawText3D((vector3D2 + vector3D3) / 2.0, $"G{(int)k} : T{remappedVertices[k].ProducedTriangleCount}", Color.Lerp(Color.Blue, Color.Green, 0.5f), 0.7f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, -1, persistent: true);
			}
			MyIsoMeshTaylor.NativeInstance.ClearBuffers();
		}

		private Vector3D GetCenter(VrSewGuide mesh)
		{
			return Vector3D.Transform((Vector3D)(mesh.End + mesh.Start) / 2.0 * mesh.Scale, Clipmap.LocalToWorld);
		}

		private Vector3D Position(VrSewGuide mesh, Vector3I pos)
		{
			pos += mesh.Start;
			pos <<= mesh.Lod;
			return Vector3D.Transform((Vector3D)pos, Clipmap.LocalToWorld);
		}

		private unsafe Vector3D Position(VrSewGuide mesh, int index)
		{
			return Vector3D.Transform(((Vector3D)mesh.Mesh.Vertices[index].Position + mesh.Start) * mesh.Scale, Clipmap.LocalToWorld);
		}

		protected override void OnComplete()
		{
			base.OnComplete();
			if (m_forceStitchCommit)
			{
				Clipmap.CommitStitchOperation(Operation);
			}
			m_cellData = default(MyVoxelRenderCellData);
			m_instancePool.Return(this);
		}

		public override void Cancel()
		{
			m_isCancelled = true;
		}

		public override void DebugDraw(Color c)
		{
		}
	}
}
