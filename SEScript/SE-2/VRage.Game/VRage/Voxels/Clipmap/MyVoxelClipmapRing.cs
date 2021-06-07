using System.Collections.Generic;
using System.Linq;
using VRage.Collections;
using VRage.Game;
using VRage.Utils;
using VRage.Voxels.Sewing;
using VRageMath;
using VRageRender;
using VRageRender.Voxels;

namespace VRage.Voxels.Clipmap
{
	/// <summary>
	/// Manages a clipmap ring.
	/// </summary>
	///
	/// The clipmap is comprised of concentric 'rings' (actually they are thick walled boxes, but the original paper calls the rings in the 2D case).
	///
	/// Each ring contains meshes of a specific level of detail.
	internal class MyVoxelClipmapRing
	{
		/// <summary>
		/// Describes the status of a clipmap cell.
		/// </summary>
		public enum CellStatus : byte
		{
			/// <summary>
			/// Waiting for calculation
			/// </summary>
			Pending,
			/// <summary>
			/// Mesh has been calculated, but is not in the scene yet.
			/// </summary>
			Calculated,
			/// <summary>
			/// Known to be empty.
			/// </summary>
			Empty,
			/// <summary>
			/// Mesh is ready to draw. Render cell exists.
			/// </summary>
			Ready,
			/// <summary>
			/// Cell is waiting for it's children to be removed.
			/// </summary>
			MarkedForRemoval
		}

		/// <summary>
		/// Data about a given clipmap cell.
		/// </summary>
		public class CellData
		{
			/// <summary>
			/// Render counterpart.
			/// </summary>
			public IMyVoxelActorCell Cell;

			/// <summary>
			/// Status of this cell.
			/// </summary>
			public CellStatus Status;

			/// <summary>
			/// Constitution of the content of this cell.
			/// </summary>
			public MyVoxelContentConstitution Constitution;

			/// <summary>
			/// Sewing aware container for the mesh.
			/// </summary>
			public VrSewGuide Guide;

			/// <summary>
			/// Whether this cell has been made visible.
			/// </summary>
			public bool Visible;

			/// <summary>
			/// Cell Vicinity signature.
			/// </summary>
			public Vicinity Vicinity = Vicinity.Invalid;

			public CellData(CellStatus status, MyVoxelContentConstitution constitution)
			{
				Status = status;
				Constitution = constitution;
				Cell = null;
			}

			public CellData()
			{
				Status = CellStatus.Pending;
				Constitution = MyVoxelContentConstitution.Mixed;
				Cell = null;
			}

			public void Dispose(MyVoxelClipmap clipmap)
			{
			}
		}

		/// <summary>
		/// Signature of a cell's vicinity.
		/// </summary>
		public struct Vicinity
		{
			public readonly uint Lods;

			public unsafe fixed sbyte Versions[8];

			/// <summary>
			/// Invalid vicinity mark.
			/// </summary>
			public static readonly Vicinity Invalid = new Vicinity(dummySelector: false);

			public unsafe Vicinity(VrSewGuide[] guides, MyCellCoord[] coords)
			{
				Lods = 0u;
				fixed (Vicinity* ptr = &this)
				{
					for (int i = 0; i < 8; i++)
					{
						int lod = MyVoxelClipmap.MakeFulfilled(coords[i]).Lod;
						if (guides[i] == null)
						{
							ptr->Versions[i] = -1;
						}
						else
						{
							Lods |= (uint)(lod << i * 4);
							ptr->Versions[i] = (sbyte)(guides[i].Version & 0x7F);
						}
					}
				}
			}

			private unsafe Vicinity(bool dummySelector)
			{
				Lods = 0u;
				fixed (Vicinity* ptr = &this)
				{
					for (int i = 0; i < 8; i++)
					{
						ptr->Versions[i] = -1;
					}
				}
			}

			public unsafe bool Equals(Vicinity other)
			{
				fixed (Vicinity* ptr = &this)
				{
					if (Lods == other.Lods)
					{
						return *(long*)ptr->Versions == *(long*)other.Versions;
					}
					return false;
				}
			}

			public override bool Equals(object obj)
			{
				if (obj == null)
				{
					return false;
				}
				if (obj is Vicinity)
				{
					return Equals((Vicinity)obj);
				}
				return false;
			}

			public unsafe override int GetHashCode()
			{
				fixed (Vicinity* ptr = &this)
				{
					return (int)(Lods * 397) ^ (int)ptr->Versions;
				}
			}

