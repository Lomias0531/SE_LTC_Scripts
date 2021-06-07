using System;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageMath.Spatial;

namespace VRage.Game.Components
{
	public abstract class MyPositionComponentBase : MyEntityComponentBase
	{
		public static Action<IMyEntity> OnReportInvalidMatrix;

		protected MatrixD m_worldMatrix = MatrixD.Identity;

		public uint m_worldMatrixCounter;

		public uint m_lastParentWorldMatrixCounter;

		public bool m_worldMatrixDirty;

		/// Internal local matrix relative to parent of entity.
		protected Matrix m_localMatrix = Matrix.Identity;

		protected BoundingBox m_localAABB;

		protected BoundingSphere m_localVolume;

		protected Vector3 m_localVolumeOffset;

		protected BoundingBoxD m_worldAABB;

		protected BoundingSphereD m_worldVolume;

		protected bool m_worldVolumeDirty;

		protected bool m_worldAABBDirty;

		private float? m_scale;

		protected bool m_normalizedInvMatrixDirty = true;

		private MatrixD m_normalizedWorldMatrixInv;

		protected bool m_invScaledMatrixDirty = true;

		private MatrixD m_worldMatrixInvScaled;

		/// <summary>
		/// World matrix of this physic object. Use it whenever you want to do world-matrix transformations with this physic objects.
		/// </summary>
		public MatrixD WorldMatrix
		{
			get
			{
				if (NeedsRecalculateWorldMatrix)
				{
					RecalculateWorldMatrixHR();
				}
				return m_worldMatrix;
			}
			set
			{
				SetWorldMatrix(value);
			}
		}

		/// <summary>
		/// Gets or sets the local matrix.
		/// </summary>
		/// <value>
		/// The local matrix.
		/// </value>
		public Matrix LocalMatrix
		{
			get
			{
				return m_localMatrix;
			}
			set
			{
				SetLocalMatrix(ref value, null, updateWorld: true);
			}
		}

		/// <summary>
		/// Gets the world aabb.
		/// </summary>
		public BoundingBoxD WorldAABB
		{
			get
			{
				if (m_worldAABBDirty || NeedsRecalculateWorldMatrix)
				{
					MatrixD m = WorldMatrix;
					m_localAABB.Transform(ref m, ref m_worldAABB);
					m_worldAABBDirty = false;
				}
				return m_worldAABB;
			}
			set
			{
				m_worldAABB = value;
				Vector3 normal = value.Center - WorldMatrix.Translation;
				MatrixD matrix = WorldMatrixInvScaled;
				Vector3.TransformNormal(ref normal, ref matrix, out normal);
				LocalAABB = new BoundingBox(normal - (Vector3)value.HalfExtents, normal + (Vector3)value.HalfExtents);
				m_worldAABBDirty = false;
			}
		}

		/// <summary>
		/// Gets the world volume.
		/// </summary>
		public BoundingSphereD WorldVolume
		{
			get
			{
				if (m_worldVolumeDirty || NeedsRecalculateWorldMatrix)
				{
					MatrixD matrix = WorldMatrix;
					m_worldVolume.Center = Vector3D.Transform(m_localVolume.Center, ref matrix);
					m_worldVolume.Radius = m_localVolume.Radius;
					m_worldVolumeDirty = false;
				}
				return m_worldVolume;
			}
			set
			{
				m_worldVolume = value;
				Vector3 normal = value.Center - WorldMatrix.Translation;
				MatrixD matrix = WorldMatrixInvScaled;
				Vector3.TransformNormal(ref normal, ref matrix, out normal);
				LocalVolume = new BoundingSphere(normal, (float)value.Radius);
				m_worldVolumeDirty = false;
			}
		}

