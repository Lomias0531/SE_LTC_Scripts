using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI
{
	public class MyStatArtificialGravity : MyStatBase
	{
		public override float MaxValue => float.MaxValue;

		public MyStatArtificialGravity()
		{
			base.Id = MyStringHash.GetOrCompute("artificial_gravity");
		}

		public override void Update()
		{
			Vector3D worldPoint = (MySession.Static.ControlledEntity == null || !(MySession.Static.ControlledEntity is MyEntity)) ? MySector.MainCamera.Position : (MySession.Static.ControlledEntity as MyEntity).PositionComp.WorldAABB.Center;
			base.CurrentValue = MyGravityProviderSystem.CalculateArtificialGravityInPoint(worldPoint, MyGravityProviderSystem.CalculateArtificialGravityStrengthMultiplier(MyGravityProviderSystem.CalculateHighestNaturalGravityMultiplierInPoint(worldPoint))).Length() / 9.81f;
		}
	}
}