			public static bool operator ==(Vicinity left, Vicinity right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(Vicinity left, Vicinity right)
			{
				return !(left == right);
			}
		}

		private readonly MyVoxelClipmap m_clipmap;

		private readonly int m_lod;

		private Vector3L m_max;

		/// <summary>
		/// Set of visible cells belonging to this ring.
		/// </summary>
		private readonly Dictionary<Vector3I, CellData> m_cells = new Dictionary<Vector3I, CellData>(Vector3I.Comparer);

		/// <summary>
		/// Set of cells that will be removed upon the current update.
		/// </summary>
		private readonly HashSet<Vector3I> m_cellsRemove = new HashSet<Vector3I>(Vector3I.Comparer);

		/// <summary>
		/// Set of cells that will be added during the current update.
		/// </summary>
		private readonly HashSet<Vector3I> m_cellsAdd = new HashSet<Vector3I>(Vector3I.Comparer);

		/// <summary>
		/// Set of cells that will be re-stitched as a result of their forward cells having changed.
		/// </summary>
		private readonly HashSet<Vector3I> m_cellsReStitch = new HashSet<Vector3I>(Vector3I.Comparer);

		/// <summary>
		/// Last bounds of this ring.
		/// </summary>
		internal BoundingBoxI Bounds;

		/// <summary>
		/// Cached bounds of the ring immediately inside this.
		/// </summary>
		internal BoundingBoxI InnerBounds;

		/// <summary>
		/// Whether the bounds of this ring have changes in the last update.
		/// </summary>
		internal bool BoundsChanged;

		/// <summary>
		/// Level of detail index for this ring.
		/// </summary>
		public int Lod => m_lod;

		/// <summary>
		/// Dictionary containing all cells that are part of this ring.
		/// </summary>
		public DictionaryReader<Vector3I, CellData> Cells => m_cells;

		/// <summary>
		/// Create a new ring.
		/// </summary>
		/// <param name="clipmap"></param>
		/// <param name="lod"></param>
		public MyVoxelClipmapRing(MyVoxelClipmap clipmap, int lod)
		{
			m_clipmap = clipmap;
			m_lod = lod;
		}

		/// <summary>
		/// Change the size of this ring, this will cause it to recalculate from scratch next update.
		/// </summary>
		/// <param name="size"></param>
		internal void UpdateSize(Vector3I size)
		{
			m_max = size + 1 >> 1 << 1;
		}

