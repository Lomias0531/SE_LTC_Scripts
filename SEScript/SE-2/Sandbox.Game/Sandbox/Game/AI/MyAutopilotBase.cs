using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;

namespace Sandbox.Game.AI
{
	public abstract class MyAutopilotBase
	{
		protected MyCockpit ShipController
		{
			get;
			private set;
		}

		public virtual bool RemoveOnPlayerControl => true;

		public abstract MyObjectBuilder_AutopilotBase GetObjectBuilder();

		public abstract void Init(MyObjectBuilder_AutopilotBase objectBuilder);

		public virtual void OnAttachedToShipController(MyCockpit newShipController)
		{
			ShipController = newShipController;
		}

		public virtual void OnRemovedFromCockpit()
		{
			ShipController = null;
		}

		public abstract void Update();

		public virtual void DebugDraw()
		{
		}
	}
}
