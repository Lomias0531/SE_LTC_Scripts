using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace Sandbox.Game.GUI
{
	public class MyStatControlBase : IMyStatControl
	{
		private Vector2 m_position;

		private Vector2 m_size;

		private MyStatControls m_parent;

		public float StatCurrent
		{
			get;
			set;
		}

		public float StatMaxValue
		{
			get;
			set;
		}

		public float StatMinValue
		{
			get;
			set;
		}

		public string StatString
		{
			get;
			set;
		}

		public uint FadeInTimeMs
		{
			get;
			set;
		}

		public uint FadeOutTimeMs
		{
			get;
			set;
		}

		public uint MaxOnScreenTimeMs
		{
			get;
			set;
		}

		public uint SpentInStateTimeMs
		{
			get;
			set;
		}

		public MyStatControlState State
		{
			get;
			set;
		}

		public VisualStyleCategory Category
		{
			get;
			set;
		}

		public Vector4 ColorMask
		{
			get;
			set;
		}

		public MyAlphaBlinkBehavior BlinkBehavior
		{
			get;
			private set;
		}

		public MyStatControls Parent => m_parent;

		public Vector2 Position
		{
			get
			{
				return m_position;
			}
			set
			{
				Vector2 position = m_position;
				m_position = value;
				OnPositionChanged(position, value);
			}
		}

		public Vector2 Size
		{
			get
			{
				return m_size;
			}
			set
			{
				Vector2 size = m_size;
				m_size = value;
				OnSizeChanged(size, value);
			}
		}

		protected MyStatControlBase(MyStatControls parent)
		{
			ColorMask = Vector4.One;
			BlinkBehavior = new MyAlphaBlinkBehavior();
			FadeInTimeMs = 0u;
			FadeOutTimeMs = 0u;
			SpentInStateTimeMs = 0u;
			State = MyStatControlState.Invisible;
			m_parent = parent;
		}

		public virtual void Draw(float transitionAlpha)
		{
		}

		protected virtual void OnPositionChanged(Vector2 oldPosition, Vector2 newPosition)
		{
		}

		protected virtual void OnSizeChanged(Vector2 oldSize, Vector2 newSize)
		{
		}
	}
}
