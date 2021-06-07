using System;
using System.Collections.Generic;
using VRage.Collections;
using VRageMath;
using VRageRender;
using VRageRender.Voxels;

namespace VRage.Voxels.Clipmap
{
	/// <inheritdoc />
	/// <summary>
	/// A lod controller that exposes cell creation/manipulation methods to the user.
	/// </summary>
	public class MyUserController : IMyLodController
	{
		private interface IMessage
		{
			void Do(MyUserController controller);
		}

		private class MCreateCell : IMessage
		{
			private readonly Vector3D m_offset;

			private readonly int m_lod;

			private readonly uint m_id;

			public MCreateCell(uint id, Vector3D offset, int lod)
			{
				m_offset = offset;
				m_lod = lod;
				m_id = id;
			}

			public void Do(MyUserController controller)
			{
				controller.m_cells.Add(m_id, controller.Actor.CreateCell(m_offset, m_lod));
			}
		}

		private class MDeleteCell : IMessage
		{
			private readonly uint m_id;

			public MDeleteCell(uint id)
			{
				m_id = id;
			}

			public void Do(MyUserController controller)
			{
				if (controller.m_cells.TryGetValue(m_id, out IMyVoxelActorCell value))
				{
					controller.Actor.DeleteCell(value);
					controller.m_cells.Remove(m_id);
				}
			}
		}

		private uint m_nextMeshId = 1u;

		private readonly Dictionary<uint, IMyVoxelActorCell> m_cells = new Dictionary<uint, IMyVoxelActorCell>();

		private readonly MyConcurrentQueue<IMessage> m_messageQueue = new MyConcurrentQueue<IMessage>();

		public IMyVoxelRenderDataProcessorProvider VoxelRenderDataProcessorProvider
		{
			get;
			set;
		}

		public IEnumerable<IMyVoxelActorCell> Cells => m_cells.Values;

		/// <summary>
		/// Actor bound to this clipmap.
		///
		/// Users should not touch this from any thread other than the render thread.
		/// </summary>
		public IMyVoxelActor Actor
		{
			get;
			private set;
		}

		public Vector3I Size
		{
			get;
			private set;
		}

		public float? SpherizeRadius => null;

		public Vector3D SpherizePosition => Vector3D.Zero;

		public event Action<IMyLodController> Loaded;

		public void Update(ref MatrixD view, BoundingFrustumD viewFrustum, float farClipping)
		{
			IMessage instance;
			while (m_messageQueue.TryDequeue(out instance))
			{
				instance.Do(this);
			}
			if (this.Loaded != null)
			{
				MyRenderProxy.EnqueueMainThreadCallback(delegate
				{
					if (this.Loaded != null)
					{
						this.Loaded(this);
					}
				});
			}
		}

		public void BindToActor(IMyVoxelActor actor)
		{
			if (Actor != null)
			{
				throw new InvalidOperationException();
			}
			Actor = actor;
		}

		public void Unload()
		{
			foreach (IMyVoxelActorCell value in m_cells.Values)
			{
				Actor.DeleteCell(value);
			}
			m_cells.Clear();
		}

		public void InvalidateRange(Vector3I min, Vector3I max)
		{
		}

		public void InvalidateAll()
		{
		}

		public void DebugDraw(ref MatrixD cameraMatrix)
		{
		}

		/// <summary>
		/// Create a clipmap cell at the specified offset and with provided lod.
		/// </summary>
		/// <param name="offset">Local space offset for this mesh.</param>
		/// <param name="lod">Lod index of this mesh, lod is used to calculate scale and for debug purposes.</param>
		/// <returns></returns>
		public uint CreateCell(Vector3D offset, int lod)
		{
			uint num = m_nextMeshId++;
			m_messageQueue.Enqueue(new MCreateCell(num, offset, lod));
			return num;
		}

		/// <summary>
		/// Delete the provided cell.
		/// </summary>
		/// <param name="id">Id of the cell to delete.</param>
		public void DeleteCell(uint id)
		{
			m_messageQueue.Enqueue(new MDeleteCell(id));
		}
	}
}
