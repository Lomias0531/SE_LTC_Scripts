using VRage.Network;
using VRageMath;

namespace VRage.Render.Scene.Components
{
	[GenerateActivator]
	public class MyActorComponent
	{
		private bool m_allowsParallelUpdate;

		private bool m_needsPerFrameUpdate;

		private bool m_needsPerFrameActorUpdate;

		public MyActor Owner
		{
			get;
			private set;
		}

		public virtual Color DebugColor => Color.Magenta;

		public float FrameTime => Owner.Scene.Environment.LastFrameDelta;

		public bool AllowsParallelUpdate
		{
			get
			{
				return m_allowsParallelUpdate;
			}
			protected set
			{
				bool needsPerFrameUpdate = NeedsPerFrameUpdate;
				NeedsPerFrameUpdate = false;
				m_allowsParallelUpdate = value;
				NeedsPerFrameUpdate = needsPerFrameUpdate;
			}
		}

		public bool NeedsPerFrameUpdate
		{
			get
			{
				return m_needsPerFrameUpdate;
			}
			protected set
			{
				if (m_needsPerFrameUpdate == value)
				{
					return;
				}
				m_needsPerFrameUpdate = value;
				if (AllowsParallelUpdate)
				{
					if (value)
					{
						Owner.Scene.Updater.AddForParallelUpdate(this);
					}
					else
					{
						Owner.Scene.Updater.RemoveFromParallelUpdate(this);
					}
				}
				else if (value)
				{
					Owner.Scene.Updater.AddToAlwaysUpdate(this);
				}
				else
				{
					Owner.Scene.Updater.RemoveFromAlwaysUpdate(this);
				}
			}
		}

		public bool NeedsPerFrameActorUpdate
		{
			get
			{
				return m_needsPerFrameActorUpdate;
			}
			set
			{
				if (m_needsPerFrameActorUpdate != value)
				{
					m_needsPerFrameActorUpdate = value;
					Owner.AlwaysUpdate = value;
				}
			}
		}

		public bool IsVisible => Owner.IsVisible;

		public virtual void OnRemove(MyActor owner)
		{
			Destruct();
			NeedsPerFrameUpdate = false;
			NeedsPerFrameActorUpdate = false;
			Owner = null;
			owner.Scene.ComponentFactory.Deallocate(this);
		}

		public virtual void OnVisibilityChange()
		{
		}

		public virtual void Assign(MyActor owner)
		{
			Owner = owner;
		}

		public virtual void Construct()
		{
			Owner = null;
			AllowsParallelUpdate = false;
		}

		public virtual void Destruct()
		{
		}

		public virtual MyChildCullTreeData GetCullTreeData()
		{
			return null;
		}

		public virtual void OnParentSet()
		{
		}

		public virtual void OnParentRemoved()
		{
		}

		public virtual void OnUpdateBeforeDraw()
		{
		}

		public virtual bool StartFadeOut()
		{
			return true;
		}
	}
}
