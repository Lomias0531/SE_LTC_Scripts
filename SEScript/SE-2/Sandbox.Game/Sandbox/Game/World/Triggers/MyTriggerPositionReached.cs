using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Triggers;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World.Triggers
{
	[TriggerType(typeof(MyObjectBuilder_TriggerPositionReached))]
	public class MyTriggerPositionReached : MyTrigger, ICloneable
	{
		public Vector3D TargetPos = Vector3D.Zero;

		protected double m_maxDistance2 = 10000.0;

		private StringBuilder m_progress = new StringBuilder();

		public double Radius
		{
			get
			{
				return Math.Sqrt(m_maxDistance2);
			}
			set
			{
				m_maxDistance2 = value * value;
			}
		}

		public MyTriggerPositionReached()
		{
		}

		public MyTriggerPositionReached(MyTriggerPositionReached pos)
			: base(pos)
		{
			TargetPos = pos.TargetPos;
			m_maxDistance2 = pos.m_maxDistance2;
		}

		public override object Clone()
		{
			return new MyTriggerPositionReached(this);
		}

		public override bool Update(MyPlayer player, MyEntity me)
		{
			if (me != null && Vector3D.DistanceSquared(me.PositionComp.GetPosition(), TargetPos) < m_maxDistance2)
			{
				m_IsTrue = true;
			}
			return IsTrue;
		}

		public override void Init(MyObjectBuilder_Trigger ob)
		{
			base.Init(ob);
			TargetPos = ((MyObjectBuilder_TriggerPositionReached)ob).Pos;
			m_maxDistance2 = ((MyObjectBuilder_TriggerPositionReached)ob).Distance2;
		}

		public override MyObjectBuilder_Trigger GetObjectBuilder()
		{
			MyObjectBuilder_TriggerPositionReached obj = (MyObjectBuilder_TriggerPositionReached)base.GetObjectBuilder();
			obj.Pos = TargetPos;
			obj.Distance2 = m_maxDistance2;
			return obj;
		}

		public override StringBuilder GetProgress()
		{
			m_progress.Clear().AppendFormat(MyTexts.GetString(MySpaceTexts.ScenarioProgressPositionReached), TargetPos.X, TargetPos.Y, TargetPos.Z, Math.Sqrt(m_maxDistance2));
			return m_progress;
		}

		public override void DisplayGUI()
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenTriggerPositionReached(this));
		}

		public new static MyStringId GetCaption()
		{
			return MySpaceTexts.GuiTriggerCaptionPositionReached;
		}
	}
}
