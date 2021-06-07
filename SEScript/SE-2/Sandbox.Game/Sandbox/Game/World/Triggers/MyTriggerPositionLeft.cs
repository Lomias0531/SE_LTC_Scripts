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
	[TriggerType(typeof(MyObjectBuilder_TriggerPositionLeft))]
	public class MyTriggerPositionLeft : MyTrigger, ICloneable
	{
		public Vector3D TargetPos = new Vector3D(0.0, 0.0, 0.0);

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

		public MyTriggerPositionLeft()
		{
		}

		public MyTriggerPositionLeft(MyTriggerPositionLeft pos)
			: base(pos)
		{
			TargetPos = pos.TargetPos;
			m_maxDistance2 = pos.m_maxDistance2;
		}

		public override object Clone()
		{
			return new MyTriggerPositionLeft(this);
		}

		public override bool Update(MyPlayer player, MyEntity me)
		{
			if (me != null && Vector3D.DistanceSquared(me.PositionComp.GetPosition(), TargetPos) > m_maxDistance2)
			{
				m_IsTrue = true;
			}
			return IsTrue;
		}

		public override void Init(MyObjectBuilder_Trigger ob)
		{
			base.Init(ob);
			TargetPos = ((MyObjectBuilder_TriggerPositionLeft)ob).Pos;
			m_maxDistance2 = ((MyObjectBuilder_TriggerPositionLeft)ob).Distance2;
		}

		public override MyObjectBuilder_Trigger GetObjectBuilder()
		{
			MyObjectBuilder_TriggerPositionLeft obj = (MyObjectBuilder_TriggerPositionLeft)base.GetObjectBuilder();
			obj.Pos = TargetPos;
			obj.Distance2 = m_maxDistance2;
			return obj;
		}

		public override StringBuilder GetProgress()
		{
			m_progress.Clear().AppendFormat(MyTexts.GetString(MySpaceTexts.ScenarioProgressPositionLeft), TargetPos.X, TargetPos.Y, TargetPos.Z, Math.Sqrt(m_maxDistance2));
			return m_progress;
		}

		public override void DisplayGUI()
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenTriggerPositionLeft(this));
		}

		public new static MyStringId GetCaption()
		{
			return MySpaceTexts.GuiTriggerCaptionPositionLeft;
		}
	}
}