		/// <summary>
		/// Sets the local aabb.
		/// </summary>
		/// <value>
		/// The local aabb.
		/// </value>
		public virtual BoundingBox LocalAABB
		{
			get
			{
				return m_localAABB;
			}
			set
			{
				if (m_localAABB != value)
				{
					m_localAABB = value;
					m_localVolume = BoundingSphere.CreateFromBoundingBox(m_localAABB);
					m_worldVolumeDirty = true;
					m_worldAABBDirty = true;
					if (this.OnLocalAABBChanged != null)
					{
						this.OnLocalAABBChanged(this);
					}
				}
			}
		}

		/// <summary>
		/// Sets the local volume.
		/// </summary>
		/// <value>
		/// The local volume.
		/// </value>
		public BoundingSphere LocalVolume
		{
			get
			{
				return m_localVolume;
			}
			set
			{
				m_localVolume = value;
				m_localAABB = MyMath.CreateFromInsideRadius(value.Radius);
				m_localAABB = m_localAABB.Translate(value.Center);
				m_worldVolumeDirty = true;
				m_worldAABBDirty = true;
			}
		}

		/// <summary>
		/// Gets or sets the local volume offset.
		/// </summary>
		/// <value>
		/// The local volume offset.
		/// </value>
		public Vector3 LocalVolumeOffset
		{
			get
			{
				return m_localVolumeOffset;
			}
			set
			{
				m_localVolumeOffset = value;
				m_worldVolumeDirty = true;
			}
		}

		protected virtual bool ShouldSync => base.Container.Get<MySyncComponentBase>() != null;

		public float? Scale
		{
			get
			{
				return m_scale;
			}
			set
			{
				if (m_scale == value)
				{
					return;
				}
				m_scale = value;
				Matrix m = LocalMatrix;
				if (m_scale.HasValue)
				{
					MatrixD m2 = WorldMatrix;
					if (base.Container.Entity.Parent == null)
					{
						MyUtils.Normalize(ref m2, out m2);
						WorldMatrix = Matrix.CreateScale(m_scale.Value) * m2;
					}
					else
					{
						MyUtils.Normalize(ref m, out m);
						LocalMatrix = Matrix.CreateScale(m_scale.Value) * m;
					}
				}
				else
				{
					MyUtils.Normalize(ref m, out m);
					LocalMatrix = m;
				}
				UpdateWorldMatrix();
			}
		}

		public bool NeedsRecalculateWorldMatrix
		{
			get
			{
				if (m_worldMatrixDirty)
				{
					return true;
				}
				if (base.Entity == null)
				{
					return false;
				}
				IMyEntity parent = base.Entity.Parent;
				uint lastParentWorldMatrixCounter = m_lastParentWorldMatrixCounter;
				while (parent != null)
				{
					if (lastParentWorldMatrixCounter < parent.PositionComp.m_worldMatrixCounter)
					{
						return true;
					}
					lastParentWorldMatrixCounter = parent.PositionComp.m_lastParentWorldMatrixCounter;
					parent = parent.Parent;
				}
				return false;
			}
		}

		public MatrixD WorldMatrixNormalizedInv
		{
			get
			{
				if (m_normalizedInvMatrixDirty || NeedsRecalculateWorldMatrix)
				{
					MatrixD matrix = WorldMatrix;
					if (!MyUtils.IsZero(matrix.Left.LengthSquared() - 1.0))
					{
						MatrixD matrix2 = MatrixD.Normalize(matrix);
						MatrixD.Invert(ref matrix2, out m_normalizedWorldMatrixInv);
					}
					else
					{
						MatrixD.Invert(ref matrix, out m_normalizedWorldMatrixInv);
					}
					m_normalizedInvMatrixDirty = false;
					if (!Scale.HasValue)
					{
						m_worldMatrixInvScaled = m_normalizedWorldMatrixInv;
						m_invScaledMatrixDirty = false;
					}
				}
				return m_normalizedWorldMatrixInv;
			}
			private set
			{
				m_normalizedWorldMatrixInv = value;
			}
		}

