using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Blocks;
using Sandbox.ModAPI.Ingame;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.EntityComponents
{
	[MyEntityComponentDescriptor(typeof(MyObjectBuilder_MyProgrammableBlock), false, new string[]
	{

	})]
	public class MyIngameScriptComponent : MyGameLogicComponent
	{
		private class Sandbox_Game_EntityComponents_MyIngameScriptComponent_003C_003EActor : IActivator, IActivator<MyIngameScriptComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyIngameScriptComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyIngameScriptComponent CreateInstance()
			{
				return new MyIngameScriptComponent();
			}

			MyIngameScriptComponent IActivator<MyIngameScriptComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyProgrammableBlock m_block;

		private UpdateType m_nextUpdate;

		public UpdateType NextUpdate
		{
			get
			{
				return m_nextUpdate;
			}
			set
			{
				if (value != 0)
				{
					m_nextUpdate |= value;
					base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				}
				else
				{
					m_nextUpdate = value;
					base.NeedsUpdate = MyEntityUpdateEnum.NONE;
				}
			}
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			base.Init(objectBuilder);
			m_block = (MyProgrammableBlock)base.Entity;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			UpdateType nextUpdate = m_nextUpdate;
			m_nextUpdate = UpdateType.None;
			if (nextUpdate != 0)
			{
				m_block.Run(string.Empty, nextUpdate);
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			NextUpdate |= UpdateType.Update1;
		}

		public override void UpdateBeforeSimulation10()
		{
			base.UpdateBeforeSimulation();
			NextUpdate |= UpdateType.Update10;
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation();
			NextUpdate |= UpdateType.Update100;
		}
	}
}