		/// <summary>
		/// Update this ring with respect to the world space position of the camera.
		/// </summary>
		/// <param name="relativePosition"></param>
		public void Update(Vector3L relativePosition)
		{
			BoundsChanged = false;
			Vector3L value = relativePosition - m_clipmap.Ranges[m_lod] >> m_lod >> m_clipmap.CellBits;
			Vector3L value2 = relativePosition + m_clipmap.Ranges[m_lod] >> m_lod >> m_clipmap.CellBits;
			Vector3L.Clamp(ref value, ref Vector3L.Zero, ref m_max, out value);
			Vector3L.Clamp(ref value2, ref Vector3L.Zero, ref m_max, out value2);
			BoundingBoxI boundingBoxI = new BoundingBoxI((Vector3I)value, (Vector3I)value2);
			boundingBoxI.Min = boundingBoxI.Min >> 1 << 1;
			boundingBoxI.Max = boundingBoxI.Max + 1 >> 1 << 1;
			BoundingBoxI innerBounds = default(BoundingBoxI);
			bool flag;
			if (m_lod > 0)
			{
				MyVoxelClipmapRing myVoxelClipmapRing = m_clipmap.Rings[m_lod - 1];
				innerBounds.Min = myVoxelClipmapRing.Bounds.Min >> 1;
				innerBounds.Max = myVoxelClipmapRing.Bounds.Max >> 1;
				flag = myVoxelClipmapRing.BoundsChanged;
			}
			else
			{
				innerBounds = InnerBounds;
				flag = false;
			}
			if (boundingBoxI != Bounds)
			{
				foreach (Vector3I item in BoundingBoxI.IterateDifference(Bounds, boundingBoxI))
				{
					if (m_cells.ContainsKey(item))
					{
						m_cellsRemove.Add(item);
					}
				}
				foreach (Vector3I item2 in BoundingBoxI.IterateDifference(boundingBoxI, Bounds))
				{
					if ((m_lod <= 0 || !item2.IsInside(ref innerBounds.Min, ref innerBounds.Max)) && !m_cells.ContainsKey(item2))
					{
						MyVoxelContentConstitution myVoxelContentConstitution = m_clipmap.ApproximateCellConstitution(item2, m_lod);
						if (myVoxelContentConstitution == MyVoxelContentConstitution.Mixed)
						{
							m_cellsAdd.Add(item2);
						}
						else
						{
							m_cells.Add(item2, new CellData(CellStatus.Empty, myVoxelContentConstitution));
						}
					}
				}
				BoundingBoxI left = boundingBoxI.Intersect(Bounds);
				foreach (Vector3I item3 in BoundingBoxI.IterateDifference(right: new BoundingBoxI(left.Min, left.Max - 1), left: left))
				{
					if (m_cells.TryGetValue(item3, out CellData value3) && value3.Guide != null)
					{
						m_cellsReStitch.Add(item3);
					}
				}
				BoundsChanged = true;
				Bounds = boundingBoxI;
			}
			if (flag)
			{
				BoundingBoxI boundingBoxI2 = Bounds.Intersect(InnerBounds);
				foreach (Vector3I item4 in BoundingBoxI.IterateDifference(boundingBoxI2, innerBounds))
				{
					MyVoxelContentConstitution myVoxelContentConstitution2 = m_clipmap.ApproximateCellConstitution(item4, m_lod);
					if (myVoxelContentConstitution2 == MyVoxelContentConstitution.Mixed)
					{
						m_cellsAdd.Add(item4);
					}
					else
					{
						m_cells.Add(item4, new CellData(CellStatus.Empty, myVoxelContentConstitution2));
					}
				}
				foreach (Vector3I item5 in BoundingBoxI.IterateDifference(innerBounds, boundingBoxI2))
				{
					if (m_cells.ContainsKey(item5))
					{
						m_cellsRemove.Add(item5);
						m_cellsReStitch.Remove(item5);
					}
				}
				BoundingBoxI left2 = Bounds.Intersect(new BoundingBoxI(innerBounds.Min - 1, innerBounds.Max));
				foreach (Vector3I item6 in BoundingBoxI.IterateDifference(Bounds.Intersect(new BoundingBoxI(InnerBounds.Min - 1, InnerBounds.Max)), InnerBounds).Concat(BoundingBoxI.IterateDifference(left2, innerBounds)))
				{
					if (!m_cellsRemove.Contains(item6) && m_cells.TryGetValue(item6, out CellData value4) && value4.Guide != null)
					{
						m_cellsReStitch.Add(item6);
					}
				}
				InnerBounds = innerBounds;
			}
		}

		/// <summary>
		/// Process all queued changes during update, dispatching cell requests and queuing dependencies.
		/// </summary>
		public void ProcessChanges()
		{
			foreach (Vector3I item in m_cellsAdd)
			{
				AddCell(item);
			}
			m_cellsAdd.Clear();
			foreach (Vector3I item2 in m_cellsRemove)
			{
				RemoveCell(item2);
			}
			m_cellsRemove.Clear();
		}

		/// <summary>
		/// Dispatch all changes related to re-stitching.
		///
		/// This have to happen after all changes have been processed for all lods, otherwise you may break stitch dependencies.
		/// </summary>
		public void DispatchStitchingRefreshes()
		{
			foreach (Vector3I item in m_cellsReStitch)
			{
				m_clipmap.Stitch(new MyCellCoord(m_lod, item));
			}
			m_cellsReStitch.Clear();
		}

		/// <summary>
		/// Dispose a cell and it's contents.
		/// </summary>
		/// <param name="coord"></param>
		/// <param name="data"></param>
		private void DisposeCell(Vector3I coord, CellData data)
		{
			data.Dispose(m_clipmap);
			if (data.Cell != null)
			{
				m_clipmap.Actor.DeleteCell(data.Cell);
				data.Cell = null;
			}
			if (data.Guide != null)
			{
				data.Guide.RemoveReference();
				data.Guide = null;
			}
		}

		/// <summary>
		/// Introduce a new cell.
		///
		/// Newly added cells are calculated, stitched and then post-processed so they can be rendered.
		///
		/// New cells need time to be constructed, as a result they are placed as dependencies for the
		/// parent cell which invariably needs to be removed, but will do so only when all children are ready.
		/// </summary>
		/// <param name="cell"></param>
		private void AddCell(Vector3I cell)
		{
			CellData value = new CellData();
			m_cells.Add(cell, value);
			m_clipmap.RequestCell(cell, m_lod);
		}

