using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Passage))]
	public class MyPassage : MyCubeBlock, Sandbox.ModAPI.IMyPassage, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyPassage
	{
		private class Sandbox_Game_Entities_Cube_MyPassage_003C_003EActor : IActivator, IActivator<MyPassage>
		{
			private sealed override object CreateInstance()
			{
				return new MyPassage();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPassage CreateInstance()
			{
				return new MyPassage();
			}

			MyPassage IActivator<MyPassage>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override bool GetIntersectionWithSphere(ref BoundingSphereD sphere)
		{
			return false;
		}
	}
}
