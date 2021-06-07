using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Gui
{
	internal class MyDebugEntity : MyEntity
	{
		private class Sandbox_Game_Gui_MyDebugEntity_003C_003EActor : IActivator, IActivator<MyDebugEntity>
		{
			private sealed override object CreateInstance()
			{
				return new MyDebugEntity();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDebugEntity CreateInstance()
			{
				return new MyDebugEntity();
			}

			MyDebugEntity IActivator<MyDebugEntity>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			base.Init(objectBuilder);
			base.Render.ModelStorage = MyModels.GetModelOnlyData("Models\\StoneRoundLargeFull.mwm");
		}
	}
}
