using Sandbox.Engine.Physics;
using VRage.Game.Entity;

namespace Sandbox.Game.WorldEnvironment
{
	public delegate void MySectorContactEvent(int itemId, MyEntity other, ref MyPhysics.MyContactPointEvent evt);
}