		public MatrixD WorldMatrixInvScaled
		{
			get
			{
				if (m_invScaledMatrixDirty || NeedsRecalculateWorldMatrix)
				{
					MatrixD matrix = WorldMatrix;
					if (!MyUtils.IsZero(matrix.Left.LengthSquared() - 1.0))
					{
						matrix = MatrixD.Normalize(matrix);
					}
					if (Scale.HasValue)
					{
						matrix *= Matrix.CreateScale(Scale.Value);
					}
					MatrixD.Invert(ref matrix, out m_worldMatrixInvScaled);
					m_invScaledMatrixDirty = false;
					if (!Scale.HasValue)
					{
						m_normalizedWorldMatrixInv = m_worldMatrixInvScaled;
						m_normalizedInvMatrixDirty = false;
					}
				}
				return m_worldMatrixInvScaled;
			}
			private set
			{
				m_worldMatrixInvScaled = value;
			}
		}

		public override string ComponentTypeDebugString => "Position";

		public event Action<MyPositionComponentBase> OnPositionChanged;

		public event Action<MyPositionComponentBase> OnLocalAABBChanged;

		protected void RaiseOnPositionChanged(MyPositionComponentBase component)
		{
			this.OnPositionChanged.InvokeIfNotNull(component);
		}

		/// <summary>
		/// Sets the world matrix.
		/// </summary>
		/// <param name="worldMatrix">The world matrix.</param>
		/// <param name="source">The source object that caused this change or null when not important.</param>
		public virtual void SetWorldMatrix(MatrixD worldMatrix, object source = null, bool forceUpdate = false, bool updateChildren = true, bool updateLocal = true, bool skipTeleportCheck = false, bool forceUpdateAllChildren = false, bool ignoreAssert = false)
		{
			if (OnReportInvalidMatrix != null && !worldMatrix.IsValid())
			{
				OnReportInvalidMatrix(base.Entity);
			}
			else if (!skipTeleportCheck && base.Entity.InScene && Vector3D.DistanceSquared(worldMatrix.Translation, WorldMatrix.Translation) > (double)MyClusterTree.IdealClusterSizeHalfSqr.X)
			{
				base.Entity.Teleport(worldMatrix, source, ignoreAssert);
			}
			else
			{
				if (base.Entity.Parent != null && source != base.Entity.Parent)
				{
					return;
				}
				if (Scale.HasValue)
				{
					MyUtils.Normalize(ref worldMatrix, out worldMatrix);
					worldMatrix = MatrixD.CreateScale(Scale.Value) * worldMatrix;
				}
				if (forceUpdate || !m_worldMatrix.EqualsFast(ref worldMatrix, 1E-06))
				{
					if (base.Container.Entity.Parent == null)
					{
						m_worldMatrix = worldMatrix;
						m_localMatrix = worldMatrix;
					}
					else if (updateLocal)
					{
						MatrixD worldMatrixInvScaled = base.Container.Entity.Parent.PositionComp.WorldMatrixInvScaled;
						m_localMatrix = worldMatrix * worldMatrixInvScaled;
					}
					m_worldMatrixCounter++;
					UpdateWorldMatrix(source, updateChildren, forceUpdateAllChildren);
				}
			}
		}

		public void RecalculateWorldMatrixHR(bool updateChildren = false)
		{
			if (base.Entity.Parent != null)
			{
				base.Entity.Parent.PositionComp.RecalculateWorldMatrixHR();
				MatrixD matrix = base.Entity.Parent.WorldMatrix;
				MatrixD other = m_worldMatrix;
				MatrixD.Multiply(ref m_localMatrix, ref matrix, out m_worldMatrix);
				m_worldMatrixDirty = false;
				if (!m_worldMatrix.EqualsFast(ref other))
				{
					m_lastParentWorldMatrixCounter = base.Entity.Parent.PositionComp.m_worldMatrixCounter;
					m_worldMatrixCounter++;
					m_worldVolumeDirty = true;
					m_worldAABBDirty = true;
					m_normalizedInvMatrixDirty = true;
					m_invScaledMatrixDirty = true;
				}
			}
		}

