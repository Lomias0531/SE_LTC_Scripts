using System;
using System.Text;
using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.Gui
{
	public struct MyHudEntityParams
	{
		private IMyEntity m_entity;

		private Vector3D m_position;

		public IMyEntity Entity
		{
			get
			{
				return m_entity;
			}
			set
			{
				m_entity = value;
				if (value != null)
				{
					EntityId = value.EntityId;
				}
			}
		}

		public long EntityId
		{
			get;
			set;
		}

		public Vector3D Position
		{
			get
			{
				if (m_entity == null || m_entity.PositionComp == null)
				{
					return m_position;
				}
				return m_entity.PositionComp.GetPosition();
			}
			set
			{
				m_position = value;
			}
		}

		public StringBuilder Text
		{
			get;
			set;
		}

		public MyHudIndicatorFlagsEnum FlagsEnum
		{
			get;
			set;
		}

		public long Owner
		{
			get;
			set;
		}

		public MyOwnershipShareModeEnum Share
		{
			get;
			set;
		}

		public float BlinkingTime
		{
			get;
			set;
		}

		/// <summary>
		/// Function that checks whether indicator should be drawn.
		/// Useful when reacting to some player settings.
		/// </summary>
		public Func<bool> ShouldDraw
		{
			get;
			set;
		}

		public MyHudEntityParams(StringBuilder text, long Owner, MyHudIndicatorFlagsEnum flagsEnum)
		{
			this = default(MyHudEntityParams);
			Text = text;
			FlagsEnum = flagsEnum;
			this.Owner = Owner;
		}

		public MyHudEntityParams(MyObjectBuilder_HudEntityParams builder)
		{
			this = default(MyHudEntityParams);
			m_entity = null;
			m_position = builder.Position;
			EntityId = builder.EntityId;
			Text = new StringBuilder(builder.Text);
			FlagsEnum = builder.FlagsEnum;
			Owner = builder.Owner;
			Share = builder.Share;
			BlinkingTime = builder.BlinkingTime;
		}

		public MyObjectBuilder_HudEntityParams GetObjectBuilder()
		{
			return new MyObjectBuilder_HudEntityParams
			{
				EntityId = EntityId,
				Position = Position,
				Text = Text.ToString(),
				FlagsEnum = FlagsEnum,
				Owner = Owner,
				Share = Share,
				BlinkingTime = BlinkingTime
			};
		}
	}
}
