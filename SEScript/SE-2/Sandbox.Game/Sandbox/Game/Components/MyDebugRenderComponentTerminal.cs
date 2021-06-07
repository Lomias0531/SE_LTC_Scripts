using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using System;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderComponentTerminal : MyDebugRenderComponent
	{
		private MyTerminalBlock m_terminal;

		public MyDebugRenderComponentTerminal(MyTerminalBlock terminal)
			: base(terminal)
		{
			m_terminal = terminal;
		}

		public override void DebugDraw()
		{
			base.DebugDraw();
			if (MyDebugDrawSettings.DEBUG_DRAW_BLOCK_NAMES && m_terminal.CustomName != null && MySession.Static.ControlledEntity != null)
			{
				Vector3D value = (MySession.Static.ControlledEntity as MyCharacter)?.WorldMatrix.Up ?? Vector3D.Zero;
				Vector3D vector3D = m_terminal.PositionComp.WorldMatrix.Translation + value * m_terminal.CubeGrid.GridSize * 0.40000000596046448;
				Vector3D translation = MySession.Static.ControlledEntity.Entity.WorldMatrix.Translation;
				double num = (vector3D - translation).Length();
				if (!(num > 35.0))
				{
					Color lightSteelBlue = Color.LightSteelBlue;
					lightSteelBlue.A = ((num < 15.0) ? byte.MaxValue : ((byte)((15.0 - num) * 12.75)));
					double num2 = Math.Min(8.0 / num, 1.0);
					MyRenderProxy.DebugDrawText3D(vector3D, "<- " + m_terminal.CustomName.ToString(), lightSteelBlue, (float)num2, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
				}
			}
		}
	}
}