		/// <summary>
		/// Remove a cell during update.
		/// </summary>
		/// <param name="cell"></param>
		private void RemoveCell(Vector3I cell)
		{
			CellData cellData = m_cells[cell];
			RemoveImmediately(cell, cellData);
		}

		/// <summary>
		/// Remove the cell right away.
		///
		/// This method is separated from finish remove so we can have more
		/// control over what code paths can modify cells at which times.
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="cellData"></param>
		private void RemoveImmediately(Vector3I cell, CellData cellData)
		{
			DisposeCell(cell, cellData);
			m_cells.Remove(cell);
		}

		/// <summary>
		/// Set the cell to visible when it is finally ready.
		/// </summary>
		/// <param name="cell"></param>
		public void FinishAdd(Vector3I cell)
		{
			if (m_cells.TryGetValue(cell, out CellData value) && value.Cell != null)
			{
				value.Cell.SetVisible(visible: true);
			}
		}

		/// <summary>
		/// Hide and delete a cell whose dependencies are finally ready.
		/// </summary>
		/// <param name="cell"></param>
		public void FinishRemove(Vector3I cell)
		{
			if (m_cells.TryGetValue(cell, out CellData value))
			{
				RemoveImmediately(cell, value);
			}
		}

		/// <summary>
		/// Update cell data about a mesh.
		///
		///
		/// When the cell is a back edge cell the mesh will be generated regardless.
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="guide"></param>
		public void UpdateCellData(Vector3I cell, VrSewGuide guide, MyVoxelContentConstitution constitution)
		{
			if (!m_cells.TryGetValue(cell, out CellData value))
			{
				return;
			}
			if (value.Guide != null && value.Guide != guide && value.Guide != null)
			{
				value.Guide.RemoveReference();
			}
			value.Guide = guide;
			value.Constitution = constitution;
			if (guide != null && guide.Mesh != null)
			{
				value.Status = CellStatus.Calculated;
				return;
			}
			value.Status = CellStatus.Empty;
			if (value.Cell != null)
			{
				m_clipmap.Actor.DeleteCell(value.Cell);
				value.Cell = null;
			}
		}

		/// <summary>
		/// Update a cell's render data.
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="updateData"></param>
		/// <returns>Whether the update caused a new cell to be created and made visible.</returns>
		public bool UpdateCellRender(Vector3I cell, ref MyVoxelRenderCellData updateData)
		{
			bool result = false;
			if (m_cells.TryGetValue(cell, out CellData value))
			{
				if (updateData.Parts != null && updateData.Parts.Length != 0)
				{
					if (value.Cell == null)
					{
						value.Cell = m_clipmap.Actor.CreateCell(cell << m_lod + m_clipmap.CellBits, m_lod);
						result = true;
					}
					value.Cell.UpdateMesh(ref updateData);
					value.Status = CellStatus.Ready;
					value.Cell.SetVisible(visible: true);
				}
				else if (value.Cell != null)
				{
					m_clipmap.Actor.DeleteCell(value.Cell);
					value.Cell = null;
				}
			}
			return result;
		}

		/// <summary>
		/// Get cell data for position.
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		internal bool TryGetCell(Vector3I cell, out CellData data)
		{
			return m_cells.TryGetValue(cell, out data);
		}

		/// <summary>
		/// Check if a cell stands at the back edge of the inner lod.
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		internal bool IsInnerLodEdge(Vector3I cell)
		{
			if (cell.X != InnerBounds.Min.X - 1 && cell.Y != InnerBounds.Min.Y - 1)
			{
				return cell.Z == InnerBounds.Min.Z - 1;
			}
			return true;
		}

		/// <summary>
		/// Check if a cell stands at the back edge of the inner lod.
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="innerCornerIndex">The index of the inner lod cell that needs to be stitched to this.</param>
		/// <returns></returns>
		internal bool IsInnerLodEdge(Vector3I cell, out int innerCornerIndex)
		{
			bool num = cell.X == InnerBounds.Min.X - 1;
			bool flag = cell.Y == InnerBounds.Min.Y - 1;
			bool flag2 = cell.Z == InnerBounds.Min.Z - 1;
			innerCornerIndex = 0;
			if (num)
			{
				innerCornerIndex |= 1;
			}
			if (flag)
			{
				innerCornerIndex |= 2;
			}
			if (flag2)
			{
				innerCornerIndex |= 4;
			}
			return innerCornerIndex != 0;
		}