		public void SetLocalMatrix(ref Matrix localMatrix, object source, bool updateWorld, ref Matrix renderLocal, bool forceUpdateRender = false)
		{
			if (SetLocalMatrix(ref localMatrix, source, updateWorld) || forceUpdateRender)
			{
				base.Entity.Render?.UpdateRenderObjectLocal(renderLocal);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="localMatrix"></param>
		/// <param name="source"></param>
		/// <param name="updateWorld"></param>
		/// <returns> true when World matrix needed recalcualtions as it got changed in here</returns>
		public bool SetLocalMatrix(ref Matrix localMatrix, object source, bool updateWorld)
		{
			bool num = !m_localMatrix.EqualsFast(ref localMatrix);
			if (num)
			{
				m_localMatrix = localMatrix;
				m_worldMatrixCounter++;
				m_worldMatrixDirty = true;
			}
			if (NeedsRecalculateWorldMatrix && updateWorld)
			{
				UpdateWorldMatrix(source);
			}
			return num;
		}

		/// <summary>
		/// Gets the entity position.
		/// </summary>
		/// <returns></returns>
		public Vector3D GetPosition()
		{
			return WorldMatrix.Translation;
		}

		/// <summary>
		/// Sets the position.
		/// </summary>
		/// <param name="pos">The pos.</param>
		public void SetPosition(Vector3D pos, object source = null, bool forceUpdate = false, bool updateChildren = true)
		{
			if (!MyUtils.IsZero(m_worldMatrix.Translation - pos))
			{
				SetWorldMatrix(MatrixD.CreateWorld(pos, m_worldMatrix.Forward, m_worldMatrix.Up), source, forceUpdate, updateChildren);
			}
		}

		/// <summary>
		/// Gets the entity orientation.
		/// </summary>
		/// <returns></returns>
		public MatrixD GetOrientation()
		{
			return WorldMatrix.GetOrientation();
		}

		public virtual MatrixD GetViewMatrix()
		{
			return WorldMatrixNormalizedInv;
		}

		/// <summary>
		/// Updates the world matrix (change caused by this entity)
		/// </summary>
		protected virtual void UpdateWorldMatrix(object source = null, bool updateChildren = true, bool forceUpdateAllChildren = false)
		{
			if (base.Container.Entity.Parent != null)
			{
				MatrixD parentWorldMatrix = base.Container.Entity.Parent.WorldMatrix;
				UpdateWorldMatrix(ref parentWorldMatrix, source, updateChildren, forceUpdateAllChildren);
			}
			else
			{
				OnWorldPositionChanged(source, updateChildren, forceUpdateAllChildren);
			}
		}

		/// <summary>
		/// Updates the world matrix (change caused by parent)
		/// </summary>
		public virtual void UpdateWorldMatrix(ref MatrixD parentWorldMatrix, object source = null, bool updateChildren = true, bool forceUpdateAllChildren = false)
		{
			if (base.Entity.Parent != null)
			{
				MatrixD.Multiply(ref m_localMatrix, ref parentWorldMatrix, out m_worldMatrix);
				m_lastParentWorldMatrixCounter = base.Entity.Parent.PositionComp.m_worldMatrixCounter;
				m_worldMatrixCounter++;
				OnWorldPositionChanged(source, updateChildren, forceUpdateAllChildren);
			}
		}

		/// <summary>
		/// Called when [world position changed].
		/// </summary>
		/// <param name="source">The source object that caused this event.</param>
		protected virtual void OnWorldPositionChanged(object source, bool updateChildren = true, bool forceUpdateAllChildren = false)
		{
			m_worldVolumeDirty = true;
			m_worldAABBDirty = true;
			m_normalizedInvMatrixDirty = true;
			m_invScaledMatrixDirty = true;
			RaiseOnPositionChanged(this);
		}

		public override string ToString()
		{
			return string.Concat("worldpos=", GetPosition(), ", worldmat=", WorldMatrix);
		}
	}
}
