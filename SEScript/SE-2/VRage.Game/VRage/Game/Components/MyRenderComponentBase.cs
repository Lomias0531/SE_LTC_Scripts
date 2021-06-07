using System;
using System.Collections.Generic;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Game.Components
{
	public abstract class MyRenderComponentBase : MyEntityComponentBase
	{
		public static uint[] UNINITIALIZED_IDs = new uint[1]
		{
			4294967295u
		};

		public static readonly Vector3 OldRedToHSV = new Vector3(0f, 0f, 0.05f);

		public static readonly Vector3 OldYellowToHSV = new Vector3(11f / 90f, -0.1f, 0.26f);

		public static readonly Vector3 OldBlueToHSV = new Vector3(0.575f, 0f, 0f);

		public static readonly Vector3 OldGreenToHSV = new Vector3(0.333333343f, -0.48f, -0.25f);

		public static readonly Vector3 OldBlackToHSV = new Vector3(0f, -0.96f, -0.5f);

		public static readonly Vector3 OldWhiteToHSV = new Vector3(0f, -0.95f, 0.4f);

		public static readonly Vector3 OldGrayToHSV = new Vector3(0f, -1f, 0f);

		protected bool m_enableColorMaskHsv;

		protected Vector3 m_colorMaskHsv = OldGrayToHSV;

		protected Dictionary<string, MyTextureChange> m_textureChanges;

		protected Color m_diffuseColor = Color.White;

		public int LastMomentUpdateIndex = -1;

		public Action NeedForDrawFromParentChanged;

		public bool FadeIn;

		public bool FadeOut;

		protected uint[] m_parentIDs = UNINITIALIZED_IDs;

		protected uint[] m_renderObjectIDs = UNINITIALIZED_IDs;

		public float Transparency;

		public byte DepthBias;

		private bool m_visibilityUpdates;

		/// <summary>
		/// Used by game to store model here. In game this is always of type MyModel.
		/// Implementation should only store and return passed object.
		/// </summary>
		public abstract object ModelStorage
		{
			get;
			set;
		}

		public bool EnableColorMaskHsv
		{
			get
			{
				return m_enableColorMaskHsv;
			}
			set
			{
				m_enableColorMaskHsv = value;
				if (EnableColorMaskHsv)
				{
					UpdateRenderEntity(m_colorMaskHsv);
					base.Container.Entity.EnableColorMaskForSubparts(value);
				}
			}
		}

		public Vector3 ColorMaskHsv
		{
			get
			{
				return m_colorMaskHsv;
			}
			set
			{
				m_colorMaskHsv = value;
				if (EnableColorMaskHsv)
				{
					UpdateRenderEntity(m_colorMaskHsv);
					base.Container.Entity.SetColorMaskForSubparts(value);
				}
			}
		}

		public Dictionary<string, MyTextureChange> TextureChanges
		{
			get
			{
				return m_textureChanges;
			}
			set
			{
				m_textureChanges = value;
				if (EnableColorMaskHsv)
				{
					UpdateRenderTextureChanges(value);
					base.Container.Entity.SetTextureChangesForSubparts(value);
				}
			}
		}

		public MyPersistentEntityFlags2 PersistentFlags
		{
			get;
			set;
		}

		public uint[] ParentIDs => m_parentIDs;

		public uint[] RenderObjectIDs => m_renderObjectIDs;

		public bool Visible
		{
			get
			{
				return (base.Container.Entity.Flags & EntityFlags.Visible) != 0;
			}
			set
			{
				EntityFlags flags = base.Container.Entity.Flags;
				if (value)
				{
					base.Container.Entity.Flags = (base.Container.Entity.Flags | EntityFlags.Visible);
				}
				else
				{
					base.Container.Entity.Flags = (base.Container.Entity.Flags & ~EntityFlags.Visible);
				}
				if (flags != base.Container.Entity.Flags)
				{
					UpdateRenderObjectVisibilityIncludingChildren(value);
				}
			}
		}

		public bool DrawInAllCascades
		{
			get;
			set;
		}

		public bool MetalnessColorable
		{
			get;
			set;
		}

		public virtual bool NearFlag
		{
			get
			{
				return (base.Container.Entity.Flags & EntityFlags.Near) != 0;
			}
			set
			{
				bool num = value != NearFlag;
				if (value)
				{
					base.Container.Entity.Flags |= EntityFlags.Near;
				}
				else
				{
					base.Container.Entity.Flags &= ~EntityFlags.Near;
				}
				if (num)
				{
					for (int i = 0; i < m_renderObjectIDs.Length; i++)
					{
						if (m_renderObjectIDs[i] != uint.MaxValue)
						{
							MyRenderProxy.UpdateRenderObjectVisibility(m_renderObjectIDs[i], Visible, NearFlag);
						}
					}
				}
				if (base.Container.TryGet(out MyHierarchyComponentBase component))
				{
					foreach (MyHierarchyComponentBase child in component.Children)
					{
						MyRenderComponentBase component2 = null;
						if (child.Container.Entity.InScene && child.Container.TryGet(out component2))
						{
							component2.NearFlag = value;
						}
					}
				}
			}
		}

		public bool NeedsDrawFromParent
		{
			get
			{
				return (base.Container.Entity.Flags & EntityFlags.NeedsDrawFromParent) != 0;
			}
			set
			{
				if (value != NeedsDrawFromParent)
				{
					base.Container.Entity.Flags &= ~EntityFlags.NeedsDrawFromParent;
					if (value)
					{
						base.Container.Entity.Flags |= EntityFlags.NeedsDrawFromParent;
					}
					if (NeedForDrawFromParentChanged != null)
					{
						NeedForDrawFromParentChanged();
					}
				}
			}
		}

		public bool CastShadows
		{
			get
			{
				return (PersistentFlags & MyPersistentEntityFlags2.CastShadows) != 0;
			}
			set
			{
				if (value)
				{
					PersistentFlags |= MyPersistentEntityFlags2.CastShadows;
				}
				else
				{
					PersistentFlags &= ~MyPersistentEntityFlags2.CastShadows;
				}
			}
		}

		public bool NeedsResolveCastShadow
		{
			get
			{
				return (base.Container.Entity.Flags & EntityFlags.NeedsResolveCastShadow) != 0;
			}
			set
			{
				if (value)
				{
					base.Container.Entity.Flags |= EntityFlags.NeedsResolveCastShadow;
				}
				else
				{
					base.Container.Entity.Flags &= ~EntityFlags.NeedsResolveCastShadow;
				}
			}
		}

		public bool FastCastShadowResolve
		{
			get
			{
				return (base.Container.Entity.Flags & EntityFlags.FastCastShadowResolve) != 0;
			}
			set
			{
				if (value)
				{
					base.Container.Entity.Flags |= EntityFlags.FastCastShadowResolve;
				}
				else
				{
					base.Container.Entity.Flags &= ~EntityFlags.FastCastShadowResolve;
				}
			}
		}

		public bool SkipIfTooSmall
		{
			get
			{
				return (base.Container.Entity.Flags & EntityFlags.SkipIfTooSmall) != 0;
			}
			set
			{
				if (value)
				{
					base.Container.Entity.Flags |= EntityFlags.SkipIfTooSmall;
				}
				else
				{
					base.Container.Entity.Flags &= ~EntityFlags.SkipIfTooSmall;
				}
			}
		}

		public bool DrawOutsideViewDistance
		{
			get
			{
				return (base.Container.Entity.Flags & EntityFlags.DrawOutsideViewDistance) != 0;
			}
			set
			{
				if (value)
				{
					base.Container.Entity.Flags |= EntityFlags.DrawOutsideViewDistance;
				}
				else
				{
					base.Container.Entity.Flags &= ~EntityFlags.DrawOutsideViewDistance;
				}
			}
		}

		public bool ShadowBoxLod
		{
			get
			{
				return (base.Container.Entity.Flags & EntityFlags.ShadowBoxLod) != 0;
			}
			set
			{
				if (value)
				{
					base.Container.Entity.Flags |= EntityFlags.ShadowBoxLod;
				}
				else
				{
					base.Container.Entity.Flags &= ~EntityFlags.ShadowBoxLod;
				}
			}
		}

		public bool OffsetInVertexShader
		{
			get;
			set;
		}

		public virtual bool NeedsDraw
		{
			get
			{
				return (base.Container.Entity.Flags & EntityFlags.NeedsDraw) != 0;
			}
			set
			{
				if (value != NeedsDraw)
				{
					base.Container.Entity.Flags &= ~EntityFlags.NeedsDraw;
					if (value)
					{
						base.Container.Entity.Flags |= EntityFlags.NeedsDraw;
					}
				}
			}
		}

		public override string ComponentTypeDebugString => "Render";

		public abstract void SetRenderObjectID(int index, uint ID);

		public uint GetRenderObjectID()
		{
			if (m_renderObjectIDs.Length != 0)
			{
				return m_renderObjectIDs[0];
			}
			return uint.MaxValue;
		}

		public virtual void RemoveRenderObjects()
		{
			for (int i = 0; i < m_renderObjectIDs.Length; i++)
			{
				ReleaseRenderObjectID(i);
			}
		}

		public abstract void ReleaseRenderObjectID(int index);

		public void ResizeRenderObjectArray(int newSize)
		{
			int num = m_renderObjectIDs.Length;
			Array.Resize(ref m_renderObjectIDs, newSize);
			Array.Resize(ref m_parentIDs, newSize);
			for (int i = num; i < newSize; i++)
			{
				m_renderObjectIDs[i] = uint.MaxValue;
				m_parentIDs[i] = uint.MaxValue;
			}
		}

		public bool IsRenderObjectAssigned(int index)
		{
			return m_renderObjectIDs[index] != uint.MaxValue;
		}

		public virtual void InvalidateRenderObjects()
		{
			if ((!base.Container.Entity.Visible && !base.Container.Entity.CastShadows) || !base.Container.Entity.InScene || !base.Container.Entity.InvalidateOnMove)
			{
				return;
			}
			MatrixD worldMatrix = base.Container.Entity.PositionComp.WorldMatrix;
			for (int i = 0; i < m_renderObjectIDs.Length; i++)
			{
				if (RenderObjectIDs[i] != uint.MaxValue)
				{
					MyRenderProxy.UpdateRenderObject(RenderObjectIDs[i], worldMatrix, null, LastMomentUpdateIndex);
				}
			}
		}

		public void UpdateRenderObjectLocal(Matrix renderLocalMatrix)
		{
			if (!base.Container.Entity.Visible && !base.Container.Entity.CastShadows)
			{
				return;
			}
			for (int i = 0; i < m_renderObjectIDs.Length; i++)
			{
				if (RenderObjectIDs[i] != uint.MaxValue)
				{
					MyRenderProxy.UpdateRenderObject(RenderObjectIDs[i], null, null, LastMomentUpdateIndex, renderLocalMatrix);
				}
			}
		}

		public virtual void UpdateRenderEntity(Vector3 colorMaskHSV)
		{
			m_colorMaskHsv = colorMaskHSV;
			if (m_renderObjectIDs[0] != uint.MaxValue)
			{
				MyRenderProxy.UpdateRenderEntity(m_renderObjectIDs[0], m_diffuseColor, m_colorMaskHsv);
			}
		}

		public virtual void UpdateRenderTextureChanges(Dictionary<string, MyTextureChange> skinTextureChanges)
		{
			m_textureChanges = skinTextureChanges;
			if (m_renderObjectIDs[0] != uint.MaxValue)
			{
				MyRenderProxy.ChangeMaterialTexture(m_renderObjectIDs[0], m_textureChanges);
			}
		}

		protected virtual bool CanBeAddedToRender()
		{
			return true;
		}

		public void UpdateRenderObject(bool visible, bool updateChildren = true)
		{
			if (!base.Container.Entity.InScene && visible)
			{
				return;
			}
			MyHierarchyComponentBase myHierarchyComponentBase = base.Container.Get<MyHierarchyComponentBase>();
			if (visible)
			{
				if (myHierarchyComponentBase != null && Visible && (myHierarchyComponentBase.Parent == null || myHierarchyComponentBase.Parent.Container.Entity.Visible) && CanBeAddedToRender())
				{
					if (!IsRenderObjectAssigned(0))
					{
						AddRenderObjects();
					}
					else
					{
						UpdateRenderObjectVisibility(visible);
					}
				}
			}
			else
			{
				if (m_renderObjectIDs[0] != uint.MaxValue)
				{
					UpdateRenderObjectVisibility(visible);
				}
				RemoveRenderObjects();
			}
			if (updateChildren && myHierarchyComponentBase != null)
			{
				foreach (MyHierarchyComponentBase child in myHierarchyComponentBase.Children)
				{
					MyRenderComponentBase component = null;
					if (child.Container.TryGet(out component))
					{
						component.UpdateRenderObject(visible);
					}
				}
			}
		}

		protected virtual void UpdateRenderObjectVisibility(bool visible)
		{
			uint[] renderObjectIDs = m_renderObjectIDs;
			foreach (uint num in renderObjectIDs)
			{
				if (num != uint.MaxValue)
				{
					MyRenderProxy.UpdateRenderObjectVisibility(num, visible, base.Container.Entity.NearFlag);
				}
			}
		}

		private void UpdateRenderObjectVisibilityIncludingChildren(bool visible)
		{
			UpdateRenderObjectVisibility(visible);
			if (base.Container.TryGet(out MyHierarchyComponentBase component))
			{
				foreach (MyHierarchyComponentBase child in component.Children)
				{
					MyRenderComponentBase component2 = null;
					if (child.Container.Entity.InScene && child.Container.TryGet(out component2))
					{
						component2.UpdateRenderObjectVisibilityIncludingChildren(visible);
					}
				}
			}
		}

		public Color GetDiffuseColor()
		{
			return m_diffuseColor;
		}

		public virtual RenderFlags GetRenderFlags()
		{
			RenderFlags renderFlags = (RenderFlags)0;
			if (NearFlag)
			{
				renderFlags |= RenderFlags.Near;
			}
			if (CastShadows)
			{
				renderFlags |= (RenderFlags.CastShadows | RenderFlags.CastShadowsOnLow);
			}
			if (Visible)
			{
				renderFlags |= RenderFlags.Visible;
			}
			if (NeedsResolveCastShadow)
			{
				renderFlags |= RenderFlags.NeedsResolveCastShadow;
			}
			if (FastCastShadowResolve)
			{
				renderFlags |= RenderFlags.FastCastShadowResolve;
			}
			if (SkipIfTooSmall)
			{
				renderFlags |= RenderFlags.SkipIfTooSmall;
			}
			if (DrawOutsideViewDistance)
			{
				renderFlags |= RenderFlags.DrawOutsideViewDistance;
			}
			if (ShadowBoxLod)
			{
				renderFlags |= RenderFlags.ShadowLodBox;
			}
			if (DrawInAllCascades)
			{
				renderFlags |= RenderFlags.DrawInAllCascades;
			}
			if (MetalnessColorable)
			{
				renderFlags |= RenderFlags.MetalnessColorable;
			}
			return renderFlags;
		}

		public virtual CullingOptions GetRenderCullingOptions()
		{
			return CullingOptions.Default;
		}

		public abstract void AddRenderObjects();

		public abstract void Draw();

		public abstract bool IsVisible();

		public void SetParent(int index, uint cellParentCullObject, Matrix? childToParent = null)
		{
			if (m_parentIDs == UNINITIALIZED_IDs)
			{
				m_parentIDs = new uint[index + 1];
			}
			m_parentIDs[index] = cellParentCullObject;
			MyRenderProxy.SetParentCullObject(RenderObjectIDs[index], cellParentCullObject, childToParent);
		}

		public bool IsChild(int index)
		{
			return m_parentIDs[index] != uint.MaxValue;
		}

		public void SetVisibilityUpdates(bool state)
		{
			m_visibilityUpdates = state;
			PropagateVisibilityUpdates(always: true);
		}

		protected void PropagateVisibilityUpdates(bool always = false)
		{
			if (!always && !m_visibilityUpdates)
			{
				return;
			}
			uint[] renderObjectIDs = m_renderObjectIDs;
			foreach (uint num in renderObjectIDs)
			{
				if (num != uint.MaxValue)
				{
					MyRenderProxy.SetVisibilityUpdates(num, m_visibilityUpdates);
				}
			}
		}

		public void UpdateTransparency()
		{
			if (m_renderObjectIDs[0] != uint.MaxValue)
			{
				MyRenderProxy.UpdateRenderEntity(m_renderObjectIDs[0], null, null, Transparency);
			}
		}
	}
}
