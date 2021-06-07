using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI
{
	public class MyStatNaturalGravity : MyStatBase
	{
		public override float MaxValue => float.MaxValue;

		public MyStatNaturalGravity()
		{
			base.Id = MyStringHash.GetOrCompute("natural_gravity");
		}

		public override void Update()
		{
			Vector3D worldPoint = (MySession.Static.ControlledEntity == null || !(MySession.Static.ControlledEntity is MyEntity)) ? MySector.MainCamera.Position : (MySession.Static.ControlledEntity as MyEntity).WorldMatrix.Translation;
			base.CurrentValue = MyGravityProviderSystem.CalculateNaturalGravityInPoint(worldPoint).Length() / 9.81f;
		}
	}
}