		/// <summary>
		/// Check if a cell is contained in the inner lod.
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		internal bool IsInsideInnerLod(Vector3I cell)
		{
			return InnerBounds.Contains(cell) == ContainmentType.Contains;
		}

		/// <summary>
		/// Whether the given cell is inside the bounds of this ring.
		/// </summary>
		/// <param name="cell">The cell coordinates.</param>
		/// <returns></returns>
		internal bool IsInBounds(Vector3I cell)
		{
			return Bounds.Contains(cell) == ContainmentType.Contains;
		}

		/// <summary>
		/// Whether the given edge stands at the forward boundary of this lod.
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		public bool IsForwardEdge(Vector3I cell)
		{
			if (cell.X != Bounds.Max.X - 1 && cell.Y != Bounds.Max.Y - 1)
			{
				return cell.Z == Bounds.Max.Z - 1;
			}
			return true;
		}

		internal void InvalidateRange(BoundingBoxI range)
		{
			Vector3I minRange = range.Min >> m_lod;
			range.Min >>= m_lod + m_clipmap.CellBits;
			range.Max += (1 << m_lod + m_clipmap.CellBits) - 1;
			range.Max >>= m_lod + m_clipmap.CellBits;
			range = range.Intersect(Bounds);
			foreach (Vector3I item in BoundingBoxI.EnumeratePoints(range))
			{
				if (m_cells.TryGetValue(item, out CellData value) && value.Status != CellStatus.MarkedForRemoval)
				{
					if (value.Guide != null)
					{
						value.Guide.InvalidateGenerated(minRange);
					}
					if (m_clipmap.ApproximateCellConstitution(item, m_lod) != value.Constitution || value.Status != CellStatus.Empty)
					{
						value.Status = CellStatus.Pending;
						m_clipmap.RequestCell(item, m_lod, value.Guide);
					}
				}
			}
		}

		/// <summary>
		/// Invalidate all cells in this ring.
		/// </summary>
		/// This simply discards all cells and re-sets the ring.
		internal void InvalidateAll()
		{
			foreach (KeyValuePair<Vector3I, CellData> cell in m_cells)
			{
				DisposeCell(cell.Key, cell.Value);
			}
			m_cells.Clear();
			Bounds = default(BoundingBoxI);
			InnerBounds = default(BoundingBoxI);
		}

		public void DebugDraw()
		{
			Vector3D translation = MyTransparentGeometry.Camera.Translation;
			int num = m_clipmap.CellSize * (2 + (1 << m_lod + 1));
			num *= num;
			Vector4 vector = MyClipmap.LodColors[m_lod];
			using (IMyDebugDrawBatchAabb myDebugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(m_clipmap.LocalToWorld, new Color(vector - new Vector4(0.2f), 0.07f)))
			{
				using (IMyDebugDrawBatchAabb myDebugDrawBatchAabb2 = MyRenderProxy.DebugDrawBatchAABB(m_clipmap.LocalToWorld, new Color(vector, 0.4f), depthRead: true, shaded: false))
				{
					foreach (KeyValuePair<Vector3I, CellData> cell in m_cells)
					{
						if (cell.Value.Guide != null && cell.Value.Guide.Mesh != null)
						{
							Vector3I vector3I = cell.Key << m_clipmap.CellBits << m_lod;
							Vector3I value = vector3I + (m_clipmap.CellSize << m_lod);
							BoundingBoxD aabb = new BoundingBoxD(vector3I, value);
							myDebugDrawBatchAabb.Add(ref aabb);
							myDebugDrawBatchAabb2.Add(ref aabb);
							Vector3D vector3D = Vector3D.Transform((Vector3D)vector3I + (float)(m_clipmap.CellSize << m_lod >> 1), m_clipmap.LocalToWorld);
							double num2 = Vector3D.DistanceSquared(vector3D, translation);
							if (num2 < (double)num)
							{
								float num3 = 1f - (float)num2 / (float)num;
								MyRenderProxy.DebugDrawText3D(vector3D, $"{(int)m_lod}:{cell.Key}", new Color(vector, num3), 0.8f * num3, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
							}
						}
					}
				}
			}
		}
	}
}
