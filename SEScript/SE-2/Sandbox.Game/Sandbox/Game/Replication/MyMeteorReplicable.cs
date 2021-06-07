using Sandbox.Game.Entities;

namespace Sandbox.Game.Replication
{
	internal class MyMeteorReplicable : MyEntityReplicableBaseEvent<MyMeteor>
	{
		public override void OnDestroyClient()
		{
			if (base.Instance != null && base.Instance.Save)
			{
				base.Instance.Close();
			}
		}
	}
}
